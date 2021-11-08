//
//  NSString+HBBase64.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSData (HBBase64)

+ (NSData *)hbDataWithBase64EncodedString:(NSString *)string;
- (NSString *)hb_base64EncodedString;

@end


@interface NSString (HBBase64)
- (NSString *)hb_base64EncodedString:(NSStringEncoding)encoding;
- (NSString *)hb_base64EncodedString;
- (NSString *)hb_base64DecodedString:(NSStringEncoding)encoding;
- (NSString *)hb_base64DecodedString;

+ (BOOL)isBlankString:(NSString *)string;

/**
 验证用户名、密码输入格式
 */
+ (BOOL)judgeAccountPassLegal:(NSString *)string ;

/**
 用户名不能为全数字
 */
+ (BOOL)judgeAccountAllNumber:(NSString *)string ;
@end

NS_ASSUME_NONNULL_END
