using UnityEngine;
using System.Collections;

namespace herousdk
{
    // HeroUSDKListener
    public abstract class HeroUSDKListener : MonoBehaviour
    {
        //融合初始化成功、失败回调
        public abstract void usdk_onInitCallBack(HeroUSDKResult result, string msg);
        //登录成功、失败、取消回调
        public abstract void usdk_onLoginCallBack(HeroUSDKResult result,UserInfo userInfo,string msg);
        //切换账号成功、失败、取消回调
        public abstract void usdk_onSwitchAccountCallBack(HeroUSDKResult result, UserInfo userInfo,string msg);
        //退出登录成功、失败回调
        public abstract void usdk_onLogoutCallBack(HeroUSDKResult result, string msg);
        //退出成功、失败回调
        public abstract void usdk_onExitCallBack(HeroUSDKResult result, string msg);
        //支付成功、失败、取消回调
        public abstract void usdk_onPayCallBack(HeroUSDKResult result, HeroUPaymentOrder payResult,string msg);
        //登录失效[防沉迷被踢下线]
        public abstract void usdk_onLoginInvalid(string msg) ;
        //用户已经同意协议，在这里进行init等操作
        public abstract void usdk_onProtocolAgree();
        //分享成功、取消、失败回调
        public virtual void usdk_onShareCallBack(HeroUSDKResult result, string msg) {
            this.usdk_onShareCallBack(result, msg);
        }
        //广告点击、关闭、播放完成、播放失败回调
        public virtual void usdk_onAdCallBack(HeroUSDKAdResult result, string msg) {
            this.usdk_onAdCallBack(result, msg);
        }
        //获取oaid结果
        public virtual void usdk_onGetOAIDResultAction(string msg) {
            this.usdk_onGetOAIDResultAction(msg);
        }

        /*
         * 融合初始化成功
         * **/
        public void onInitSuccess()
        {
            usdk_onInitCallBack(HeroUSDKResult.HeroUSDKResultSuccess,"初始化成功");
        }
        /*
         * 融合初始化失败
         * **/
        public void onInitFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var errorMsg = data["msg"].Value;
            usdk_onInitCallBack(HeroUSDKResult.HeroUSDKResultFailed,errorMsg);
        }
        /*
         * 登录成功
         * **/
        public void onLoginSuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string plat = data["plat"].Value;
            UserInfo userInfo = new UserInfo();
            userInfo.plat = plat;
            if (plat == "android")
            {
                userInfo.channelToken = data["channelToken"].Value;
                userInfo.extendParams = data["extendParams"].Value;
                userInfo.isFristLogin = data["isFristLogin"].Value;
                userInfo.serverMessage = data["serverMessage"].Value;
                userInfo.token = data["token"].Value;
                userInfo.uid = data["uid"].Value;
                userInfo.userName = data["userName"].Value;
            } else {
                userInfo.accessCode = data["accessCode"].Value;
                userInfo.accessToken = data["accessToken"].Value;
                userInfo.sdkUserId = data["sdkuserid"].Value;
                userInfo.userName = data["username"].Value;
            }
            //回调
            usdk_onLoginCallBack(HeroUSDKResult.HeroUSDKResultSuccess,userInfo,"登陆成功");
        }
        /*
         * 取消登录
         * **/
        public void onLoginCancel()
        {
            usdk_onLoginCallBack(HeroUSDKResult.HeroUSDKResultCancel,null,"取消登陆");
        }
        /*
         * 登录失败
         * **/
        public void onLoginFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
			var errorMsg = data["msg"].Value;
            usdk_onLoginCallBack(HeroUSDKResult.HeroUSDKResultFailed,null,errorMsg);
        }
        /*
         * 登录失效
         * **/
        public void onLogonInvalid(string msg){
            var data = SimpleJSON.JSONNode.Parse(msg);
			var errMsg = data["msg"].Value;
            usdk_onLoginInvalid(errMsg);
        }
        /*
         * 切换账号成功
         * **/
        public void onSwitchAccountSuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string plat = data["plat"].Value;
            UserInfo userInfo = new UserInfo();
            userInfo.plat = plat;
            if (plat == "android")
            {
                userInfo.channelToken = data["channelToken"].Value;
                userInfo.extendParams = data["extendParams"].Value;
                userInfo.isFristLogin = data["isFristLogin"].Value;
                userInfo.serverMessage = data["serverMessage"].Value;
                userInfo.token = data["token"].Value;
                userInfo.uid = data["uid"].Value;
                userInfo.userName = data["userName"].Value; 
            } 
            usdk_onSwitchAccountCallBack(HeroUSDKResult.HeroUSDKResultSuccess, userInfo, null);
        }
        /*
         * 切换账号后失败
         * **/
        public void onSwtichAccountFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var errorMsg = data["msg"].Value;
            usdk_onSwitchAccountCallBack(HeroUSDKResult.HeroUSDKResultFailed,null,errorMsg);
        }
        /*
         * 切换账号取消
         * **/
        public void onSwitchAccountCancel()
        {
            usdk_onSwitchAccountCallBack(HeroUSDKResult.HeroUSDKResultCancel,null,"取消切换账号");
        }
        /*
         * 注销登录成功
         * **/
        public void onLogoutSuccess() {
            usdk_onLogoutCallBack(HeroUSDKResult.HeroUSDKResultSuccess,null);
        }
        /*
         * 注销登录失败
         * **/
        public void onLogoutFailure(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var errorMsg = data["msg"].Value;
            usdk_onLogoutCallBack(HeroUSDKResult.HeroUSDKResultFailed,errorMsg);
        }
        /*
         * 退出成功
         * **/
        public void onExitSuccess()
        {
            usdk_onExitCallBack(HeroUSDKResult.HeroUSDKResultSuccess,"退出成功");
        }
        /*
         * 退出失败
         * **/
        public void onExitFailure(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var errorMsg = data["msg"].Value;
            usdk_onExitCallBack(HeroUSDKResult.HeroUSDKResultFailed,errorMsg);
        }
        /*
         * 支付成功
         * **/
        public void onPaySuccess(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string plat = data["plat"].Value;
            HeroUPaymentOrder result = new HeroUPaymentOrder();
            result.plat = plat;
            if (plat == "android")
            {
                result.orderId = data["orderId"].Value;
                result.cpOrderId = data["cpOrderId"].Value;
                result.extraParams = data["extraParams"].Value;
            } else {
                result.orderId = data["orderId"].Value;
                result.orderAmount = data["orderAmount"].Value;
                result.currency = data["currency"].Value;
            }
            usdk_onPayCallBack(HeroUSDKResult.HeroUSDKResultSuccess,result,null);
        }
        /*
         * 支付失败
         * **/
        public void onPayFailed(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var errorMsg = data["msg"].Value;
            usdk_onPayCallBack(HeroUSDKResult.HeroUSDKResultFailed,null,errorMsg);
        }
        /*
         * 支付取消，Android
         * **/
        public void onPayCancel(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            var cpOrderId = data["cpOrderId"].Value;
            usdk_onPayCallBack(HeroUSDKResult.HeroUSDKResultCancel,null,"取消支付");
        }
        /*
         * 分享取消
         * **/
        public void onShareCancelAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string shareType = data["shareType"].Value;
            usdk_onShareCallBack(HeroUSDKResult.HeroUSDKResultCancel,shareType);
        }
        /*
         * 分享失败
         * **/
        public void onShareFailedAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string tmsg = data["msg"].Value;
            usdk_onShareCallBack(HeroUSDKResult.HeroUSDKResultFailed, tmsg);
        }
        /*
         * 分享成功
         * **/
        public void onShareSuccessdAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string shareType = data["shareType"].Value;
            usdk_onShareCallBack(HeroUSDKResult.HeroUSDKResultSuccess,"分享成功");
        }
        /*
         * 同意协议回调
         * **/
        public void onClickProtocol(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            usdk_onProtocolAgree();
        }
        /*
         * 广告视频点击回调
         * **/
        public void onAdClickedAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string type = data["type"].Value;
            usdk_onAdCallBack(HeroUSDKAdResult.HeroUSDKAdResultClicked,"视频点击回调");
        }
        /*
         * 广告视频关闭回调
         * **/
        public void onAdClosedAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string type = data["type"].Value;
            usdk_onAdCallBack(HeroUSDKAdResult.HeroUSDKAdResultClosed, "视频关闭回调");
        }
        /*
         * 广告视频播放完成回调
         * **/
        public void onAdPlayCompleteAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string type = data["type"].Value;
            usdk_onAdCallBack(HeroUSDKAdResult.HeroUSDKAdResultPlayComplete,"视频播放完成回调");
        }
        /*
         * 广告视频播放失败回调
         * **/
        public void onAdPlayFailedAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string tmsg = data["msg"].Value;
            usdk_onAdCallBack(HeroUSDKAdResult.HeroUSDKAdResultPlayFailed, tmsg);
        }
        /*
         * 获取oaid回调
         * **/
        public void onGetOAIDResultAction(string msg)
        {
            var data = SimpleJSON.JSONNode.Parse(msg);
            string oaid = data["oaid"].Value;
            usdk_onGetOAIDResultAction(oaid);
        }
    }
}
