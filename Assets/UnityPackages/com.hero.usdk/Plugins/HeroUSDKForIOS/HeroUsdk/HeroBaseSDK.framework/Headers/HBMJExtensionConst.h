
#ifndef __HBMJExtensionConst__H__
#define __HBMJExtensionConst__H__

#import <Foundation/Foundation.h>

#ifndef HBMJ_LOCK
#define HBMJ_LOCK(lock) dispatch_semaphore_wait(lock, DISPATCH_TIME_FOREVER);
#endif

#ifndef HBMJ_UNLOCK
#define HBMJ_UNLOCK(lock) dispatch_semaphore_signal(lock);
#endif

// 信号量
#define HBMJExtensionSemaphoreCreate \
static dispatch_semaphore_t signalSemaphore; \
static dispatch_once_t onceTokenSemaphore; \
dispatch_once(&onceTokenSemaphore, ^{ \
    signalSemaphore = dispatch_semaphore_create(1); \
});

#define HBMJExtensionSemaphoreWait HBMJ_LOCK(signalSemaphore)
#define HBMJExtensionSemaphoreSignal HBMJ_UNLOCK(signalSemaphore)

// 过期
#define HBMJExtensionDeprecated(instead) NS_DEPRECATED(2_0, 2_0, 2_0, 2_0, instead)

// 构建错误
#define HBMJExtensionBuildError(clazz, msg) \
NSError *error = [NSError errorWithDomain:msg code:250 userInfo:nil]; \
[clazz setMj_error:error];

// 日志输出
#ifdef DEBUG
#define HBMJExtensionLog(...) NSLog(__VA_ARGS__)
#else
#define HBMJExtensionLog(...)
#endif

/**
 * 断言
 * @param condition   条件
 * @param returnValue 返回值
 */
#define HBMJExtensionAssertError(condition, returnValue, clazz, msg) \
[clazz setMj_error:nil]; \
if ((condition) == NO) { \
    HBMJExtensionBuildError(clazz, msg); \
    return returnValue;\
}

#define HBMJExtensionAssert2(condition, returnValue) \
if ((condition) == NO) return returnValue;

/**
 * 断言
 * @param condition   条件
 */
#define HBMJExtensionAssert(condition) HBMJExtensionAssert2(condition, )

/**
 * 断言
 * @param param         参数
 * @param returnValue   返回值
 */
#define HBMJExtensionAssertParamNotNil2(param, returnValue) \
HBMJExtensionAssert2((param) != nil, returnValue)

/**
 * 断言
 * @param param   参数
 */
#define HBMJExtensionAssertParamNotNil(param) HBMJExtensionAssertParamNotNil2(param, )

/**
 * 打印所有的属性
 */
#define MJLogAllIvars \
- (NSString *)description \
{ \
    return [self hb_keyValues].description; \
}
#define HBMJExtensionLogAllProperties MJLogAllIvars

/** 仅在 Debugger 展示所有的属性 */
#define MJImplementDebugDescription \
- (NSString *)debugDescription \
{ \
return [self hb_keyValues].debugDescription; \
}

/**
 *  类型（属性类型）
 */
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeInt;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeShort;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeFloat;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeDouble;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeLong;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeLongLong;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeChar;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeBOOL1;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeBOOL2;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypePointer;

FOUNDATION_EXPORT NSString *const HBMJPropertyTypeIvar;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeMethod;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeBlock;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeClass;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeSEL;
FOUNDATION_EXPORT NSString *const HBMJPropertyTypeId;

#endif
