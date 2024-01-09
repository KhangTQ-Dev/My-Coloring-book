using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Game;

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

    private Tween tweenMove;

    private bool isCheckToRemuse;

    void Start()
    {
        StartCoroutine(WaitLoading());
    }

    private void Update()
    {

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

        tweenMove = brush.DOPath(pathTween, timeLoad).SetEase(DG.Tweening.Ease.Linear).SetUpdate(true).OnUpdate(() => 
        {
            imgFill.fillAmount = GetFillAmount();
        }).OnComplete(() =>
        {
            actionAfterLoad?.Invoke();
        }); ;
    }

    public void OnPauseGDPR()
    {
        if (tweenMove != null)
        {
            tweenMove.Pause();
        }
    }

    public void OnRemuseGDPR()
    {
        if (tweenMove != null)
        {
            tweenMove.Play();
        }
    }

    public float GetFillAmount()
    {
        float distanceMax = positionBrushMax.position.x - positionBrushMin.position.x;

        float distanceCurrent = brush.position.x - positionBrushMin.position.x;

        float a = distanceCurrent / distanceMax;

        return a;
    }
}
