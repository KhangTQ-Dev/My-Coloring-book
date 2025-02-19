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

    private bool canDetect;

    private int countDraw;

    void Start()
    {
        //Instantiate(prefab, gameObject.transform);
    }

    public void Init(Color colorDefaul)
    {
        colorDraw = colorDefaul;

        canDetect = true;
    }

    public void SetCanDetect(bool isTrue)
    {
        canDetect = isTrue;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.GamePlayManager.CanInteract)
        {
            if (!isEyeDrop)
            {
                if (Input.GetMouseButtonUp(0) && !IsPointerOverUIObject() && canDetect)
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
                                GameManager.Instance.SoundManager.PlaySoundDraw();

                                LevelManager.Instance.GamePlayManager.AddHistory(a.Item1, a.Item2, a.Item3);

                                LevelManager.Instance.UiManager.UiGamePlayManager.SetBackNext();

                                countDraw++;

                                if(countDraw >= 2)
                                {
                                    bool k = GameManager.Instance.DataManager.GetFirstDone();

                                    bool b = GameManager.Instance.DataManager.GetShowRate();

                                    if (!k & !b)
                                    {
                                        LevelManager.Instance.UiManager.RateManager.ShowPopup();

                                        GameManager.Instance.DataManager.SetFirstDone();

                                        GameManager.Instance.DataManager.SetShowRate();
                                    }
                                }
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
                if(Input.GetMouseButtonUp(0) && !IsPointerOverUIObject() && canDetect)
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

                                SetEyeDrop(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
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

    private void SetEyeDrop(Vector3 positon)
    {
        isEyeDrop = false;

        LevelManager.Instance.UiManager.UiGamePlayManager.UiColorPick.OnPick(GetColor(), positon);

        LevelManager.Instance.UiManager.UiGamePlayManager.SetEyeDrop(true);

        LevelManager.Instance.UiManager.UiGamePlayManager.ChangeBar(TypeBarDraw.Bar);
    }
}

public enum TypeColor
{
    Pick,
    Draw
}
