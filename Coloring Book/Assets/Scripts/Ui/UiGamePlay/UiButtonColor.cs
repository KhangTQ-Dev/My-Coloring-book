using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UiButtonColor : MonoBehaviour
{
    [SerializeField] private bool isDefaul;

    [SerializeField] private Color color;

    [SerializeField] private Button btnColor;

    [SerializeField] private List<Image> imgRenderColors;

    [SerializeField] private GameObject spriteChoose;

    [SerializeField] private GameObject spriteUnChoose;

    private int id;

    // Start is called before the first frame update
    void Start()
    {
        btnColor.onClick.AddListener(OnClickBtnColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void Setup(Color _color)
    {
        color = _color;

        for(int i = 0; i < imgRenderColors.Count; i++)
        {
            imgRenderColors[i].color = color;
        }
    }

    public void Init(int _id)
    {
        id = _id;

        if (isDefaul)
        {
            SetupUi(true);

            LevelManager.Instance.GamePlayManager.Detect.SetColor(color);
        }
        else
        {
            SetupUi(false);
        }
    }

    private void SetupUi(bool isChoose)
    {
        if (isChoose)
        {
            Debug.Log("choose");

            spriteChoose.gameObject.SetActive(true);

            spriteUnChoose.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Unchoose");

            spriteChoose.gameObject.SetActive(false);

            spriteUnChoose.gameObject.SetActive(true);
        }

        //imgRenderColor.SetNativeSize();
    }

    public void OnChoose(int idChoose)
    {
        if(idChoose == id)
        {
            SetupUi(true);
        }
        else
        {
            SetupUi(false);
        }
    }

    private void OnClickBtnColor()
    {
        LevelManager.Instance.UiManager.UiGamePlayManager.OnClickBtnChoose(id);

        LevelManager.Instance.GamePlayManager.Detect.SetColor(color);
    }
}
