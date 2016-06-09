using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BigValve))]
public class PipeSystemEditor : Editor
{


    private Vector3 worldMousePos;

    BigValve myValve;

    private Transform myValveTransform;
    private Quaternion handleRotation;

    private Color Line_1_debugColor;
    private Color Line_2_debugColor;
    int prevLine1CachedLength = 0;
    int prevLine2CachedLength = 0;

    [SerializeField]
    public bool showLine1 = false;
    [SerializeField]
    public bool showLine2 = false;
    

    public void OnEnable()
    {
        myValve = (BigValve)target;
        Line_1_debugColor.a = 1;
        Line_2_debugColor.a = 1;
        Debug.Log("Hi ! " + myValve.gameObject.name + " <- Activated!");


        showLine1 = EditorPrefs.GetBool("ShowLine1");
        showLine2 = EditorPrefs.GetBool("ShowLine2");
    }


    private void OnSceneGUI()
    {
        Event e = Event.current;

        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
        worldMousePos = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos).origin;

        myValveTransform = myValve.transform;

        if (showLine1)
        {
            //myValve.pipeLine1Start.SetActive(true);
            //myValve.pipeLine1End.SetActive(true);
            //myValve.pipeLine2Start.SetActive(false);
            //myValve.pipeLine2End.SetActive(false);

            if (myValve.pipeLine1Points.Length == 0)
            {
                return;
            }
            else
            {
                if (myValve.pipeLine1Points[0] == null) { prevLine1CachedLength = 0; return; }
                DrawPipeLine1();
            }
        }
       



        if (showLine2)
        {
            //myValve.pipeLine1Start.SetActive(false);
            //myValve.pipeLine1End.SetActive(false);
            //myValve.pipeLine2Start.SetActive(true);
            //myValve.pipeLine2End.SetActive(true);
            if (myValve.pipeLine2Points.Length == 0)
            {
                //Debug.Log("EMPTY POINT ARRAY");
                return;
            }
            else
            {
                if (myValve.pipeLine2Points[0] == null) { prevLine2CachedLength = 0; return; }
                DrawPipeLine2();
            }
        }
    }

    public override void OnInspectorGUI()
    {
  
        DrawDefaultInspector();


   
        EditorGUILayout.LabelField("EDITOR PART !");
        EditorGUILayout.BeginVertical();

        showLine1 = EditorGUILayout.Toggle("Show Line 1", showLine1);
        showLine2 = EditorGUILayout.Toggle("Show Line 2", showLine2);

        EditorPrefs.SetBool("ShowLine1", showLine1);
        EditorPrefs.SetBool("ShowLine2", showLine2);

        EditorGUILayout.EndVertical();

        serializedObject.Update();
        if(showLine1)
        {
          
            EditorGUILayout.LabelField("Line 1");
            if (GUILayout.Button("Build corner base"))
            {
                myValve.CreateLineJoints(1);
            }
            if (GUILayout.Button("Destroy corner base"))
            {
                myValve.DestroyJointLine(1);
            }
            if (GUILayout.Button("Build pipe-line"))
            {
                myValve.BuildPipeLine(1);
            }
            if (GUILayout.Button("Destroy pipe-line"))
            {
                myValve.DestroyPipeConnections(1);

            }


            Line_1_debugColor = EditorGUILayout.ColorField(Line_1_debugColor);
            EditorList.Show(serializedObject.FindProperty("pipeLine1Points"), EditorListOption.Buttons);
        }

        if(showLine2)
        {
            EditorGUILayout.LabelField("Line 2 ");

            if (GUILayout.Button("Build corner base"))
            {
                myValve.CreateLineJoints(2);
            }
            if (GUILayout.Button("Destroy corner base"))
            {
                myValve.DestroyJointLine(2);
            }
            if (GUILayout.Button("Build pipe-line"))
            {
                myValve.BuildPipeLine(2);
            }
            if (GUILayout.Button("Destroy pipe-line"))
            {
                myValve.DestroyPipeConnections(2);

            }

            Line_2_debugColor = EditorGUILayout.ColorField(Line_2_debugColor);
            EditorList.Show(serializedObject.FindProperty("pipeLine2Points"), EditorListOption.Buttons);


        }





        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(myValve);
    }
    void DrawPipeLine1()
    {
        Vector3 v0 = createPoint(0, 1);

        int cachedLength = myValve.pipeLine1Points.Length;
        Handles.color = Line_1_debugColor;

        for (int i = 1; i < cachedLength; i += 2)
        {
            Vector3 v1 = createPoint(i, 1);
            Vector3 v2 = Vector3.zero;

            Handles.DrawLine(v0, v1);
            if (i + 1 != cachedLength)
            {
                v2 = createPoint(i + 1, 1);
                Handles.DrawLine(v1, v2);
                v0 = v2;
            }



        }
        if (prevLine1CachedLength == 0) prevLine1CachedLength = cachedLength;

        if (cachedLength > prevLine1CachedLength)
        {
            Debug.Log("new item being exhanged.");
            myValve.pipeLine1Points[cachedLength - 1] = myValve.pipeLine1End;
            Vector3 posForNewObject = myValve.pipeLine1Points[cachedLength - 3].transform.localPosition + Vector3.right ;

            myValve.pipeLine1Points[cachedLength - 2] = myValve.AddJointToPipeLine(myValve.jointHolder1.transform, posForNewObject);
             
            Debug.Log("new item exhanged.");
        }
        else if (cachedLength < prevLine1CachedLength)
        {
            myValve.pipeLine1Points[cachedLength - 1] = myValve.pipeLine1End;
            myValve.DeleteJointFromPipeLine(1);
        }
        prevLine1CachedLength = cachedLength;
    }
    //Private functions
    void DrawPipeLine2()
    {
        Vector3 v0 = createPoint(0, 2);

        int cachedLength = myValve.pipeLine2Points.Length;
        Handles.color = Line_2_debugColor;

        for (int i = 1; i < cachedLength; i += 2)
        {
            Vector3 v1 = createPoint(i, 2);
            Vector3 v2 = Vector3.zero;

            Handles.DrawLine(v0, v1);
            if (i + 1 != cachedLength)
            {
                v2 = createPoint(i + 1, 2);
                Handles.DrawLine(v1, v2);
                v0 = v2;
            }



        }
        if (prevLine2CachedLength == 0) prevLine2CachedLength = cachedLength;

        if (cachedLength > prevLine2CachedLength)
        {
            Debug.Log("new item being exhanged.");
            myValve.pipeLine2Points[cachedLength - 1] = myValve.pipeLine2End;
            Vector3 posForNewObject = myValve.pipeLine2Points[cachedLength - 3].transform.localPosition + Vector3.right;

            myValve.pipeLine2Points[cachedLength - 2] = myValve.AddJointToPipeLine(myValve.jointHolder2.transform, posForNewObject);




            Debug.Log("new item exhanged.");
        }
        else if (cachedLength < prevLine2CachedLength)
        {
            myValve.pipeLine2Points[cachedLength - 1] = myValve.pipeLine2End;
            myValve.DeleteJointFromPipeLine(2);
        }
        prevLine2CachedLength = cachedLength;
    }


    private Vector3 createPoint(int index, int line)
    {




        Quaternion rotationHandle = Tools.pivotRotation == PivotRotation.Local ?
            line == 1 ? myValve.pipeLine1Points[index].transform.rotation : myValve.pipeLine2Points[index].transform.rotation :
             Quaternion.identity;

        //transform the point from local to world space
        Vector3 position = line == 1 ? myValveTransform.TransformPoint(myValve.pipeLine1Points[index].transform.localPosition) : myValveTransform.TransformPoint(myValve.pipeLine2Points[index].transform.localPosition);
        EditorGUI.BeginChangeCheck();

        position = Handles.DoPositionHandle(position, rotationHandle);

        if (index != 0)
        {
       

            if (EditorGUI.EndChangeCheck())
            {
                Vector3 eulerRot = SceneView.lastActiveSceneView.rotation.eulerAngles;
                if (eulerRot.x == 90)
                {
                    Vector2 snapPoint = snapPointXZ();
                    position.x = snapPoint.x;
                    position.z = snapPoint.y;

                }
                else if (eulerRot.y == 180 || eulerRot.y == 0)//we are drawing a X/Y grid
                {
                    Vector2 snapPoint = snapPointXY();
                    position.x = snapPoint.x;
                    position.y = snapPoint.y;
                }
                else
                {
                    Vector2 snapPoint = snapPointZY();
                    position.z = snapPoint.x;
                    position.y = snapPoint.y;
                }

                if (line == 1)
                {
                    Undo.RecordObject(myValve.pipeLine1Points[index].transform, "Move Point");
                    EditorUtility.SetDirty(myValve.pipeLine1Points[index].transform);

                    myValve.pipeLine1Points[index].transform.localPosition = myValveTransform.InverseTransformPoint(position);
                    //    myValve.Pipe_Line_1[index].transform.rotation = rot;
                }

                else
                {
                    Undo.RecordObject(myValve.pipeLine2Points[index].transform, "Move Point");
                    EditorUtility.SetDirty(myValve.pipeLine2Points[index].transform);
                    myValve.pipeLine2Points[index].transform.localPosition = myValveTransform.InverseTransformPoint(position);
                }

            }
        }
       
       
        return position;
    }

    Vector2 snapPointXZ()
    {
        float snapX = worldMousePos.x;
        float snapZ = worldMousePos.z;

        Vector2 snappedPoint;
        snappedPoint.x = snapX % myValve.cellSizeX < myValve.cellSizeX / 2 ? snapX -= snapX % myValve.cellSizeX : snapX += (myValve.cellSizeX - snapX % myValve.cellSizeX);
        snappedPoint.y = snapZ % myValve.cellSizeY < myValve.cellSizeY / 2 ? snapZ -= snapZ % myValve.cellSizeY : snapZ += (myValve.cellSizeY - snapZ % myValve.cellSizeY);


        return snappedPoint;
    }
    Vector2 snapPointXY()
    {
        float snapX = worldMousePos.x;
        float snapY = worldMousePos.y;

        Vector2 snappedPoint;
        snappedPoint.x = snapX % myValve.cellSizeX < myValve.cellSizeX / 2 ? snapX -= snapX % myValve.cellSizeX : snapX += (myValve.cellSizeX - snapX % myValve.cellSizeX);
        snappedPoint.y = snapY % myValve.cellSizeY < myValve.cellSizeY / 2 ? snapY -= snapY % myValve.cellSizeY : snapY += (myValve.cellSizeY - snapY % myValve.cellSizeY);


        return snappedPoint;
    }
    Vector2 snapPointZY()
    {
        float snapZ = worldMousePos.z;
        float snapY = worldMousePos.y;

        Vector2 snappedPoint;
        snappedPoint.x = snapZ % myValve.cellSizeX < myValve.cellSizeX / 2 ? snapZ -= snapZ % myValve.cellSizeX : snapZ += (myValve.cellSizeX - snapZ % myValve.cellSizeX);
        snappedPoint.y = snapY % myValve.cellSizeY < myValve.cellSizeY / 2 ? snapY -= snapY % myValve.cellSizeY : snapY += (myValve.cellSizeY - snapY % myValve.cellSizeY);


        return snappedPoint;
    }

}


