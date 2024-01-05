using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWarningLeave : UiCanvas
{
    public Action actionOkay;

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

    public void Init(Action _action)
    {
        actionOkay = _action;
    }

    private void OnClickBtnClose()
    {
        Show(false);
    }

    private void OnClickBtnOkay()
    {
        actionOkay?.Invoke();

        Show(false);
    }

    private void OnClickBtnNo()
    {
        Show(false);
    }
}
