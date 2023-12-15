using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGalleryTab : MonoBehaviour
{
    [SerializeField] private bool isTabAll;

    [SerializeField] private TypeGallery typeGallery;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(TypeGallery _typeGallery)
    {
        typeGallery = _typeGallery;
    }
}
