using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryGroupManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabElementGroup;

    [SerializeField] private List<ElementGalleryGroup> elementGalleryGroups;

    [SerializeField] private Transform parentElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(List<DataPicture> _dataPictures)
    {
        for(int i = 0;  i< _dataPictures.Count; i++)
        {
            GameObject objElement = Instantiate<GameObject>(prefabElementGroup, parentElement);

            ElementGalleryGroup elementGalleryGroup = objElement.GetComponent<ElementGalleryGroup>();

            elementGalleryGroup.Setup(_dataPictures[i]);
        }
    }
}
