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
