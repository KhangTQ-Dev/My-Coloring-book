using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ElementPiecePicture : MonoBehaviour
{
    [SerializeField] private TypePiecePicture typePiecePicture;

    [SerializeField] private TypePicture typePicture;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Image image;

    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPaint(Color _color, Vector2 pos)
    {
        color = _color;

        Color pixel = GetPixelColor(pos);

        // Draw

        if (pixel.a > 0)
        {
            Debug.Log("object clicked: " + gameObject.name);

            spriteRenderer.color = color;
        }
    }

    [Button]
    public void SetupPiece(TypePicture _typePicture)
    {
        typePicture = _typePicture;

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        switch (typePicture)
        {
            case TypePicture.Sprite:
                gameObject.AddComponent<BoxCollider2D>();
                break;
            case TypePicture.Image:
                image = gameObject.AddComponent<Image>();

                image.sprite = spriteRenderer.sprite;

                DestroyImmediate(spriteRenderer);
                break;
        }

        DestroyImmediate(GetComponent<SVGSpriteLoaderBehaviour>());
    }

    public Color GetPixelColor(Vector2 pos)
    {
        Sprite sprite = spriteRenderer.sprite;
        Rect rect = sprite.rect;

        // calculate the distance of the mouse from the center of the sprite's transform
        float x = pos.x - gameObject.transform.position.x;
        float y = pos.y - gameObject.transform.position.y;

        // scale
        x /= gameObject.transform.lossyScale.x;

        y /= gameObject.transform.lossyScale.y;

        // convert the x and y values from units to pixels
        x *= sprite.pixelsPerUnit;
        y *= sprite.pixelsPerUnit;

        // modify so pixel distance from bottom left corner instead of from center
        x += rect.width / 2;
        y += rect.height / 2;

        // adjust for location of sprite on original texture
        x += rect.x;
        y += rect.y;

        // mouse position x and y subtract transform position x and y, then multiply by pixels per unit
        Color pixel = sprite.texture.GetPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        Debug.Log(pixel.ToString());

        return pixel;
    }
}

public enum TypePiecePicture
{
    Defaul,
    Surround
}

public enum TypePicture
{
    Sprite,
    Image
}
