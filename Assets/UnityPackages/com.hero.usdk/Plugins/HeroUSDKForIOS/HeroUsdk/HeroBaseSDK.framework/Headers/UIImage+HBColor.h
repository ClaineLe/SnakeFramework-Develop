//
//  UIImage+HBColor.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface UIImage (HBColor)
+ (UIImage *)hb_imageWithColor:(UIColor *)color;

+ (UIImage *)hb_imageFromData:(NSData *)data;
@end

NS_ASSUME_NONNULL_END
