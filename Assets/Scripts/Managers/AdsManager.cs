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
        public InterstitialAd interstitial;
        public RewardedAd rewardedAd;

        [Label("The ad ID are only test ID")]
        public string intersitialAdId, rewardedAdId;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        
        void Start()
        {
            //request rewarded
            RequestIntersitial();
            RequestRewarded();
        }


        #region Intersitial Ad
        public void RequestIntersitial()
        {
            interstitial = new InterstitialAd(intersitialAdId);
            interstitial.OnAdLoaded += HandleInterstitialLoaded;
            interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
            interstitial.OnAdOpening += HandleInterstitialOpened;
            interstitial.OnAdClosed += HandleInterstitialClosed;

            AdRequest adRequest = new AdRequest.Builder().Build();
            interstitial.LoadAd(adRequest);
        }

        /// <summary>
        /// Showing interstitial ad, invoked after round is over, if null or failed to load, automatically show answer UI
        /// </summary>
        public void ShowInterstitial()
        {
            if (interstitial == null)
            {
                PlayUIManager.instance.ShowAnswersUI();
                return;
            }
                

            if (!interstitial.IsLoaded())
            {
                PlayUIManager.instance.ShowAnswersUI();
                return;
            }

            interstitial.Show();
            
        }

        #region Interstitial callback handlers

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialLoaded event received");
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("HandleInterstitialFailedToLoad event received");
            AdsManager.instance.RequestIntersitial();
        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialOpened event received");
        }

        /// <summary>
        /// When interstitial is sucessfully loaded and shown, when closing the ad, show answer UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            Debug.Log("HandleInterstitialClosed event received");
            PlayUIManager.instance.ShowAnswersUI();
            interstitial.Destroy();
            AdsManager.instance.RequestIntersitial();
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
            Time.timeScale = 1f;
            RequestRewarded();
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
            AdsManager.instance.RequestRewarded();
        }

        public void HandleRewardedAdOpening(object sender, EventArgs args)
        {
            Debug.Log("HandleRewardedAdOpening event received");
            AdsManager.instance.rewardOpen = true;
        }

        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
        {
            Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.AdError.GetMessage());
            AdsManager.instance.rewardClosed = true;
            AdsManager.instance.RequestRewarded();
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

        /// <summary>
        /// For rewarded ad added this handling because sometimes there are issue in thread dispatcher, especially in iOS build later.
        /// </summary>
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
                GameManager.instance.DoubledReward();
                CloseRewarded();
            }
            if (rewardOpen)
            {
                rewardOpen = false;
                //invoke when reward opened
                Time.timeScale = 0f;
            }
            if (rewardClosed)
            {
                rewardClosed = false;
                //invoke when reward closed
                CloseRewarded();
            }
        }
    }
}

