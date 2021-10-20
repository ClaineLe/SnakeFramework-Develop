//
//  NSString+HBMJExtension.h
//  HBMJExtensionExample
//
//  Created by MJ Lee on 15/6/7.
//  Copyright (c) 2015年 小码哥. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBMJExtensionConst.h>

@interface NSString (HBMJExtension)
/**
 *  驼峰转下划线（loveYou -> love_you）
 */
- (NSString *)hb_underlineFromCamel;
/**
 *  下划线转驼峰（love_you -> loveYou）
 */
- (NSString *)hb_camelFromUnderline;
/**
 * 首字母变大写
 */
- (NSString *)hb_firstCharUpper;
/**
 * 首字母变小写
 */
- (NSString *)hb_firstCharLower;

- (BOOL)hb_isPureInt;

- (NSURL *)hb_url;
@end
