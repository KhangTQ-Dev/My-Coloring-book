using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HG.Nam.Demo
{
    public class Age : MonoBehaviour
    {
        [SerializeField] int currentAge = 2007;
        [SerializeField] TextMeshProUGUI txtLeft, txtCenter, txtRight;
        [SerializeField] GameObject gOK, gDontShare;
        int delayShowTime = 5, delayShowTimeNoADS = 10, autoNextTime = 20;
        public static bool IsShowing = false;
        bool showButton = false;
        Coroutine coShowButton, coNext;
        private void Start()
        {
            Construct();
            PlayerPrefs.SetInt("HG_AGE", 1);
        
        }
        void Construct()
        {
            txtLeft.text = (currentAge - 1).ToString();
            txtCenter.text = "----";
            txtRight.text = (currentAge + 1).ToString();

        }
        IEnumerator IEShowButton(int seconds)
        {
            LoadingStateManager.Instance?.SetText($"Thanks for choosing our game!");
            int value = seconds;
            for (int i = 0; i < seconds; i++)
            {
                value--;
                if (i == 2) LoadingStateManager.Instance?.SetText($"Choosing your birth year!");
                AdsManager.Instance.ShowMREC();
                yield return new WaitForSeconds(1);
            }

            gOK.SetActive(true);
            gDontShare.SetActive(true);
        }
        void CountdownShowButton()
        {
            AdsManager.Instance.ShowMaxBanner();
            showButton = true;
            coNext = StartCoroutine(IEAutoNext(autoNextTime));
            if (AdsManager.Instance.IsMRECReady) coShowButton = StartCoroutine(IEShowButton(delayShowTime));
            else coShowButton = StartCoroutine(IEShowButton(delayShowTimeNoADS));
        }
        private void LateUpdate()
        {
            if (!IsShowing) return;
            if (showButton) return;
            CountdownShowButton();
        }
        public void AdjustAge(int value)
        {
            if (coNext != null)
            {
                LoadingStateManager.Instance?.SetText($"Choosing your birth year!");
                StopCoroutine(coNext);
                coNext = null;
            }
            currentAge += value;
            txtLeft.text = (currentAge - 1).ToString();
            txtCenter.text = (currentAge).ToString();
            txtRight.text = (currentAge + 1).ToString();


            IsShowing = true;
        }

        IEnumerator IEAutoNext(int seconds)
        {
            var value = seconds;
            for (int i = 0; i < seconds; i++)
            {
                if (value <= 5) LoadingStateManager.Instance?.SetText($"Auto close and show later in {value}s");
                value--;
                yield return new WaitForSeconds(1);
            }
            ShowAOA();
        }

        void ShowAOA()
        {
            AdsManager.Instance.ShowFirstAOA();
            StartCoroutine(IENextScene());
        }

        IEnumerator IENextScene()
        {
            while (!AdsManager.Instance.AoaShowSuccess) yield return new WaitForEndOfFrame();
            yield return 2 * Time.deltaTime;
            AdsManager.Instance.HideMREC();
            AdsManager.Instance.UpdateMRecPosition(EMRecPosition.BottomCenter);
            //SceneManager.UnloadSceneAsync(2);
            gameObject.SetActive(false);
        }
        //Button OK and dont share
        public void Ok()
        {
            if (txtCenter.text != "----")
            {
                HandleFireBase.Instance.LogEventWithParameter("age", new FirebaseParam("id", currentAge));
                PlayerPrefs.SetInt("USER_AGE", currentAge);
            }
            if (coNext != null) StopCoroutine(coNext);
            AdsManager.Instance.ShowImageInter(ShowAOA);
        }
        public void DontShare()
        {
            if (coNext != null) StopCoroutine(coNext);
            AdsManager.Instance.ShowImageInter(ShowAOA);
        }
        private void OnDestroy()
        {

           
        }

        private void OnDisable()
        {
            if (coShowButton != null) StopCoroutine(coShowButton);
            LoadingStateManager.Instance?.DestroySelf();
            AdsManager.Instance.RefreshBanner();
        }
        public void OpenLink()
        {
            Application.OpenURL("https://higame.com.vn/privacy-policy/");
        }
    }
}
