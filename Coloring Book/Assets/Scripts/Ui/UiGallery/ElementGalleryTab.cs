using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementGalleryTab : MonoBehaviour
{
    [SerializeField] private bool isTabAll;

    [SerializeField] private TypeGallery typeGallery;

    [SerializeField] private Button btnTab;

    [SerializeField] private TextMeshProUGUI textRender;

    [SerializeField] private Image imgChoose;

    private int id;

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

    }

    public void Init(int _id)
    {
        id = _id;

        if (isTabAll)
        {
            imgChoose.gameObject.SetActive(true);
        }
        else
        {
            imgChoose.gameObject.SetActive(false);
        }
    }

    public void OnChangeTab(bool _isTabAll, int _id)
    {
        if (_isTabAll)
        {
            if (isTabAll)
            {
                imgChoose.gameObject.SetActive(true);
            }
            else
            {
                imgChoose.gameObject.SetActive(false);
            }
        }
        else
        {
            if (isTabAll)
            {
                imgChoose.gameObject.SetActive(false);
            }
            else
            {
                if(id == _id)
                {
                    imgChoose.gameObject.SetActive(true);
                }
                else
                {
                    imgChoose.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Setup(TypeGallery _typeGallery)
    {
        typeGallery = _typeGallery;

        textRender.text = typeGallery.ToString();
    }

    public void OnClickBtnTab()
    {
        LevelManager.Instance.UiManager.GalleryManager.ChangeTab(isTabAll, typeGallery);

        Debug.Log(typeGallery.ToString());
    }
}
