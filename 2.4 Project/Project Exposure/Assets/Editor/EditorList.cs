using UnityEditor;
using UnityEngine;
using System;

[Flags]
public enum EditorListOption {
	None = 0,
	ListSize = 1,
	ListLabel = 2,
	ElementLabels = 4,
	Buttons = 8,
	Default = ListSize | ListLabel | ElementLabels,
	NoElementLabels = ListSize | ListLabel,
	All = Default | Buttons , Events
}



public static class EditorList
{

    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete"),
        addButtonContent = new GUIContent("+", "add element");

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(50);
    private static GUILayoutOption miniButtonHeigth = GUILayout.Height(35);

    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool
            showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        else if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            }
            else {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    public static void ShowWithBool(SerializedProperty list, bool[] booleans, EditorListOption options = EditorListOption.Default) {
        if (!list.isArray) {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool
            showEvents = (options == EditorListOption.Events);

        if (showEvents) {
            ShowElementsCustom(list, booleans, options);
        }
    }


    private static void ShowElementsCustom(SerializedProperty list, bool[] booleans, EditorListOption options) {
        bool
            showEvents = (options == EditorListOption.Events);

        if (showEvents) {
          
            for (int i = 0; i < list.arraySize; i++) {

                //   EditorGUILayout.TextField ("Event " + i.ToString());
                booleans[i] = EditorGUILayout.Foldout(booleans[i], new GUIContent("Event " + i.ToString()));
                if (booleans[i]) {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("action"), new GUIContent("Type of action"));
                    CustomEventTrigger.Action action = (CustomEventTrigger.Action)list.GetArrayElementAtIndex(i).FindPropertyRelative("action").enumValueIndex;
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("triggerMore"), new GUIContent("trigger more than once?"));
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("onTrigger"), new GUIContent("When to Trigger"));
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("fireType"), new GUIContent("Fire Type"));
                    CustomEventTrigger.FireType fireType = (CustomEventTrigger.FireType)list.GetArrayElementAtIndex(i).FindPropertyRelative("fireType").enumValueIndex;
                  
                    switch (fireType) {
                        case CustomEventTrigger.FireType.Delayed:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("delay"), new GUIContent("Delay Time"));
                            break;
                        case CustomEventTrigger.FireType.Repeat:
                             EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("repeatTime"), new GUIContent("Time Between fires"));
                             EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("repeatAmount"), new GUIContent("Amount of fires"));
                         
                            break;
                        case CustomEventTrigger.FireType.RepeatDelayed:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("delay"), new GUIContent("Delay Time"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("repeatTime"), new GUIContent("Time Between fires"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("repeatAmount"), new GUIContent("Amount of fires"));
                            break;
                    }

              
                    switch (action) {
                        case CustomEventTrigger.Action.PlayAnimation:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("go"), new GUIContent("Gameobject to perform action"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("animation"), new GUIContent("Animation to play"));
                            break;
                        case CustomEventTrigger.Action.ActivateInteractable:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("interactable"), new GUIContent("Interactable to activate"));
                            break;
                        case CustomEventTrigger.Action.DeactivateInteractable:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("interactable"), new GUIContent("Interactable to deactivate"));
                            break;
                        case CustomEventTrigger.Action.PlaySound:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("go"), new GUIContent("Gameobject to perform action"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("audioClip"), new GUIContent("AudioClip to activate"));
                            break;
                        case CustomEventTrigger.Action.PlayCameraPath:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("path"), new GUIContent("Path"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("startAtPlayer"), new GUIContent("Start at player?"));
                            break;
                        case CustomEventTrigger.Action.ShowTutorial:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("animator"), new GUIContent("Animator"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("animationName"), new GUIContent("Animation trigger name"));
                            break;
                        case CustomEventTrigger.Action.ActivateLight:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("light"), new GUIContent("light to activate"));
                            break;
                        case CustomEventTrigger.Action.DisableLight:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("light"), new GUIContent("light to deactivate"));
                            break;
                        case CustomEventTrigger.Action.ChangeLightValues:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("light"), new GUIContent("light to deactivate"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("color"), new GUIContent("Color"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("range"), new GUIContent("Range if PointLight"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("intensity"), new GUIContent("Intesity"));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("bounceIntensity"), new GUIContent("BounceIntesity"));
                            break;
                        case CustomEventTrigger.Action.ActivateObject: 
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("go"), new GUIContent("Gameobject to activate"));
                            break;
                        case CustomEventTrigger.Action.DeactivateObject:
                           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("go"), new GUIContent("Gameobject to deactivate"));
                           break;
                        case CustomEventTrigger.Action.PlayParticle:
                           EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("particle"), new GUIContent("particle to play"));
                             break;
                        case CustomEventTrigger.Action.StopParticle:
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("particle"), new GUIContent("particle to stop"));
                             break;
                        case CustomEventTrigger.Action.ChangeCameraOffset:
                             EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("offset"), new GUIContent("offset"));
                             break;
                    }
                    GUILayout.FlexibleSpace();
                }
            }
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (list.arraySize != 15) {
            if (GUILayout.Button(addButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth, miniButtonHeigth)) {

                list.arraySize += 1;
            }
        }

        if (list.arraySize != 0) {
            if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth, miniButtonHeigth)) {

                list.arraySize -= 1;
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private static void ShowElements(SerializedProperty list, EditorListOption options) {
        bool
            showElementLabels = (options & EditorListOption.ElementLabels) != 0,
            showButtons = (options & EditorListOption.Buttons) != 0,
        showEvents = (options == EditorListOption.Events);
        for (int i = 0; i < list.arraySize; i++) {
            if (showButtons) {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels) {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons) {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(addButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth, miniButtonHeigth)) {
            list.arraySize += 1;
        }

        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth, miniButtonHeigth)) {
            list.arraySize -= 1;
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        //EditorGUILayout.EndVertical();
        //if (showButtons /*&& list.arraySize == 0*/ && GUILayout.Button(addButtonContent, EditorStyles.miniButton, miniButtonWidth)) {
        //	list.arraySize += 1;
        //}
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
      
        //if (GUILayout.Button(addButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        //{
        //    list.arraySize += 1;
        //}
      
        //if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        //{
        //    list.arraySize -= 1;
        //}
        //if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        //{
        //    list.MoveArrayElement(index, index + 1);
        //}
        //if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        //{
        //    list.InsertArrayElementAtIndex(index);

        //}
        //if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
        //	int oldSize = list.arraySize;
        //	list.DeleteArrayElementAtIndex(index);
        //	if (list.arraySize == oldSize) {
        //		list.DeleteArrayElementAtIndex(index);
        //	}
        //}
    }
}