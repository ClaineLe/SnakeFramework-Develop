/*
 * This file is part of the HeroWebImage package.
 * (c) Olivier Poitrey <rs@dailymotion.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

#import <Foundation/Foundation.h>
#import <HeroBaseSDK/HBmetamacros.h>

#ifndef HB_LOCK
#define HB_LOCK(lock) dispatch_semaphore_wait(lock, DISPATCH_TIME_FOREVER);
#endif

#ifndef HB_UNLOCK
#define HB_UNLOCK(lock) dispatch_semaphore_signal(lock);
#endif

#ifndef HB_OPTIONS_CONTAINS
#define HB_OPTIONS_CONTAINS(options, value) (((options) & (value)) == (value))
#endif

#ifndef HB_CSTRING
#define HB_CSTRING(str) #str
#endif

#ifndef HB_NSSTRING
#define HB_NSSTRING(str) @(HB_CSTRING(str))
#endif

#ifndef HB_SEL_SPI
#define HB_SEL_SPI(name) NSSelectorFromString([NSString stringWithFormat:@"_%@", HB_NSSTRING(name)])
#endif

#ifndef weakify
#define weakify(...) \
hb_keywordify \
metamacro_foreach_cxt(hb_weakify_,, __weak, __VA_ARGS__)
#endif

#ifndef strongify
#define strongify(...) \
hb_keywordify \
_Pragma("clang diagnostic push") \
_Pragma("clang diagnostic ignored \"-Wshadow\"") \
metamacro_foreach(hb_strongify_,, __VA_ARGS__) \
_Pragma("clang diagnostic pop")
#endif

#define hb_weakify_(INDEX, CONTEXT, VAR) \
CONTEXT __typeof__(VAR) metamacro_concat(VAR, _weak_) = (VAR);

#define hb_strongify_(INDEX, VAR) \
__strong __typeof__(VAR) VAR = metamacro_concat(VAR, _weak_);

#if DEBUG
#define hb_keywordify autoreleasepool {}
#else
#define hb_keywordify try {} @catch (...) {}
#endif

#ifndef onExit
#define onExit \
hb_keywordify \
__strong hb_cleanupBlock_t metamacro_concat(hb_exitBlock_, __LINE__) __attribute__((cleanup(hb_executeCleanupBlock), unused)) = ^
#endif

typedef void (^hb_cleanupBlock_t)(void);

#if defined(__cplusplus)
extern "C" {
#endif
    void hb_executeCleanupBlock (__strong hb_cleanupBlock_t *block);
#if defined(__cplusplus)
}
#endif
