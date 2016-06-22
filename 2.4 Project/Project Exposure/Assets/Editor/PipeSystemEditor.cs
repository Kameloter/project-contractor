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

    Color Line_1_debugColor;
    Color Line_2_debugColor;

    /// <summary>
    /// Used to checked wether the list size has changed, so it adds/removes an object.
    /// </summary>
    int prevLine1CachedLength = 0;
    /// <summary>
    /// Used to checked wether the list size has changed, so it adds/removes an object.
    /// </summary>
    int prevLine2CachedLength = 0;

    [SerializeField]
    public bool showLine1 = false;
    [SerializeField]
    public bool showLine2 = false;
    

    public void OnEnable()
    {
        
        //Set our MonoBehaviour script as Editor target.
        myValve = (BigValve)target;

        Line_1_debugColor.a = 1;//set default value for debug color, because on creation alpha is '0'
        Line_2_debugColor.a = 1;

        //Debug.Log("Hi ! " + myValve.gameObject.name + " <- Activated!");

        //Get back stored bools in order to keep the same shown/hidden line
        showLine1 = EditorPrefs.GetBool("ShowLine1");
        showLine2 = EditorPrefs.GetBool("ShowLine2");
    }

   
    private void OnSceneGUI()
    {
        Vector2 mousePos = Event.current.mousePosition;
        mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y;
        worldMousePos = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(mousePos).origin;

        myValveTransform = myValve.transform;

        //If we are drawing line 1
        if (showLine1)
        {
            //Draw nothing if we have no points to draw
            if (myValve.pipeLine1Points.Length == 0)
            {
                return;
            }
            else
            {
                //if ACCIDENTLY the start is null (pipe-line got deleted) restart the cached lenght and safely return!
                if (myValve.pipeLine1Points[0] == null) { prevLine1CachedLength = 0; return; }
                
                //Finally draw the line if its safe.
                DrawPipeLine1();
            }
        }
       


        //If we are drawing line 2 
        if (showLine2)
        {
            //Draw nothing if we have no points to draw
            if (myValve.pipeLine2Points.Length == 0)
            {
                return;
            }
            else
            {
                //if ACCIDENTLY the start is null (pipe-line got deleted) restart the cached lenght and safely return!
                if (myValve.pipeLine2Points[0] == null) { prevLine2CachedLength = 0; return; }

                //Finally draw the line if its safe.
                DrawPipeLine2();
            }
        }
    }

   // GUILayoutOption label = GUI.skin.label;

    public override void OnInspectorGUI()
    {
        //Shows inspector of (target) script
        DrawDefaultInspector();

     
        EditorGUILayout.BeginVertical();   //Align objects in a vertical rect
     //   GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("EDITOR PART");


  
        showLine1 = EditorGUILayout.Toggle("Show Line 1", showLine1);
        showLine2 = EditorGUILayout.Toggle("Show Line 2", showLine2);
      //  GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical(); //end aligning
     
        EditorPrefs.SetBool("ShowLine1", showLine1);
        EditorPrefs.SetBool("ShowLine2", showLine2);

       

        //Before working with the serializedobject (myValve) we have to use .Update()

        serializedObject.Update();

        if(showLine1) //if we are drawing line 1 => show available options to the designer
        {
            EditorGUILayout.LabelField("Line 1");
            if (GUILayout.Button("Build corner base"))
            {
               CreateLineJoints(1);
            }
            if (GUILayout.Button("Destroy corner base"))
            {
                DestroyJointLine(1);
            }
            if (GUILayout.Button("Build pipe-line"))
            {
                BuildPipeLine(1);
            }
            if (GUILayout.Button("Destroy pipe-line"))
            {
                DestroyPipeConnections(1);

            }


            Line_1_debugColor = EditorGUILayout.ColorField(Line_1_debugColor);
            EditorList.Show(serializedObject.FindProperty("pipeLine1Points"), EditorListOption.Buttons);
        }

        if(showLine2)
        {
            EditorGUILayout.LabelField("Line 2 ");

            if (GUILayout.Button("Build corner base"))
            {
                CreateLineJoints(2);
            }
            if (GUILayout.Button("Destroy corner base"))
            {
                DestroyJointLine(2);
            }
            if (GUILayout.Button("Build pipe-line"))
            {
                BuildPipeLine(2);
            }
            if (GUILayout.Button("Destroy pipe-line"))
            {
                DestroyPipeConnections(2);

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

            myValve.pipeLine1Points[cachedLength - 2] = AddJointToPipeLine(myValve.jointHolder1.transform, posForNewObject);
             
            Debug.Log("new item exhanged.");
        }
        else if (cachedLength < prevLine1CachedLength)
        {
            myValve.pipeLine1Points[cachedLength - 1] = myValve.pipeLine1End;
            DeleteJointFromPipeLine(1);
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

            myValve.pipeLine2Points[cachedLength - 2] = AddJointToPipeLine(myValve.jointHolder2.transform, posForNewObject);




            Debug.Log("new item exhanged.");
        }
        else if (cachedLength < prevLine2CachedLength)
        {
            myValve.pipeLine2Points[cachedLength - 1] = myValve.pipeLine2End;
            DeleteJointFromPipeLine(2);
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


    #region Snap_Editor_Node_Points_Helper_functions

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

    #endregion

    public void BuildPipeLine(int lineIndex)
    {
        if (lineIndex == 1)
        {
            int line1Lenght = myValve.pipeLine1Points.Length;
            Vector3 v0, v1, v2;
            //Debug.Log("array lenght" + line1Lenght);
            v0 = myValve.pipeLine1Points[0].transform.position;
            for (int i = 1; i < line1Lenght; i += 2) //we start at 1 and we increment by 2
            {
                v1 = myValve.pipeLine1Points[i].transform.position;
                //Debug.Log("First Drawing between " + (i - 1) + "-" + (i));
                //Debug.Log("Vectors before func call  v0 => " + v0 + " .... v1 => " + v1);
                PipePartsCalculation(v0, v1, lineIndex);

                if (i + 1 != line1Lenght)//a hack so  that the algorithm works for even/uneven array size.
                {
                    v2 = myValve.pipeLine1Points[i + 1].transform.position;
                    //Debug.Log("Second Drawing between " + (i) + "-" + (i + 1));
                    //Debug.Log("Vectors before func call  v1 => " + v1 + " .... v2 => " + v2);
                    PipePartsCalculation(v1, v2, lineIndex);
                    v0 = v2;
                }
            }
            //----------------------PROPER ROTATION OF JOINTS !!! ------------------//
            for (int i = 1; i < line1Lenght; i++)
            {
                if (i != line1Lenght - 1)
                {

                    Vector3 dirToPrevious = myValve.pipeLine1Points[i - 1].transform.position - myValve.pipeLine1Points[i].transform.position;
                    Vector3 dirToNext = myValve.pipeLine1Points[i + 1].transform.position - myValve.pipeLine1Points[i].transform.position;

                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i - 1].name + " calc..");
                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i + 1].name + " calc..");

                    FixRotationOfJoints(dirToPrevious, dirToNext, i, lineIndex);
                    //Debug.Log("Calculated for => " + pipeLine1Points[i].name);
                }
            }
            //--------------------------------------------------------------------//
        }
        else {
            int line2Lenght = myValve.pipeLine2Points.Length;
            Vector3 v0, v1, v2;

            //Debug.Log("array lenght" + line2Lenght);
            v0 = myValve.pipeLine2Points[0].transform.position;
            for (int i = 1; i < line2Lenght; i += 2)
            {

                v1 = myValve.pipeLine2Points[i].transform.position;
                PipePartsCalculation(v0, v1, lineIndex);

                if (i + 1 != line2Lenght)
                {
                    v2 = myValve.pipeLine2Points[i + 1].transform.position;
                    PipePartsCalculation(v1, v2, lineIndex);
                    v0 = v2;
                }
            }

            //----------------------PROPER ROTATION OF JOINTS !!! ------------------//
            for (int i = 1; i < line2Lenght; i++)
            {
                if (i != line2Lenght - 1)
                {

                    Vector3 dirToPrevious = myValve.pipeLine2Points[i - 1].transform.position - myValve.pipeLine2Points[i].transform.position;
                    Vector3 dirToNext = myValve.pipeLine2Points[i + 1].transform.position - myValve.pipeLine2Points[i].transform.position;

                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i - 1].name + " calc..");
                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i + 1].name + " calc..");

                    FixRotationOfJoints(dirToPrevious, dirToNext, i, lineIndex);
                    //Debug.Log("Calculated for => " + pipeLine1Points[i].name);
                }
            }
            //--------------------------------------------------------------------//
        }
    }



    void PipePartsCalculation(Vector3 start, Vector3 end, int line)
    {

        //   Debug.Log("FUNCTION  CALCULATE !");
        Vector3 dir = end - start;

        float distance = Mathf.Round(dir.magnitude);
        distance -= 2;
        dir.Normalize();
        //    Debug.Log("--DIRECTION " + dir);
        //     Debug.Log("---DISTANCE " + distance);

        float remainingDistance = distance;

        Vector3 offset = dir;
        Vector3 posToSet;

        while (remainingDistance > 0)
        {
            int random = Random.Range(1, 3);
            GameObject pipePart;
            //  Debug.Log("Random numbher -> " + random);
            if (random == 2)//small 
            {
                if (remainingDistance <= 1)
                {
                    if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                    {
                        offset += new Vector3(0, 0, dir.z / 2);

                        posToSet = start + offset;

                        pipePart = CreateSmallPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);

                        pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        offset += new Vector3(0, 0, dir.z / 2);
                    }
                    else if (Mathf.Abs(dir.x) == 1)//we are moving along X
                    {
                        offset += new Vector3(dir.x / 2, 0, 0);
                        posToSet = start + offset;
                        pipePart = CreateSmallPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);
                        offset += new Vector3(dir.x / 2, 0, 0);

                    }
                    else //we move on Y
                    {
                        offset += new Vector3(0, dir.y / 2, 0);
                        posToSet = start + offset;
                        pipePart = CreateSmallPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);
                        pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                        offset += new Vector3(0, dir.y / 2, 0);
                    }
                    //  Debug.Log("picked small");
                    remainingDistance--;
                }
            }
            if (random == 1)
            {
                if (remainingDistance >= 2)
                {
                    if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                    {
                        offset += new Vector3(0, 0, dir.z);

                        posToSet = start + offset;

                        pipePart = CreateLongPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);

                        pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        offset += new Vector3(0, 0, dir.z);
                    }
                    else if (Mathf.Abs(dir.x) == 1)//we are moving along X
                    {
                        offset += new Vector3(dir.x, 0, 0);
                        posToSet = start + offset;
                        pipePart = CreateLongPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);
                        offset += new Vector3(dir.x, 0, 0);
                    }
                    else //we move on Y
                    {
                        offset += new Vector3(0, dir.y, 0);
                        posToSet = start + offset;
                        pipePart = CreateLongPipe(posToSet, line == 1 ? myValve.pipeLine1Start.transform : myValve.pipeLine2Start.transform);
                        pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                        offset += new Vector3(0, dir.y, 0);
                    }
                    // Debug.Log("picked big");
                    remainingDistance -= 2;
                }
            }
            //Debug.Log("Remaining dist -> " + remainingDistance);
        }
    }




    public void DestroyPipeConnections(int index)
    {
        if (index == 1)
        {
            int line1Lenght = myValve.pipeLine1Start.transform.childCount;
            for (int i = line1Lenght; i > 0; i--)
            {
                if (!Application.isPlaying)
                {
                    Undo.DestroyObjectImmediate(myValve.pipeLine1Start.transform.GetChild(i - 1).gameObject);
                }
                else {
                    Destroy(myValve.pipeLine1Start.transform.GetChild(i - 1).gameObject);
                }
            }
        }
        else {
            int line2Lenght = myValve.pipeLine2Start.transform.childCount;
            for (int i = line2Lenght; i > 0; i--)
            {

                if (!Application.isPlaying)
                {
                    Undo.DestroyObjectImmediate(myValve.pipeLine2Start.transform.GetChild(i - 1).gameObject);
                }
                else {
                    Destroy(myValve.pipeLine2Start.transform.GetChild(i - 1).gameObject);
                }
            }
        }
    }



    public void CreateLineJoints(int lineIndex)
    {
        Undo.RecordObject(myValve, "undo create lines");
        Transform parent;
        if (lineIndex == 1) //We are drawing line 1 ->
        {
            parent = myValve.jointHolder1.transform;    //Choose correct parent
            if (myValve.pipeLine1Points.Length == 0)
            {
                Debug.LogError("Piple-line-1 has 0 objects, make sure you add objects to the line before building,duh.");
                return;
            }

            if (myValve.pipeLine1Points[0] == null)     //Assuming we have nothing in the lineStart => create.
            {
                myValve.pipeLine1Points[0] = myValve.pipeLine1Start;//set start
                int lenght = myValve.pipeLine1Points.Length;
                for (int i = 1; i < lenght - 1; i++)
                {
                    myValve.pipeLine1Points[i] = CreatePipeLineJoint(myValve.pipeLine1Start.transform.localPosition + Vector3.up, parent);
                }
                myValve.pipeLine1Points[lenght - 1] = myValve.pipeLine1End;//set end
            }
        }
        else//We are drawing line 2 =>
        {
            parent = myValve.jointHolder2.transform; //Choose correct parent
            if (myValve.pipeLine2Points.Length == 0)
            {
                Debug.LogError("Piple-line-1 has 0 objects, make sure you add objects to the line before building,duh.");
                return;
            }
            if (myValve.pipeLine2Points[0] == null)//Assuming we have nothing in the lineStart => create.
            {

                myValve.pipeLine2Points[0] = myValve.pipeLine2Start;//set start
                int lenght = myValve.pipeLine2Points.Length;
                for (int i = 1; i < lenght - 1; i++)
                {
                    myValve.pipeLine2Points[i] = CreatePipeLineJoint(myValve.pipeLine2Start.transform.localPosition + Vector3.up, parent);
                }
                myValve.pipeLine2Points[lenght - 1] = myValve.pipeLine2End;//set end
            }
        }
    }

    /// <summary>
    /// Create a joint and return the object. (Expects you to assign the created object to the correct line array index)
    /// Supports UNDO.
    /// </summary>
    /// <param name="parent"> Parent of the joint.</param>
    /// <param name="pos">Position RELATIVE to the parent(Local position)</param>
    /// <returns></returns>
    public GameObject AddJointToPipeLine(Transform parent, Vector3 pos)
    {
        Undo.RecordObject(this, "undo added joint to pipeline");//this one records the script 
                                                                //so we can undo the array size(which is really important in our case)
        GameObject pipeJoint = CreatePipeLineJoint(pos, parent);
        return pipeJoint;
    }



    /// <summary>
    /// Deletes the last pipe-joit in the chosen line.
    /// Supports UNDO.
    /// </summary>
    /// <param name="lineIndex"></param>
    public void DeleteJointFromPipeLine(int lineIndex)
    {
        Undo.RecordObject(myValve, "undo deleted joint from pipeline"); //this one records the script 
                                                                     //so we can undo the array sizze(which is really important in our case)
        if (!Application.isPlaying)
        {
            if (lineIndex == 1)//undo records the destroyed object
            {
                Undo.DestroyObjectImmediate(myValve.jointHolder1.transform.GetChild(myValve.jointHolder1.transform.childCount - 1).gameObject);
                prevLine1CachedLength = 0;//reset the cached legth cause when we delete and then the designer wants to press cntrl-z to revert the change, array size changes and that adds another object so we end up with more objects.
            }
               
            else
            {
                Undo.DestroyObjectImmediate(myValve.jointHolder2.transform.GetChild(myValve.jointHolder2.transform.childCount - 1).gameObject);
            }
              
        }
    }

    public void DestroyJointLine(int index)
    {
        Undo.RecordObject(myValve, "undo destroy lines");
        if (index == 1)
        {
            int lenght = myValve.jointHolder1.transform.childCount;

            //   Undo.RecordObject(pipeLine1Points, "undo start");
            myValve.pipeLine1Points[myValve.pipeLine1Points.Length - 1] = null;
            myValve.pipeLine1Points[0] = null;

            for (int i = lenght; i > 0; i--)
            {
                if (!Application.isPlaying)
                {
                    Undo.DestroyObjectImmediate(myValve.jointHolder1.transform.GetChild(i - 1).gameObject);
                }
                else {
                    Destroy(myValve.jointHolder1.transform.GetChild(i - 1).gameObject);
                }
            }
        }
        else {
            if (myValve.pipeLine2Points[0] != null)
            {
                int lenght = myValve.jointHolder2.transform.childCount;
                //Undo.RecordObject(pipeLine2Points[0].gameObject, "object-null");
                myValve.pipeLine2Points[myValve.pipeLine2Points.Length - 1] = null;
                myValve.pipeLine2Points[0] = null;
                for (int i = lenght; i > 0; i--)
                {
                    if (!Application.isPlaying)
                    {
                        Undo.DestroyObjectImmediate(myValve.jointHolder2.transform.GetChild(i - 1).gameObject);
                    }
                    else {
                        Destroy(myValve.jointHolder2.transform.GetChild(i - 1).gameObject);
                    }
                }
            }
        }
    }


    //Vladislav : this is the dirtiest function i've ever wrote as a coder, sorry. could not figure a way to 
    // rotate objects based on orientation ;/
    void FixRotationOfJoints(Vector3 dirToPrevious, Vector3 dirToNext, int jointArrayIndex, int lineIndex)
    {
        Vector3 prev = dirToPrevious;
        Vector3 next = dirToNext;
        prev.Normalize();
        next.Normalize();

        //Debug.Log("Previous ->> " + prev);
        //Debug.Log("Next     ->> " + next);
        float angleY = 0;
        float angleX = 0;
        float angleZ = 0;

        if (Mathf.Abs(prev.x) == 1) //its coming from left or right
        {
            if (prev.x == 1 && next.z == 1)    //if we go up
            {
                //Debug.Log("-------------- 90 magic degrees !");
                // angleY = 90;
                angleZ = 180;
            }
            else if (prev.x == 1 && next.z == -1)
            {
                //Debug.Log("-------------- 180 magic degrees !");
                angleY = 180;
            }
            else if (prev.x == -1 && next.z == 1)
            {
                //Debug.Log("-------------- 0 magic degrees !");
                angleY = 0;
            }
            else if (prev.x == -1 && next.z == -1)
            {
                angleX = 180;
            }
            else if (next.y == 1)
            {
                angleX = 270;
                angleY = 180;
                if (prev.x == -1) { angleX = 270; angleY = 0; }
            }
            else {
                angleX = 90;
                angleY = 180;
                if (prev.x == -1)
                {
                    angleY = 0;
                    angleX = 90;
                }
            }

        }
        else if (Mathf.Abs(prev.z) == 1)//its coming from forward/back
        {
            if (prev.z == 1 && next.x == 1)    //if we go up
            {
                //Debug.Log("-------------- 90 magic degrees !");
                angleX = 0;
                angleY = 90;
                angleZ = 0;
            }
            else if (prev.z == 1 && next.x == -1)
            {
                //Debug.Log("-------------- 0 magic degrees !");
                angleX = 0;
                angleY = 270;
                angleZ = 180;
            }
            else if (prev.z == -1 && next.x == 1)
            {
                //Debug.Log("-------------- 180 magic degrees !");
                angleX = 0;
                angleY = 90;
                angleZ = 180;
            }
            else if (prev.z == -1 && next.x == -1)
            {
                //Debug.Log("-------------- 270 magic degrees !");
                angleX = 180;
                angleY = 90;
                angleZ = 180;
            }
            else if (next.y == 1)//maybe needs change ? check others too
            {
                //prev Z == 1 && next.y == 1
                angleX = 270;
                angleZ = 90;
                angleY = 0;
                if (prev.z == -1)
                {
                    angleX = 270;
                    angleY = 270;
                    angleZ = 0;
                }
            }
            else// next Y == -1
            {
                //prev.z ==1
                angleX = 90;
                angleY = 90;
                angleZ = 0;
                if (prev.z == -1) { angleX = 90; angleY = 270; angleZ = 0; }
            }
        }
        else if (Mathf.Abs(prev.y) == 1)//its coming from up/down
        {
            if (prev.y == 1)
            {
                angleZ = 270;
            }
            else
            {
                angleZ = 90;
            }

            if (next.x == 1) { angleY = 90; }
            else if (next.x == -1) { angleY = 270; }
            else if (next.z == 1) { angleY = 0; }
            else if (next.z == -1) { angleY = 180; }
        }

        if (lineIndex == 1)
        {
            myValve.pipeLine1Points[jointArrayIndex].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myValve.pipeLine1Points[jointArrayIndex].transform.Rotate(new Vector3(angleX, angleY - myValve.transform.localRotation.eulerAngles.y, angleZ));
        }
        else {
            myValve.pipeLine2Points[jointArrayIndex].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            myValve.pipeLine2Points[jointArrayIndex].transform.Rotate(new Vector3(angleX, angleY - myValve.transform.localRotation.eulerAngles.y, angleZ));
        }
    }




    /// <summary>
    /// Creates a pipe-joint at the desired position with the desired parent.
    /// </summary>
    /// <param name="localPosition"> position relative to the parent </param>
    /// <param name="parent"> parent of the object;</param>
    /// <returns></returns>
    GameObject CreatePipeLineJoint(Vector3 localPosition, Transform parent)
    {
        GameObject obj = null;
        if (!Application.isPlaying)
        {
            obj = (GameObject)PrefabUtility.InstantiatePrefab(myValve.pipeJoint);
        }
        else {
            obj = Instantiate(myValve.pipeJoint) as GameObject;
        }
        obj.transform.parent = parent;
        obj.transform.localPosition = localPosition;


        Undo.RegisterCreatedObjectUndo(obj, "Created go");


        return obj;
    }

    GameObject CreateSmallPipe(Vector3 pos, Transform parent)
    {
        GameObject obj = null;


        if (!Application.isPlaying)
        {

            obj = (GameObject)PrefabUtility.InstantiatePrefab(myValve.pipeSmall);

        }
        else {
            obj = Instantiate(myValve.pipeSmall) as GameObject;
        }

        obj.transform.position = pos;
        obj.transform.parent = parent;


        Undo.RegisterCreatedObjectUndo(obj, "Created go");

        return obj;
    }


    GameObject CreateLongPipe(Vector3 pos, Transform parent)
    {
        GameObject obj = null;
        int rnd = Random.Range(1, 4);

        if (!Application.isPlaying)
        {
            obj = (GameObject)PrefabUtility.InstantiatePrefab(rnd == 1 ? myValve.pipeLongWindow : myValve.pipeLong);
        }
        else {
            obj = Instantiate(rnd == 1 ? myValve.pipeLongWindow : myValve.pipeLong) as GameObject;
        }

        obj.transform.position = pos;
        obj.transform.parent = parent;
        Undo.RegisterCreatedObjectUndo(obj, "Created go");

        return obj;
    }


}


