using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Ads
{
    public class GoogleAds : MonoBehaviour
    {
        // [SerializeField] private string[] rewardAds,fScreenAds,bannerAds;
        // private List<RewardedAd> rewards = new List<RewardedAd>();
        // private List<InterstitialAd> fScreens = new List<InterstitialAd>();
        // private List<BannerView> banners = new List<BannerView>();
        // private AdRequest adRequest;

        // private void Awake()
        // {
        //     MobileAds.Initialize( adStat => { } );
        //
        //     adRequest = new AdRequest.Builder().Build();
        // }
        //
        // private void Start()
        // {
        //
        //     foreach (var reward in rewardAds)
        //     {
        //         rewards.Add(
        //             new RewardedAd(reward)
        //         );
        //         
        //     }
        //
        //     LoadRewardAd(null, null);
        //
        //     foreach (var ad in fScreenAds)
        //     {
        //         fScreens.Add(new InterstitialAd(ad)
        //             );
        //     }
        //     LoadFullScreen(null, null);
        //
        //
        //     if (bannerAds.Length > 0)
        //     {
        //         foreach (var banner in bannerAds)
        //         {
        //             banners.Add(new BannerView(banner,AdSize.Banner, AdPosition.Top));
        //         }
        //     
        //         foreach (var banner in banners)
        //         {
        //             banner.Show();
        //         }
        //     }
        //
        //
        //     foreach (var reward in rewards)
        //     {
        //         // Called when an ad request failed to load.
        //         reward.OnAdFailedToLoad += NotConnection;
        //         // Called when an ad is shown.
        //         reward.OnAdFailedToShow += ErrorAd;
        //         // Called when the user should be rewarded for interacting with the ad.
        //         reward.OnUserEarnedReward += Prize;
        //         // Called when the ad is closed.
        //         reward.OnAdClosed += LoadRewardAd;
        //         
        //         // reward.OnAdOpening += HandleRewardedAdOpening;
        //         // Called when an ad request failed to show.
        //         // Called when an ad request has successfully loaded.
        //         // reward.OnAdLoaded += LoadRewardAd;
        //     }
        //
        //     foreach (var inter in fScreens)
        //     {
        //         inter.OnAdClosed += LoadFullScreen;
        //     }
        //     
        // }
        //
        //
        //
        // private void Prize(object sender, Reward prize)
        // {
        //     AdsSystem.AdsPrize.Invoke();
        // }
        //
        // private void ErrorAd(object sender, EventArgs arg)
        // {
        //     MenuSystem.OpenWarning.Invoke("NO ADS HERE :(");
        // }
        //
        // private void NotConnection(object sender, EventArgs arg)
        // {
        //     MenuSystem.OpenWarning.Invoke("CHECK YOU INTERNET");
        // }
        //
        // private void ShowRewardAd()
        // {
        //     foreach (var reward in rewards)
        //     {
        //         if (reward.IsLoaded())
        //         {
        //             Debug.Log("GOSTERDI");
        //             reward.Show();
        //             break;
        //         }
        //     }
        //     
        // }
        //
        // private void ShowFullScreen()
        // {
        //     foreach (var fullAd in fScreens)
        //     {
        //         if (fullAd.IsLoaded())
        //         {
        //             Debug.Log("GOSTERDI");
        //             fullAd.Show();
        //             break;
        //         }
        //     }
        //     
        // }
        //
        // private void LoadRewardAd(object sender, EventArgs arg)
        // {
        //     foreach (var reward in rewards)
        //     {
        //         reward.LoadAd(adRequest);
        //     }
        //     
        // }
        //
        // private void LoadFullScreen(object sender, EventArgs eventArgs)
        // {
        //     foreach (var ad in fScreens)
        //     {
        //         ad.LoadAd(adRequest);
        //     }
        // }
        //
        // private void OnEnable()
        // {
        //     AdsSystem.ShowFScreenAdsEvent.AddListener(ShowFullScreen);
        //     AdsSystem.ShowRewardEvent.AddListener(ShowRewardAd);
        // }
        //
        // private void OnDisable()
        // {
        //     AdsSystem.ShowFScreenAdsEvent.RemoveListener(ShowFullScreen);
        //     AdsSystem.ShowRewardEvent.RemoveListener(ShowRewardAd);
        // }
        
    }


}

