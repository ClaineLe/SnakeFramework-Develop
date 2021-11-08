//
//  GameChinaProject.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/11/25.
//  Copyright © 2020 Hero. All rights reserved.
//

#import <HeroUSDK/GameNativeProject.h>

NS_ASSUME_NONNULL_BEGIN

@interface GameChinaProject : GameNativeProject
// 客服邮箱
@property (nonatomic, strong) NSString * customerServiceEmail;

// 客服电话
@property (nonatomic, strong) NSString * customerServiceTel;

// 开启防沉迷  默认是NO
@property (nonatomic, assign) BOOL openAntiAddiction;

// 开启公共版  默认是NO
@property (nonatomic, assign) BOOL openPublicVersion;

@property (nonatomic, assign) BOOL openHeroVersion;

@property (nonatomic, strong) NSString *imageAppKey;

@property (nonatomic, strong) NSString *imageAppSecret;

/** 镜像id，用于区分渠道 */
@property (nonatomic, strong) NSString *imgId;

//关闭自动登录
@property (nonatomic, assign) BOOL closeAutoLogin;
@end

NS_ASSUME_NONNULL_END
