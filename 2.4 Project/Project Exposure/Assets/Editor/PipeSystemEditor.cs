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
    int prev_line_1_array_size = 0;
    int prev_line_2_array_size;

    public void OnEnable()
    {
        myValve = (BigValve)target;
        Line_1_debugColor.a = 1;
     
        Line_2_debugColor.a = 1;
        Debug.Log("Hi ! " + myValve.gameObject.name + " <- Activated!");
    }


    private void OnSceneGUI()
    {
        Event e = Event.current;
       
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
        worldMousePos = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos).origin;

        //if (e.isKey)
        //{
        //    if (e.character == 'h')
        //        Debug.Log("tha pos => " + position);
        //}
          
          

        




        myValveTransform = myValve.transform;

        //handleRotation = Tools.pivotRotation == PivotRotation.Local ?
        //    myValveTransform.rotation : Quaternion.identity;

        if (myValve.Pipe_Line_1.Length == 0)
        {
            return;
        }
        else
        {
            if (myValve.Pipe_Line_1[0] == null) return;
            DrawPipeLine(1);
        }



        if (myValve.Pipe_Line_2.Length == 0)
        {
            //Debug.Log("EMPTY POINT ARRAY");
            return;
        }
        else
        {
            if (myValve.Pipe_Line_2[0] == null) return;
            DrawPipeLine(2);
        }



    }

    public override void OnInspectorGUI()
    {
  
        DrawDefaultInspector();
        EditorGUILayout.LabelField("EDITOR PART !");

        serializedObject.Update();
        EditorGUILayout.LabelField("Line 1");

        if (GUILayout.Button("Build corner base"))
        {
            myValve.CreateLine1();
        }
        if (GUILayout.Button("Destroy corner base"))
        {
            myValve.DestroyLine1();
        }
        if (GUILayout.Button("Build pipe-line"))
        {
            myValve.BuildPipeLine();
        }
        if (GUILayout.Button("Destroy pipe-line"))
        {
            myValve.DestroyPipeLine();
        }


        Line_1_debugColor = EditorGUILayout.ColorField(Line_1_debugColor);
        EditorList.Show(serializedObject.FindProperty("Pipe_Line_1"), EditorListOption.Buttons);
        


        EditorGUILayout.LabelField("Line 2 ");
        Line_2_debugColor = EditorGUILayout.ColorField(Line_2_debugColor);
        EditorList.Show(serializedObject.FindProperty("Pipe_Line_2"), EditorListOption.Buttons);



        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(myValve);
    }

    //Private functions
    void DrawPipeLine(int line)
    {
        Vector3 v0 = createPoint(0, line);

        int cachedLength = line == 1 ? myValve.Pipe_Line_1.Length : myValve.Pipe_Line_2.Length;
       
        for (int i = 1; i < cachedLength; i += 2)
        {
            //if ((i + 1) == cashedLenght) return;

            if (line == 1)
            {
                Handles.color = Line_1_debugColor;
                //if (myValve.Pipe_Line_1[i] == null)
                //{
                //    myValve.AddObjectToLine(i, 1, myValve.Pipe_Line_1[0].transform.position);
                //}
                    

            }
            else
            {
                Handles.color = Line_2_debugColor;
                //if (myValve.Pipe_Line_2[i] == null)
                //    myValve.AddObjectToLine(i, 1, myValve.Pipe_Line_1[0].transform.position);
            }

          

            Vector3 v1 = createPoint(i, line);
            Vector3 v2 = Vector3.zero;
    

         
            Handles.color = line == 1 ? Line_1_debugColor : Line_2_debugColor;

            Handles.DrawLine(v0, v1);
            if (i + 1 != cachedLength)
            {
                v2 = createPoint(i + 1, line);
                Handles.DrawLine(v1, v2);
                v0 = v2;
            }
           
         
            
        }
        if (prev_line_1_array_size == 0) prev_line_1_array_size = cachedLength;

        if (cachedLength > prev_line_1_array_size)
        {
            Debug.Log("new item being exhanged.");


            myValve.Pipe_Line_1[cachedLength - 1] = myValve.AddObjectToLine(cachedLength - 1, 1, myValve.Pipe_Line_1[cachedLength - 2].transform.position + new Vector3(1, 0, 1));
            Debug.Log("new item exhanged.");
        }else if(cachedLength < prev_line_1_array_size)
        {
            if (!Application.isPlaying)
            {
                Debug.Log(cachedLength );
                Debug.Log(prev_line_1_array_size);
                Undo.DestroyObjectImmediate(myValve.pipeLine1Holder.transform.GetChild(myValve.pipeLine1Holder.transform.childCount - 1).gameObject);
            }
            else
            {
                Destroy(myValve.pipeLine1Holder.transform.GetChild(myValve.pipeLine1Holder.transform.childCount - 1).gameObject);
            }
            
        }
        prev_line_1_array_size = cachedLength;
    }


    private Vector3 createPoint(int index, int line)
    {

 


        Quaternion rotationHandle = Tools.pivotRotation == PivotRotation.Local ?
            line == 1 ? myValve.Pipe_Line_1[index].transform.rotation : myValveTransform.rotation
             : Quaternion.identity;

        //transform the point from local to world space
        Quaternion rot = Quaternion.identity;

        Vector3 position = line == 1 ? myValveTransform.TransformPoint(myValve.Pipe_Line_1[index].transform.localPosition) : myValveTransform.TransformPoint(myValve.Pipe_Line_2[index]);

        EditorGUI.BeginChangeCheck();

        position = Handles.DoPositionHandle(position, rotationHandle);

        

        if(line == 1)
        {
        //   rot = Handles.RotationHandle(myValve.Pipe_Line_1[index].transform.rotation, position);
            
        }
    
        // Quaternion rotation = Handles.DoRotationHandle()


        if (EditorGUI.EndChangeCheck())
        {
            Vector2 snapPoint = snapPointXZ();
            position.x = snapPoint.x;
            position.z = snapPoint.y;

            Debug.Log("Finished dragging!");

            


            if(line == 1)
            {
                Undo.RecordObject(myValve.Pipe_Line_1[index].transform, "Move Point");
                EditorUtility.SetDirty(myValve.Pipe_Line_1[index].transform);

                myValve.Pipe_Line_1[index].transform.localPosition = myValveTransform.InverseTransformPoint(position);
            //    myValve.Pipe_Line_1[index].transform.rotation = rot;
            }
               
            else
            {
                Undo.RecordObject(myValve, "Move Point");
                EditorUtility.SetDirty(myValve);
                myValve.Pipe_Line_2[index] = myValveTransform.InverseTransformPoint(position);
            }
               
        }

        return position;

    }

    Vector2 snapPointXZ ()
    {
        float snapX = worldMousePos.x;
        float snapZ = worldMousePos.z;

        Vector2 snappedPoint;
        snappedPoint.x = snapX % myValve.cellSizeX < myValve.cellSizeX / 2 ? snapX -= snapX % myValve.cellSizeX : snapX += (myValve.cellSizeX - snapX % myValve.cellSizeX);
        snappedPoint.y = snapZ % myValve.cellSizeY < myValve.cellSizeY / 2 ? snapZ -= snapZ % myValve.cellSizeY : snapZ += (myValve.cellSizeY - snapZ % myValve.cellSizeY);


        return snappedPoint;
    }
}


