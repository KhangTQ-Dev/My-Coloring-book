using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAOACallback
{
    void OnAOALoadedEvent();
    void OnAOAFullScreenContentOpened();
    void OnAOAHiddenEvent();
    void OnAOALoadFailedEvent(string err);
    void OnAOAShowFailedEvent(string err);
    void OnAOAPaidEvent(long value, string currencyCode);
}
public class GoogleAD : MonoBehaviour
{
    [SerializeField] string aoaID;
    [SerializeField] string bannerID;
    [SerializeField] string interID;
    [SerializeField] bool _isInitialized;
    [SerializeField] bool on_off_collap_banner;
    [SerializeField] IAOACallback aoaCallback;

    public void SetAOACallback(IAOACallback value)
    {
        aoaCallback = value;
    }

    public string AOAID => aoaID;
    public void SetAOAID(string aoaID)
    {
        this.aoaID = aoaID;
    }

    public void SetCollapBannerID(string collapBannerId)
    {
        this.bannerID = collapBannerId;
    }

    public void SetCollapBannerOnOff(bool isOn)
    {
        this.on_off_collap_banner = isOn;
    }

    public bool IsInitialized => _isInitialized;
    public void Init(params Action[] onLoads)
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize((value) =>
        {
            _isInitialized = true;
            for (int i = 0; i < onLoads.Length; i++) onLoads[i].Invoke();

            InitBanner();
            InitAOA();
            InitInter();

        });
    }
    #region collab banner
    private BannerView _bannerView;
    public void InitBanner() { }

    void CreateBannerView()
    {
        if (_bannerView != null)
        {
            DestroyBanner();
        }
        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        _bannerView = new BannerView(bannerID, adaptiveSize, AdPosition.Bottom);
        _bannerView.OnBannerAdLoaded += () => { };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) => { AdsManager.Instance.ShowMaxBanner(); };
        _bannerView.OnAdPaid += (infor) => { AdsManager.Instance.OnAdRevenuePaidEvent(new RevalueInfor("Google Admob", "Google Admob", bannerID, "Banner", infor.Value, infor.CurrencyCode)); };

    }
    public void LoadBanner()
    {
        if (bannerID == "") return;
        if (!_isInitialized) return;
        //if (!on_off_collap_banner) return;
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        var adRequest = new AdRequest();
        adRequest.Extras.Add("collapsible", "bottom");
        _bannerView.LoadAd(adRequest);
        _bannerView.Show();
    }
    public void ShowBanner()
    {
        //if (!on_off_collap_banner) return;

        if (_bannerView != null)
        {
            _bannerView.Show();
        }
    }
    //[MyBox.ButtonMethod]
    public void HideBanner()
    {
        //if (!on_off_collap_banner) return;

        if (_bannerView != null)
        {
            DestroyBanner();
            //_bannerView.Hide();
        }
    }
    void DestroyBanner()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    public void OnBannerEvent() { }
    #endregion
    #region inter
    InterstitialAd _interstitialAd;
    Action callbackInter;

    public void InitInter()
    {
        if (interID == "") return;
        LoadInter();
    }
    public void LoadInter()
    {
        if (_interstitialAd != null)
        {
            DestroyInter();
        }
        var adRequest = new AdRequest();
        InterstitialAd.Load(interID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                return;
            }
            if (ad == null)
            {
                return;
            }
            _interstitialAd = ad;
            OnInterEvent();

        });
    }
    void DestroyInter()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

    }
    public void ShowInter(Action callbackInter)
    {
     
        this.callbackInter = callbackInter;
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            callbackInter?.Invoke();
        }
    }
    public void OnInterEvent()
    {
        _interstitialAd.OnAdPaid += (AdValue adValue) => { AdsManager.Instance.OnAdRevenuePaidEvent(new RevalueInfor("Google Admob", "Google Admob", interID, "Inter", adValue.Value, adValue.CurrencyCode)); };
        _interstitialAd.OnAdFullScreenContentClosed += () => { callbackInter?.Invoke(); };
        _interstitialAd.OnAdFullScreenContentFailed += (err) => { callbackInter?.Invoke(); };

    }
    #endregion
    #region AOA
    AppOpenAd _appOpenAd;
    public bool IsAOAAvailable => _appOpenAd != null && _appOpenAd.CanShowAd();
    public void InitAOA()
    {
        AdsManager.Instance.LoadAOA();
    }
    public void LoadAOA()
    {
        if (aoaID == "") return;
        if (_appOpenAd != null) DestroyAOA();
        var adRequest = new AdRequest();
#if UNITY_EDITOR
        AppOpenAd.Load(aoaID, ScreenOrientation.LandscapeLeft, adRequest, OnAppOpenAdLoad);
#else
        AppOpenAd.Load(aoaID, adRequest, OnAppOpenAdLoad);
#endif
    }
    void OnAppOpenAdLoad(AppOpenAd ad, LoadAdError error)
    {
        if (ad == null || error != null)
        {
            aoaCallback.OnAOALoadFailedEvent(error.GetCode().ToString());
            return;
        }
        aoaCallback.OnAOALoadedEvent();
        _appOpenAd = ad;
        AOACallback();
    }
    void DestroyAOA()
    {
        if (_appOpenAd != null)
        {
            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

    }
    public void ShowAOA()
    {
        if (aoaID == "") return;
        _appOpenAd.Show();
    }
    void AOACallback()
    {
        _appOpenAd.OnAdFullScreenContentOpened += aoaCallback.OnAOAFullScreenContentOpened;
        _appOpenAd.OnAdFullScreenContentFailed += (err) => { aoaCallback.OnAOAShowFailedEvent(err.GetCode().ToString()); };
        _appOpenAd.OnAdFullScreenContentClosed += () => { aoaCallback.OnAOAHiddenEvent(); };
        _appOpenAd.OnAdPaid += (value) => { aoaCallback.OnAOAPaidEvent(value.Value, value.CurrencyCode); };
    }
    #endregion
}
