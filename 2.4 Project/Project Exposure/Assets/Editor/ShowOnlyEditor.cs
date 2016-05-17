using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ShowOnlyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        string value;

        switch (property.propertyType) {
            case SerializedPropertyType.String:
                value = property.stringValue;
                break;
            case SerializedPropertyType.Boolean:
                value = property.boolValue.ToString();
                break;
            case SerializedPropertyType.Integer:
                value = property.intValue.ToString();
                break;
            case SerializedPropertyType.Float:
                value = property.floatValue.ToString("F4");
                break;
            case SerializedPropertyType.Vector2:
                value = property.vector2Value.ToString("F4");
                break;
            case SerializedPropertyType.Vector3:
                value = property.vector3Value.ToString("F4");
                break;
            case SerializedPropertyType.Vector4:
                value = property.vector4Value.ToString("F4");
                break;
            case SerializedPropertyType.Quaternion:
                value = property.quaternionValue.ToString("F4");
                break;
            case SerializedPropertyType.Rect:
                value = property.rectValue.ToString("F4");
                break;
            case SerializedPropertyType.Color:
                value = property.colorValue.ToString("F4");
                break;
            case SerializedPropertyType.ObjectReference:    //Needs a GameObject assigned (HACK: assign one via inspector without [ReadOnly] first.) 
                if (property.objectReferenceValue != null) value = property.objectReferenceValue.ToString();
                else value = "Null --[PlayMode Only]";
                break;
            default:
                value = "ERROR: Value type unsupported for ReadOnly";
                break;
        }

        EditorGUI.LabelField(position, label.text, value);
    }
}