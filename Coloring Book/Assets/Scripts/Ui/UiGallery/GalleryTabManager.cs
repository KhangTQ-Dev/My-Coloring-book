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

    public void Setup(DataAllPicture dataAllPicture)
    {
        for(int i = 0; i <dataAllPicture.dataPictures.Count; i++)
        {
            GameObject objElementTab = Instantiate<GameObject>(prefabElementTab, parentTab);

            ElementGalleryTab elementGalleryTabNormal = objElementTab.GetComponent<ElementGalleryTab>();

            elementGalleryTabNormal.Setup((TypeGallery)i);
        }
    }
}
