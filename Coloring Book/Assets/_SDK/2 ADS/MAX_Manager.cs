
using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAX_Manager : AdsManager, IAOACallback
{
    [Header("ADS Key - Applovin")]
    [SerializeField] string maxSdkKey = "FkIINe06ZPB_SS55Bhv7WdUGzgWIf6LLRw88mxB1LxAh6qhSkQnHZxsdarNaqSIJCfADTdKXV2xQN0xyv-Tvao";
    [SerializeField] string bannerID = "";
    [SerializeField] string interID = "";
    [SerializeField] string rewardedID = "";

    #region Init
    public override void Init()
    {
        base.Init();
        MaxSdkCallbacks.OnSdkInitializedEvent += (res) => { OnSdkInitializedEvent(); };
        MaxSdk.SetSdkKey(maxSdkKey);
        MaxSdk.InitializeSdk();
        googleAD.Init(InitAOACallback);
    }
    #endregion
    #region Banner

    public override void InitBannerCallback()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += (str, inf) => { OnBannerLoadedEvent(); };
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (str, err) => { OnBannerLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        LoadBanner();

    }
    public override void LoadBanner()
    {

        if (bannerID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.BANNER_AD_ON_OFF) return;
        MaxSdk.CreateBanner(bannerID, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(bannerID, "adaptive_banner", IsPortrait ? "false" : "true");
        MaxSdk.SetBannerBackgroundColor(bannerID, IsPortrait ? Color.black : Color.clear);

    }
    bool bannerShowing = false;

    public override void RefreshBanner()
    {
        StartCoroutine(IERefreshBanner());
    }
    IEnumerator IERefreshBanner()
    {
        HideBanner();
        yield return new WaitForSeconds(3 * Time.deltaTime);
        ShowBanner();
    }
    public override void ShowBanner()
    {
        if (bannerShowing) return;
        bannerShowing = true;
        if (bannerID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.BANNER_AD_ON_OFF) return;
        if (IsPortrait)
        {
            googleAD.LoadBanner();
            Invoke(nameof(ShowMaxBannerAndRefresh), 6);
        }
        else
        {
            ShowMaxBanner();
        }
    }
    public override void ShowMaxBanner()
    {
        googleAD.HideBanner();
        MaxSdk.ShowBanner(bannerID);
    }
    public void ShowMaxBannerAndRefresh()
    {
        googleAD.HideBanner();
        MaxSdk.ShowBanner(bannerID);
        Invoke(nameof(RefreshBanner), FirebaseRemoteData.TIME_REFRESH_BANNER);
    }
    public override void HideBanner()
    {
        bannerShowing = false;
        CancelInvoke(nameof(ShowMaxBannerAndRefresh));
        CancelInvoke(nameof(RefreshBanner));
        MaxSdk.HideBanner(bannerID);
        googleAD.HideBanner();
    }
    #endregion
    #region Interstitial
    public override void InitInterstitialCallback()
    {

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += (placament, infor) => { OnInterstitialLoadedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += (placament, err) => { OnInterstitialLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += (placament, infor) => { OnInterstitialDisplayedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += (placament, err, infor) => { OnInterstitialDisplayFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += (placament, infor) => { OnInterstitialClickedEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += (placament, infor) => { OnInterstitialHiddenEvent(); };
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };

        Invoke(nameof(LoadInterstitial), 0.5f);
    }
    public override void LoadInterstitial()
    {
        if (IsRemoveAds) return;
        MaxSdk.LoadInterstitial(interID);
    }

    public override void ShowInterstitial(string placementInter, bool useCounter = true, bool ignoreCapping = false)
    {
        this.placementInter = placementInter;
        if (IsRemoveAds) { return; }
        if (interShowing) { return; }
        if (!FirebaseRemoteData.INTER_AD_ON_OFF) { return; }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_CALL_SHOW);
        DateTime now = DateTime.Now;
        if (!ignoreCapping && (now - timeCloseIntersAds).TotalSeconds < FirebaseRemoteData.INTER_AD_CAPPING_TIME) { return; }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_PASSED_CAPPING_TIME);
        if (!MaxSdk.IsInterstitialReady(interID)) { return; }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_INTERS_AVAILABLE);
        interShowing = true;
        MaxSdk.ShowInterstitial(interID);

    }
    #endregion
    #region Rewarded
    public override void InitRewardedCallback()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += (placament, infor) => { OnRewardedLoadedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += (placament, err) => { OnRewardedLoadFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += (placament, err, infor) => { OnRewardedDisplayFailedEvent((int)err.Code); };
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += (placament, infor) => { OnRewardedDisplayedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += (placament, infor) => { OnRewardedClickedEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += (placament, infor) => { OnRewardedHiddenEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += (placament, reward, adInfo) => { OnAdReceivedRewardEvent(); };
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        Invoke(nameof(LoadRewarded), 1.5f);
    }
    public override bool IsRewardedAdReady()
    {
        return MaxSdk.IsRewardedAdReady(rewardedID);
    }
    public override void LoadRewarded()
    {
        if (rewardedID == "") return;
        MaxSdk.LoadRewardedAd(rewardedID);
    }

    public override void ShowRewarded(Action callbackReward, string placementReward, Action onNoADS = null)
    {
        this.placementReward = placementReward;
        this.callbackReward = callbackReward;
        if (rewardedID == "") return;
        if (isTestReward)
        {
            callbackReward.Invoke();
            return;
        }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_CALL_SHOW);
        if (!MaxSdk.IsRewardedAdReady(rewardedID))
        {
            onNoADS?.Invoke();
            return;
        }
        HandleAppsflyer.Instance.LogEventWithName(HandleAppsflyer.AF_REWARDED_AVAILABLE);


        rewardShowing = true;
        MaxSdk.ShowRewardedAd(rewardedID);
    }
    #endregion
    #region AOA

    public override void InitAOACallback()
    {
        googleAD.SetAOACallback(this);
    }
    public override void LoadAOA()
    {
        if (IsRemoveAds) return;
        googleAD.LoadAOA();
    }
    public override void ShowFirstAOA()
    {
        if (AoaShowSuccess) return;
        if (isRemoveAds)
        {
            SetAoaShowSuccess(true);
            return;
        }
        if (!FirebaseRemoteData.OPEN_AD_ON_OFF) { SetAoaShowSuccess(true); return; }
        if (aoaShowing)
        {
            return;
        }
        if (!googleAD.IsAOAAvailable)
        {
            SetAoaShowSuccess(true);
            return;
        }
        aoaShowing = true;
        googleAD.ShowAOA();
    }
    public override void ShowAOA(bool irgoneCampingTime = false)
    {
        if (isRemoveAds) return;
        Debug.Log("AOA 2");
        if (interShowing) return;
        Debug.Log("AOA 3");
        if (rewardShowing) return;
        Debug.Log("AOA 4");
        if (!FirebaseRemoteData.OPEN_AD_ON_OFF) { return; }
        Debug.Log("AOA 5");
        if (aoaShowing) return;
        Debug.Log("AOA 6");
        if (!irgoneCampingTime && (DateTime.Now - timeOutGame).TotalSeconds < FirebaseRemoteData.OPEN_AD_CAPPING_TIME) return;
        Debug.Log("AOA 7");
        //if (OpenGameCount == 0 && onPauseCount == 0) return;
        //Debug.Log("AOA 8");

        if (!googleAD.IsAOAAvailable) return;
        Debug.Log("AOA 9");
        aoaShowing = true;
        googleAD.ShowAOA();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            onPauseCount++;
            timeOutGame = DateTime.Now;
        }
        else
        {
            ShowAOA();
        }
    }
    #endregion
    #region MREC
    public override void InitMRECCallback()
    {
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += (id, infor) => { OnMRECLoadedEvent(); };
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += (id, infor) => { OnMRECLoadFailedEvent((int)infor.Code); };
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (placament, infor) => { OnAdRevenuePaidEvent(new RevalueInfor("AppLovin", infor.NetworkName, infor.AdUnitIdentifier, infor.Placement, infor.Revenue, "USD")); };
        CreateMREC();
    }
    void CreateMREC()
    {
        if (mrecID == "") return;
        if (IsRemoveAds) return;
        var pos = MaxSdkBase.AdViewPosition.TopCenter;
        if (MRECPosition == EMRecPosition.TopCenter)
        {
            pos = MaxSdkBase.AdViewPosition.TopCenter;
            var density = MaxSdkUtils.GetScreenDensity();
            float x = Screen.width / 2;
            float y = 100f;
            x = x / density - 150;
            y = y / density;
            MaxSdk.CreateMRec(mrecID, x, y);
        }
        else if (MRECPosition == EMRecPosition.BottomCenter)
        {
            pos = MaxSdkBase.AdViewPosition.BottomCenter;
            MaxSdk.CreateMRec(mrecID, pos);
        }
    }
    public override void LoadMREC()
    {
        if (mrecID == "") return;
        if (!FirebaseRemoteData.MREC_AD_ON_OFF) return;
        if (IsRemoveAds) return;
        MaxSdk.LoadMRec(mrecID);
    }

    public override void ShowMREC()
    {
        if (mrecID == "") return;
        if (IsRemoveAds) return;
        if (!FirebaseRemoteData.MREC_AD_ON_OFF) return;
        MaxSdk.ShowMRec(mrecID);
    }

    public override void HideMREC()
    {
        if (mrecID == "") return;
        MaxSdk.HideMRec(mrecID);
    }






    public override void UpdateMRecPosition(EMRecPosition mrecPosition)
    {
        MaxSdkBase.AdViewPosition pos = (MaxSdkBase.AdViewPosition)(int)mrecPosition;
        MaxSdk.UpdateMRecPosition(MREC_ID, pos);
    }

    public override void ShowImageInter(Action callback)
    {
        if (IsRemoveAds)
        {

            callback?.Invoke();
            return;
        }


        googleAD.ShowInter(callback);
    }


    #endregion
}
