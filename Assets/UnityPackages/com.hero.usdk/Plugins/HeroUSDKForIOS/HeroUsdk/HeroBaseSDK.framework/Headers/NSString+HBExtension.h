//
//  NSString+HBExtension.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/11/9.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSString (HBExtension)
/**将nil转换成@""*/
- (NSString *)convertNull;
@end

NS_ASSUME_NONNULL_END
