//
//  GamePlatformDefines.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/11/9.
//  Copyright © 2020 Hero. All rights reserved.
//

#import <Foundation/Foundation.h>

/******************************************/
/*******************返回码******************/
/******************************************/
#ifndef __GAME_PUBLIC_PLATFORM_CODES_H__
#define __GAME_PUBLIC_PLATFORM_CODES_H__

// 0, 操作成功
extern const int GAME_PUBLIC_CODE_SUCCESS;

// -1000, 没有设置project ID或者Product Id
extern const int GAME_PUBLIC_CODE_NOT_SET_PROJECT_ID_OR_GAME_ID;

// -1001, 没有设置用户名
extern const int GAME_PUBLIC_CODE_NOT_SET_USERNAME;

// -1200, 单机游戏不允许调用此接口
extern const int GAME_PUBLIC_CODE_NOT_ALLOWED_FOR_STANDALONE_GAME;


/*****************Login错误的Code****************/
// 1, 用户不存在
extern const int GAME_PUBLIC_CODE_LOGIN_ERROR_USER_NOT_EXIST;
// 2, 密码错误
extern const int GAME_PUBLIC_CODE_LOGIN_ERROR_WRONG_PASSWORD;
// 3. 第三方登陆 授权失败
extern const int GAME_PUBLIC_CODE_LOGIN_THIRD_PARTY_AUTH_FAILED;


/*****************Register错误的Code*************/
// 2, 注册的账号已经存在
extern const int GAME_PUBLIC_CODE_REGISTER_USER_ALREADY_EXIST;


/*********************平台退出Code****************/
// 从Login模块关闭game平台
extern const int GAME_PUBLIC_CODE_PLATFORM_EXIT_LOGIN;

// 从Payment模块关闭game平台
extern const int GAME_PUBLIC_CODE_PLATFORM_EXIT_PAYMENT;

// 从Exchange模块关闭game平台
extern const int GAME_PUBLIC_CODE_PLATFORM_EXIT_EXCHANGE;

#endif

/******************************************/
/********************TAGS******************/
/******************************************/
#ifndef __GAME_PUBLIC_PLATFORM_TAGS_H__
#define __GAME_PUBLIC_PLATFORM_TAGS_H__

// Login错误Code
extern NSString * const GAME_PUBLIC_TAG_LOGIN_ERROR_CODE;

// Login是否成功的key
extern NSString * const GAME_PUBLIC_TAG_LOGIN_IS_SUCCESS;

// Login成功的AccessCode
extern NSString * const GAME_PUBLIC_TAG_ACCESS_CODE;

// Login成功的AccessToken
extern NSString * const GAME_PUBLIC_TAG_ACCESS_TOKEN;

// Login成功的AccessToken
extern NSString * const GAME_PUBLIC_TAG_HEROU_ACCESS_TOKEN;

// 支付成功的对象的key
extern NSString * const GAME_PUBLIC_TAG_PAYMENT_ORDER;

// 支付对象的Price
extern NSString * const GAME_PUBLIC_TAG_PAYMENT_PRICE;

// 支付的对象的Currency
extern NSString * const GAME_PUBLIC_TAG_PAYMENT_CURRENCY;

// 上报小票状态
extern NSString * const GAME_PUBLIC_TAG_RECEIPTS_STATUS;

// Register错误Code
extern NSString * const GAME_PUBLIC_TAG_REGISTER_ERROR_CODE;

// Register是否成功的key
extern NSString * const GAME_PUBLIC_TAG_REGISTER_IS_SUCCESS;

// Iap购买是否成功的key
extern NSString * const GAME_PUBLIC_TAG_IAPPURCHASE_IS_SUCCESS;

// Game平台从何处退出的Key
extern NSString * const GAME_PUBLIC_TAG_PLATFORM_EXIT_CODE;

#endif

/******************************************/
/********************CP回调*******************/
/******************************************/

// 下线结果
extern NSString * const GAME_PUBLIC_CP_KICKOFF_RESULT;
// 下线原因
extern NSString * const GAME_PUBLIC_CP_KICKOFF_REASON;

/******************************************/
/********************通知*******************/
/******************************************/
#ifndef __GAME_PUBLIC_PLATFORM_NOTIFICATIONS_H__
#define __GAME_PUBLIC_PLATFORM_NOTIFICATIONS_H__

// 平台退出时的通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_PLATFORM_EXIT;

// 登录的通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_LOGIN;

// 注册的通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_REGISTER;

//切换账号的通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_CHANGE_ACCOUNT;

//IAP内购的通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_IAPPURCHASE_FINISH;

// 登录失效通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_LOGONINVALID;

// 点击同意协议时通知
extern NSString * const GAME_PUBLIC_NOTIFICATION_NAME_PROTOCOL;
#endif


/******************************************/
/********************Enum******************/
/******************************************/
#ifndef __GAME_PUBLIC_PLATFORM_ENUMS_H__
#define __GAME_PUBLIC_PLATFORM_ENUMS_H__

/******************国家地区****************/
typedef NS_ENUM(NSUInteger, GAME_PUBLIC_COUNTRY_AREA_CODE)
{
    GAME_PUBLIC_COUNTRY_AREA_CODE_UNKOWN     = 0,

    GAME_PUBLIC_COUNTRY_CODE_THAILAND,
    GAME_PUBLIC_COUNTRY_CODE_KOREA,
    GAME_PUBLIC_COUNTRY_CODE_RUSSIA,

    GAME_PUBLIC_AREA_CODE_TAIWAN,
};
/**
 浮标状态类型
 
 - GAME_PUBLICT_FLOAT_CODE_OPEN: 进入浮标二级页面
 - GAME_PUBLICT_FLOAT_CODE_CLOSE: 退出浮标二级页面
 - GAME_PUBLICT_FLOAT_CODE_CLOSE: 关闭浮标
 */
typedef NS_ENUM(NSInteger,GAME_PUBLICT_FLOAT_CODE)
{
    GAME_PUBLICT_FLOAT_CODE_OPEN   =0,
    GAME_PUBLICT_FLOAT_CODE_CLOSE  =1,
    GAME_PUBLICT_FLOAT_CODE_EXIT   =2,
};

#endif


