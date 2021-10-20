/*
* This file is part of the HeroWebImage package.
* (c) Olivier Poitrey <rs@dailymotion.com>
*
* For the full copyright and license information, please view the LICENSE
* file that was distributed with this source code.
*/

#import <HeroBaseSDK/HBWebImageCompat.h>

#if HB_MAC

#import <QuartzCore/QuartzCore.h>

/// Helper method for Core Animation transition
FOUNDATION_EXPORT CAMediaTimingFunction * _Nullable SDTimingFunctionFromAnimationOptions(HeroWebImageAnimationOptions options);
FOUNDATION_EXPORT CATransition * _Nullable HeroTransitionFromAnimationOptions(HeroWebImageAnimationOptions options);

#endif
