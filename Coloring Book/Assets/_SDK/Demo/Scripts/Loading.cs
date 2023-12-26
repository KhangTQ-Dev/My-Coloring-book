using DG.Tweening;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace HG.Nam.Demo
{

    public class Loading : MonoBehaviour
    {
        [SerializeField] RectTransform rectTransform;
        [SerializeField]
        GDPRScript gdprScript;
        Vector2 rootSize;
        Sequence sequence;
        private void Start()
        {
            StartCoroutine(IEStart());
        }
        IEnumerator IEStart()
        {
            float fillTime0 = 2;
            float fillTime1 = 2;
            float fillTime2 = 5;
            LoadingStateManager.Instance?.SetText("Checking ATT");
            bool passAge = PlayerPrefs.GetInt("HG_AGE", 0) != 0;
            AdsManager.Instance.SetAoaShowSuccess(passAge);
            if (passAge) AdsManager.Instance.SetMRECPosition(EMRecPosition.BottomCenter);
            yield return new WaitForSeconds(0.25f);
            rootSize = rectTransform.sizeDelta;
            rootSize = new Vector2(932f, rootSize.y);
            rectTransform.sizeDelta = new Vector2(0, rootSize.y);
            rectTransform.DOSizeDelta(new Vector2(0.1f, 1) * rootSize, 0.5f);
            yield return new WaitForSeconds(0.5f);
#if GleyIAPGooglePlay
            HandleIAP.Instance.Initialize();
#endif
            yield return new WaitForSeconds(0.5f);
            CalculateData();
            LoadingStateManager.Instance?.SetText("Admob initialize ...");
            yield return new WaitForSeconds(0.5f);
            gdprScript.CallGDPR();
            rectTransform.DOSizeDelta(new Vector2(0.4f, 1) * rootSize, fillTime0);
            yield return new WaitForSeconds(fillTime0);
            while (!gdprScript.ShowGDPRPopupDone) yield return new WaitForEndOfFrame();
            LoadingStateManager.Instance?.SetText("Firebase manager getting remote config... ");
            rectTransform.DOSizeDelta(new Vector2(0.7f, 1) * rootSize, fillTime1);
            yield return new WaitForSeconds(fillTime1);

            LoadingStateManager.Instance?.SetText("Checking ... Resource data ...");
            if (!passAge) SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
            var home = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);

            rectTransform.DOSizeDelta(new Vector2(1, 1) * rootSize, fillTime2);
            yield return new WaitForSeconds(fillTime2);

            yield return new WaitForSeconds(0.5f);
            while (!home.isDone) yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.5f);
            if (!AdsManager.Instance.IsRemoveAds)
            {
                //if (PlayerPrefs.GetInt("IsRemoveAds") == 1)
                //{
                //    AdsManager.Instance.SetIsRemoveAds(true);
                //}
                //else if (HandleIAP.Instance.HaveBought(ShopProductNames.RemoveADS))
                //{
                //    AdsManager.Instance.SetIsRemoveAds(true);
                //}
            }
            if (passAge)
            {
                AdsManager.Instance.ShowAOA();
                yield return new WaitForSeconds(0.2f);
                //SceneTransition.Instance.GoIn(() =>
                //{
                SceneManager.UnloadSceneAsync(0);
                SceneManager.UnloadSceneAsync(1);

                LoadingStateManager.Instance?.DestroySelf();
                //SceneTransition.Instance.GoOut(() => { });
                //});
                AdsManager.Instance.ShowBanner();
            }
            else
            {
                SceneManager.UnloadSceneAsync(0);
                SceneManager.UnloadSceneAsync(1);
                Age.IsShowing = true;
                AdsManager.Instance.ShowMREC();

            }





        }

        void CalculateData()
        {
           
        }
    }

}