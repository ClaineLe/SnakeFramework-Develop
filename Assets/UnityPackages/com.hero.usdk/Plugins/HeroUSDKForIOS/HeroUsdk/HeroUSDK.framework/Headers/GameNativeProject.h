//
//  GameNativeProject.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/11/25.
//  Copyright © 2020 Hero. All rights reserved.
//

#import <HeroUSDK/GameProject.h>

NS_ASSUME_NONNULL_BEGIN

@interface GameNativeProject : GameProject
// 是否是网游  默认是YES
@property (nonatomic, assign) BOOL isOnlineGame;
@end

NS_ASSUME_NONNULL_END
