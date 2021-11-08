//
//  HBGzipUtility.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/4/20.
//  Copyright © 2020 Cmge. All rights reserved.
//



#import <Foundation/Foundation.h>
#import <zlib.h>
 
@interface HBGzipUtility : NSObject

+(NSData*) gzipData:(NSData*)pUncompressedData;  //压缩
+(NSData*) ungzipData:(NSData *)compressedData;  //解压缩
 
@end
