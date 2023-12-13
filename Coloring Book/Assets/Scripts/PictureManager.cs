using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    [SerializeField] private List<ElementPiecePicture> elementPiecePictures;

    [SerializeField] private TypePicture typePicture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupInintial(TypePicture _typePicture)
    {
        typePicture = _typePicture;

        elementPiecePictures = new List<ElementPiecePicture>();

        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;

            ElementPiecePicture elementPiecePicture = child.AddComponent<ElementPiecePicture>();

            elementPiecePicture.SetupPiece(typePicture);

            elementPiecePictures.Add(elementPiecePicture);
        }
    }
}