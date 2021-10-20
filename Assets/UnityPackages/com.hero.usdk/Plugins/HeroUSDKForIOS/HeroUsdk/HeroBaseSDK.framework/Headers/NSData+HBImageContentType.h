/*
 * This file is part of the HeroWebImage package.
 * (c) Olivier Poitrey <rs@dailymotion.com>
 * (c) Fabrice Aneche
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBWebImageCompat.h>

/**
 You can use switch case like normal enum. It's also recommended to add a default case. You should not assume anything about the raw value.
 For custom coder plugin, it can also extern the enum for supported format. See `HBImageCoder` for more detailed information.
 */
typedef NSInteger HBImageFormat NS_TYPED_EXTENSIBLE_ENUM;
static const HBImageFormat HBImageFormatUndefined = -1;
static const HBImageFormat HBImageFormatJPEG      = 0;
static const HBImageFormat HBImageFormatPNG       = 1;
static const HBImageFormat HBImageFormatGIF       = 2;
static const HBImageFormat HBImageFormatTIFF      = 3;
static const HBImageFormat HBImageFormatWebP      = 4;
static const HBImageFormat HBImageFormatHEIC      = 5;
static const HBImageFormat HBImageFormatHEIF      = 6;
static const HBImageFormat HBImageFormatPDF       = 7;
static const HBImageFormat HBImageFormatSVG       = 8;

/**
 NSData category about the image content type and UTI.
 */
@interface NSData (ImageContentType)

/**
 *  Return image format
 *
 *  @param data the input image data
 *
 *  @return the image format as `HBImageFormat` (enum)
 */
+ (HBImageFormat)hb_imageFormatForImageData:(nullable NSData *)data;

/**
 *  Convert HBImageFormat to UTType
 *
 *  @param format Format as HBImageFormat
 *  @return The UTType as CFStringRef
 *  @note For unknown format, `kUTTypeImage` abstract type will return
 */
+ (nonnull CFStringRef)hb_UTTypeFromImageFormat:(HBImageFormat)format CF_RETURNS_NOT_RETAINED NS_SWIFT_NAME(hb_UTType(from:));

/**
 *  Convert UTType to HBImageFormat
 *
 *  @param uttype The UTType as CFStringRef
 *  @return The Format as HBImageFormat
 *  @note For unknown type, `HBImageFormatUndefined` will return
 */
+ (HBImageFormat)hb_imageFormatFromUTType:(nonnull CFStringRef)uttype;

@end
