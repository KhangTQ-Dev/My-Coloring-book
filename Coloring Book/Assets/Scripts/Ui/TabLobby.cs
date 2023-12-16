using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabLobby : MonoBehaviour
{
    [SerializeField] private TabUi tabUi;

    [SerializeField] private Button btnTab;

    [SerializeField] private Image imgRender;

    [SerializeField] private Sprite spriteOn;

    [SerializeField] private Sprite spriteOff;

    // Start is called before the first frame update
    void Start()
    {
        btnTab.onClick.AddListener(OnClickBtnTab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool isTrue)
    {
        if (isTrue)
        {
            imgRender.sprite = spriteOn;
        }
        else
        {
            imgRender.sprite = spriteOff;
        }
    }

    private void OnClickBtnTab()
    {
        LevelManager.Instance.UiManager.ChangeTab(tabUi);
    }
}
