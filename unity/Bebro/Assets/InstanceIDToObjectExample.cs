using UnityEngine;
using UnityEditor;

public class InstanceIDToObjectExample : EditorWindow
{
    static int id;

    [MenuItem("Example/ID To Name")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        InstanceIDToObjectExample window = (InstanceIDToObjectExample)EditorWindow.GetWindow(typeof(InstanceIDToObjectExample));
        window.Show();
    }

    void OnGUI()
    {
        id = EditorGUILayout.IntField("Instance ID:", id);
        if (GUILayout.Button("Find Name"))
        {
            Object obj = EditorUtility.InstanceIDToObject(id);
            if (!obj)
                Debug.LogError("No object could be found with instance id: " + id);
            else
            {
                Debug.Log("Object's name: " + obj.name);
                Debug.Log(obj.GetType().Name);
            }
        }
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}