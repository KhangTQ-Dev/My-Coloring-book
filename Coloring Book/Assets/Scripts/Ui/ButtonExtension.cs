using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ButtonExtension : Button
{
    public TypeSoundButton typeSoundButton;

    public bool nonAdsButton;

    public bool hasTween;

    public override void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SoundManager.PlaySoundButton(typeSoundButton);

        if (hasTween)
        {
            enabled = false;

            transform.DOScale(Vector3.one * 0.9f, 0.1f).SetUpdate(true).SetEase(DG.Tweening.Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, 0.1f).SetUpdate(true).SetEase(DG.Tweening.Ease.Linear).OnComplete(() =>
                {
                    //interactable = true;

                    enabled = true;

                    if (!nonAdsButton)
                    {
                        if (GameManager.Instance.DataManager.GetShowRate()) 
                        {
                            AdsManager.Instance.ShowInterstitial("Button");
                        }
                    }

                    base.OnPointerClick(eventData);

                });
            });
        }
        else
        {
            if (GameManager.Instance.DataManager.GetShowRate())
            {
                AdsManager.Instance.ShowInterstitial("Button");
            }

            base.OnPointerClick(eventData);
        }


    }
}