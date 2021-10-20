//
//  NSString+HBAESCrypt.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/NSData+HBAESCrypt.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSString (HBAESCrypt)
/*
 \internal
 @function AES256EncryptWithKey
 @abstract This function accepts the key to encrypt the NSString and return the encrypted
           string.
 @discussion Converts the string to NSData and calls the encryption method of NSData and
             then converts the encrypted NSData to string and returns the encrypted string.
 @result Returns the encrypted string.
 */
- (NSString *)hb_AES256EncryptWithKey:(NSString *)key;

/*
 \internal
 @function AES256DecryptWithKey
 @abstract This function accepts the key to dencrypt the NSString and return the plain
 string.
 @discussion Converts the encrypted string to encrypted NSData and calls the decryption method of NSData to get the plain NSData which is then converted to string.
 @result Returns the encrypted string.
 */
- (NSString *)hb_AES256DecryptWithKey:(NSString *)key;

-(NSString *)hb_AES128EncryptWithkey:(NSString *)key;
@end

NS_ASSUME_NONNULL_END
