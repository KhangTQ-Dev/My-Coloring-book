using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBtnColorManager : UiCanvas
{
    [SerializeField] private List<UiButtonColor> uiButtonColors;

    [SerializeField] private UiChooseFollowColorManager uiChooseFollowColorManager;

    public UiChooseFollowColorManager UiChooseFollowColorManager => uiChooseFollowColorManager;

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

        uiChooseFollowColorManager.Init(uiButtonColors[0].GetListColorFollow());

        Show(true);
    }

    public void OnClickBtnChoose(int idChoose)
    {
        for (int i = 0; i < uiButtonColors.Count; i++)
        {
            uiButtonColors[i].OnChoose(idChoose);
        }

        uiChooseFollowColorManager.Init(uiButtonColors[idChoose].GetListColorFollow());

    }

    public void OnClickBtnChooseFollow(int idChoose)
    {
        uiChooseFollowColorManager.OnClickBtnChooseFollow(idChoose);
    }
}
