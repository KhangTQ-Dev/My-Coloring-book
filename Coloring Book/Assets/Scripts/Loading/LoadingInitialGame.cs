using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class LoadingInitialGame : MonoBehaviour
{
    [SerializeField] private float timeLoad;

    [SerializeField] private Image imgFill;

    [SerializeField] private Transform brush;

    [SerializeField] private Transform[] path;

    [SerializeField] private Transform positionBrushMin;

    [SerializeField] private Transform positionBrushMax;

    [SerializeField] private UnityEvent actionAfterLoad;

    private Vector3[] pathTween;

    void Start()
    {
        StartCoroutine(WaitLoading());
    }

    IEnumerator WaitLoading()
    {
        yield return new WaitForEndOfFrame();

        pathTween = new Vector3[path.Length];

        for (int i = 0; i < path.Length; i++)
        {
            pathTween[i] = path[i].position;
        }

        //DOTween.To((x) => 
        //{
        //    imgFill.fillAmount = x;
        //}, 0, 1, 5.0f).OnComplete(() => 
        //{
        //    actionAfterLoad?.Invoke();
        //});

        brush.DOPath(pathTween, timeLoad).SetEase(DG.Tweening.Ease.Linear).SetUpdate(true).OnUpdate(() => 
        {
            imgFill.fillAmount = GetFillAmount();
        }).OnComplete(() =>
        {
            actionAfterLoad?.Invoke();
        }); ;
    }

    public float GetFillAmount()
    {
        float distanceMax = positionBrushMax.position.x - positionBrushMin.position.x;

        float distanceCurrent = brush.position.x - positionBrushMin.position.x;

        float a = distanceCurrent / distanceMax;

        return a;
    }
}
