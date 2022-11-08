using GoogleMobileAds.Api;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PikoruaTest
{
    public class AdsManager : MonoBehaviour
    {
        public static AdsManager instance;
        public InterstitialAd interstitialAd;
        public RewardedAd rewardedAd;

        [Label("The ad ID are only test ID")]
        public string intersitialAdId, rewardedAdId;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }


        #region Intersitial Ad
        public void RequestIntersitial()
        {
            interstitialAd = new InterstitialAd(intersitialAdId);
            interstitialAd.OnAdLoaded += HandleInterstitialLoaded;
            interstitialAd.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
            interstitialAd.OnAdOpening += HandleInterstitialOpened;
            interstitialAd.OnAdClosed += HandleInterstitialClosed;

            AdRequest adRequest = new AdRequest.Builder().Build();
            interstitialAd.LoadAd(adRequest);
        }

        public void ShowInterstitial()
        {
            if (interstitialAd.IsLoaded())
            {
                interstitialAd.Show();
            }
        }

        #region Interstitial callback handlers

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialLoaded event received");
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {

        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialOpened event received");
        }

        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialClosed event received");
            RequestIntersitial();
        }

        #endregion

        #endregion

        #region Rewarded Ad

        public void RequestRewarded()
        {             
            rewardedAd = new RewardedAd(rewardedAdId);

            // Called when an ad request has successfully loaded.
            rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            rewardedAd.OnAdClosed += HandleRewardedAdClosed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            rewardedAd.LoadAd(request);

            Debug.Log(rewardedAd.IsLoaded());
        }

        public void ShowRewarded()
        {
            if (rewardedAd == null)
                return;
            if (!rewardedAd.IsLoaded())
                return;

            rewardedAd.Show();
        }

        public void CloseRewarded()
        {
            rewardedAd.Destroy();
        }

        #region RewardedAd callback handlers

        public void HandleRewardedAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdLoaded event received");
        }

        public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log(
                "HandleRewardedAdFailedToLoad event received with message: " + args.LoadAdError.GetMessage());
        }

        public void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdOpening event received");
            AdsManager.instance.rewardOpen = true;
        }

        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.AdError.GetMessage());
        }

        public void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdClosed event received");
            
            AdsManager.instance.rewardClosed = true;
        }
        public void HandleRewardedAdStarted(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdStarted event received");

        }

        public void HandleUserEarnedReward(object sender, Reward args)
        {
            string type = args.Type;
            double amount = args.Amount;

            AdsManager.instance.getReward = true;

            Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

        }

        #endregion



        #endregion

        [Header("For rewarded ad callbacks")]
        public bool getReward = false;
        public bool rewardOpen = false;
        public bool rewardClosed = false;
        private void Update()
        {
            if (getReward)
            {
                getReward = false;
                //invoke when you got a reward
            }
            if (rewardOpen)
            {
                rewardOpen = false;
                //invoke when reward opened
            }
            if (rewardClosed)
            {
                rewardClosed = false;
                //invoke when reward closed
            }
        }
    }
}

