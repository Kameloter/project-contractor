using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Editor script used to make an custom inspector for the CustomEventTrigger script
/// </summary>
[CustomEditor(typeof(CustomEventTrigger))]
public class CustomEventEditor : Editor {
    //bool array for the foldouts
    bool [] foldouts;
    //reference to the customEventTrigger script
    CustomEventTrigger trigger;
    //serializedProperty, used to change the inspector
    SerializedProperty prop;
    //used to make script apear in inspector just like unity does
    MonoScript script;

    /// <summary>
    /// get references and initialize values
    /// </summary>
    void OnEnable() {
        trigger = (CustomEventTrigger) target;
        prop = serializedObject.FindProperty("customEvents");
        script = MonoScript.FromMonoBehaviour((CustomEventTrigger)target);
        foldouts = new bool[20];
    }

    /// <summary>
    /// making an own inspector
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();
        //making script apear in inspector
        EditorGUILayout.ObjectField(script, typeof(MonoScript), false);
        
        //making a custom Inspector
        EditorList.ShowWithBool(prop,foldouts, EditorListOption.Events);

        //apply changes
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(trigger);
    }
}
