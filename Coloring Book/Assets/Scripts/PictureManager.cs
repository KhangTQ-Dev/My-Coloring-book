using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PictureManager : MonoBehaviour
{
    [SerializeField] private List<ElementPiecePicture> elementPiecePictures;

    [SerializeField] private TypePicture typePicture;

    [SerializeField] private float minZoom;

    [SerializeField] private float maxZoom;

    [SerializeField] private Vector3 scaleDefaul;

    public List<ElementPiecePicture> ElementPiecePictures => elementPiecePictures;

    private TypeGallery typeGallery;

    private TypeId typeId;

    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {


        if(typePicture == TypePicture.Sprite)
        {
            float width =  (float)Screen.width / (float)Screen.height;

            Debug.Log(width);

            //float k = Screen.width / 1080;

            //if (Screen.width != 1080)
            //{
            //    k = Camera.main.orthographicSize * 2 * k;
            //}

            float m = 1080.0f / 1920.0f;

            


            ////k = k / m;

            ////float y = Screen.height / 1920;

            ////float k = x <= y ? x : y;

            float z = width / m;

            Debug.Log(z);

            scaleDefaul = scaleDefaul * z;

            initialScale = scaleDefaul;

            transform.localScale = scaleDefaul;

            minZoom = minZoom * z;

            maxZoom = maxZoom * z;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(TypeGallery _typeGallery, TypeId _typeId)
    {
        typeGallery = _typeGallery;

        typeId = _typeId;

        try
        {
            DataColorPicture dataColorPicture = GameManager.Instance.DataManager.LoadPicture(_typeGallery, typeId, elementPiecePictures.Count);

            Debug.Log(dataColorPicture);

            Debug.Log(dataColorPicture.DataElementColorPictures);

            Debug.Log(dataColorPicture.DataElementColorPictures.Count);

            for (int i = 0; i < elementPiecePictures.Count; i++)
            {
                Color color = dataColorPicture.DataElementColorPictures[i];

                elementPiecePictures[i].Init(color, i);
            }
        }
        catch
        {

        }


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

    [Button]
    public void SavePicture()
    {
        DataColorPicture dataColorPicture = new DataColorPicture();

        dataColorPicture.DataElementColorPictures = new List<Color>();

        for(int i = 0; i < elementPiecePictures.Count; i++)
        {
            dataColorPicture.DataElementColorPictures.Add(elementPiecePictures[i].GetColor());
        }

        dataColorPicture.IsSave = true;

        GameManager.Instance.DataManager.SavePicture(typeGallery, typeId, dataColorPicture);
    }

    public void Unload()
    {
        Destroy(gameObject);
    }

    public float OnZoom(float increment)
    {
        float a = Mathf.Clamp(transform.localScale.x - increment, minZoom, maxZoom);

        transform.localScale = Vector3.one * a;

        return a / initialScale.x;
    }
}