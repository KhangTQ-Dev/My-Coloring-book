using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryTabManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabElementTab;

    [SerializeField] private Transform parentTab;

    [SerializeField] private List<ElementGalleryTab> elementGalleryTabs;

    [SerializeField] private ElementGalleryTab elementGalleryTabAll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        for(int i = 0; i< elementGalleryTabs.Count; i++)
        {
            elementGalleryTabs[i].Init(i);
        }

        elementGalleryTabAll.Init(0);
    }

    public void OnChangeTab(bool isTabAll, int id)
    {
        for (int i = 0; i < elementGalleryTabs.Count; i++)
        {
            elementGalleryTabs[i].OnChangeTab(isTabAll, id);
        }

        elementGalleryTabAll.OnChangeTab(isTabAll, id);
    }

    public void Setup(DataAllPicture dataAllPicture)
    {
        elementGalleryTabs = new List<ElementGalleryTab>();

        for (int i = 0; i <dataAllPicture.dataPictures.Count; i++)
        {
            GameObject objElementTab = Instantiate<GameObject>(prefabElementTab, parentTab);

            ElementGalleryTab elementGalleryTabNormal = objElementTab.GetComponent<ElementGalleryTab>();

            elementGalleryTabNormal.Setup((TypeGallery)i);

            elementGalleryTabs.Add(elementGalleryTabNormal);
        }
    }
}