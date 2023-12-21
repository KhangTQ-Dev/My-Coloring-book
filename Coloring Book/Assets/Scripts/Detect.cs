using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class Detect : MonoBehaviour
{
    //[SerializeField] private Color colorFill;

    [SerializeField] private Color colorDraw;

    //[SerializeField] private TypeColor typeColor;

    //[SerializeField] private GameObject prefab;

    // Start is called before the first frame update

    private bool isEyeDrop;

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
            if (!isEyeDrop)
            {
                if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    try
                    {
                        for (int i = 0; i < hit.Length; i++)
                        {
                            GameObject gameObject = hit[i].collider.gameObject;

                            ElementPiecePicture elementPiecePicture = gameObject.GetComponent<ElementPiecePicture>();

                            var a = elementPiecePicture.OnPaint(colorDraw, pos);

                            if (a.Item4)
                            {
                                LevelManager.Instance.GamePlayManager.AddHistory(a.Item1, a.Item2, a.Item3);

                                LevelManager.Instance.UiManager.UiGamePlayManager.SetBackNext();
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    try
                    {
                        for (int i = 0; i < hit.Length; i++)
                        {
                            GameObject gameObject = hit[i].collider.gameObject;

                            ElementPiecePicture elementPiecePicture = gameObject.GetComponent<ElementPiecePicture>();

                            var a = elementPiecePicture.GetPixelColor(pos);

                            if (a.a > 0)
                            {
                                SetColor(elementPiecePicture.GetColor());

                                SetEyeDrop();
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }


        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

        pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }

    public void SetColor(Color colorSet)
    {
        colorDraw = colorSet;
    }

    public Color GetColor()
    {
        return colorDraw;
    }

    public void Init()
    {

    }

    public void SetCanEyeDrop()
    {
        isEyeDrop = true;

        LevelManager.Instance.UiManager.UiGamePlayManager.SetEyeDrop(false);
    }

    private void SetEyeDrop()
    {
        isEyeDrop = false;

        LevelManager.Instance.UiManager.UiGamePlayManager.SetEyeDrop(true);

        LevelManager.Instance.UiManager.UiGamePlayManager.ChangeBar(TypeBarDraw.Bar);
    }
}

public enum TypeColor
{
    Pick,
    Draw
}
