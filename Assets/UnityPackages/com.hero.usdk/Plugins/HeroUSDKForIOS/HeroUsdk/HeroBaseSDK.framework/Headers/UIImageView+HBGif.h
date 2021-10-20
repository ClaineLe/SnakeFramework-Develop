//
//  UIImageView+HBGif.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/26.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface UIImageView (HBGif)
- (void)hb_showGifImageWithData:(NSData *)data;
- (void)hb_showGifImageWithURL:(NSURL *)url;
@end

NS_ASSUME_NONNULL_END
