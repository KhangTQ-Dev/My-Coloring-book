using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiChooseFollowColorManager : MonoBehaviour
{
    [SerializeField] private List<UiBtnChooseFollowColor> uiBtnChooseFollowColors;

    [SerializeField] private Image imgRender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(List<Color> colors)
    {
        for (int i = 0; i < uiBtnChooseFollowColors.Count; i++) 
        {
            uiBtnChooseFollowColors[i].Setup(colors[i]);

            uiBtnChooseFollowColors[i].Init(i);
        }

        imgRender.color = LevelManager.Instance.GamePlayManager.Detect.GetColor();
    }

    public void OnClickBtnChooseFollow(int idChoose)
    {
        for (int i = 0; i < uiBtnChooseFollowColors.Count; i++)
        {
            uiBtnChooseFollowColors[i].OnChoose(idChoose);
        }

        imgRender.color = LevelManager.Instance.GamePlayManager.Detect.GetColor();
    }
}
