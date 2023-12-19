using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Detect : MonoBehaviour
{
    //[SerializeField] private Color colorFill;

    [SerializeField] private Color colorDraw;

    //[SerializeField] private TypeColor typeColor;

    //[SerializeField] private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(prefab, gameObject.transform);
    }

    public void Init(Color colorDefaul)
    {
        colorDraw = colorDefaul;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.GamePlayManager.CanInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                try
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        GameObject gameObject = hit[i].collider.gameObject;

                        ElementPiecePicture elementPiecePicture = gameObject.GetComponent<ElementPiecePicture>();

                        elementPiecePicture.OnPaint(colorDraw, pos);
                    }
                }
                catch
                {

                }
            }
        }
    }

    public void SetColor(Color colorSet)
    {
        colorDraw = colorSet;
    }
}

public enum TypeColor
{
    Pick,
    Draw
}
