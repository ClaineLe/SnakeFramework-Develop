//
//
//

#import <Foundation/Foundation.h>
#import "HeroUSDKInterfaceUnity.h"
#import "HeroUSDK_ios.h"

#if defined(__cplusplus)
extern "C"{
#endif
    
    void game_nativeSetListener(const char *gameObjectName)
    {
        [[HeroUSDK_ios shareInstance] setListener:[NSString stringWithUTF8String:gameObjectName]];
    }
    //初始化SDK
    void game_initWithHeroUSDK(const char *usdkProductId, const char *usdkProductKey)
    {
        
        NSString *usdkProductIdString = [NSString stringWithCString:usdkProductId encoding:NSUTF8StringEncoding];
        NSString *usdkProductKeyString = [NSString stringWithCString:usdkProductKey encoding:NSUTF8StringEncoding];
        [[HeroUSDK_ios shareInstance] initWithUSDKKey:usdkProductKeyString productID:usdkProductIdString];
    }
    //登录
    void game_enterLoginView()
    {
        [[HeroUSDK_ios shareInstance] enterLoginView];
    }
    //退出登录
    void game_logout()
    {
        [[HeroUSDK_ios shareInstance] logout];
    }
    //退出登录并显示账号历史界面
    void game_logoutAndSowLoginView()
    {
        [[HeroUSDK_ios shareInstance] logoutAndSowLoginView];
    }
    //用户中心
    void game_showUserCenter()
    {
        [[HeroUSDK_ios shareInstance] showUserCenter];
    }
    //清理本地账号
    void game_cleanUserEntities()
    {
        [[HeroUSDK_ios shareInstance] cleanUserEntities];
    }
    //用户名
    const char *game_getUserName()
    {
         NSString* sl = [[HeroUSDK_ios shareInstance] getUserName] ;
         char* ret = nullptr;
         ret = (char*) malloc([sl length] + 1);
         memcpy(ret,[sl UTF8String],([sl length] + 1));
         return ret;
    }
    //用户ID
    const char *game_getUserId()
    {
        NSString* sl = [[HeroUSDK_ios shareInstance] getUserId] ;
        char* ret = nullptr;
        ret = (char*) malloc([sl length] + 1);
        memcpy(ret,[sl UTF8String],([sl length] + 1));
        return ret;
    }
    //SDKID
    const char *game_getSdkId()
    {
        NSString* sl = [[HeroUSDK_ios shareInstance] getSdkId] ;
        char* ret = nullptr;
        ret = (char*) malloc([sl length] + 1);
        memcpy(ret,[sl UTF8String],([sl length] + 1));
        return ret;
    }
    //设备号
    const char *game_getDeviceNum()
    {
        NSString* sl = [[HeroUSDK_ios shareInstance] getDeviceNum] ;
        char* ret = nullptr;
        ret = (char*) malloc([sl length] + 1);
        memcpy(ret,[sl UTF8String],([sl length] + 1));
        return ret;
    }
    //内购
    void game_iapPurchaseWithData(const char *gamePropID, const char *gameRole, const char *cpOrder, const char *callbackUrl)
    {
        GamePaymentParameters *parameters = [[GamePaymentParameters alloc] init];
        parameters.gamePropID = [NSString stringWithUTF8String:gamePropID] ;
        parameters.gameRole = [NSString stringWithUTF8String:gameRole] ;
        parameters.cpOrder = [NSString stringWithUTF8String:cpOrder] ;
        parameters.callbackUrl = [NSString stringWithUTF8String:callbackUrl] ;
        [[HeroUSDK_ios shareInstance] iapPurchaseWithData:parameters] ;
    }
    //基础参数
    void game_setBaseRoleInfoWithData(const char *channelUserId, const char *gameUserId, const char *serverId, const char *serverName,
                              const char *roleId,const char *roleName,const char *roleAvatar)
    {
        HeroHDCBaseGameRoleInfo *baseRoleInfo = [HeroHDCBaseGameRoleInfo sharedInstance];
        baseRoleInfo.channelUserId = [NSString stringWithUTF8String:channelUserId] ;
        baseRoleInfo.gameUserId = [NSString stringWithUTF8String:gameUserId] ;
        baseRoleInfo.serverId = [NSString stringWithUTF8String:serverId] ;
        baseRoleInfo.serverName = [NSString stringWithUTF8String:serverName] ;
        baseRoleInfo.roleId = [NSString stringWithUTF8String:roleId] ;
        baseRoleInfo.roleName = [NSString stringWithUTF8String:roleName] ;
        baseRoleInfo.roleAvatar = [NSString stringWithUTF8String:roleAvatar] ;
        [[HeroUSDK_ios shareInstance] setBaseRoleInfoWithData:baseRoleInfo];
    }
    //角色登录
    void game_roleLoginWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden)
    {
        HeroHDCGameRoleInfo *roleInfo = [[HeroHDCGameRoleInfo alloc] init];
        roleInfo.level = [NSString stringWithUTF8String:level] ;
        roleInfo.vipLevel = [NSString stringWithUTF8String:vipLevel] ;
        roleInfo.gold1 = [NSString stringWithUTF8String:gold1] ;
        roleInfo.gold2 = [NSString stringWithUTF8String:gold2] ;
        roleInfo.sumPay = [NSString stringWithUTF8String:sumPay] ;
        roleInfo.levelExp = [NSString stringWithUTF8String:levelExp] ;
        roleInfo.vipScore = [NSString stringWithUTF8String:vipScore] ;
        roleInfo.rankLevel = [NSString stringWithUTF8String:rankLevel] ;
        roleInfo.rankExp = [NSString stringWithUTF8String:rankExp] ;
        roleInfo.rankLeve2 = [NSString stringWithUTF8String:rankLeve2] ;
        roleInfo.rankExp2 = [NSString stringWithUTF8String:rankExp2] ;
        roleInfo.cupCount1 = [NSString stringWithUTF8String:cupCount1];
        roleInfo.cupCount2 = [NSString stringWithUTF8String:cupCount2] ;
        roleInfo.totalKill = [NSString stringWithUTF8String:totalKill] ;
        roleInfo.totalHead = [NSString stringWithUTF8String:totalHead] ;
        roleInfo.avgKD = [NSString stringWithUTF8String:avgKD] ;
        roleInfo.maxKD = [NSString stringWithUTF8String:maxKD] ;
        roleInfo.maxCK = [NSString stringWithUTF8String:maxCK] ;
        roleInfo.mainWeaponId = [NSString stringWithUTF8String:mainWeaponId] ;
        roleInfo.viceWeaponId = [NSString stringWithUTF8String:viceWeaponId];
        roleInfo.teamId = [NSString stringWithUTF8String:teamId];
        roleInfo.teamName = [NSString stringWithUTF8String:teamName];
        roleInfo.floatHidden = floatHidden ;
        [[HeroUSDK_ios shareInstance] roleLoginWithGameRoleInfo:roleInfo];
    }
    //角色注册
    void game_roleRegisterWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden)
    {
       HeroHDCGameRoleInfo *roleInfo = [[HeroHDCGameRoleInfo alloc] init];
       roleInfo.level = [NSString stringWithUTF8String:level] ;
       roleInfo.vipLevel = [NSString stringWithUTF8String:vipLevel] ;
       roleInfo.gold1 = [NSString stringWithUTF8String:gold1] ;
       roleInfo.gold2 = [NSString stringWithUTF8String:gold2] ;
       roleInfo.sumPay = [NSString stringWithUTF8String:sumPay] ;
       roleInfo.levelExp = [NSString stringWithUTF8String:levelExp] ;
       roleInfo.vipScore = [NSString stringWithUTF8String:vipScore] ;
       roleInfo.rankLevel = [NSString stringWithUTF8String:rankLevel] ;
       roleInfo.rankExp = [NSString stringWithUTF8String:rankExp] ;
       roleInfo.rankLeve2 = [NSString stringWithUTF8String:rankLeve2] ;
       roleInfo.rankExp2 = [NSString stringWithUTF8String:rankExp2] ;
       roleInfo.cupCount1 = [NSString stringWithUTF8String:cupCount1];
       roleInfo.cupCount2 = [NSString stringWithUTF8String:cupCount2] ;
       roleInfo.totalKill = [NSString stringWithUTF8String:totalKill] ;
       roleInfo.totalHead = [NSString stringWithUTF8String:totalHead] ;
       roleInfo.avgKD = [NSString stringWithUTF8String:avgKD] ;
       roleInfo.maxKD = [NSString stringWithUTF8String:maxKD] ;
       roleInfo.maxCK = [NSString stringWithUTF8String:maxCK] ;
       roleInfo.mainWeaponId = [NSString stringWithUTF8String:mainWeaponId] ;
       roleInfo.viceWeaponId = [NSString stringWithUTF8String:viceWeaponId];
       roleInfo.teamId = [NSString stringWithUTF8String:teamId];
       roleInfo.teamName = [NSString stringWithUTF8String:teamName];
       roleInfo.floatHidden = floatHidden ;
       [[HeroUSDK_ios shareInstance] roleRegisterWithGameRoleInfo:roleInfo];
    }
    //角色升级
    void game_roleLevelUpWithGameRoleInfo(const char *level, const char *vipLevel, const char *gold1, const char *gold2,
                          const char *sumPay, const char *levelExp, const char *vipScore,const char *rankLevel,
                          const char *rankExp, const char *rankLeve2,const char *rankExp2,const char *cupCount1,
                          const char *cupCount2,const char *totalKill,const char *totalHead,const char *avgKD,
                          const char *maxKD,const char *maxCK,const char *mainWeaponId,const char *viceWeaponId,
                          const char *teamId,const char *teamName,BOOL floatHidden)
    {
       HeroHDCGameRoleInfo *roleInfo = [[HeroHDCGameRoleInfo alloc] init];
       roleInfo.level = [NSString stringWithUTF8String:level] ;
       roleInfo.vipLevel = [NSString stringWithUTF8String:vipLevel] ;
       roleInfo.gold1 = [NSString stringWithUTF8String:gold1] ;
       roleInfo.gold2 = [NSString stringWithUTF8String:gold2] ;
       roleInfo.sumPay = [NSString stringWithUTF8String:sumPay] ;
       roleInfo.levelExp = [NSString stringWithUTF8String:levelExp] ;
       roleInfo.vipScore = [NSString stringWithUTF8String:vipScore] ;
       roleInfo.rankLevel = [NSString stringWithUTF8String:rankLevel] ;
       roleInfo.rankExp = [NSString stringWithUTF8String:rankExp] ;
       roleInfo.rankLeve2 = [NSString stringWithUTF8String:rankLeve2] ;
       roleInfo.rankExp2 = [NSString stringWithUTF8String:rankExp2] ;
       roleInfo.cupCount1 = [NSString stringWithUTF8String:cupCount1];
       roleInfo.cupCount2 = [NSString stringWithUTF8String:cupCount2] ;
       roleInfo.totalKill = [NSString stringWithUTF8String:totalKill] ;
       roleInfo.totalHead = [NSString stringWithUTF8String:totalHead] ;
       roleInfo.avgKD = [NSString stringWithUTF8String:avgKD] ;
       roleInfo.maxKD = [NSString stringWithUTF8String:maxKD] ;
       roleInfo.maxCK = [NSString stringWithUTF8String:maxCK] ;
       roleInfo.mainWeaponId = [NSString stringWithUTF8String:mainWeaponId] ;
       roleInfo.viceWeaponId = [NSString stringWithUTF8String:viceWeaponId];
       roleInfo.teamId = [NSString stringWithUTF8String:teamId];
       roleInfo.teamName = [NSString stringWithUTF8String:teamName];
       roleInfo.floatHidden = floatHidden ;
       [[HeroUSDK_ios shareInstance] roleLevelUpWithGameRoleInfo:roleInfo];
    }
    //分享
    void game_share(bool hasUi,const char *title,const char *content,const char *imagePath,const char *url,const char *shareTo)
    {
        NSString *ctitle = [NSString stringWithUTF8String:title] ;
        NSString *ccontent = [NSString stringWithUTF8String:content] ;
        NSString *cimagePath = [NSString stringWithUTF8String:imagePath] ;
        NSString *curl = [NSString stringWithUTF8String:url] ;
        NSString *cshareTo = [NSString stringWithUTF8String:shareTo] ;
        //分享
        [[HeroUSDK_ios shareInstance] share:hasUi title:ctitle content:ccontent imagePath:cimagePath url:curl shareTo:cshareTo] ;
    }

    //上报闪屏
    void game_postSplashScreenEndSuccess()
    {
        [[HeroUSDK_ios shareInstance] postSplashScreenEndSuccess];
    }
    //扫码登录
    void game_showScanViewWithExt(const char *exit)
    {
        [[HeroUSDK_ios shareInstance] showScanViewWithExt:[NSString stringWithUTF8String:exit]];
    }
    //收到踢下线结果回调SDK
    void game_cpKickOffCallBackWithResult(const char *result)
    {
        [[HeroUSDK_ios shareInstance] cpKickOffCallBackWithResult:[NSString stringWithUTF8String:result]];
    }
    //打开客服
    void game_openServiceView()
    {
        [[HeroUSDK_ios shareInstance] openServiceView] ;
    }
    //获取协议内容
    const char *game_getProtocolResult()
    {
        NSString* sl = [[HeroUSDK_ios shareInstance] getProtocolResult] ;
        char* ret = nullptr;
        ret = (char*) malloc([sl length] + 1);
        memcpy(ret,[sl UTF8String],([sl length] + 1));
        return ret;
    }
    //同意协议
    void game_setAgreeProtocol()
    {
        [[HeroUSDK_ios shareInstance] setAgreeProtocol] ;
    }
    
#if defined(__cplusplus)
}
#endif
