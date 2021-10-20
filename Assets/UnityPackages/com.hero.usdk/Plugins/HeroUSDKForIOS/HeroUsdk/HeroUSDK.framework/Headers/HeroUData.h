//
//  HeroUData.h
//  HeroUSDK
//
//  Created by 魏太山 on 2020/10/27.
//  Copyright © 2020 Hero. All rights reserved.
//
//  本类负责HeroU【数据】相关操作

#import <Foundation/Foundation.h>

@interface HeroUData : NSObject

@end

@interface HeroHDCBaseGameRoleInfo : NSObject

+ (HeroHDCBaseGameRoleInfo *)sharedInstance;

@property (nonatomic,copy) NSString *channelUserId;//渠道用户ID(玩家登录账号)
@property (nonatomic,copy) NSString *gameUserId;   //游戏生成的账号ID
@property (nonatomic,copy) NSString *serverId;     //区服ID
@property (nonatomic,copy) NSString *serverName;   //区服名称
@property (nonatomic,copy) NSString *roleId;       //角色ID
@property (nonatomic,copy) NSString *roleName;     //角色名
@property (nonatomic,copy) NSString *roleAvatar;   //头像

@end

@interface HeroHDCGameRoleInfo : NSObject

@property (nonatomic,copy  ) NSString     *level;       // 角色等级
@property (nonatomic,copy  ) NSString     *vipLevel;    // VIP等级
@property (nonatomic,copy  ) NSString     *gold1;       // 当前一级货币数量（充值获得）
@property (nonatomic,copy  ) NSString     *gold2;       // 当前二级货币数量（游戏内产出）

//以下参数是助手需要的参数，可不填
@property (nonatomic,copy  ) NSString     *sumPay;      // 累计充值金额

@property (nonatomic,copy  ) NSString     *levelExp;    // 当前经验值
@property (nonatomic,copy  ) NSString     *vipScore;    // VIP积分
@property (nonatomic,copy  ) NSString     *rankLevel;   // 排位等级
@property (nonatomic,copy  ) NSString     *rankExp;     // 排位经验
@property (nonatomic,copy  ) NSString     *rankLeve2;   // 当前赛季排位排名
@property (nonatomic,copy  ) NSString     *rankExp2;    // 当前赛季排位胜场
@property (nonatomic,copy  ) NSString     *cupCount1;   // 驾照等级
@property (nonatomic,copy  ) NSString     *cupCount2;   // 累计充值金额
@property (nonatomic,copy  ) NSString     *totalKill;   // 总击杀数
@property (nonatomic,copy  ) NSString     *totalHead;   // 总爆头数
@property (nonatomic,copy  ) NSString     *avgKD;       // 平均击杀/死亡比
@property (nonatomic,copy  ) NSString     *maxKD;       // 最高击杀死亡比
@property (nonatomic,copy  ) NSString     *maxCK;       // 最高连杀数
@property (nonatomic,copy  ) NSString     *mainWeaponId;// 常用主武器ID
@property (nonatomic,copy  ) NSString     *viceWeaponId;// 常用副武器ID
@property (nonatomic,strong) NSArray      *medalCount;  // 勋章(勋章a,b,c的数量 如@[@"1",@"4",@"3"])
@property (nonatomic,strong) NSArray      *items;       // 所有装备 (传入id数组， 例@[@"1",@"3",@"4"])
@property (nonatomic,strong) NSDictionary *extend;      // 额外数据 (字典类型数据)
@property (nonatomic,copy  ) NSString     *teamId;      // 车队id
@property (nonatomic,copy  ) NSString     *teamName;    // 车队名字
@property (nonatomic,strong) NSArray      *itemComposes;// cp根据游戏具体数据来传必须为（BlocItemCompose对象）
//(类似创造与魔法多角色上传，以最后一个上传角色为准m，显示小恐龙浮标-默认NO(需要显示)，YES(不显示))
@property (nonatomic, assign) BOOL floatHidden ;

@end

