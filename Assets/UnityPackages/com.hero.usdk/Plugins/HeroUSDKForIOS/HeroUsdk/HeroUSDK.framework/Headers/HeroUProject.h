//
//  HeroUProject.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/10/27.
//  Copyright © 2020年 Hero. All rights reserved.
//
//  本类负责HeroU【初始化】相关操作

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBMJExtension.h>

NS_ASSUME_NONNULL_BEGIN

@interface HeroUProject : NSObject

/** 融合产品ID */
@property (nonatomic, strong) NSString *usdkProductId;
/** 融合产品秘钥 */
@property (nonatomic, strong) NSString *usdkProductKey;
/** 镜像Id */
@property (nonatomic, strong) NSString *imgId;

/**
 渠道初始化方法，提供2种初始化的方式
 1. 获取HeroService-Info.plist文件，放入工程中即可初始化，不用调用此方法（建议）
 2. 自己写字典参数传入初始化，需要调用此方法
 @param parameters 对应的参数
 */
- (void)channelInitWithParameters:(NSDictionary *)parameters;

@end

NS_ASSUME_NONNULL_END
