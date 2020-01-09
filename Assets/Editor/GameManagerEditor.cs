using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GameManager mapGen = (GameManager)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.Destroy();
                mapGen.Generate();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.Destroy();
            mapGen.Generate();
        }

        if (GUILayout.Button("Destroy"))
        {
            mapGen.Destroy();
        }

    }
}