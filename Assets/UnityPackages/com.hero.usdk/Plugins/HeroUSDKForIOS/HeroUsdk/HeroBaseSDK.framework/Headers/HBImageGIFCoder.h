/*
 * This file is part of the HeroWebImage package.
 * (c) Olivier Poitrey <rs@dailymotion.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBImageIOAnimatedCoder.h>

/**
 Built in coder using ImageIO that supports animated GIF encoding/decoding
 @note `HBImageIOCoder` supports GIF but only as static (will use the 1st frame).
 @note Use `HBImageGIFCoder` for fully animated GIFs. For `UIImageView`, it will produce animated `UIImage`(`NSImage` on macOS) for rendering. For `HBAnimatedImageView`, it will use `HBAnimatedImage` for rendering.
 @note The recommended approach for animated GIFs is using `HBAnimatedImage` with `HBAnimatedImageView`. It's more performant than `UIImageView` for GIF displaying(especially on memory usage)
 */
@interface HBImageGIFCoder : HBImageIOAnimatedCoder <HeroProgressiveImageCoder, HBAnimatedImageCoder>

@property (nonatomic, class, readonly, nonnull) HBImageGIFCoder *sharedCoder;

@end
