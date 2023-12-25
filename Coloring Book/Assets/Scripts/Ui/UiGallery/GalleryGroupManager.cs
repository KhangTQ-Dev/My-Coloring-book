using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryGroupManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabElementGroup;

    [SerializeField] private List<ElementGalleryGroup> elementGalleryGroups;

    [SerializeField] private Transform parentElement;

    [SerializeField] private GameObject canvasRender;

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
        canvasRender.gameObject.SetActive(isTrue);

        parentElement.transform.localPosition = new Vector3(0, 548.4692f, 0);

        if (isTrue)
        {
            Debug.Log("Khang");

            Init();
        }

        if (isShow != isTrue)
        {
            isShow = isTrue;
        }


    }

    public void Init()
    {
        for(int i = 0; i < elementGalleryGroups.Count; i++)
        {
            elementGalleryGroups[i].Init();
        }
    }

    public void InitAll()
    {
        for (int i = 0; i < elementGalleryGroups.Count; i++)
        {
            elementGalleryGroups[i].InitAll();
        }
    }

    public void Setup(List<DataPicture> _dataPictures)
    {
        elementGalleryGroups = new List<ElementGalleryGroup>();

        for (int i = 0;  i< _dataPictures.Count; i++)
        {
            GameObject objElement = Instantiate<GameObject>(prefabElementGroup, parentElement);

            ElementGalleryGroup elementGalleryGroup = objElement.GetComponent<ElementGalleryGroup>();

            elementGalleryGroup.Setup(_dataPictures[i]);

            elementGalleryGroups.Add(elementGalleryGroup);
        }
    }
}
