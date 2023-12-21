using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBtnColorManager : UiCanvas
{
    [SerializeField] private List<UiButtonColor> uiButtonColors;

    protected override void Start()
    {
        base.Start();
    }

    public override void Show(bool _isShow)
    {
        base.Show(_isShow);
    }

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < uiButtonColors.Count; i++)
        {
            uiButtonColors[i].Init(i);
        }

        Show(true);
    }

    public void OnClickBtnChoose(int idChoose)
    {
        for (int i = 0; i < uiButtonColors.Count; i++)
        {
            uiButtonColors[i].OnChoose(idChoose);
        }
    }
}
