using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class ScaleFollowScreen : MonoBehaviour
{
    [SerializeField] private float defaulScale;

    [SerializeField] private float widthConstant;

    [SerializeField] private float heightConstant;

    // Start is called before the first frame update
    void Start()
    {
        float width = (float)Screen.width / (float)Screen.height;

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

        //float a = x <= y ? x : y;

        transform.localScale = Vector3.one * z * defaulScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
