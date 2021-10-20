//
//  NSString+HBMD5.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSString (HBMD5)
- (NSString*)hb_md5;

- (NSString *)hb_minmd5;

- (NSString *)hb_maxmd5;

- (NSString *)hb_sha1;

- (NSString *)hb_MD5Str;

- (NSString *)hb_MD5Hash;
@end

NS_ASSUME_NONNULL_END
