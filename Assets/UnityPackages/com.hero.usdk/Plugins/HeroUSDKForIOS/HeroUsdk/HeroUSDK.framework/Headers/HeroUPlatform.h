//
//  HeroUPlatform.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/10/26.
//  Copyright © 2020年 Hero. All rights reserved.

#import <HeroUSDK/HeroUProject.h>
#import <HeroUSDK/HeroUPay.h>
#import <HeroUSDK/HeroUData.h>
#import <HeroUSDK/HeroUBloc.h>
#import <HeroUSDK/HeroUShare.h>
#import <HeroUSDK/GameProject.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

//usdk信息类型
typedef NS_ENUM(NSInteger, HeroUInfoType){
    /**用户id*/
    HeroUInfoTypeUserId,
    /**用户名*/
    HeroUInfoTypeUserName,
    /**SDKId*/
    HeroUInfoTypeSDKId,
    /**渠道Id*/
    HeroUInfoTypeChannelId,
    /**渠道名称*/
    HeroUInfoTypeChannelName,
    /**设备号*/
    HeroUInfoTypeDeviceNum,

};

@interface HeroUPlatform : NSObject

#pragma mark - 初始化方法
/*
 *  获取单例实例对象
 */
+ (HeroUPlatform *)sharedInstance;

/**
 HeroU初始化方法
 先初始化HeroUProject对象
 e.g.
 HeroUProject *project = [[HeroUProject alloc] init];
 project.productId = @"融合SDK服务器后台申请的产品id";
 project.productKey = @"融合SDK服务器后台申请的产品key";
 
 NSDictionary *params = @{@"gameId"  : @"",
                          @"productCode" : @"",
                          @"productKey" : @"",
                          @"projectId" : @"",
                          @"urlServer" : @"",
                          @"twitterKey" : @"",
                          @"twitterSecret" : @""
 
 }
 [project channelInitWithParameters:params];
 [[HeroUPlatform sharedInstance] initWithProject:project];
 @param project 项目参数对象
 */
- (void)initWithProject:(HeroUProject *)project;


/**  设置URL */
@property (nonatomic, copy) NSString *serverUrl DEPRECATED_MSG_ATTRIBUTE("v3.5.3以上建议调用initWithProject方法");

/**
 * 老的初始化方法，建议改为"initWithProject:"初始化方法
 @brief 设置项目参数
 @param project 项目设置参数对象
 @note 调用SDK之前必须保证GameProject对象的各项参数正确设置
 */
- (void)launchingPlatformWithApplication:(nullable UIApplication *)application
                                 project:(GameProject *)project
                             withOptions:(nullable NSDictionary *)launchOptions DEPRECATED_MSG_ATTRIBUTE("v3.5.3以上建议调用initWithProject方法");

#pragma mark - =================== 登录相关API ===================

/**
 @brief  调用SDK默认的登陆界面
 @note   如果开发者想自定义用户登陆界面，请调用后序用户相关的接口方法
 */
- (void)enterLoginView;

/**
 @brief  注销、退出登陆
 */
- (void)logout;

/**
 @brief  注销、退出登陆
 */
- (void)logoutAndSowLoginView;

#pragma mark - =================== 支付相关API ===================
/**
 @brief 调用SDKIAP内购
 @param paymentParametersData iap支付参数配置
 @result 0表示方法调用成功
 */
- (int)iapPurchaseWithData:(GamePaymentParameters *)paymentParametersData;

#pragma mark - =================== 数据上报API ===================

/** 设置基础数据 */
- (void)setBaseRoleInfoWithData:(HeroHDCBaseGameRoleInfo *)data;

/**  角色登录  */
- (void)roleLoginWithGameRoleInfo:(HeroHDCGameRoleInfo *)gameRoleInfo;

/**  角色注册  */
- (void)roleRegisterWithGameRoleInfo:(HeroHDCGameRoleInfo *)gameRoleInfo;

/**  角色升级  */
- (void)roleLevelUpWithGameRoleInfo:(HeroHDCGameRoleInfo *)gameRoleInfo;

/** 上报闪屏结束  */
- (void)postSplashScreenEndSuccess:(void (^)(id obj))success
                             faild:(void (^)(id obj))faild;

#pragma mark - =================== 其他API ===================

/**根据类型获取用户信息*/
- (NSString *)getInfoWithType:(HeroUInfoType)type;

/*
 *  打开用户中心
 */
- (void)showUserCenter;

/*
 *  清空用户存储
 */
- (void)cleanUserEntities;

/**
 @brief 获取登录用户名
 @result 若用户没登录，则返回nil
 */
- (NSString *)getUserName;

/**
 @brief 获取登录用户ID
 @result 若用户没登录，则返回nil
 */
- (NSString *)getUserId;

/**
 @brief 获取ID（部分游戏使用@"id"字段作为唯一标示符）
 @result 若用户没登录，则返回nil
 */
- (NSString *)getSdkId;


/**
 *  设置Cmge SDK内部是否为公共版
 *  如果设置为YES, 那么为公共版
 *  如果设置为NO, 那么为非公共版
 */
- (void)setPublicLogin:(BOOL)publicLogin;


/**
 设置Cmge SDK是否为单机版
 
 @param isOnlineGame 如果设置YES,那么为单机版，如果设置为NO，那么为网游版
 
 */
- (void)setIsOnlineGame:(BOOL)isOnlineGame;

/**
 *  当前是否已经登录
 *
 *  @return YES 已经登录
 *          NO 没有登录
 */
- (BOOL)isLogined;

/*
 * 获取设备号(优先取的IDFA、没取到则取的UUID)
 */
- (NSString *)getDeviceNum;


/** 扫码登录（在PC端登录,SDK需要登录状态）*/
- (void)showScanViewWithExt:(NSString *)exit;

/*
 * 打开客服界面
 */
- (void)openServiceView;

/**
下线回传
@param result 结果
*/
- (void)cpKickOffCallBackWithResult:(NSDictionary *)result ;

/**获取首次弹窗协议内容
{
       childAgrName = "个人信息保护政策";
       childAgrUrl = "";
       priAgrName = "个人信息保护政策";
       priAgrUrl = "";
       userAgrName = "用户协议";
       userAgrUrl = "";
       version = 1;
};
 */
- (NSDictionary *)getProtocolResult;

/**
 cp自绘制协议弹窗界面，点击同意设置接口
 */
- (void)setAgreeProtocol;

#pragma mark - =================== 助手相关API ===================
/*
 *  java 服务器，请求配置参数等
 */
@property (nonatomic, copy) NSString *javaServer;
/*
 *  php服务器，打开网页，红点等
 */
@property (nonatomic, copy) NSString *phpServer;

/**
 初始化助手模块
 在此方法中调用- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
 */
-(void)setBlocConfig:(HeroUBlocConfig *)config;

/*
 *  打开指定页面回调
 */
@property (nonatomic, copy  ) void (^onSdkRedirectMessage)(NSString *action,id data);
/*
 *  设置助手浮标位置
 */
@property (nonatomic, assign) CGPoint floatBtnFrame;

/*
 *  是否显示小恐龙浮标
 */
- (void)needShowPhoneFloat:(BOOL)needShow;

/*
 *  =================== 攻略、赛事、社区模块（根据业务需要接入） ===================
 */

/**
 *  上报比赛信息
 *  BlocMatchData
 *  @param matchData 比赛信息数据模型
 */
- (void)postMatch:(NSDictionary *)matchData;

/**
 是否显示入口
 
 @param key 入口对应key值 （攻略：tactic  赛事：match  拉新：spread 商城：shop）对应值可咨询用户中心
 
 @return YES(是，显示) NO(否，隐藏)
 */
- (BOOL)showEntrance:(NSString *)key;

/**
 *  打开专区
 *
 *  @param action    action(必填)
 *  @param data      data(可选)
 *  @param module    必填，可根据对照表填写
 *  @param exitBlock 退出回调
 */
- (void)openForumViewWithAction:(NSString *)action
                           data:(NSString *)data
                         module:(NSString *)module
                           exit:(void (^)(void))exitBlock;
/*
 *  直接传URL打开专区
 *
 *  @param openForumViewWithUrl 是否需要拼接公共参数
 *  @param exitBlock 退出回调
 */
- (void)openForumViewWithUrl:(NSString *)urlStr
        needCommonParamiters:(BOOL)needCommonParamiters
                        exit:(void (^)(void))exitBlock;

/**
 *  主动获取红点信息（被动使用通知 BLOC_TAG_NOTIFICATION_RED）
 *
 *  @param resBlock 获取红点的回调
 */
- (void)checkRedPoint:(void(^)(void))resBlock;

/**
 根据key值是否显示红点
 在通知中调用或- (void)checkRedPoint:(void(^)())resBlock;回调中使用
 @param key key
 
 @return yes为显示 no为隐藏
 */
- (BOOL)showRed:(NSString *)key;

/*
 *  =================== 关系链模块 ===================
 */

/**
 打开邀请密友列表页面
 */
- (void)openRelationChainView;


/**
 打开邀请密友页面
 */
- (void)openAddressBookView;


/**
 密友列表：监测密友状态是否已经激活
 chainState = YES，已经激活
 chainState = NO，没有激活
 
 @param chainState chainState，BOOL值
 */
- (void)checkChainState:(void(^)(BOOL state))chainState;

#pragma mark - =================== 分享相关API ===================

/*
 *  初始化QQ
 */
- (void)initWithQQkey:(NSString *)key universalLink:(NSString *)universalLink;
/*
 *  初始化微信
 */
- (void)initWithWechatKey:(NSString *)key universalLink:(NSString *)universalLink;
/**
 *  是否安装QQ
 */
- (BOOL)isQQInstalled;
/**
 *  是否安装微信
 */
- (BOOL)isWXAppInstalled;
/*
 * 有分享界⾯面 - shareModels为分享数据模型，包含分享链接、描述、分享类型等必要参数
 * success成功回调 failure失败回调
 */
- (void)showShareViewWithShareDataArray:(NSArray *)shareModels
                                success:(void(^)(NSString *successMessage,HeroUShareStatus status,HeroUShareTaget target))success
                                failure:(void(^)(NSString *failureMessage,HeroUShareStatus status,HeroUShareTaget target))failure;
/*
 * 分享--⽆分享界⾯面
 * shareModel为分享数据模型，包含分享链接、描述、分享类型等必要参数
 * success成功回调 failure失败回调 */
- (void)shareWithShareData:(HeroUShareModel *)shareModel
                   success:(void(^)(NSString *successMessage,HeroUShareStatus status,HeroUShareTaget target))success
                   failure:(void(^)(NSString *failureMessage,HeroUShareStatus status,HeroUShareTaget target))failure;

/**
 三方登录回调
 */
- (BOOL)heroU_application:(UIApplication *)application
      handleOpenURL:(NSURL *)url ;

/*
*  三方登录返回来的回调
*/
- (BOOL)handleOpenUniversalLink:(NSUserActivity *)userActivity ;

@end

NS_ASSUME_NONNULL_END
