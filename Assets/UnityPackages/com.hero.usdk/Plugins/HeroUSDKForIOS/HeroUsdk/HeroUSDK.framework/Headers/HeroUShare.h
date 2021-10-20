//
//  HeroUShare.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/10/27.
//  Copyright © 2020 Hero. All rights reserved.
//
//  本类负责HeroU【分享插件】相关操作

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, HeroUSharePlatform) {
    
    HeroU_Share_Platform_QQ = 1, //QQ好友
    HeroU_Share_Platform_QQ_Space, //QQ空间
    HeroU_Share_Platform_WeChat, //微信好友
    HeroU_Share_Platform_WXTimeLine, //朋友圈
    HeroU_Share_Platform_Weibo,
    HeroU_Share_Platform_HeroCommunity //英雄社区
};

typedef NS_ENUM(NSInteger, HeroUShareType) {
    HeroUShareType_image = 0, //分享图片
    HeroUShareType_link  = 1, //分享链接
    HeroUShareType_text  = 2, //分享文本
};

typedef NS_ENUM(NSInteger, HeroUShareStatus) {
    HeroUShareStatus_success = 0, //分享成功
    HeroUShareStatus_failed  = 1, //分享失败
    HeroUShareStatus_cancel  = 2, //分享取消
};

typedef NS_ENUM(NSInteger, HeroUShareTaget) {
    HeroUShareTaget_wechat      = 1, //分享到微信
    HeroUShareTaget_wechatLine  = 2, //分享到朋友圈
    HeroUShareTaget_QQ          = 3, //分享到QQ
    HeroUShareTaget_QQSpace     = 4, //分享到QQ空间
    HeroUShareTaget_Weibo       = 6, //分享到微博
};
NS_ASSUME_NONNULL_BEGIN

@interface HeroUShare : NSObject

@end

@interface HeroUShareModel : NSObject
@property (nonatomic,assign) HeroUShareType      shareType; //分享类型 （必填）
@property (nonatomic,assign) HeroUSharePlatform  sharePlatform ; //分享平台（必填）
//分享图片需要填写的参数
@property (nonatomic,strong) NSData            *shareImage;//分享图片（分享图片与链接2选一）
@property (nonatomic,copy  ) NSString          *imageLink;//分享图片链接（分享图片与链接2选一）
//分享链接需要填写的参数
@property (nonatomic,copy  ) NSString          *shareLink;//分享链接 （分享链接必填）
@property (nonatomic,copy  ) NSString          *shareLinkTitle;//分享链接标题（分享链接必填）
@property (nonatomic,copy  ) NSString          *shareLinkDescription;//分享链接描述（分享链接必填）
//分享文本需要填写的参数
@property (nonatomic,copy  ) NSString          *shareText;//分享纯文本文字 （分享纯文本必填）
@end

NS_ASSUME_NONNULL_END




