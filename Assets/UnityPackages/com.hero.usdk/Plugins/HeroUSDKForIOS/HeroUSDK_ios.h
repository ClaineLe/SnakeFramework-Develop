//
//

#import <Foundation/Foundation.h>
#import <HeroUSDK/HeroUSDK.h>


@interface HeroUSDK_ios : NSObject
{
    NSString *_gameObjectName;
    BOOL bU3dInited; //setListener表明u3d初始化成功
}
+ (HeroUSDK_ios *)shareInstance;
//listener
- (void)setListener:(NSString *)gameObjectName;

//融合、国内初始化
- (void)initWithUSDKKey:(NSString *)productKey
              productID:(NSString *)productID ;
//登录
- (void)enterLoginView ;
//退出登录
- (void)logout ;
//退出登录并显示账号历史界面
- (void)logoutAndSowLoginView ;
//用户中心
- (void)showUserCenter ;
//清除本地账号
- (void)cleanUserEntities ;
//用户名
- (NSString *)getUserName ;
//用户ID
- (NSString *)getUserId ;
//SDKID
- (NSString *)getSdkId ;
//获取设备号
- (NSString *)getDeviceNum ;
//内购
- (void)iapPurchaseWithData:(GamePaymentParameters *)parameters ;
//设置基础参数
- (void)setBaseRoleInfoWithData:(HeroHDCBaseGameRoleInfo *)baseInfo ;
//角色登录
- (void)roleLoginWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo ;
//角色注册
- (void)roleRegisterWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo ;
//角色升级
- (void)roleLevelUpWithGameRoleInfo:(HeroHDCGameRoleInfo *)roleInfo ;
//分享
- (void)share:(BOOL)hasUi title:(NSString *)title content:(NSString *)content imagePath:(NSString *)imagePath url:(NSString *)url shareTo:(NSString *)shareTo ;
//上报闪屏
- (void)postSplashScreenEndSuccess ;
//扫码登录
- (void)showScanViewWithExt:(NSString *)exit ;
//收到踢下线结果回调SDK
- (void)cpKickOffCallBackWithResult:(NSString *)result ;
//打开客服
- (void)openServiceView ;
//获取协议内容
- (NSString *)getProtocolResult ;
//点击同意通知
- (void)setAgreeProtocol ;

@end
