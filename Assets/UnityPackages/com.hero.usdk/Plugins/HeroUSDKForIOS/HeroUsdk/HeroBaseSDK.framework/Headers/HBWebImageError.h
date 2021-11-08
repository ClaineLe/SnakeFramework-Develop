/*
 * This file is part of the HeroWebImage package.
 * (c) Olivier Poitrey <rs@dailymotion.com>
 * (c) Jamie Pinkham
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

#import <HeroBaseSDK/HBWebImageCompat.h>

FOUNDATION_EXPORT NSErrorDomain const _Nonnull HBWebImageErrorDomain;

/// The HTTP status code for invalid download response (NSNumber *)
FOUNDATION_EXPORT NSErrorUserInfoKey const _Nonnull HBWebImageErrorDownloadStatusCodeKey;

/// HeroWebImage error domain and codes
typedef NS_ERROR_ENUM(HBWebImageErrorDomain, HBWebImageError) {
    HBWebImageErrorInvalidURL = 1000, // The URL is invalid, such as nil URL or corrupted URL
    HBWebImageErrorBadImageData = 1001, // The image data can not be decoded to image, or the image data is empty
    HBWebImageErrorCacheNotModified = 1002, // The remote location specify that the cached image is not modified, such as the HTTP response 304 code. It's useful for `HeroWebImageRefreshCached`
    HBWebImageErrorBlackListed = 1003, // The URL is blacklisted because of unrecoverable failure marked by downloader (such as 404), you can use `.retryFailed` option to avoid this
    HBWebImageErrorInvalidDownloadOperation = 2000, // The image download operation is invalid, such as nil operation or unexpected error occur when operation initialized
    HBWebImageErrorInvalidDownloadStatusCode = 2001, // The image download response a invalid status code. You can check the status code in error's userInfo under `HBWebImageErrorDownloadStatusCodeKey`
    HBWebImageErrorCancelled = 2002, // The image loading operation is cancelled before finished, during either async disk cache query, or waiting before actual network request. For actual network request error, check `NSURLErrorDomain` error domain and code.
    HBWebImageErrorInvalidDownloadResponse = 2003, // When using response modifier, the modified download response is nil and marked as failed.
};
