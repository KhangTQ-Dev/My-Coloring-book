using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomController : MonoBehaviour
{
    [SerializeField] private float xMin;

    [SerializeField] private float xMax;

    [SerializeField] private float yMin;

    [SerializeField] private float yMax;

    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    private Vector3 initialPosition;

    private float indexZoom;

    private void Start()
    {
        initialPosition = Camera.main.transform.position;

        indexZoom = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.GamePlayManager.CanInteract && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Camera.main.transform.position = GetPositionClamp(Camera.main.transform.position, direction);
            }
            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void zoom(float increment)
    {
        indexZoom = LevelManager.Instance.GamePlayManager.PictureManager.OnZoom(increment);

        //Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }

    public void Init()
    {
        Camera.main.transform.position = initialPosition;
    }

    public Vector3 GetPositionClamp(Vector3 current, Vector3 direction)
    {
        current.x = Mathf.Clamp(current.x + direction.x, xMin * indexZoom, xMax * indexZoom);

        current.y = Mathf.Clamp(current.y + direction.y, yMin * indexZoom, yMax * indexZoom);

        return current;
    }
}