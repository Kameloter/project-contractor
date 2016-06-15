using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CustomEventTrigger))]
public class CustomEventEditor : Editor {

    bool [] foldouts;
    CustomEventTrigger trigger;
    SerializedProperty prop;
    MonoScript script;

    void OnEnable() {
        trigger = (CustomEventTrigger) target;
        prop = serializedObject.FindProperty("customEvents");
        script = MonoScript.FromMonoBehaviour((CustomEventTrigger)target);
        foldouts = new bool[20];
    }

    public override void OnInspectorGUI() {
       // DrawDefaultInspector();
        serializedObject.Update();
        EditorGUILayout.ObjectField(script, typeof(MonoScript), false);
            
        EditorList.ShowWithBool(prop,foldouts, EditorListOption.Events);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(trigger);
    }
}
