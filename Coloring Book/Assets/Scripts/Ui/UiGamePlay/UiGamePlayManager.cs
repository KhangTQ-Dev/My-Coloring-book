using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGamePlayManager : MonoBehaviour
{
    [SerializeField] private List<UiButtonColor> uiButtonColors;

    [SerializeField] private Button btnBack;

    [SerializeField] private Button btnBackAction;

    [SerializeField] private Button btnNextAction;

    [SerializeField] private Button btnEyeDrop;

    [SerializeField] private Button btnDone;

    // Start is called before the first frame update
    void Start()
    {
        btnBack.onClick.AddListener(OnClickBtnBack);
        btnBackAction.onClick.AddListener(OnClickBtnBackAction);
        btnNextAction.onClick.AddListener(OnClickBtnNextAction);
        btnEyeDrop.onClick.AddListener(OnClickBtnEyeDrop);
        btnDone.onClick.AddListener(OnClickBtnDone);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        for(int i = 0; i < uiButtonColors.Count; i++)
        {
            uiButtonColors[i].Init(i);
        }
    }

    public void OnClickBtnChoose(int idChoose)
    {
        for (int i = 0; i < uiButtonColors.Count; i++)
        {
            uiButtonColors[i].OnChoose(idChoose);
        }
    }

    private void OnClickBtnBack()
    {
        LevelManager.Instance.OnChangeToLobby();
    }

    private void OnClickBtnBackAction()
    {

    }

    private void OnClickBtnNextAction()
    {

    }

    private void OnClickBtnEyeDrop()
    {

    }

    private void OnClickBtnDone()
    {

    }
}
