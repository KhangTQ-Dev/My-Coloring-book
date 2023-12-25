using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiColorPick : MonoBehaviour
{
    [SerializeField] private GameObject objColorPick;

    [SerializeField] private Image imageRenderColor;

    [SerializeField] private float timeShow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPick(Color color, Vector3 position)
    {
        imageRenderColor.color = color;

        objColorPick.transform.position = position;

        objColorPick.gameObject.SetActive(true);

        DOTween.To((x) => { }, 0, 1, timeShow).OnComplete(() => { objColorPick.gameObject.SetActive(false); });
    }
}