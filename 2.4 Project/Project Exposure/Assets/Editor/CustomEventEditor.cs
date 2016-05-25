using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CustomEventTrigger))]
public class CustomEventEditor : Editor {

    bool [] foldouts;
    CustomEventTrigger trigger;
    SerializedProperty prop;
	// Use this for initialization

    void OnEnable() {
        trigger = (CustomEventTrigger) target;
        prop = serializedObject.FindProperty("Go");
        foldouts = new bool[20];
    }

	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnInspectorGUI() {
       // DrawDefaultInspector();
        serializedObject.Update();
            
        EditorList.ShowWithBool(prop,foldouts, EditorListOption.Events);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(trigger);
    }
}
