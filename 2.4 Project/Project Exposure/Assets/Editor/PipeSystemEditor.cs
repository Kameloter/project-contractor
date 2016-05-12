using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PipeJointPoint))]
public class PipeSystemEditor : Editor
{


    private Transform handleTransform;
    private Quaternion handleRotation;
    private PipeJointPoint targetPoint;

    private void OnSceneGUI()
    {
        targetPoint = target as PipeJointPoint;

        handleTransform = targetPoint.transform;

        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        if (targetPoint.points.Length == 0) { Debug.Log("EMPTY POINT ARRAY"); return; }

      

        Vector3 v0 = createPoint(0);
        int cashedLenght = targetPoint.points.Length;
        for (int i = 1; i < cashedLenght; i+=2)
        {

            if ((i + 1) == cashedLenght) return;

            Vector3 v1 = createPoint(i);
            Vector3 v2 = createPoint(i + 1);

            Handles.color = Color.red;

            Handles.DrawLine(v0, v1);
            Handles.DrawLine(v1, v2);
            v0 = v2;

        }




    }



    private Vector3 createPoint(int index)
    {
        

        Vector3 point = handleTransform.TransformPoint(targetPoint.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetPoint, "Move Point");
            EditorUtility.SetDirty(targetPoint);
            targetPoint.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}

