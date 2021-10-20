using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace herousdk
{

    public enum HeroUSDKResult
    {
        HeroUSDKResultSuccess = 0, //成功
        HeroUSDKResultFailed = -1, //失败
        HeroUSDKResultCancel = -2 , //取消
    }
    public enum HeroUSDKAdResult
    {
        HeroUSDKAdResultClicked = 0, //广告点击
        HeroUSDKAdResultClosed  = 1, //关闭广告
        HeroUSDKAdResultPlayComplete = 2, //广告播放完成
        HeroUSDKAdResultPlayFailed = 3, //广告播放失败
    }
    /*
    * SDK初始化参数
    */
    public class HeroUProject
    {
        public string usdkProductId;
        public string usdkProductKey;
    }
    /*
    * SDK内购参数
    */
    public class HeroUPaymentParameters {

	    public string goodsId;
	    public string extraParams;
	    public string cpOrder ;
	    public string callbackUrl ;
    }
    /*
    * 订单数据
    */
    public class HeroUPaymentOrder {
        public string plat;

        /*
         * iOS
         * **/
        public string orderId ;
        public string orderAmount ;
        public string currency ;

        /*
         * Android
         * **/
        public string sdkOrderId;
        public string cpOrderId;
        public string extraParams;

        public string errorMsg ;
    }
    /*
     * 角色参数
     */
    public class HeroHDCGameRoleInfo {
        public string channelUserId;
        public string gameUserId;
        public string serverId;
        public string serverName;
        public string roleId;
        public string roleName;
        public string roleAvatar;

        public string level ;
        public string vipLevel ;
        public string gold1 ;
        public string gold2 ;
        public string sumPay ;
        public string levelExp ;
        public string vipScore ;
        public string rankLevel ;
        public string rankExp ;
        public string rankLeve2 ;
        public string rankExp2 ;
        public string cupCount1 ;
        public string cupCount2 ;
        public string totalKill ;
        public string totalHead ;
        public string avgKD ;
        public string maxKD ;
        public string maxCK ;
        public string mainWeaponId ;
        public string viceWeaponId ;
        public string medalCount ;
        public string teamId ;
        public string teamName ;
        public bool floatHidden ;

        //android
        public string partyName;
        public string roleCreateTime;
        public long balanceLevelOne;
        public long balanceLevelTwo;

        //android 360渠道独有
        public string partyId;
        public string roleGender;
        public string rolePower;
        public string partyRoleId;
        public string partyRoleName;
        public string professionId;
        public string profession;
        public string friendList;
        
    }
	
	// 用户信息，登录回调中使用
	public class UserInfo 
	{
        public string plat;

        /*
         * iOS回调
         * **/
		public string accessCode;
		public string accessToken;
		public string sdkUserId;
        public string userName ;

        /*
         * android回调
         * **/
        public string channelToken;
        public string extendParams;
        public string isFristLogin;
        public string serverMessage;
        public string token;
        public string uid;
    }
    //分享
    public class ShareInfo {
        public bool hasUi; //true-有UI的分享，false-无UI的分享
        public String title;   //分享标题
        public String content;   //分享内容
        public String imgPath;   //分享图片本地地址
        public String imgUrl;   //分享图片网络地址
        public String url;   //分享链接
        public String shareTo;   //分享到哪里 [1-微信好友，2-微信朋友圈，3-QQ，4-QQ空间]
        public String extenal;   //额外备注
    }

    public class HeroUSDKImp {

    	public static HeroUSDKImp _instance;

    	public static HeroUSDKImp getInstance() {
   			if( null == _instance ) {
   				_instance = new HeroUSDKImp();
   			}
   			return _instance;
   		}
        /*
        * 初始化【处理通知】
        */
        public void setListener(HeroUSDKListener listener)
		{

            string gameObjectName = listener.gameObject.name;

#if UNITY_IOS && !UNITY_EDITOR

			    
		     game_nativeSetListener(gameObjectName);

#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.setListener(gameObjectName);

#endif
        }
        /*
        * 初始化SDK
        */
        public void initWithHeroUSDK(HeroUProject project) {
        
            string usdkProductId = String.IsNullOrEmpty(project.usdkProductId) ? "" : project.usdkProductId;
            string usdkProductKey = String.IsNullOrEmpty(project.usdkProductKey) ? "" : project.usdkProductKey;
            
            Debug.Log (" ===========> usdkProductId :" + usdkProductId + " <=========");
            Debug.Log (" ===========> usdkProductKey :" + usdkProductKey + " <=========");
          

#if UNITY_IOS && !UNITY_EDITOR

			    game_initWithHeroUSDK(usdkProductId,usdkProductKey);

#elif UNITY_ANDROID && !UNITY_EDITOR

                HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
                androidSupport.requestUsdkInit(project.usdkProductId,project.usdkProductKey);
#endif

        }

        /*
        * =================== 登录支付相关API ===================
        */
        /*
         * 登录
         * **/
        public void login()
        {

#if UNITY_IOS && !UNITY_EDITOR
             //登录
             game_enterLoginView();

#elif UNITY_ANDROID && !UNITY_EDITOR

             HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
             androidSupport.requestUsdkEnterLoginView();

#endif
        }
        /*
   		* 进入游戏
   		*/
        public void enterGame(HeroHDCGameRoleInfo roleInfo) {

#if UNITY_IOS && !UNITY_EDITOR
             //上报数据
             roleLoginWithGameRoleInfo(roleInfo);

#elif UNITY_ANDROID && !UNITY_EDITOR

             HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
             androidSupport.requestUsdkEnterGame(roleInfo);
#endif
        }
        /*
        * 角色注册
        */
        public void createNewRole(HeroHDCGameRoleInfo roleInfo)
        {

            string channelUserId = String.IsNullOrEmpty(roleInfo.channelUserId) ? "" : roleInfo.channelUserId;
            string gameUserId = String.IsNullOrEmpty(roleInfo.gameUserId) ? "" : roleInfo.gameUserId;
            string serverId = String.IsNullOrEmpty(roleInfo.serverId) ? "" : roleInfo.serverId;
            string serverName = String.IsNullOrEmpty(roleInfo.serverName) ? "" : roleInfo.serverName;
            string roleId = String.IsNullOrEmpty(roleInfo.roleId) ? "" : roleInfo.roleId;
            string roleName = String.IsNullOrEmpty(roleInfo.roleName) ? "" : roleInfo.roleName;
            string roleAvatar = String.IsNullOrEmpty(roleInfo.roleAvatar) ? "" : roleInfo.roleAvatar;


            string level = String.IsNullOrEmpty(roleInfo.level) ? "" : roleInfo.level;
            string vipLevel = String.IsNullOrEmpty(roleInfo.vipLevel) ? "" : roleInfo.vipLevel;
            string gold1 = String.IsNullOrEmpty(roleInfo.gold1) ? "" : roleInfo.gold1;
            string gold2 = String.IsNullOrEmpty(roleInfo.gold2) ? "" : roleInfo.gold2;
            string sumPay = String.IsNullOrEmpty(roleInfo.sumPay) ? "" : roleInfo.sumPay;
            string levelExp = String.IsNullOrEmpty(roleInfo.levelExp) ? "" : roleInfo.levelExp;
            string vipScore = String.IsNullOrEmpty(roleInfo.vipScore) ? "" : roleInfo.vipScore;
            string rankLevel = String.IsNullOrEmpty(roleInfo.rankLevel) ? "" : roleInfo.rankLevel;
            string rankExp = String.IsNullOrEmpty(roleInfo.rankExp) ? "" : roleInfo.rankExp;
            string rankLeve2 = String.IsNullOrEmpty(roleInfo.rankLeve2) ? "" : roleInfo.rankLeve2;
            string rankExp2 = String.IsNullOrEmpty(roleInfo.rankExp2) ? "" : roleInfo.rankExp2;
            string cupCount1 = String.IsNullOrEmpty(roleInfo.cupCount1) ? "" : roleInfo.cupCount1;
            string cupCount2 = String.IsNullOrEmpty(roleInfo.cupCount2) ? "" : roleInfo.cupCount2;
            string totalKill = String.IsNullOrEmpty(roleInfo.totalKill) ? "" : roleInfo.totalKill;
            string totalHead = String.IsNullOrEmpty(roleInfo.totalHead) ? "" : roleInfo.totalHead;
            string avgKD = String.IsNullOrEmpty(roleInfo.avgKD) ? "" : roleInfo.avgKD;
            string maxKD = String.IsNullOrEmpty(roleInfo.maxKD) ? "" : roleInfo.maxKD;
            string maxCK = String.IsNullOrEmpty(roleInfo.maxCK) ? "" : roleInfo.maxCK;
            string mainWeaponId = String.IsNullOrEmpty(roleInfo.mainWeaponId) ? "" : roleInfo.mainWeaponId;
            string viceWeaponId = String.IsNullOrEmpty(roleInfo.viceWeaponId) ? "" : roleInfo.viceWeaponId;
            string teamId = String.IsNullOrEmpty(roleInfo.teamId) ? "" : roleInfo.teamId;
            string teamName = String.IsNullOrEmpty(roleInfo.teamName) ? "" : roleInfo.teamName;



#if UNITY_IOS && !UNITY_EDITOR

            //设置基础数据
            game_setBaseRoleInfoWithData(channelUserId,gameUserId,serverId,serverName,roleId,roleName,roleAvatar);
            //上报角色登录
            game_roleRegisterWithGameRoleInfo(level,vipLevel,gold1,gold2,sumPay,levelExp,
                                        vipScore,rankLevel,rankExp,rankLeve2,rankExp2,cupCount1,
                                        cupCount2,totalKill,totalHead,avgKD,maxKD,maxCK,mainWeaponId,
                                        viceWeaponId,teamId,teamName,roleInfo.floatHidden);

#elif UNITY_ANDROID && !UNITY_EDITOR

             HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkCreateRole(roleInfo);
#endif
        }

        /*
        * 角色升级
        */
        public void roleLevelUp(HeroHDCGameRoleInfo roleInfo)
        {

            string channelUserId = String.IsNullOrEmpty(roleInfo.channelUserId) ? "" : roleInfo.channelUserId;
            string gameUserId = String.IsNullOrEmpty(roleInfo.gameUserId) ? "" : roleInfo.gameUserId;
            string serverId = String.IsNullOrEmpty(roleInfo.serverId) ? "" : roleInfo.serverId;
            string serverName = String.IsNullOrEmpty(roleInfo.serverName) ? "" : roleInfo.serverName;
            string roleId = String.IsNullOrEmpty(roleInfo.roleId) ? "" : roleInfo.roleId;
            string roleName = String.IsNullOrEmpty(roleInfo.roleName) ? "" : roleInfo.roleName;
            string roleAvatar = String.IsNullOrEmpty(roleInfo.roleAvatar) ? "" : roleInfo.roleAvatar;

            string level = String.IsNullOrEmpty(roleInfo.level) ? "" : roleInfo.level;
            string vipLevel = String.IsNullOrEmpty(roleInfo.vipLevel) ? "" : roleInfo.vipLevel;
            string gold1 = String.IsNullOrEmpty(roleInfo.gold1) ? "" : roleInfo.gold1;
            string gold2 = String.IsNullOrEmpty(roleInfo.gold2) ? "" : roleInfo.gold2;
            string sumPay = String.IsNullOrEmpty(roleInfo.sumPay) ? "" : roleInfo.sumPay;
            string levelExp = String.IsNullOrEmpty(roleInfo.levelExp) ? "" : roleInfo.levelExp;
            string vipScore = String.IsNullOrEmpty(roleInfo.vipScore) ? "" : roleInfo.vipScore;
            string rankLevel = String.IsNullOrEmpty(roleInfo.rankLevel) ? "" : roleInfo.rankLevel;
            string rankExp = String.IsNullOrEmpty(roleInfo.rankExp) ? "" : roleInfo.rankExp;
            string rankLeve2 = String.IsNullOrEmpty(roleInfo.rankLeve2) ? "" : roleInfo.rankLeve2;
            string rankExp2 = String.IsNullOrEmpty(roleInfo.rankExp2) ? "" : roleInfo.rankExp2;
            string cupCount1 = String.IsNullOrEmpty(roleInfo.cupCount1) ? "" : roleInfo.cupCount1;
            string cupCount2 = String.IsNullOrEmpty(roleInfo.cupCount2) ? "" : roleInfo.cupCount2;
            string totalKill = String.IsNullOrEmpty(roleInfo.totalKill) ? "" : roleInfo.totalKill;
            string totalHead = String.IsNullOrEmpty(roleInfo.totalHead) ? "" : roleInfo.totalHead;
            string avgKD = String.IsNullOrEmpty(roleInfo.avgKD) ? "" : roleInfo.avgKD;
            string maxKD = String.IsNullOrEmpty(roleInfo.maxKD) ? "" : roleInfo.maxKD;
            string maxCK = String.IsNullOrEmpty(roleInfo.maxCK) ? "" : roleInfo.maxCK;
            string mainWeaponId = String.IsNullOrEmpty(roleInfo.mainWeaponId) ? "" : roleInfo.mainWeaponId;
            string viceWeaponId = String.IsNullOrEmpty(roleInfo.viceWeaponId) ? "" : roleInfo.viceWeaponId;
            string teamId = String.IsNullOrEmpty(roleInfo.teamId) ? "" : roleInfo.teamId;
            string teamName = String.IsNullOrEmpty(roleInfo.teamName) ? "" : roleInfo.teamName;

#if UNITY_IOS && !UNITY_EDITOR

                //设置基础参数
                game_setBaseRoleInfoWithData(channelUserId,gameUserId,serverId,serverName,roleId,roleName,roleAvatar);

                //上报角色升级
                game_roleLevelUpWithGameRoleInfo(level,vipLevel,gold1,gold2,sumPay,levelExp,
                                        vipScore,rankLevel,rankExp,rankLeve2,rankExp2,cupCount1,
                                        cupCount2,totalKill,totalHead,avgKD,maxKD,maxCK,mainWeaponId,
                                        viceWeaponId,teamId,teamName,roleInfo.floatHidden);

#elif UNITY_ANDROID && !UNITY_EDITOR

             
            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkLevelUp(roleInfo);

#endif
        }

        /*
   		* 注销、退出登陆
   		*/
        public void logout() {
            
#if UNITY_IOS && !UNITY_EDITOR

            game_logoutAndSowLoginView();

#elif UNITY_ANDROID && !UNITY_EDITOR

             
            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkLogout();
#endif
        }
        /*
   		* 获取登录用户名
   		*/
        public string getUserName()
        {
#if UNITY_IOS && !UNITY_EDITOR
                return game_getUserName();
#else
            return "";
#endif
        }
        /*
   		* 获取登录用户ID
   		*/
        public string getUserId()
        {
#if UNITY_IOS && !UNITY_EDITOR
                return game_getUserId();
#else
            return "";
#endif
        }
        /*
        * 获取ID（部分游戏使用@"id"字段作为唯一标示符）
        */
        public string getSdkId()
        {
#if UNITY_IOS && !UNITY_EDITOR
                return game_getSdkId();
#else
            return "";
#endif
        }
        /*
        * 获取设备号(优先取的IDFA、没取到则取的UUID)
        */
        public string getDeviceNum()
        {
#if UNITY_IOS && !UNITY_EDITOR
                return game_getDeviceNum();
#else
            return "";
#endif
        }

        /*
		 * 渠道ID
		 * **/
        public int getChannelId()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkGetChannelId();
#endif
            return 0;
        }
        /*
		 * 渠道名称
		 * **/
        public string getChannelName()
        {

#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkGetChannelName();
#endif
            return "";
        }
        /*
		 * 获取渠道SDK的版本名
		 * **/
        public string getChannelSdkVersionName()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkGetChannelSdkVersionName();
#endif
            return "";
        }
        /*
		 * 判断当前渠道在调用退出接口时是否会弹出退出框
		 * **/
        public bool isChannelHasExitDialog()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkisChannelHasExitDialog();
#endif
            return false ;
        }
        /*
		 * 获取英雄官网渠道的ProjectId，常用于游戏需要CPS分包时
		 * **/
        public string getProjectId()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkGetProjectId();
#endif
            return "";
        }
        /*
		 * 获取用户在HeroUSDK后台配置的自定义参数值。
		 * **/
        public string getCustomParams(string key)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkGetCustomParams(key);
#endif
            return "";
        }
        /*
		 * 调用渠道的扩展方法(如显示/隐藏悬浮框，防沉迷查询，进入用户中心等)。
		 * **/
        public bool callExtendApi(int extendType)
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            return androidSupport.requestUsdkcallExtendApi(extendType);
#endif
            return false ;
        }
        /*
		 * 获取当前设备的OAID值
		 * **/
        public void getOAID()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkGetOAID();
#endif
        }

        /*
        * 调用SDKIAP内购
        */
        public void pay(HeroUPaymentParameters paymentParametersData, HeroHDCGameRoleInfo roleInfo)
        {
            string goodsId = String.IsNullOrEmpty(paymentParametersData.goodsId) ? "" : paymentParametersData.goodsId;
            string extraParams = String.IsNullOrEmpty(paymentParametersData.extraParams) ? "" : paymentParametersData.extraParams;
            string cpOrder = String.IsNullOrEmpty(paymentParametersData.cpOrder) ? "" : paymentParametersData.cpOrder;
            string callbackUrl = String.IsNullOrEmpty(paymentParametersData.callbackUrl) ? "" : paymentParametersData.callbackUrl;

            Debug.Log(" ===========> goodsId :" + goodsId + " <=========");
            Debug.Log(" ===========> extraParams :" + extraParams + " <=========");
            Debug.Log(" ===========> cpOrder :" + cpOrder + " <=========");
            Debug.Log(" ===========> callbackUrl :" + callbackUrl + " <=========");


#if UNITY_IOS && !UNITY_EDITOR

                game_iapPurchaseWithData(goodsId,extraParams,cpOrder,callbackUrl);

#elif UNITY_ANDROID && !UNITY_EDITOR

             
                 HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
                androidSupport.requestUsdkPay(roleInfo, paymentParametersData);
#endif
        }

        /*
        * 扫码登录（在PC端登录,SDK需要登录状态）
        */
        public void showScanViewWithExt(string exit)
        {

#if UNITY_IOS && !UNITY_EDITOR

                game_showScanViewWithExt(exit);

#elif UNITY_ANDROID && !UNITY_EDITOR

                HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
                androidSupport.showScanViewWithExt(exit);
#endif
        }

        /*
        * 收到踢下线结果回调SDK
        */
        public void cpKickOffCallBackWithResult(string result)
        {

#if UNITY_IOS && !UNITY_EDITOR

            game_cpKickOffCallBackWithResult(result);

#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkNotifyKickResult(result);  
#endif
        }

        /*
   		* 注销、退出登陆并显示账号历史界面
   		*/
        public void logoutAndSowLoginView() {
#if UNITY_IOS && !UNITY_EDITOR
                game_logoutAndSowLoginView();
#endif
   		}
   		/*
   		* 打开国内SDK用户中心
   		*/
   		public void showUserCenter() {
#if UNITY_IOS && !UNITY_EDITOR
                game_showUserCenter();
#endif
   		}
   		/*
   		* 清空本地用户存储
   		*/
   		public void cleanUserEntities() {
#if UNITY_IOS && !UNITY_EDITOR
                game_cleanUserEntities();
#endif
   		}
   		
        
       
        /*
        * 角色登录
        */
        public void roleLoginWithGameRoleInfo(HeroHDCGameRoleInfo roleInfo) {

            string channelUserId = String.IsNullOrEmpty(roleInfo.channelUserId) ? "" : roleInfo.channelUserId;
            string gameUserId = String.IsNullOrEmpty(roleInfo.gameUserId) ? "" : roleInfo.gameUserId;
            string serverId = String.IsNullOrEmpty(roleInfo.serverId) ? "" : roleInfo.serverId;
            string serverName = String.IsNullOrEmpty(roleInfo.serverName) ? "" : roleInfo.serverName;
            string roleId = String.IsNullOrEmpty(roleInfo.roleId) ? "" : roleInfo.roleId;
            string roleName = String.IsNullOrEmpty(roleInfo.roleName) ? "" : roleInfo.roleName;
            string roleAvatar = String.IsNullOrEmpty(roleInfo.roleAvatar) ? "" : roleInfo.roleAvatar;

            string level = String.IsNullOrEmpty(roleInfo.level) ? "" : roleInfo.level;
            string vipLevel = String.IsNullOrEmpty(roleInfo.vipLevel) ? "" : roleInfo.vipLevel;
            string gold1 = String.IsNullOrEmpty(roleInfo.gold1) ? "" : roleInfo.gold1;
            string gold2 = String.IsNullOrEmpty(roleInfo.gold2) ? "" : roleInfo.gold2;
            string sumPay = String.IsNullOrEmpty(roleInfo.sumPay) ? "" : roleInfo.sumPay;
            string levelExp = String.IsNullOrEmpty(roleInfo.levelExp) ? "" : roleInfo.levelExp;
            string vipScore = String.IsNullOrEmpty(roleInfo.vipScore) ? "" : roleInfo.vipScore;
            string rankLevel = String.IsNullOrEmpty(roleInfo.rankLevel) ? "" : roleInfo.rankLevel;
            string rankExp = String.IsNullOrEmpty(roleInfo.rankExp) ? "" : roleInfo.rankExp;
            string rankLeve2 = String.IsNullOrEmpty(roleInfo.rankLeve2) ? "" : roleInfo.rankLeve2;
            string rankExp2 = String.IsNullOrEmpty(roleInfo.rankExp2) ? "" : roleInfo.rankExp2;
            string cupCount1 = String.IsNullOrEmpty(roleInfo.cupCount1) ? "" : roleInfo.cupCount1;
            string cupCount2 = String.IsNullOrEmpty(roleInfo.cupCount2) ? "" : roleInfo.cupCount2;
            string totalKill = String.IsNullOrEmpty(roleInfo.totalKill) ? "" : roleInfo.totalKill;
            string totalHead = String.IsNullOrEmpty(roleInfo.totalHead) ? "" : roleInfo.totalHead;
            string avgKD = String.IsNullOrEmpty(roleInfo.avgKD) ? "" : roleInfo.avgKD;
            string maxKD = String.IsNullOrEmpty(roleInfo.maxKD) ? "" : roleInfo.maxKD;
            string maxCK = String.IsNullOrEmpty(roleInfo.maxCK) ? "" : roleInfo.maxCK;
            string mainWeaponId = String.IsNullOrEmpty(roleInfo.mainWeaponId) ? "" : roleInfo.mainWeaponId;
            string viceWeaponId = String.IsNullOrEmpty(roleInfo.viceWeaponId) ? "" : roleInfo.viceWeaponId;
            string teamId = String.IsNullOrEmpty(roleInfo.teamId) ? "" : roleInfo.teamId;
            string teamName = String.IsNullOrEmpty(roleInfo.teamName) ? "" : roleInfo.teamName;

#if UNITY_IOS && !UNITY_EDITOR

                //设置基础参数
                game_setBaseRoleInfoWithData(channelUserId,gameUserId,serverId,serverName,roleId,roleName,roleAvatar);

                //上报角色登录
                game_roleLoginWithGameRoleInfo(level,vipLevel,gold1,gold2,sumPay,levelExp,
                                        vipScore,rankLevel,rankExp,rankLeve2,rankExp2,cupCount1,
                                        cupCount2,totalKill,totalHead,avgKD,maxKD,maxCK,mainWeaponId,
                                        viceWeaponId,teamId,teamName,roleInfo.floatHidden);                                       
#endif
        }

        /*
        * 上报闪屏
        */
        public void postSplashScreenEndSuccess(){
#if UNITY_IOS && !UNITY_EDITOR
            game_postSplashScreenEndSuccess();
#endif
        }
        /*
         * 退出
         * **/
        public void exit() {



#if UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.requestUsdkExit();
#endif
        }

        /*
        * =================== 附加功能 ===================
        */
        public void share(ShareInfo shareInfo)
        {
#if UNITY_IOS && !UNITY_EDITOR

            game_share(shareInfo.hasUi,shareInfo.title,shareInfo.content,shareInfo.imgPath,shareInfo.url,shareInfo.shareTo);

#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.share(shareInfo);
#endif
        }

        //获取协议内容
        public string getProtocolResult()
        {
            #if UNITY_IOS && !UNITY_EDITOR

                        return game_getProtocolResult();

            #elif UNITY_ANDROID && !UNITY_EDITOR

                        HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
                        return androidSupport.getProtocolResult();
            #endif
            return "";
        }
        //点击同意通知
        public void setAgreeProtocol()
        {
            #if UNITY_IOS && !UNITY_EDITOR

                                    game_setAgreeProtocol();

            #elif UNITY_ANDROID && !UNITY_EDITOR

                                    HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
                                    androidSupport.setAgreeProtocol();
            #endif
        }

        /*
		 * banner广告
		 * **/
        public void showAdBanner(string bannerID)
        {
#if UNITY_IOS && !UNITY_EDITOR


#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.showAdBanner(bannerID);
#endif
        }
        /*
		 * 插屏广告
		 * **/
        public void showAdInterstialBanner(string interstialBannerID)
        {
#if UNITY_IOS && !UNITY_EDITOR


#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.showAdInterstialBanner(interstialBannerID);
#endif
        }
        /*
		* 激励视频广告
		* **/
        public void showAdVideo(string videoID)
        {
#if UNITY_IOS && !UNITY_EDITOR


#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.showAdVideo(videoID);
#endif
        }
        /*
		 * 隐藏横幅广告，很少使用
		 * **/
        public void hideAdBanner()
        {
#if UNITY_IOS && !UNITY_EDITOR


#elif UNITY_ANDROID && !UNITY_EDITOR

            HeroUSDKUnitySupportAndroid androidSupport = HeroUSDKUnitySupportAndroid.getInstance();
            androidSupport.hideAdBanner();
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR

        [DllImport("__Internal")]
		private static extern void game_nativeSetListener(string gameObjectName);
		[DllImport("__Internal")]
		private static extern void game_initWithHeroUSDK(string usdkProductId,string usdkProductKey) ;
		[DllImport("__Internal")]
		private static extern void game_enterLoginView();
		[DllImport("__Internal")]
		private static extern void game_logout();
        [DllImport("__Internal")]
		private static extern void game_logoutAndSowLoginView();
        [DllImport("__Internal")]
		private static extern void game_showUserCenter();
        [DllImport("__Internal")]
		private static extern void game_cleanUserEntities();
        [DllImport("__Internal")]
		private static extern string game_getUserName();
        [DllImport("__Internal")]
		private static extern string game_getUserId();
        [DllImport("__Internal")]
		private static extern string game_getSdkId();
        [DllImport("__Internal")]
		private static extern string game_getDeviceNum();
        [DllImport("__Internal")]
		private static extern void game_iapPurchaseWithData(string gamePropID,string gameRole,string cpOrder,string callbackUrl) ;
        [DllImport("__Internal")]
        private static extern void game_setBaseRoleInfoWithData(string channelUserId,string gameUserId,string serverId,
                                        string serverName,string roleId,string roleName, string roleAvatar);
        [DllImport("__Internal")]
        private static extern void game_roleLoginWithGameRoleInfo(string level,string vipLevel,string gold1,
                                        string gold2,string sumPay,string levelExp,
                                        string vipScore,string rankLevel,string rankExp,
                                        string rankLeve2,string rankExp2,string cupCount1,
                                        string cupCount2,string totalKill,string totalHead,
                                        string avgKD,string maxKD,string maxCK,string mainWeaponId,
                                        string viceWeaponId,string teamId,
                                        string teamName,bool floatHidden);
        [DllImport("__Internal")]
        private static extern void game_roleRegisterWithGameRoleInfo(string level,string vipLevel,string gold1,
                                        string gold2,string sumPay,string levelExp,
                                        string vipScore,string rankLevel,string rankExp,
                                        string rankLeve2,string rankExp2,string cupCount1,
                                        string cupCount2,string totalKill,string totalHead,
                                        string avgKD,string maxKD,string maxCK,string mainWeaponId,
                                        string viceWeaponId,string teamId,
                                        string teamName,bool floatHidden);
        [DllImport("__Internal")]
        private static extern void game_roleLevelUpWithGameRoleInfo(string level,string vipLevel,string gold1,
                                        string gold2,string sumPay,string levelExp,
                                        string vipScore,string rankLevel,string rankExp,
                                        string rankLeve2,string rankExp2,string cupCount1,
                                        string cupCount2,string totalKill,string totalHead,
                                        string avgKD,string maxKD,string maxCK,string mainWeaponId,
                                        string viceWeaponId,string teamId,
                                        string teamName,bool floatHidden);

        [DllImport("__Internal")]
        private static extern void game_share(bool hasUi,string title,string content,string imagePath,string url,string shareTo);

        [DllImport("__Internal")]                                        
        private static extern void game_postSplashScreenEndSuccess();                                                                                                                                               
        [DllImport("__Internal")]
		private static extern void game_showScanViewWithExt(string exit);
        [DllImport("__Internal")]
        private static extern void game_cpKickOffCallBackWithResult(string result) ;
        [DllImport("__Internal")]
        private static extern string game_getProtocolResult() ;
        [DllImport("__Internal")]
        private static extern void game_setAgreeProtocol() ;
        
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    public class HeroUSDKUnitySupportAndroid {

        AndroidJavaObject ao;
        private static HeroUSDKUnitySupportAndroid instance;

        private HeroUSDKUnitySupportAndroid()
        {
            AndroidJavaClass ac = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            ao = ac.GetStatic<AndroidJavaObject>("currentActivity");
        }
        public static HeroUSDKUnitySupportAndroid getInstance()
        {
            if (instance == null)
            {
                instance = new HeroUSDKUnitySupportAndroid();
            }

            return instance;
        }
        /*
         * Unity监听
         */
        public void setListener(string gameObjectName)
        {
            Debug.Log("gameObject is " + gameObjectName);
            if (ao == null)
            {
                Debug.LogError("setListener error, current activity is null");
            }
            else
            {
                ao.Call("requestUDKSetListener", gameObjectName);
            }
        }
        /*
         * 初始化融合SDK
         * **/
        public void requestUsdkInit(string productId, string productKey)
        {
            ao.Call("requestUsdkInit", productId, productKey);
        }
        /*
         * 登录
         * **/
        public void requestUsdkEnterLoginView()
        {
            ao.Call("requestUsdkEnterLoginView");
        }
        /*
         * 创建角色
         * **/
        public void requestUsdkCreateRole(HeroHDCGameRoleInfo roleInfo)
        {
            ao.Call("requestUsdkCreateRole",  roleInfo.serverId, roleInfo.serverName, roleInfo.roleId, roleInfo.roleName,
                                      roleInfo.level, roleInfo.vipLevel, roleInfo.partyName, roleInfo.roleCreateTime,
                                      roleInfo.balanceLevelOne, roleInfo.balanceLevelTwo, roleInfo.sumPay, roleInfo.partyId,
                                      roleInfo.roleGender, roleInfo.rolePower, roleInfo.partyRoleId, roleInfo.partyRoleName,
                                      roleInfo.professionId, roleInfo.profession, roleInfo.friendList);
        }
        /*
         * 进入游戏
         * **/
        public void requestUsdkEnterGame(HeroHDCGameRoleInfo roleInfo)
        {
            ao.Call("requestUsdkEnterGame", roleInfo.serverId, roleInfo.serverName, roleInfo.roleId, roleInfo.roleName,
                                      roleInfo.level, roleInfo.vipLevel, roleInfo.partyName, roleInfo.roleCreateTime,
                                      roleInfo.balanceLevelOne, roleInfo.balanceLevelTwo, roleInfo.sumPay, roleInfo.partyId,
                                      roleInfo.roleGender, roleInfo.rolePower, roleInfo.partyRoleId, roleInfo.partyRoleName,
                                      roleInfo.professionId, roleInfo.profession, roleInfo.friendList);
        }
        /*
         * 角色升级
         * **/
        public void requestUsdkLevelUp(HeroHDCGameRoleInfo roleInfo)
        {
            ao.Call("requestUsdkLevelUp", roleInfo.serverId, roleInfo.serverName, roleInfo.roleId, roleInfo.roleName,
                                      roleInfo.level, roleInfo.vipLevel, roleInfo.partyName, roleInfo.roleCreateTime,
                                      roleInfo.balanceLevelOne, roleInfo.balanceLevelTwo, roleInfo.sumPay, roleInfo.partyId,
                                      roleInfo.roleGender, roleInfo.rolePower, roleInfo.partyRoleId, roleInfo.partyRoleName,
                                      roleInfo.professionId, roleInfo.profession, roleInfo.friendList);
        }
        /*
         * 注销登录
         * **/
        public void requestUsdkLogout()
        {
            ao.Call("requestUsdkLogout");
        }
        /*
         * 退出
         * **/
        public void requestUsdkExit()
        {
            ao.Call("requestUsdkExit");
        }
        /*
         * 支付
         * **/
        public void requestUsdkPay(HeroHDCGameRoleInfo roleInfo, HeroUPaymentParameters paymentParameters)
        {

            ao.Call("requestUsdkPay", paymentParameters.goodsId, paymentParameters.extraParams, paymentParameters.cpOrder, paymentParameters.callbackUrl,
                                      roleInfo.serverId, roleInfo.serverName, roleInfo.roleId, roleInfo.roleName,
                                      roleInfo.level, roleInfo.vipLevel, roleInfo.partyName, roleInfo.roleCreateTime,
                                      roleInfo.balanceLevelOne, roleInfo.balanceLevelTwo, roleInfo.sumPay, roleInfo.partyId,
                                      roleInfo.roleGender, roleInfo.rolePower, roleInfo.partyRoleId, roleInfo.partyRoleName,
                                      roleInfo.professionId, roleInfo.profession, roleInfo.friendList);
        }
        /*
         * 上报踢玩家下线结果
         * **/
        public void requestUsdkNotifyKickResult(string result)
        {
            ao.Call("requestUsdkNotifyKickResult", result);
        }
        /*
         * 扫码登录
         * **/
        public void showScanViewWithExt(string extra)
        {
            ao.Call("requestUsdkScanViewWithExtra", extra);
        }
        /*
         * 分享
         * **/
        public void share(ShareInfo shareInfo)
        {
            ao.Call("requestShare",shareInfo.hasUi,shareInfo.title,shareInfo.content,shareInfo.imgPath,
                shareInfo.imgUrl,shareInfo.url,shareInfo.shareTo,shareInfo.extenal);
        }
        /*
		 * banner广告
		 * **/
        public void showAdBanner(string bannerID)
        {
            ao.Call("requestShowAdBanner", bannerID);
        }
        /*
		 * 插屏广告
		 * **/
        public void showAdInterstialBanner(string interstialBannerID)
        {
            ao.Call("requestShowAdInterstialBanner", interstialBannerID);
        }
        /*
		* 激励视频广告
		* **/
        public void showAdVideo(string videoID)
        {
            ao.Call("requestShowAdVideo", videoID);
        }
        /*
		 * 隐藏横幅广告，很少使用
		 * **/
        public void hideAdBanner()
        {
            ao.Call("requestHideAdBanner");
        }
        /*
		 * 渠道ID
		 * **/
        public int requestUsdkGetChannelId() {
            return ao.Call<int>("requestUsdkGetChannelId");
        }
        /*
		 * 渠道名称
		 * **/
        public string requestUsdkGetChannelName()
        {
            return ao.Call<string>("requestUsdkGetChannelName");
        }
        /*
		 * 获取渠道SDK的版本名
		 * **/
        public string requestUsdkGetChannelSdkVersionName()
        {
            return ao.Call<string>("requestUsdkGetChannelSdkVersionName");
        }
        /*
		 * 判断当前渠道在调用退出接口时是否会弹出退出框
		 * **/
        public bool requestUsdkisChannelHasExitDialog()
        {
            return ao.Call<bool>("requestUsdkisChannelHasExitDialog");
        }
        /*
		 * 获取英雄官网渠道的ProjectId，常用于游戏需要CPS分包时
		 * **/
        public string requestUsdkGetProjectId()
        {
            return ao.Call<string>("requestUsdkGetProjectId");
        }
        /*
		 * 获取用户在HeroUSDK后台配置的自定义参数值。
		 * **/
        public string requestUsdkGetCustomParams(string key)
        {
            return ao.Call<string>("requestUsdkGetCustomParams",key);
        }
        /*
		 * 调用渠道的扩展方法(如显示/隐藏悬浮框，防沉迷查询，进入用户中心等)。
		 * **/
        public bool requestUsdkcallExtendApi(int extendType)
        {
            return ao.Call<bool>("requestUsdkcallExtendApi", extendType); ;
        }
        /*
		 * 获取当前设备的OAID值
		 * **/
        public void requestUsdkGetOAID()
        {
            ao.Call("requestUsdkGetOAID");
        }
        /*
		 * 获取协议内容
		 * **/
        public string getProtocolResult()
        {
            return ao.Call<string>("requestUsdkGetProtocolResult");
        }
        /*
		 * 点击同意通知
		 * **/
        public void setAgreeProtocol()
        {
            ao.Call("requestUsdkSetAgreeProtocol");
        }
    }
#endif
}
