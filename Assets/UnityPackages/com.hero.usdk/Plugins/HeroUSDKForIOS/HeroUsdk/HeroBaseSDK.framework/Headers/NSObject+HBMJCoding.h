//
//  NSObject+HBMJCoding.h
//  HBMJExtension
//
//  Created by mj on 14-1-15.
//  Copyright (c) 2014年 小码哥. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBMJExtensionConst.h>

/**
 *  Codeing协议
 */
@protocol HBMJCoding <NSObject>
@optional
/**
 *  这个数组中的属性名才会进行归档
 */
+ (NSArray *)hb_allowedCodingPropertyNames;
/**
 *  这个数组中的属性名将会被忽略：不进行归档
 */
+ (NSArray *)hb_ignoredCodingPropertyNames;
@end

@interface NSObject (HBMJCoding) <HBMJCoding>
/**
 *  解码（从文件中解析对象）
 */
- (void)hb_decode:(NSCoder *)decoder;
/**
 *  编码（将对象写入文件中）
 */
- (void)hb_encode:(NSCoder *)encoder;
@end

/**
 归档的实现
 */
#define HBMJCodingImplementation \
- (id)initWithCoder:(NSCoder *)decoder \
{ \
if (self = [super init]) { \
[self hb_decode:decoder]; \
} \
return self; \
} \
\
- (void)encodeWithCoder:(NSCoder *)encoder \
{ \
[self hb_encode:encoder]; \
}

#define HBMJExtensionCodingImplementation HBMJCodingImplementation
