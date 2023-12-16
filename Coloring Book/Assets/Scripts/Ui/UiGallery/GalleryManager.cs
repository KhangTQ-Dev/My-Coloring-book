using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private GalleryTabManager galleryTabManager;

    [SerializeField] private List<GalleryGroupManager> galleryGroupManagers;

    [SerializeField] private GameObject prefabGroupShow;

    [SerializeField] private GalleryGroupManager galleryGroupManagerAll;

    [SerializeField] private Transform parentGroup;

    [SerializeField] private Canvas canvas;

    private bool isShow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool isTrue)
    {
        canvas.enabled = isTrue;

        if (isTrue && !isShow)
        {
            isShow = isTrue;

            Init();
        }
    }

    public void Init()
    {
        ChangeTab(true);
    }

    public void Setup(DataAllPicture dataAllPicture)
    {
        // group

        galleryGroupManagers = new List<GalleryGroupManager>();

        GameObject objGroupAll = Instantiate<GameObject>(prefabGroupShow, parentGroup);

        GalleryGroupManager galleryGroupManager = objGroupAll.GetComponent<GalleryGroupManager>();

        galleryGroupManagerAll = galleryGroupManager;

        List<DataPicture> dataPicturesGroupAll = new List<DataPicture>();

        for(int i = 0; i < dataAllPicture.dataPictures.Count; i++)
        {
            GameObject objGroup = Instantiate<GameObject>(prefabGroupShow, parentGroup);

            GalleryGroupManager galleryGroupManagerNormal = objGroup.GetComponent<GalleryGroupManager>();

            List<DataPicture> dataPicturesGroupNormal = new List<DataPicture>();

            for (int j = 0; j < dataAllPicture.dataPictures[i].Count; j++)
            {
                dataPicturesGroupAll.Add(dataAllPicture.dataPictures[i][j]);
                dataPicturesGroupNormal.Add(dataAllPicture.dataPictures[i][j]);
            }

            galleryGroupManagerNormal.Setup(dataPicturesGroupNormal);

            galleryGroupManagers.Add(galleryGroupManagerNormal);
        }

        galleryGroupManager.Setup(dataPicturesGroupAll);

        // tab

        galleryTabManager.Setup(dataAllPicture);
    }

    public void ChangeTab(bool isTabAll, TypeGallery _typeGallery = TypeGallery.Unicorn)
    {
        if (isTabAll)
        {
            galleryGroupManagerAll.Show(true);

            for(int i = 0; i < galleryGroupManagers.Count; i++)
            {
                galleryGroupManagers[i].Show(false);
            }
        }
        else
        {
            galleryGroupManagerAll.Show(false);

            for (int i = 0; i < galleryGroupManagers.Count; i++)
            {
                if(i == (int)_typeGallery)
                {
                    galleryGroupManagers[i].Show(true);
                    continue;
                }

                galleryGroupManagers[i].Show(false);
            }
        }
    }
}
