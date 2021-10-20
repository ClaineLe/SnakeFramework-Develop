using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace herousdk
{
	public class HeroUSDK
	{
		private static HeroUSDK _instance;
		
		public static HeroUSDK getInstance() {
			if( null == _instance ) {
				_instance = new HeroUSDK();
            }
			return _instance;
		}
		/*
        * =================== 登录支付相关API ===================
        */

		//初始化
		public void setListener(HeroUSDKListener listener)
		{
			HeroUSDKImp.getInstance ().setListener (listener);
        }
		//初始化SDK
		public void initWithHeroUSDK(HeroUProject project)
        {
			HeroUSDKImp.getInstance().initWithHeroUSDK(project) ;
        }
		//登录
		public void login()
		{
			HeroUSDKImp.getInstance().login();
		}
		//进入游戏
		public void enterGame(HeroHDCGameRoleInfo roleInfo)
		{
			HeroUSDKImp.getInstance().enterGame(roleInfo);
        }
		//角色注册
		public void createNewRole(HeroHDCGameRoleInfo roleInfo)
		{
			HeroUSDKImp.getInstance().createNewRole(roleInfo);
		}
		//角色升级
		public void roleLevelUp(HeroHDCGameRoleInfo roleInfo)
		{
			HeroUSDKImp.getInstance().roleLevelUp(roleInfo);
		}
		//注销、退出登陆
		public void logout ()
		{
			HeroUSDKImp.getInstance().logout();
        }
		
		//调用SDKIAP内购
		public void pay(HeroUPaymentParameters paymentParametersData, HeroHDCGameRoleInfo roleInfo)
		{
			HeroUSDKImp.getInstance ().pay(paymentParametersData,roleInfo);
        }
		
		//扫码登录（在PC端登录,SDK需要登录状态）
		public void showScanViewWithExt(string exit)
		{
			HeroUSDKImp.getInstance ().showScanViewWithExt(exit);
		}
		//收到踢下线结果回调SDK
		public void cpKickOffCallBackWithResult(string result)
		{
			HeroUSDKImp.getInstance ().cpKickOffCallBackWithResult(result);
		}
		//获取协议内容
		public string getProtocolResult()
		{
			return HeroUSDKImp.getInstance().getProtocolResult();
		}
		//点击同意通知
		public void setAgreeProtocol()
		{
			HeroUSDKImp.getInstance().setAgreeProtocol();
		}


		/*
        * =================== 附加功能 ===================
        */
		public void share(ShareInfo shareInfo)
		{
			HeroUSDKImp.getInstance().share(shareInfo);
		}

		/*
		 *  ========================== iOS ==========================
		 * **/
		//获取登录用户名
		public string getUserName()
		{
			return HeroUSDKImp.getInstance().getUserName();
		}
		//获取登录用户ID
		public string getUserId()
		{
			return HeroUSDKImp.getInstance().getUserId();
		}
		//获取ID（部分游戏使用@"id"字段作为唯一标示符）
		public string getSdkId()
		{
			return HeroUSDKImp.getInstance().getSdkId();
		}
		//获取设备号(优先取的IDFA、没取到则取的UUID)
		public string getDeviceNum()
		{
			return HeroUSDKImp.getInstance().getDeviceNum();
		}

		//iOS【注销、退出登陆并显示账号历史界面】
		public void logoutAndSowLoginView()
		{
			HeroUSDKImp.getInstance().logoutAndSowLoginView();
		}
		//iOS【打开国内SDK用户中心】
		public void showUserCenter()
		{
			HeroUSDKImp.getInstance().showUserCenter();
		}
		//上报闪屏
		public void postSplashScreenEndSuccess()
		{
			HeroUSDKImp.getInstance().postSplashScreenEndSuccess();
		}

		/*
		 *  ========================== android ==========================
		 * **/
		/*
		 * 渠道ID
		 * **/
		public int getChannelId()
		{
			return HeroUSDKImp.getInstance().getChannelId();
		}
		/*
		 * 渠道名称
		 * **/
		public string getChannelName()
		{
			return HeroUSDKImp.getInstance().getChannelName();
		}
		/*
		 * 获取渠道SDK的版本名
		 * **/
		public string getChannelSdkVersionName()
		{
			return HeroUSDKImp.getInstance().getChannelSdkVersionName();
		}
		/*
		 * 判断当前渠道在调用退出接口时是否会弹出退出框
		 * **/
		public bool isChannelHasExitDialog() {
			return HeroUSDKImp.getInstance().isChannelHasExitDialog();
		}
		/*
		 * 获取英雄官网渠道的ProjectId，常用于游戏需要CPS分包时
		 * **/
		public string getProjectId() {
			return HeroUSDKImp.getInstance().getProjectId();
		}
		/*
		 * 获取用户在HeroUSDK后台配置的自定义参数值。
		 * **/
		public string getCustomParams(string key) {
			return HeroUSDKImp.getInstance().getCustomParams(key);
		}
		/*
		 * 调用渠道的扩展方法(如显示/隐藏悬浮框，防沉迷查询，进入用户中心等)。
		 * **/
		public bool callExtendApi(int extendType) {
			return HeroUSDKImp.getInstance().callExtendApi(extendType);
		}
		/*
		 * 获取当前设备的OAID值
		 * **/
		public void getOAID() {
			 HeroUSDKImp.getInstance().getOAID();
		}

		//android【退出】
		public void exit()
		{
			HeroUSDKImp.getInstance().exit();
		}
		/*
		 * banner广告
		 * **/
		public void showAdBanner(string bannerID)
		{
			HeroUSDKImp.getInstance().showAdBanner(bannerID);
		}
		/*
		 * 插屏广告
		 * **/
		public void showAdInterstialBanner(string interstialBannerID)
		{
			HeroUSDKImp.getInstance().showAdInterstialBanner(interstialBannerID);
		}
		/*
		* 激励视频广告
		* **/
		public void showAdVideo(string videoID)
		{
			HeroUSDKImp.getInstance().showAdVideo(videoID);
		}
		/*
		 * 隐藏横幅广告，很少使用
		 * **/
		public void hideAdBanner()
		{
			HeroUSDKImp.getInstance().hideAdBanner();
		}
	}
}
