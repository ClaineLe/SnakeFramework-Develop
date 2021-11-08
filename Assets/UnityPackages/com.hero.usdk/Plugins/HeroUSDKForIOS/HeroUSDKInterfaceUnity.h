//
//
//

#ifndef _HEROUSDKINTERFACE_UNITY_H_
#define _HEROUSDKINTERFACE_UNITY_H_


#if defined(__cplusplus)
extern "C"{
#endif
    //初始化
    extern void game_nativeSetListener(const char *gameObjectName);

    //初始化SDK
    extern void game_initWithHeroUSDK(const char *usdkProductId, const char *usdkProductKey);

    //登录
    extern void game_enterLoginView();

    //退出登录
    extern void game_logout();

    //退出登录并显示账号历史界面
    extern void game_logoutAndSowLoginView();

    //用户中心
    extern void game_showUserCenter();

    //清理本地账号
    extern void game_cleanUserEntities();

    //用户名
    extern const char *game_getUserName();

    //用户ID
    extern const char *game_getUserId();

    //SDKID
    extern const char *game_getSdkId();

    //设备号
    extern const char *game_getDeviceNum();

    //内购
    extern void game_iapPurchaseWithData(const char *gamePropID, const char *gameRole, const char *cpOrder, const char *callbackUrl);

    //基础参数
    extern void game_setBaseRoleInfoWithData(const char *channelUserId, const char *gameUserId, const char *serverId, const char *serverName, const char *roleId,const char *roleName,const char *roleAvatar);

    //角色登录
    extern void game_roleLoginWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden);

    //角色注册
    extern void game_roleRegisterWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden);

    //角色升级
    extern void game_roleLevelUpWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden);

    //分享
    extern void game_share(bool hasUi,const char *title,const char *content,const char *imagePath,const char *url,const char *shareTo) ;
    
    //上报闪屏
    extern void game_postSplashScreenEndSuccess();

    //扫码登录
    extern void game_showScanViewWithExt(const char *exit);

    //收到踢下线结果回调SDK
    extern void game_cpKickOffCallBackWithResult(const char *result);

    //打开客服
    extern void game_openServiceView();

    //获取协议内容
    extern const char *game_getProtocolResult() ;

    //同意协议
    extern void game_setAgreeProtocol();
    
#if defined(__cplusplus)
}
#endif

#endif
