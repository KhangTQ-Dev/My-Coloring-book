using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ConvertGameObjectSpriteToPrefab : MonoBehaviour
{
    [TitleGroup("Object")]

    [SerializeField] private string namePrefab;

    [SerializeField] private string pathSave = "Assets/Prefabs/Pictures/namePrefab";

    [SerializeField] private float defaulScale;

    [Space]
    [TitleGroup("UI")]
    [SerializeField] private string namePrefabUI;

    [SerializeField] private string pathSaveUI = "Assets/Prefabs/Pictures/namePrefab";

    [SerializeField] private float defaulScaleUI;

    private List<GameObject> listSprite;

    [Button]
    public void ToolConvertToObject()
    {
        GameObject objSave = CreateModelPrefab();
        SavePrefab(objSave);
    }

    public GameObject CreateModelPrefab()
    {
        listSprite = new List<GameObject>();

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log(gameObjects.Length);

        for(int i = 0; i < gameObjects.Length; i++)
        {
            listSprite.Add(gameObjects[i]);
        }

        GameObject prefab = new GameObject();

        prefab.name = namePrefab;

        PictureManager pictureManager = prefab.AddComponent<PictureManager>();

        for(int i = 0; i < listSprite.Count; i++)
        {
            listSprite[i].transform.SetParent(prefab.transform);
        }

        pictureManager.SetupInintial(TypePicture.Sprite);

        pictureManager.gameObject.transform.localScale = Vector3.one * defaulScale;

        return prefab;
    }

    public void SavePrefab(GameObject gameObjectSave)
    {
#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(gameObjectSave, pathSave + namePrefab + ".prefab");

        DestroyImmediate(gameObjectSave);
#endif
    }

    [Button]
    public void ToolConvertToUI()
    {
        GameObject objSave = CreateModelPrefabUI();
        SavePrefabUI(objSave);
    }

    public GameObject CreateModelPrefabUI()
    {
        listSprite = new List<GameObject>();

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log(gameObjects.Length);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            listSprite.Add(gameObjects[i]);
        }

        GameObject prefab = new GameObject();

        prefab.name = namePrefab;

        PictureManager pictureManager = prefab.AddComponent<PictureManager>();

        for (int i = 0; i < listSprite.Count; i++)
        {
            listSprite[i].transform.SetParent(prefab.transform);
        }

        pictureManager.SetupInintial(TypePicture.Image);

        pictureManager.gameObject.transform.localScale = Vector3.one * defaulScaleUI;

        return prefab;
    }

    public void SavePrefabUI(GameObject gameObjectSave)
    {
#if UNITY_EDITOR
        PrefabUtility.SaveAsPrefabAsset(gameObjectSave, pathSaveUI + namePrefabUI + ".prefab");

        DestroyImmediate(gameObjectSave);
#endif
    }
}