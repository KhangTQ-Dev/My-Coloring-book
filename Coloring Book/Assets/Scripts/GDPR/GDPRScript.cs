using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GDPRScript : MonoBehaviour
    {
        public bool ShowGDPRPopupDone = false;
        [SerializeField] bool test;
        public void CallGDPR()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.SetiOSAppPauseOnBackground(true);
#if UNITY_EDITOR
            Invoke(nameof(InitADS), Time.deltaTime);
            return;
#endif
            var debugSettings = new ConsentDebugSettings
            {
                DebugGeography = DebugGeography.EEA,
                TestDeviceHashedIds = new List<string> { "F88118C8CFD2C6CC56FB12458A1EF905", "B481B40EA3991523BEF337A7ABB1E229", "042343634A935EBC371BC1696E2D7826", "0FC6649D954AF2F6297E3991920E151D", "485EB5D6A85267574A67CFA124782889", "2fb3b88c-a5f7-4d42-b660-f081b17b413d" }
            };
            ConsentRequestParameters request = new ConsentRequestParameters { TagForUnderAgeOfConsent = false, };
            if (test)
            {
                request = new ConsentRequestParameters
                {
                    TagForUnderAgeOfConsent = false,
                    ConsentDebugSettings = debugSettings,
                };
            }
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {

                Invoke(nameof(InitADS), Time.deltaTime);
                return;
            }

            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                GameStatic.IsPrivacyOptionsRequired = IsPrivacyOptionsRequired();
                Invoke(nameof(InitADS), Time.deltaTime);
                if (formError != null)
                {
                    UnityEngine.Debug.LogError(consentError);

                    return;
                }

            });
        }
        public bool IsPrivacyOptionsRequired()
        {
            return ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required;
        }

        void InitADS()
        {
            ShowGDPRPopupDone = true;
            AdsManager.Instance.Init();
        }

    }
}
