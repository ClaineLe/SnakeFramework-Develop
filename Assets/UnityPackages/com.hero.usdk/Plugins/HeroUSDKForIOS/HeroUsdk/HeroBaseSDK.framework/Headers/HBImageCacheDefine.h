/*
 * This file is part of the HeroWebImage package.
 * (c) Olivier Poitrey <rs@dailymotion.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBWebImageCompat.h>
#import <HeroBaseSDK/HBWebImageOperation.h>
#import <HeroBaseSDK/HBWebImageDefine.h>

/// Image Cache Type
typedef NS_ENUM(NSInteger, HBImageCacheType) {
    /**
     * For query and contains op in response, means the image isn't available in the image cache
     * For op in request, this type is not available and take no effect.
     */
    HBImageCacheTypeNone,
    /**
     * For query and contains op in response, means the image was obtained from the disk cache.
     * For op in request, means process only disk cache.
     */
    HBImageCacheTypeDisk,
    /**
     * For query and contains op in response, means the image was obtained from the memory cache.
     * For op in request, means process only memory cache.
     */
    HBImageCacheTypeMemory,
    /**
     * For query and contains op in response, this type is not available and take no effect.
     * For op in request, means process both memory cache and disk cache.
     */
    HBImageCacheTypeAll
};

typedef void(^HBImageCacheCheckCompletionBlock)(BOOL isInCache);
typedef void(^HBImageCacheQueryDataCompletionBlock)(NSData * _Nullable data);
typedef void(^HBImageCacheCalculateSizeBlock)(NSUInteger fileCount, NSUInteger totalSize);
typedef NSString * _Nullable (^HBImageCacheAdditionalCachePathBlock)(NSString * _Nonnull key);
typedef void(^HBImageCacheQueryCompletionBlock)(UIImage * _Nullable image, NSData * _Nullable data, HBImageCacheType cacheType);
typedef void(^HBImageCacheContainsCompletionBlock)(HBImageCacheType containsCacheType);

/**
 This is the built-in decoding process for image query from cache.
 @note If you want to implement your custom loader with `queryImageForKey:options:context:completion:` API, but also want to keep compatible with HeroWebImage's behavior, you'd better use this to produce image.
 
 @param imageData The image data from the cache. Should not be nil
 @param cacheKey The image cache key from the input. Should not be nil
 @param options The options arg from the input
 @param context The context arg from the input
 @return The decoded image for current image data query from cache
 */
FOUNDATION_EXPORT UIImage * _Nullable HBImageCacheDecodeImageData(NSData * _Nonnull imageData, NSString * _Nonnull cacheKey, HeroWebImageOptions options, HeroWebImageContext * _Nullable context);

/**
 This is the image cache protocol to provide custom image cache for `HBWebImageManager`.
 Though the best practice to custom image cache, is to write your own class which conform `HBMemoryCache` or `HBDiskCache` protocol for `HBImageCache` class (See more on `HBImageCacheConfig.memoryCacheClass & HBImageCacheConfig.diskCacheClass`).
 However, if your own cache implementation contains more advanced feature beyond `HBImageCache` itself, you can consider to provide this instead. For example, you can even use a cache manager like `HBImageCachesManager` to register multiple caches.
 */
@protocol HBImageCache <NSObject>

@required
/**
 Query the cached image from image cache for given key. The operation can be used to cancel the query.
 If image is cached in memory, completion is called synchronously, else asynchronously and depends on the options arg (See `HeroWebImageQueryDiskSync`)

 @param key The image cache key
 @param options A mask to specify options to use for this query
 @param context A context contains different options to perform specify changes or processes, see `HeroWebImageContextOption`. This hold the extra objects which `options` enum can not hold.
 @param completionBlock The completion block. Will not get called if the operation is cancelled
 @return The operation for this query
 */
- (nullable id<HBWebImageOperation>)queryImageForKey:(nullable NSString *)key
                                             options:(HeroWebImageOptions)options
                                             context:(nullable HeroWebImageContext *)context
                                          completion:(nullable HBImageCacheQueryCompletionBlock)completionBlock;

/**
 Query the cached image from image cache for given key. The operation can be used to cancel the query.
 If image is cached in memory, completion is called synchronously, else asynchronously and depends on the options arg (See `HeroWebImageQueryDiskSync`)

 @param key The image cache key
 @param options A mask to specify options to use for this query
 @param context A context contains different options to perform specify changes or processes, see `HeroWebImageContextOption`. This hold the extra objects which `options` enum can not hold.
 @param cacheType Specify where to query the cache from. By default we use `.all`, which means both memory cache and disk cache. You can choose to query memory only or disk only as well. Pass `.none` is invalid and callback with nil immediately.
 @param completionBlock The completion block. Will not get called if the operation is cancelled
 @return The operation for this query
 */
- (nullable id<HBWebImageOperation>)queryImageForKey:(nullable NSString *)key
                                             options:(HeroWebImageOptions)options
                                             context:(nullable HeroWebImageContext *)context
                                           cacheType:(HBImageCacheType)cacheType
                                          completion:(nullable HBImageCacheQueryCompletionBlock)completionBlock;

/**
 Store the image into image cache for the given key. If cache type is memory only, completion is called synchronously, else asynchronously.

 @param image The image to store
 @param imageData The image data to be used for disk storage
 @param key The image cache key
 @param cacheType The image store op cache type
 @param completionBlock A block executed after the operation is finished
 */
- (void)storeImage:(nullable UIImage *)image
         imageData:(nullable NSData *)imageData
            forKey:(nullable NSString *)key
         cacheType:(HBImageCacheType)cacheType
        completion:(nullable HeroWebImageNoParamsBlock)completionBlock;

/**
 Remove the image from image cache for the given key. If cache type is memory only, completion is called synchronously, else asynchronously.

 @param key The image cache key
 @param cacheType The image remove op cache type
 @param completionBlock A block executed after the operation is finished
 */
- (void)removeImageForKey:(nullable NSString *)key
                cacheType:(HBImageCacheType)cacheType
               completion:(nullable HeroWebImageNoParamsBlock)completionBlock;

/**
 Check if image cache contains the image for the given key (does not load the image). If image is cached in memory, completion is called synchronously, else asynchronously.

 @param key The image cache key
 @param cacheType The image contains op cache type
 @param completionBlock A block executed after the operation is finished.
 */
- (void)containsImageForKey:(nullable NSString *)key
                  cacheType:(HBImageCacheType)cacheType
                 completion:(nullable HBImageCacheContainsCompletionBlock)completionBlock;

/**
 Clear all the cached images for image cache. If cache type is memory only, completion is called synchronously, else asynchronously.

 @param cacheType The image clear op cache type
 @param completionBlock A block executed after the operation is finished
 */
- (void)clearWithCacheType:(HBImageCacheType)cacheType
                completion:(nullable HeroWebImageNoParamsBlock)completionBlock;

@end
