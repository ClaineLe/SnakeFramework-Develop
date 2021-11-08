//
//  GameProject.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/11/25.
//  Copyright © 2020 Hero. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface GameProject : NSObject

/**
 *  获取一个Project对象
 *
 *  所有的参数都不得为空.这些参数是游戏所必须的参数.
 *
 *  @param projectId 项目ID
 *  @param productId 产品ID
 *  @param serverId 服务ID
 *  @param appKey app秘钥
 *  @param usdkProductKey 融合产品秘钥
 *  @param usdkProductId 融合产品ID
 *  @return GameProject实例
 */

+ (instancetype)projectWithId:(NSString *)projectId
                   productId:(NSString *)productId
                    serverId:(NSString *)serverId
                      appkey:(NSString *)appKey
              usdkProductKey:(NSString *)usdkProductKey
               usdkProductId:(NSString *)usdkProductId
            initializerBlock:(void(^)(GameProject * aSelf))initializerBlock;

// 登陆成功完成, 进行验证的secretKey.
// 如果这个值不为nil, 那么SDK将会帮助游戏客户端进行验证操作.
@property (nonatomic, strong) NSString * secretKey;

// 项目id
@property (nonatomic, copy) NSString *projectId;

// 游戏id
@property (nonatomic, readonly) NSString *productId;

// appkey
@property (nonatomic, readonly) NSString *appKey;

// 玩家所选游戏服务器ID
@property (nonatomic, readonly) NSString *serverId;

// clientId, 目前值与appKey相同
@property (nonatomic, readonly) NSString *clientId;

// 通用设置, 固定为1
@property (nonatomic, readonly) NSString *redirectUri;

//SDK默认开启IAP内购功能，在初始化时，将“aProject.closeIAPFunction“设置为 YES 可禁用该功能
@property (nonatomic, assign) BOOL closeIAPFunction;

//关闭浮标中，切换账号
@property (nonatomic, assign) BOOL closeFloatChangeAccount ;

@property (nonatomic, copy  ) NSString *usdkProductKey;

@property (nonatomic, copy  ) NSString *usdkProductId;

/**服务器地址*/
@property (nonatomic, copy  ) NSString *serverUrl;

@end

@interface GameProject (Protected)

-(id)initWithProjectId:(NSString *)projectId
             productId:(NSString *)productId
              serverId:(NSString *)serverId
                appKey:(NSString *)appKey
      initializerBlock:(void(^)(GameProject * aSelf))initializerBlock;

-(id)initWithProjectId:(NSString *)projectId
             productId:(NSString *)productId
              serverId:(NSString *)serverId
                appKey:(NSString *)appKey
        usdkProductKey:(NSString *)usdkProductKey
         usdkProductId:(NSString *)usdkProductId
      initializerBlock:(void(^)(GameProject * aSelf))initializerBlock;

@end

NS_ASSUME_NONNULL_END
