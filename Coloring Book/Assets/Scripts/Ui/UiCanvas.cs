using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.Events;

public class UiCanvas : SerializedMonoBehaviour
{
    [SerializeField] private TypeSort typeSort;

    [SerializeField] protected TypeUi typeUi;

    public TypeUi TypeUi => typeUi;

    // tween start
    
    [SerializeField] private bool hasTweenStart;

    [ShowIf("hasTweenStart")] [SerializeField] private List<AnimationUI> uiTweensStart;
    
    [ShowIf("hasTweenStart")] [SerializeField] private AnimationUI uiTweensStartComplete;
    
    [ShowIf("hasTweenStart")] [SerializeField] private UnityEvent eventStartComplete;
    
    // tween stop
    
    [SerializeField] private bool hasTweenStop;
    
    [ShowIf("hasTweenStop")] [SerializeField] private bool isPlayRevert;
    
    [ShowIf("hasTweenStop")] [SerializeField] private List<AnimationUI> uiTweensStop;
    
    [ShowIf("hasTweenStop")] [SerializeField] private AnimationUI uiTweensStopComplete;
    
    [ShowIf("hasTweenStop")] [SerializeField] private UnityEvent eventStopComplete;
    
    protected virtual void Start()
    {

    }

    public virtual void Init()
    {

    }

    public virtual void Show(bool _isShow)
    {
        if (_isShow)
        {
            gameObject.SetActive(_isShow);

            switch (typeSort)
            {
                case TypeSort.None:
                    break;
                case TypeSort.First:
                    transform.SetAsFirstSibling();
                    break;
                case TypeSort.Last:
                    transform.SetAsLastSibling();
                    break;
            }

            if (hasTweenStart)
            {
                if (uiTweensStart != null)
                {
                    for (int i = 0; i < uiTweensStart.Count; i++)
                    {
                        uiTweensStart[i].Play();
                    }
                }

                if (uiTweensStartComplete != null)
                {
                    uiTweensStartComplete.AddFunctionAtEnd(() => { eventStartComplete?.Invoke();});
                    uiTweensStartComplete.Play();
                }
            }
        }
        else
        {

            if (hasTweenStop)
            {
                if (isPlayRevert)
                {
                    if (uiTweensStart != null)
                    {
                        for (int i = 0; i < uiTweensStart.Count; i++)
                        {
                            uiTweensStart[i].PlayReversed();
                        }
                    }

                    if (uiTweensStartComplete != null)
                    {
                        uiTweensStartComplete.AddFunctionAtEnd(() =>
                        {
                            eventStopComplete?.Invoke();
                            gameObject.SetActive(_isShow);
                        }).Play();
                        //uiTweensStartComplete.PlayReversed();
                    }
                }
                else
                {
                    if (uiTweensStop != null)
                    {
                        for (int i = 0; i < uiTweensStop.Count; i++)
                        {
                            uiTweensStop[i].Play();
                        }
                    }

                    if (uiTweensStopComplete != null)
                    {
                        uiTweensStopComplete.AddFunctionAtEnd(() =>
                        {
                            eventStopComplete?.Invoke();
                            gameObject.SetActive(_isShow);
                        }).Play();
                        //uiTweensStopComplete.Play();
                    }
                }
                

            }
            else
            {
                gameObject.SetActive(_isShow);
            }
        }
    }
}

public enum TypeUi
{
    InstanWhenStart,
    Popup
}

public enum TypeSort
{
    None,
    First,
    Last
}

public enum TypeMoveUi
{
    X,
    Y,
    XY
}