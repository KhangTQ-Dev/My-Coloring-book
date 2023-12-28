using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDelete : UiCanvas
{
    private Action actionOkay;

    [SerializeField] private Button btnClose;

    [SerializeField] private Button btnOkay;

    [SerializeField] private Button btnNo;

    protected override void Start()
    {
        base.Start();

        btnClose.onClick.AddListener(OnClickBtnClose);

        btnOkay.onClick.AddListener(OnClickBtnOkay);

        btnNo.onClick.AddListener(OnClickBtnNo);
    }

    public override void Show(bool _isShow)
    {
        base.Show(_isShow);
    }

    public void Init(Action action)
    {
        actionOkay = action;
    }

    private void OnClickBtnClose()
    {
        Show(false);
    }

    private void OnClickBtnOkay()
    {
        HandleFireBase.Instance.LogEventDrawComplete(LevelManager.Instance.GamePlayManager.GetCurrentTypeGallery(), LevelManager.Instance.GamePlayManager.GetCurrentTypeId());

        Show(false);

        actionOkay?.Invoke();
    }

    private void OnClickBtnNo()
    {
        Show(false);
    }
}