using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Detect : MonoBehaviour
{
    [SerializeField] private Color colorFill;

    [SerializeField] private Color colorDraw;

    [SerializeField] private TypeColor typeColor;

    [SerializeField] private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefab, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Collider2D[] collidersHit = Physics2D.OverlapPointAll(pos);
            //GameObject selectedObject = null;

            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            try
            {
                for(int i = 0; i < hit.Length; i++)
                {
                    GameObject gameObject = hit[i].collider.gameObject;

                    ElementPiecePicture elementPiecePicture = gameObject.GetComponent<ElementPiecePicture>();

                    elementPiecePicture.OnPaint(colorDraw, pos);

                    //SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

                    //Sprite sprite = spriteRenderer.sprite;
                    //Rect rect = sprite.rect;

                    //// calculate the distance of the mouse from the center of the sprite's transform
                    //float x = pos.x - gameObject.transform.position.x;
                    //float y = pos.y - gameObject.transform.position.y;

                    //// scale
                    //x /= gameObject.transform.lossyScale.x;

                    //y /= gameObject.transform.lossyScale.y;

                    //// convert the x and y values from units to pixels
                    //x *= sprite.pixelsPerUnit;
                    //y *= sprite.pixelsPerUnit;

                    //// modify so pixel distance from bottom left corner instead of from center
                    //x += rect.width / 2;
                    //y += rect.height / 2;

                    //// adjust for location of sprite on original texture
                    //x += rect.x;
                    //y += rect.y;

                    //// mouse position x and y subtract transform position x and y, then multiply by pixels per unit
                    //Color pixel = sprite.texture.GetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
                    //Debug.Log(pixel.ToString());
                    //if (pixel.a > 0)
                    //{
                    //    Debug.Log("object clicked: " + hit[i].collider.name);

                    //    switch (typeColor)
                    //    {
                    //        case TypeColor.Pick:

                    //            spriteRenderer.color = colorFill;

                    //            break;
                    //        case TypeColor.Draw:

                    //            //sprite.texture.SetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y), colorDraw);

                    //            spriteRenderer.color = colorDraw;

                    //            break;
                    //    }
                    //}
                }


            }
            catch
            {

            }
        }


        //if (Input.GetMouseButton(0))
        //{
        //    Vector2 mousePos = Input.mousePosition;
        //    Vector2 viewportPos = Camera.main.ScreenToViewportPoint(mousePos);

        //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    //Collider2D[] collidersHit = Physics2D.OverlapPointAll(pos);
        //    //GameObject selectedObject = null;

        //    Ray ray = Camera.main.ViewportPointToRay(viewportPos);

        //    RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //    for (int i = 0; i < hit.Length; i++)
        //    {
        //        GameObject gameObject = hit[i].collider.gameObject;

        //        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //        Sprite sprite = spriteRenderer.sprite;

        //        Texture2D texture = sprite.texture;

        //        Vector3 spritePos = spriteRenderer.transform.position;

        //        Rect textureRect = sprite.textureRect;

        //        float pixelsPerUnit = sprite.pixelsPerUnit;



        //        float halfRealTexWidth = sprite.texture.width * 0.5f; // use the real texture width here because center is based on this -- probably won't work right for atlases
        //        float halfRealTexHeight = sprite.texture.height * 0.5f;

        //        // Convert to pixel position, offsetting so 0,0 is in lower left instead of center
        //        int texPosX = (int)(spritePos.x * pixelsPerUnit + halfRealTexWidth);
        //        int texPosY = (int)(spritePos.y * pixelsPerUnit + halfRealTexHeight);

        //        // Check if pixel is within texture
        //        //if (texPosX < 0 || texPosX < textureRect.x || texPosX >= Mathf.FloorToInt(textureRect.xMax)) return false; // out of bounds
        //        //if (texPosY < 0 || texPosY < textureRect.y || texPosY >= Mathf.FloorToInt(textureRect.yMax)) return false; // out of bounds

        //        // Get pixel color
        //        var color = texture.GetPixel(texPosX, texPosY);

        //        Debug.Log(color);

        //        if (color.a > 0)
        //        {
        //            Debug.Log("object clicked: " + hit[i].collider.name);

        //            switch (typeColor)
        //            {
        //                case TypeColor.Pick:

        //                    spriteRenderer.color = colorFill;

        //                    break;
        //                case TypeColor.Draw:

        //                    sprite.texture.SetPixel(Mathf.FloorToInt(texPosX), Mathf.FloorToInt(texPosY), colorFill);

        //                    break;
        //            }
        //        }
        //    }
        //}
    }

    public bool GetSpritePixelColorUnderMousePointer(SpriteRenderer spriteRenderer, out Color color)
    {
        color = new Color();
        Camera cam = Camera.main;

        Vector2 mousePos = Input.mousePosition;
        Vector2 viewportPos = cam.ScreenToViewportPoint(mousePos);

        if (viewportPos.x < 0.0f || viewportPos.x > 1.0f || viewportPos.y < 0.0f || viewportPos.y > 1.0f) return false; // out of viewport bounds

        // Cast a ray from viewport point into world
        Ray ray = cam.ViewportPointToRay(viewportPos);

        // Check for intersection with sprite and get the color
        return IntersectsSprite(spriteRenderer, ray, out color);
    }

    private bool IntersectsSprite(SpriteRenderer spriteRenderer, Ray ray, out Color color)
    {
        color = new Color();
        if (spriteRenderer == null) return false;

        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null) return false;

        Texture2D texture = sprite.texture;
        if (texture == null) return false;

        // Check atlas packing mode
        if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
        {
            // Cannot use textureRect on tightly packed sprites
            Debug.LogError("SpritePackingMode.Tight atlas packing is not supported!");
            // TODO: support tightly packed sprites
            return false;
        }

        // Craete a plane so it has the same orientation as the sprite transform
        Plane plane = new Plane(transform.forward, transform.position);

        // Intersect the ray and the plane
        float rayIntersectDist; // the distance from the ray origin to the intersection point
        if (!plane.Raycast(ray, out rayIntersectDist)) return false; // no intersection

        // Convert world position to sprite position

        // worldToLocalMatrix.MultiplyPoint3x4 returns a value from based on the texture dimensions (+/- half texDimension / pixelsPerUnit) )
        // 0, 0 corresponds to the center of the TEXTURE ITSELF, not the center of the trimmed sprite textureRect
        Vector3 spritePos = spriteRenderer.worldToLocalMatrix.MultiplyPoint3x4(ray.origin + (ray.direction * rayIntersectDist));

        Rect textureRect = sprite.textureRect;

        float pixelsPerUnit = sprite.pixelsPerUnit;
        float halfRealTexWidth = texture.width * 0.5f; // use the real texture width here because center is based on this -- probably won't work right for atlases
        float halfRealTexHeight = texture.height * 0.5f;

        // Convert to pixel position, offsetting so 0,0 is in lower left instead of center
        int texPosX = (int)(spritePos.x * pixelsPerUnit + halfRealTexWidth);
        int texPosY = (int)(spritePos.y * pixelsPerUnit + halfRealTexHeight);

        // Check if pixel is within texture
        if (texPosX < 0 || texPosX < textureRect.x || texPosX >= Mathf.FloorToInt(textureRect.xMax)) return false; // out of bounds
        if (texPosY < 0 || texPosY < textureRect.y || texPosY >= Mathf.FloorToInt(textureRect.yMax)) return false; // out of bounds

        // Get pixel color
        color = texture.GetPixel(texPosX, texPosY);

        return true;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

public enum TypeColor
{
    Pick,
    Draw
}
