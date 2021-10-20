//
//  NSData+HBAESCrypt.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface NSData (HBAESCrypt)

/*
 \internal
 @function AES256EncryptWithKey
 @abstract This function accepts the key to encrypt the NSData.
 @discussion This function accepts the key to encrypt and converts the NSData to encrypted
             NSData.
 @result Returns the encrypted NSData.
 */
- (NSData *)hb_AES256EncryptWithKey:(NSString *)key;

/*
 \internal
 @function AES256DecryptWithKey
 @abstract This function accepts the key to decrypt the encrypted NSData.
 @discussion This function accepts the key to decrypt the NSData that is  encrypted and
             converts it to plain NSData
 @result Returns the decrypted NSData.
 */
- (NSData *)hb_AES256DecryptWithKey:(NSString *)key;

/*
 \internal
 @function dataWithBase64EncodedString
 @abstract This function returns the string in NSData format.
 @discussion Converts the string to NSData.
 @result NSData.
 */
+ (NSData *)hb_dataWithBase64EncodedString:(NSString *)string;

/*
 \internal
 @function initWithBase64EncodedString
 @abstract Convert the string to NSData.
 @discussion Convert the string to ASCII data and then to NSData.
 @result Returns NSData.
 */
- (id)initWithHBBase64EncodedString:(NSString *)string;

/*
 \internal
 @function base64Encoding
 @abstract Convert NSData to NSString.
 @discussion This function calls base64EncodingWithLineLength.
 @result Returns NSString.
 */
- (NSString *)hb_base64Encoding;

/*
 \internal
 @function base64EncodingWithLineLength
 @abstract Convert NSData to NSString.
 @discussion This function is used to convert the encrypted data to encrypted string.
 @result Returns NSString.
 */
- (NSString *)hb_base64EncodingWithLineLength:(NSUInteger)lineLength;
    

- (NSData *)hb_AES128EncryptWithKey:(NSString *)key;

@end

NS_ASSUME_NONNULL_END
