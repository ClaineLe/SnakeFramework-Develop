//
//  HeroBaseSDK.h
//  HeroBaseSDK
//
//  Created by 魏太山 on 2020/10/22.
//

#import <Foundation/Foundation.h>

//! Project version number for HeroBaseSDK.
FOUNDATION_EXPORT double HeroBaseSDKVersionNumber;

//! Project version string for HeroBaseSDK.
FOUNDATION_EXPORT const unsigned char HeroBaseSDKVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <HeroBaseSDK/PublicHeader.h>

//HBReachability
#import <HeroBaseSDK/HBNetWorkReachability.h>

//HBIQKeyboardManager
#import <HeroBaseSDK/HBIQKeyboardManager.h>
#import <HeroBaseSDK/HBIQKeyboardReturnKeyHandler.h>
#import <HeroBaseSDK/HBIQNSArray+Sort.h>
#import <HeroBaseSDK/HBIQUIScrollView+Additions.h>
#import <HeroBaseSDK/HBIQUITextFieldView+Additions.h>
#import <HeroBaseSDK/HBIQUIView+Hierarchy.h>
#import <HeroBaseSDK/HBIQUIViewController+Additions.h>
#import <HeroBaseSDK/HBIQKeyboardManagerConstants.h>
#import <HeroBaseSDK/HBIQKeyboardManagerConstantsInternal.h>
#import <HeroBaseSDK/HBIQTextView.h>
#import <HeroBaseSDK/HBIQBarButtonItem.h>
#import <HeroBaseSDK/HBIQPreviousNextView.h>
#import <HeroBaseSDK/HBIQTitleBarButtonItem.h>
#import <HeroBaseSDK/HBIQToolbar.h>
#import <HeroBaseSDK/HBIQUIView+HBIQKeyboardToolbar.h>

//HBModel
#import <HeroBaseSDK/HBMJExtension.h>
#import <HeroBaseSDK/HBMJExtensionConst.h>
#import <HeroBaseSDK/HBMJFoundation.h>
#import <HeroBaseSDK/HBMJProperty.h>
#import <HeroBaseSDK/HBMJPropertyKey.h>
#import <HeroBaseSDK/HBMJPropertyType.h>
#import <HeroBaseSDK/NSObject+HBMJClass.h>
#import <HeroBaseSDK/NSObject+HBMJCoding.h>
#import <HeroBaseSDK/NSObject+HBMJKeyValue.h>
#import <HeroBaseSDK/NSString+HBMJExtension.h>
#import <HeroBaseSDK/NSObject+HBMJProperty.h>

//HBGTMBase64
#import <HeroBaseSDK/HBGTMDefines.h>
#import <HeroBaseSDK/HBGTMBase64.h>

//HBWebImage
#import <HeroBaseSDK/HBAnimatedImage.h>
#import <HeroBaseSDK/HBAnimatedImagePlayer.h>
#import <HeroBaseSDK/HBAnimatedImageRep.h>
#import <HeroBaseSDK/HBAnimatedImageView.h>
#import <HeroBaseSDK/HBAnimatedImageView+WebCache.h>
#import <HeroBaseSDK/HBDiskCache.h>
#import <HeroBaseSDK/HBGraphicsImageRenderer.h>
#import <HeroBaseSDK/HBImageAPNGCoder.h>
#import <HeroBaseSDK/HBImageAWebPCoder.h>
#import <HeroBaseSDK/HBImageCache.h>
#import <HeroBaseSDK/HBImageCacheConfig.h>
#import <HeroBaseSDK/HBImageCacheDefine.h>
#import <HeroBaseSDK/HBImageCachesManager.h>
#import <HeroBaseSDK/HBImageCoder.h>
#import <HeroBaseSDK/HBImageCoderHelper.h>
#import <HeroBaseSDK/HBImageCodersManager.h>
#import <HeroBaseSDK/HBImageFrame.h>
#import <HeroBaseSDK/HBImageGIFCoder.h>
#import <HeroBaseSDK/HBImageGraphics.h>
#import <HeroBaseSDK/HBImageHEICCoder.h>
#import <HeroBaseSDK/HBImageIOAnimatedCoder.h>
#import <HeroBaseSDK/HBImageIOCoder.h>
#import <HeroBaseSDK/HBImageLoader.h>
#import <HeroBaseSDK/HBImageLoadersManager.h>
#import <HeroBaseSDK/HBImageTransformer.h>
#import <HeroBaseSDK/HBMemoryCache.h>
#import <HeroBaseSDK/HBWebImageCacheKeyFilter.h>
#import <HeroBaseSDK/HBWebImageCacheSerializer.h>
#import <HeroBaseSDK/HBWebImageCompat.h>
#import <HeroBaseSDK/HBWebImageDefine.h>
#import <HeroBaseSDK/HBWebImageDownloader.h>
#import <HeroBaseSDK/HBWebImageDownloaderConfig.h>
#import <HeroBaseSDK/HBWebImageDownloaderDecryptor.h>
#import <HeroBaseSDK/HBWebImageDownloaderOperation.h>
#import <HeroBaseSDK/HBWebImageDownloaderRequestModifier.h>
#import <HeroBaseSDK/HBWebImageDownloaderResponseModifier.h>
#import <HeroBaseSDK/HBWebImageError.h>
#import <HeroBaseSDK/HBWebImageIndicator.h>
#import <HeroBaseSDK/HBWebImageManager.h>
#import <HeroBaseSDK/HBWebImageOperation.h>
#import <HeroBaseSDK/HBWebImageOptionsProcessor.h>
#import <HeroBaseSDK/HBWebImagePrefetcher.h>
#import <HeroBaseSDK/HBWebImageTransition.h>
#import <HeroBaseSDK/NSButton+HBWebCache.h>
#import <HeroBaseSDK/NSData+HBImageContentType.h>
#import <HeroBaseSDK/NSImage+HBCompatibility.h>
#import <HeroBaseSDK/UIButton+HBWebCache.h>
#import <HeroBaseSDK/UIImage+HBExtendedCacheData.h>
#import <HeroBaseSDK/UIImage+HBForceDecode.h>
#import <HeroBaseSDK/UIImage+HBGIF.h>
#import <HeroBaseSDK/UIImage+HBMemoryCacheCost.h>
#import <HeroBaseSDK/UIImage+HBMetadata.h>
#import <HeroBaseSDK/UIImage+HBMultiFormat.h>
#import <HeroBaseSDK/UIImage+HBTransform.h>
#import <HeroBaseSDK/UIImageView+HBHighlightedWebCache.h>
#import <HeroBaseSDK/UIImageView+HBWebCache.h>
#import <HeroBaseSDK/UIView+HBWebCache.h>
#import <HeroBaseSDK/UIView+HBWebCacheOperation.h>
#import <HeroBaseSDK/HBAssociatedObject.h>
#import <HeroBaseSDK/HBAsyncBlockOperation.h>
#import <HeroBaseSDK/HBDeviceHelper.h>
#import <HeroBaseSDK/HBDisplayLink.h>
#import <HeroBaseSDK/HBFileAttributeHelper.h>
#import <HeroBaseSDK/HBImageAssetManager.h>
#import <HeroBaseSDK/HBImageCachesManagerOperation.h>
#import <HeroBaseSDK/HBImageIOAnimatedCoderInternal.h>
#import <HeroBaseSDK/HBInternalMacros.h>
#import <HeroBaseSDK/HBWeakProxy.h>
#import <HeroBaseSDK/HBWebImageTransitionInternal.h>
#import <HeroBaseSDK/NSBezierPath+HBSDRoundedCorners.h>
#import <HeroBaseSDK/UIColor+HBHexString.h>



//HBGZip
#import <HeroBaseSDK/HBGzipUtility.h>

//HBAttributedLabel
#import <HeroBaseSDK/HBAttributedLabel.h>

//HBKeychain
#import <HeroBaseSDK/HBKeychain.h>
#import <HeroBaseSDK/HBKeychainQuery.h>

//HBNetwork
#import <HeroBaseSDK/HBNetworking.h>
#import <HeroBaseSDK/HBCompatibilityMacros.h>
#import <HeroBaseSDK/HBHTTPSessionManager.h>
#import <HeroBaseSDK/HBNetworkReachabilityManager.h>
#import <HeroBaseSDK/HBSecurityPolicy.h>
#import <HeroBaseSDK/HBURLRequestSerialization.h>
#import <HeroBaseSDK/HBURLResponseSerialization.h>
#import <HeroBaseSDK/HBURLSessionManager.h>

//Categories
#import <HeroBaseSDK/HBSynthesizeSingleton_ARC.h>
#import <HeroBaseSDK/UIImageView+HBGif.h>
#import <HeroBaseSDK/NSData+HBAESCrypt.h>
#import <HeroBaseSDK/NSString+HBAESCrypt.h>
#import <HeroBaseSDK/UIButton+HBEdgeInsets.h>
#import <HeroBaseSDK/UIColor+HBExtension.h>
#import <HeroBaseSDK/UIImage+HBColor.h>
#import <HeroBaseSDK/UIView+HBExtension.h>
#import <HeroBaseSDK/NSString+HBBase64.h>
#import <HeroBaseSDK/NSString+HBMD5.h>
#import <HeroBaseSDK/NSString+HBExtension.h>



