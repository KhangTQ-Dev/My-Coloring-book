using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Tool/ToolGenAll")]
public class ToolGenAll : SerializedScriptableObject
{
    [SerializeField] private DataAllPicture dataAllPicture;

    [SerializeField] private List<List<SVGAtlas>> sVGAtlas;

    [Button]
    private void GenAll()
    {

    }
}
