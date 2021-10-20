//
//  HeroUBloc.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/10/27.
//  Copyright © 2020 Hero. All rights reserved.
//
//  本类负责HeroU【助手插件】相关操作

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface HeroUBloc : NSObject

@end

@interface HeroUBlocConfig : NSObject

@property (nonatomic, strong) NSString * blocGameId;    // 助手sdk gameid
@property (nonatomic, strong) NSString * hdcGameId;     // 数据sdk gameid
@property (nonatomic, strong) NSString * blocKey;       // 助手sdk key
@property (nonatomic, strong) NSString * hdcKey;        // 数据sdk key
@property (nonatomic, strong) NSString * channelId;     // 渠道id
@property (nonatomic, strong) NSString * debugToken;    // 自定义的调试密钥
@property (nonatomic, assign) BOOL isDebug;             // 调试开关(YES为测试环境 NO为正式环境，上线时要设为NO)

@end

NS_ASSUME_NONNULL_END
