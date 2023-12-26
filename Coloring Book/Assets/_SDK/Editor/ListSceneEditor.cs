using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
public class ListSceneEditor : EditorWindow
{
    private bool runWhenClick;

    [MenuItem("HIGAME/List Scene")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ListSceneEditor));
    }

    private void OnGUI()
    {
        runWhenClick = GUILayout.Toggle(runWhenClick, "Run When Click");
        var scenes = EditorBuildSettings.scenes;
        for (int i = 0; i < scenes.Length; i++)
        {
            var a = scenes[i].path.Split("/");
            if (GUILayout.Button(a[a.Length - 1]))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scenes[i].path);
                if (runWhenClick)
                {
                    EditorApplication.isPlaying = true;
                }
            }
        }
    }
}
#endif