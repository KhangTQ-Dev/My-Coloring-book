using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PFramework;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class PopupSetting : MonoBehaviour
    {
        [SerializeField] GameObject gdprButton;
        protected  void Start()
        {
            gdprButton.SetActive(GameStatic.IsPrivacyOptionsRequired);
        }
        public void ShowPrivacyOptionsForm()
        {
            GoogleMobileAds.Ump.Api.ConsentForm.ShowPrivacyOptionsForm(formError =>
            {
                if (formError != null)
                {
                    //UINotificationText.Push("Error, Please try again later");
                }
            });
        }
    }
}
