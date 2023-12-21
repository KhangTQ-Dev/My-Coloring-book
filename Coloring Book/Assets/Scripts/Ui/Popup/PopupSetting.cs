using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupSetting : UiCanvas
{
    [SerializeField] private Button btnClose;

    [SerializeField] private Button btnMusic;

    [SerializeField] private Button btnSound;

    [SerializeField] private Image imageRenderMusic;

    [SerializeField] private Image imgButtonMusic;

    [SerializeField] private Image imageRenderSound;

    [SerializeField] private Image imgButtonSound;

    [SerializeField] private Sprite spritebtnOn;

    [SerializeField] private Sprite spritebtnOff;

    [SerializeField] private Sprite spriteRenderOn;

    [SerializeField] private Sprite spriteRenderOff;

    [SerializeField] private float xOff;

    [SerializeField] private float xOn;

    protected override void Start()
    {
        base.Start();

        btnMusic.onClick.AddListener(OnClickBtnMusic);

        btnSound.onClick.AddListener(OnClickBtnSound);

        btnClose.onClick.AddListener(OnClickBtnClose);
    }

    public override void Show(bool _isShow)
    {
        base.Show(_isShow);

        if (_isShow)
        {
            Init();
        }
    }

    public override void Init()
    {
        base.Init();

        InitMusic();

        InitSound();
    }

    private void InitMusic()
    {
        bool a = GameManager.Instance.DataManager.GetMusic();

        imageRenderMusic.sprite = a ? spriteRenderOn : spriteRenderOff;

        imgButtonMusic.sprite = a ? spritebtnOn : spritebtnOff;

        imgButtonMusic.transform.DOLocalMoveX(a ? xOn : xOff, 0);
    }

    private void InitSound()
    {
        bool a = GameManager.Instance.DataManager.GetSound();

        imageRenderSound.sprite = a ? spriteRenderOn : spriteRenderOff;

        imgButtonSound.sprite = a ? spritebtnOn : spritebtnOff;

        imgButtonSound.transform.DOLocalMoveX(a ? xOn : xOff, 0);
    }

    private void OnClickBtnMusic()
    {
        bool a = GameManager.Instance.DataManager.GetMusic();

        imageRenderMusic.sprite = !a ? spriteRenderOn : spriteRenderOff;

        imgButtonMusic.sprite = !a ? spritebtnOn : spritebtnOff;

        imgButtonMusic.transform.DOLocalMoveX(!a ? xOn : xOff, 0.2f);

        GameManager.Instance.DataManager.SetMusic(!a);
    }

    private void OnClickBtnSound()
    {
        bool a = GameManager.Instance.DataManager.GetSound();

        imageRenderSound.sprite = !a ? spriteRenderOn : spriteRenderOff;

        imgButtonSound.sprite = !a ? spritebtnOn : spritebtnOff;

        imgButtonSound.transform.DOLocalMoveX(!a ? xOn : xOff, 0.2f);

        GameManager.Instance.DataManager.SetSound(!a);
    }

    private void OnClickBtnClose()
    {
        Show(false);
    }
}
