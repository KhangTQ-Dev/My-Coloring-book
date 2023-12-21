using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiChooseColorManager : UiCanvas
{
    [SerializeField] ColorPicker colorPicker;

    protected override void Start()
    {
        base.Start();

        colorPicker.onValueChanged.AddListener((a) =>
        {
            LevelManager.Instance.GamePlayManager.Detect.SetColor(a);
        });
    }

    public override void Show(bool _isShow)
    {
        base.Show(_isShow);

        if (_isShow)
        {
            colorPicker.CurrentColor = LevelManager.Instance.GamePlayManager.Detect.GetColor();
        }
    }

    public override void Init()
    {
        base.Init();

        Show(false);
    }
}
