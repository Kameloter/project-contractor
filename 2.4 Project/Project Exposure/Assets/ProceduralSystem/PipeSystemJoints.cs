
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class PipeSystemJoints : MonoBehaviour
{
    public List<PipeJointPoint> pipeJoints = new List<PipeJointPoint>();

    
}


[System.Serializable]
public class PipeJointPoint : Object
{
    public Vector3 position = Vector3.zero;
    public Vector3 tangentDir = Vector3.right;
    public float tangentWeightIn = 1.0f;
    public float tangentWeightOut = 1.0f;

}



[CustomEditor(typeof(PipeSystemJoints))]
public class ValveSystem : Editor
{

    private SerializedObject trackDataSO;
    private PipeSystemJoints myTrackData;
    private List<bool> showTrackDataPoint = new List<bool>();

    void OnEnable()
    {
        trackDataSO = new SerializedObject(target);
     
    }

    public override void OnInspectorGUI()
    {
        myTrackData = (PipeSystemJoints)target;
        trackDataSO.Update();

        EditorGUILayout.BeginVertical();

        int i = 0;
        if(myTrackData.pipeJoints.Count > 0 )
        {
            foreach (PipeJointPoint tp in myTrackData.pipeJoints)
            {
                //showTrackDataPoint[i] = EditorGUILayout.Foldout(showTrackDataPoint[i], "Point #" + i);
                //if (showTrackDataPoint[i])
                //{

                  //  tp.position = EditorGUILayout.Vector3Field("Position:", tp.position);
                   // tp.tangentDir = EditorGUILayout.Vector3Field("TangentDir:", tp.tangentDir);
                   // tp.tangentWeightIn = EditorGUILayout.FloatField("Tangent Weight In:", tp.tangentWeightIn);
//                    tp.tangentWeightOut = EditorGUILayout.FloatField("Tangent Weight Out:", tp.tangentWeightOut);

                //}
                //i++;
            }

            EditorGUILayout.EndVertical();

        }
       

        if (GUILayout.Button("Add point"))
        {
            myTrackData.pipeJoints.Add(new PipeJointPoint());
            showTrackDataPoint.Add(false);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(myTrackData);
        }

        trackDataSO.ApplyModifiedProperties();
    }
}