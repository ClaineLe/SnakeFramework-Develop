//
//  UIColor+HBExtension.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface UIColor (HBExtension)
// Color builders
+ (UIColor *)hb_randomColor;
+ (UIColor *)hb_colorWithHexString:(NSString *)stringToConvert;

@end

NS_ASSUME_NONNULL_END
