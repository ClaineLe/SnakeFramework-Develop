//
//

#import "HeroUSDK_ios.h"
#import "HeroUSDKInterfaceUnity.h"


#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
#if defined(__cplusplus)
}
#endif

@implementation HeroUSDK_ios

static HeroUSDK_ios * __singleton__;
+ (HeroUSDK_ios *)shareInstance {
    static dispatch_once_t predicate;
    dispatch_once( &predicate, ^{ __singleton__ = [[[self class] alloc] init]; } );
    return __singleton__;
}

- (instancetype)init {
    self = [super init];
    if (self) {
        [self addNotifications];
        bU3dInited = NO;
    }
    return self;
}

-(void)addNotifications
{
    static BOOL isAdded = NO;
    if (isAdded) { return ; }
    isAdded = YES;
    //添加通知
    //监听用户登陆成功的通知 单机版游戏不用监听
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(loginNotification:) name:GAME_PUBLIC_NOTIFICATION_NAME_LOGIN object:nil];
    //监听点击切换账号
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(changeAccountNotification:) name:GAME_PUBLIC_NOTIFICATION_NAME_CHANGE_ACCOUNT  object:nil];
    //内购通知
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(iapNotification:) name:GAME_PUBLIC_NOTIFICATION_NAME_IAPPURCHASE_FINISH object:nil];
    //踢下线
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(loginValidNotification:) name:GAME_PUBLIC_NOTIFICATION_NAME_LOGONINVALID object:nil];
    //同意协议通知
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(clickProtocol) name:GAME_PUBLIC_NOTIFICATION_NAME_PROTOCOL object:nil];
}
//登录通知
-(void)loginNotification:(NSNotification *)notification
{
    BOOL isLoginSuccess = [notification.object[GAME_PUBLIC_TAG_LOGIN_IS_SUCCESS] boolValue];
    if (isLoginSuccess) {
        NSString * accessCode = notification.object[GAME_PUBLIC_TAG_ACCESS_CODE];
        NSString * accessToken = notification.object[GAME_PUBLIC_TAG_HEROU_ACCESS_TOKEN];
        NSString * sdkuserid = [[HeroUPlatform sharedInstance] getUserId] ;
        NSString * username = [[HeroUPlatform sharedInstance] getUserName] ;
        NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
        if (accessCode != nil ) {
            params[@"accessCode"] = accessCode ;
        }
        if (accessToken != nil ) {
            params[@"accessToken"] = accessToken ;
        }
        if (sdkuserid != nil ) {
            params[@"sdkuserid"] = sdkuserid ;
        }
        if (username != nil ) {
            params[@"username"] = username ;
        }
        [self sendU3dMessage:@"onLoginSuccess" params:params];
        
        NSLog(@"用户登陆成功!");
        NSLog(@"==> AccessCode: %@", accessCode);
        NSLog(@"==> AccessToken: %@", accessToken);
        NSLog(@"==> sdkuserid: %@", sdkuserid);
        NSLog(@"==> username: %@", username);
        NSLog(@"==> params: %@", params);
    } else {
        NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
        params[@"msg"] = @"用户登陆失败!" ;
        [self sendU3dMessage:@"onLoginFailed" params:params];
        
        NSLog(@"用户登陆失败!");
    }
}
//切换账号通知
-(void)changeAccountNotification:(NSNotification *)notification
{
    NSMutableDictionary *params = [[NSMutableDictionary alloc] init] ;
    params[@"msg"] = @"切换账号成功!" ;
    
    [self sendU3dMessage:@"onSwitchAccountSuccess" params:params];
}
//内购通知
-(void)iapNotification:(NSNotification *)notification
{
    BOOL isIapPurchaseSucess = [notification.object[GAME_PUBLIC_TAG_IAPPURCHASE_IS_SUCCESS] boolValue];
    if (isIapPurchaseSucess) {
        GamePaymentOrder * paymentOrder = notification.object[GAME_PUBLIC_TAG_PAYMENT_ORDER];
        NSMutableDictionary  *params = [[NSMutableDictionary alloc] init];
        if (paymentOrder.orderId != nil) {
            params[@"orderId"] = paymentOrder.orderId ;
        }
        if (paymentOrder.orderAmount != nil) {
            params[@"orderAmount"] = paymentOrder.orderAmount ;
        }
        if (paymentOrder.currency != nil) {
            params[@"currency"] = paymentOrder.currency ;
        }
        [self sendU3dMessage:@"onPaySuccess" params:params];
        
        NSLog(@"iap购买成功");
        NSLog(@"支付成功订单:iap订单id: %@",paymentOrder.orderId);
        
    } else {
        GamePaymentOrder * paymentOrder = notification.object[GAME_PUBLIC_TAG_PAYMENT_ORDER];
        NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
        params[@"code"] = [NSString stringWithFormat:@"%ld",(long)paymentOrder.errorCode] ;
        if (paymentOrder.errorDescription != nil) {
            params[@"msg"] = paymentOrder.errorDescription ;
        }
        [self sendU3dMessage:@"onPayFailed" params:params];
        
        NSLog(@"iap购买失败");
    }
}
//登录失效、踢下线通知
-(void)loginValidNotification:(NSNotification *)notification
{
    NSString *reason = notification.userInfo[GAME_PUBLIC_CP_KICKOFF_REASON] ;
    if (reason != nil) {
        //被踢下线
        NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
        params[@"msg"] = reason ;
        
        [self sendU3dMessage:@"onLogonInvalid" params:params];
        
    } else {
        //登录失效
        NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
        params[@"msg"] = @"登录失效,请尝试重新登录" ;
        
        [self sendU3dMessage:@"onReLoginFromInvalid" params:params];
    }
}
//这里处理同意协议后的逻辑
-(void)clickProtocol
{
    NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
    [self sendU3dMessage:@"onClickProtocol" params:params];
}
- (NSString *)jsonStrFromDictionary:(NSDictionary *)dic {
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:0 error:&error];
    if ([error code]) {
        return @"";
    }
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

-(void)sendU3dMessage:(NSString *)messageName params:(NSDictionary *)dict
{
    if (dict != nil)
    {
        NSString *jsonString = [self jsonStrFromDictionary:dict];
        UnitySendMessage([_gameObjectName UTF8String], [messageName UTF8String], [jsonString UTF8String]);
    } else {
        UnitySendMessage([_gameObjectName UTF8String], [messageName UTF8String], "");
    }
}

- (void)setListener:(NSString *)gameObjectName
{
    _gameObjectName = gameObjectName;
    bU3dInited = YES ;
}
- (void)initWithUSDKKey:(NSString *)productKey
              productID:(NSString *)productID
{
    HeroUProject *uProject = [[HeroUProject alloc] init];
    //原usdkProductId
    uProject.usdkProductId = productID;
    //原usdkProductKey
    uProject.usdkProductKey = productKey ;
    [[HeroUPlatform sharedInstance] initWithProject:uProject];
}
//登录
- (void)enterLoginView
{
    [[HeroUPlatform sharedInstance] enterLoginView] ;
}
//退出登录
- (void)logout
{
    [[HeroUPlatform sharedInstance] logout] ;
    NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
    params[@"msg"] = @"退出登录成功";
    [self sendU3dMessage:@"onLogoutSuccess" params:params];
}
//退出登录并显示账号历史界面
- (void)logoutAndSowLoginView
{
    [[HeroUPlatform sharedInstance] logoutAndSowLoginView] ;
    NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
    params[@"msg"] = @"退出登录成功";
    [self sendU3dMessage:@"onLogoutSuccess" params:params];
}
//用户中心
- (void)showUserCenter
{
    [[HeroUPlatform sharedInstance] showUserCenter] ;
}
//清除本地账号
- (void)cleanUserEntities
{
    [[HeroUPlatform sharedInstance] cleanUserEntities] ;
}
//用户名
- (NSString *)getUserName
{
    return [[HeroUPlatform sharedInstance] getUserName] ;
}
//用户ID
- (NSString *)getUserId
{
    return [[HeroUPlatform sharedInstance] getUserId] ;
}
//SDKID
- (NSString *)getSdkId
{
    return [[HeroUPlatform sharedInstance] getSdkId] ;
}
//获取设备号
- (NSString *)getDeviceNum
{
    return [[HeroUPlatform sharedInstance] getDeviceNum] ;
}
//内购
- (void)iapPurchaseWithData:(GamePaymentParameters *)parameters
{
    [[HeroUPlatform sharedInstance] iapPurchaseWithData:parameters] ;
}
//设置基础参数
- (void)setBaseRoleInfoWithData:(HeroHDCBaseGameRoleInfo *)baseInfo
{
    [[HeroUPlatform sharedInstance] setBaseRoleInfoWithData:baseInfo] ;
}
//角色登录
- (void)roleLoginWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo
{
    [[HeroUPlatform sharedInstance] roleLoginWithGameRoleInfo:roleInfo] ;
}
//角色注册
- (void)roleRegisterWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo
{
    [[HeroUPlatform sharedInstance] roleRegisterWithGameRoleInfo:roleInfo] ;
}
//角色升级
- (void)roleLevelUpWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo
{
    [[HeroUPlatform sharedInstance] roleLevelUpWithGameRoleInfo:roleInfo] ;
}
//分享
- (void)share:(BOOL)hasUi title:(NSString *)title content:(NSString *)content imagePath:(NSString *)imagePath url:(NSString *)url shareTo:(NSString *)shareTo
{
    HeroUSharePlatform sharePlatForm = HeroU_Share_Platform_WeChat ;
    if ([shareTo intValue] == 1) {
        sharePlatForm = HeroU_Share_Platform_WeChat ;
    } else if ([shareTo intValue] == 2) {
        sharePlatForm = HeroU_Share_Platform_WXTimeLine ;
    } else if ([shareTo intValue] == 3) {
        sharePlatForm = HeroU_Share_Platform_QQ ;
    } else if ([shareTo intValue] == 4) {
        sharePlatForm = HeroU_Share_Platform_QQ_Space ;
    }
    if (hasUi) {
        NSMutableArray *array = [[NSMutableArray alloc] init];
        HeroUShareModel *shareModel = [[HeroUShareModel alloc] init];
        shareModel.sharePlatform = sharePlatForm ;
        if (url.length > 0) {
            shareModel.shareType = HeroUShareType_link ;
            shareModel.shareLink = url ;
            shareModel.shareLinkTitle = title ;
            shareModel.shareLinkDescription = content ;
        } else {
            shareModel.shareType = HeroUShareType_image ;
            shareModel.shareImage = [NSData dataWithContentsOfURL:[NSURL fileURLWithPath:imagePath]] ;
        }
        [array addObject:shareModel];
        [[HeroUPlatform sharedInstance] showShareViewWithShareDataArray:array success:^(NSString *successMessage, HeroUShareStatus status, HeroUShareTaget target) {
            [self sendU3dMessage:@"onShareSuccessdAction" params:@{@"shareType":@(target)}];
        } failure:^(NSString *failureMessage, HeroUShareStatus status, HeroUShareTaget target) {
            [self sendU3dMessage:@"onShareFailedAction" params:@{@"shareType":@(target),@"msg":failureMessage}];
        }];
    } else {
        HeroUShareModel *shareModel = [[HeroUShareModel alloc] init];
        shareModel.sharePlatform = sharePlatForm ;
        if (url.length > 0) {
            shareModel.shareType = HeroUShareType_link ;
            shareModel.shareLink = url ;
            shareModel.shareLinkTitle = title ;
            shareModel.shareLinkDescription = content ;
        } else {
            shareModel.shareType = HeroUShareType_image ;
            shareModel.shareImage = [NSData dataWithContentsOfURL:[NSURL fileURLWithPath:imagePath]] ;
        }
        [[HeroUPlatform sharedInstance] shareWithShareData:shareModel success:^(NSString *successMessage, HeroUShareStatus status, HeroUShareTaget target) {
            [self sendU3dMessage:@"onShareSuccessdAction" params:@{@"shareType":@(target)}];
        } failure:^(NSString *failureMessage, HeroUShareStatus status, HeroUShareTaget target) {
            [self sendU3dMessage:@"onShareFailedAction" params:@{@"shareType":@(target),@"msg":failureMessage}];
        }] ;
    }
}
//上报闪屏
- (void)postSplashScreenEndSuccess
{
    [[HeroUPlatform sharedInstance] postSplashScreenEndSuccess:^(id obj) {
        NSLog(@"--------------> iOS闪屏上报成功 <-------------") ;
    } faild:^(id obj) {
        NSLog(@"--------------> iOS闪屏上报失败 <------------") ;
    }] ;
}
//扫码登录
- (void)showScanViewWithExt:(NSString *)exit
{
    [[HeroUPlatform sharedInstance] showScanViewWithExt:exit] ;
}
//收到踢下线结果回调SDK
- (void)cpKickOffCallBackWithResult:(NSString *)result
{
    NSMutableDictionary *params = [[NSMutableDictionary alloc] init];
    if (result.length > 0) {
        params[GAME_PUBLIC_CP_KICKOFF_RESULT] = result ;
    }
    [[HeroUPlatform sharedInstance] cpKickOffCallBackWithResult:params];
}
//打开客服
- (void)openServiceView
{
    [[HeroUPlatform sharedInstance] openServiceView] ;
}

//获取协议内容
- (NSString *)getProtocolResult
{
    NSDictionary* resultDic = [[HeroUPlatform sharedInstance] getProtocolResult];
    if (resultDic.allValues.count > 0) {
        NSData *josnData = [NSJSONSerialization dataWithJSONObject:resultDic options:NSJSONWritingPrettyPrinted error:nil] ;
        if (josnData != nil) {
            NSString *jsonString = [[NSString alloc] initWithData:josnData encoding:NSUTF8StringEncoding] ;
            return jsonString ;
        }
    }
    return nil ;
}
//点击同意通知
- (void)setAgreeProtocol
{
    [[HeroUPlatform sharedInstance] setAgreeProtocol] ;
}

@end
