using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Windows;
using System.Linq;

[CreateAssetMenu(menuName = "Tool/ConvertSpriteToTexture")]
public class ConvertSpriteToTexture : SerializedScriptableObject
{
    //[Button]
    //public Texture2D GetTextureFromSprite(Sprite sprite)
    //{
    //    try
    //    {
    //        if (sprite.rect.width != sprite.texture.width)
    //        {
    //            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
    //            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
    //                                                         (int)sprite.textureRect.y,
    //                                                         (int)sprite.textureRect.width,
    //                                                         (int)sprite.textureRect.height);
    //            newText.SetPixels(newColors);
    //            newText.Apply();
    //            return newText;
    //        }
    //        else
    //            return sprite.texture;
    //    }
    //    catch
    //    {
    //        return sprite.texture;
    //    }
    //}

    //public string GetNameSprite(Sprite sprite)
    //{
    //    return sprite.name;
    //}

    //[Button]
    //public void SaveTextureConvert(Sprite sprite, string path)
    //{
    //    Texture2D texture = GetTextureFromSprite(sprite);

    //    string name = GetNameSprite(sprite);

    //    //then Save To Disk as PNG
    //    byte[] bytes = texture.EncodeToPNG();
    //    var dirPath = Application.dataPath + "/Save" + path;
    //    if (!Directory.Exists(dirPath))
    //    {
    //        Directory.CreateDirectory(dirPath);
    //    }
    //    File.WriteAllBytes(dirPath + name + ".png", bytes);
    //}

    //[Button]
    //public void SaveAllTextureConvert(List<Sprite> sprites)
    //{
    //    for(int i = 0; i < sprites.Count; i++)
    //    {
    //        SaveTextureConvert(sprites[i], "");
    //    }
    //}
}
