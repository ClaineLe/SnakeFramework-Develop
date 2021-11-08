//
//  GamePaymentOrder.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/11/10.
//  Copyright © 2020 Hero. All rights reserved.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface GamePaymentOrder : NSObject

-(NSString *)orderId;

-(NSString *)orderAmount;

-(NSString *)currency;

-(NSInteger)errorCode;

-(NSString *)errorDescription;

@end

NS_ASSUME_NONNULL_END
