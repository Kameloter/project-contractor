using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public class BigValve : BaseInteractable {

    [Header("Builder Parts")]
    public Object pipeLongWindow;
    public Object pipeLong;
    public Object pipeSmall;
    public Object pipeJoint;

    public GameObject pipeLine1Start;
    public GameObject pipeLine1End;
    public GameObject pipeLine2Start;
    public GameObject pipeLine2End;

    [Header("Pipe-Line-Holders")]
    public GameObject jointHolder1; //CHILDS ARE ONLY THE JOINTS !! so pipeLine1Points[1] => jointHolder.transform.getchild(0) -first child.
    public GameObject jointHolder2;

    RaycastHit hit;
    private Transform valve;

    [Tooltip("0 = OFF , 1 = Line 1 , 2 = Line 2, 3 = INPUT !")]
    public int startState;
    public int currentState = 0;
    bool InRange = false;
    public int valveID;

    Quaternion targetRotation;

    List<SteamPipeJoint> steamJointsLine1 = null;
    List<SteamPipeJoint> steamJointsLine2 = null;

    public ParticleSystem smoke1;
    public ParticleSystem smoke2;

    private List<SmallValveSocket> line1Sockets;
    private List<SmallValveSocket> line2Sockets;

    bool activated = false;

    float inStateRot = 0;
    float line2rot = 90;
    float offRot = 180;
    float line1rot = 270;

    [Header("GRID ")]
    public bool draw = false;

    public float cellSizeY;
    public float cellSizeX;

    public float gridWidth;
    public float gridHeight;

    [SerializeField] public GameObject[] pipeLine1Points;
    [SerializeField] public GameObject[] pipeLine2Points;
    [SerializeField] public GameObject testForUndo;

    bool createObjects = false;

    public override void Awake() {
        if (Application.isPlaying) {
            line1Sockets = new List<SmallValveSocket>();
            line2Sockets = new List<SmallValveSocket>();
        }
        base.Awake();
    }

    void Start() {
        if (Application.isPlaying) {
            valve = transform.GetChild(0);

            ConnectSmallValves();
            ConnectJointsTogether(1);
            ConnectJointsTogether(2);
            if (pipeLine1Points[1] != null)
                BuildPipeLine(1);
           if( pipeLine2Points[1] != null)
                 BuildPipeLine(2);
            switch (startState) {
                case 0:
                    SetRotation(offRot);
                    break;
                case 1:
                    SetRotation(line1rot);
                    break;
                case 2:
                    SetRotation(line2rot);
                    break;
                case 3:
                    SetRotation(inStateRot);
                    break;
            }
        }
    }

#if UNITY_EDITOR
    public void CreateLineJoints(int lineIndex) {
        Undo.RecordObject(this, "undo create lines");
        Transform parent;
        if (lineIndex == 1) //We are drawing line 1 ->
        {
            parent = jointHolder1.transform;    //Choose correct parent
            if (pipeLine1Points.Length == 0) {
                Debug.LogError("Piple-line-1 has 0 objects, make sure you add objects to the line before building,duh.");
                return;
            }

            if (pipeLine1Points[0] == null)     //Assuming we have nothing in the lineStart => create.
            {
                pipeLine1Points[0] = pipeLine1Start;//set start
                int lenght = pipeLine1Points.Length;
                for (int i = 1; i < lenght - 1; i++) {
                    pipeLine1Points[i] = CreatePipeLineJoint(transform.right, parent);
                }
                pipeLine1Points[lenght - 1] = pipeLine1End;//set end
            }
        } else//We are drawing line 2 =>
          {
            parent = jointHolder2.transform; //Choose correct parent
            if (pipeLine2Points.Length == 0) {
                Debug.LogError("Piple-line-1 has 0 objects, make sure you add objects to the line before building,duh.");
                return;
            }
            if (pipeLine2Points[0] == null)//Assuming we have nothing in the lineStart => create.
            {

                pipeLine2Points[0] = pipeLine2Start;//set start
                int lenght = pipeLine2Points.Length;
                for (int i = 1; i < lenght - 1; i++) {
                    pipeLine2Points[i] = CreatePipeLineJoint(-transform.right, parent);
                }
                pipeLine2Points[lenght - 1] = pipeLine2End;//set end
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
    public GameObject AddJointToPipeLine(Transform parent, Vector3 pos) {
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
    public void DeleteJointFromPipeLine(int lineIndex) {
        Undo.RecordObject(this, "undo deleted joint from pipeline"); //this one records the script 
                                                                     //so we can undo the array sizze(which is really important in our case)
        if (!Application.isPlaying) {
            if (lineIndex == 1)//undo records the destroyed object
                Undo.DestroyObjectImmediate(jointHolder1.transform.GetChild(jointHolder1.transform.childCount - 1).gameObject);
            else
                Undo.DestroyObjectImmediate(jointHolder2.transform.GetChild(jointHolder2.transform.childCount - 1).gameObject);
        }
    }

    public void DestroyJointLine(int index) {
        Undo.RecordObject(this, "undo destroy lines");
        if (index == 1) {
            int lenght = jointHolder1.transform.childCount;

            //   Undo.RecordObject(pipeLine1Points, "undo start");
            pipeLine1Points[pipeLine1Points.Length - 1] = null;
            pipeLine1Points[0] = null;

            for (int i = lenght; i > 0; i--) {
                if (!Application.isPlaying) {
                    Undo.DestroyObjectImmediate(jointHolder1.transform.GetChild(i - 1).gameObject);
                } else {
                    Destroy(jointHolder1.transform.GetChild(i - 1).gameObject);
                }
            }
        } else {
            if (pipeLine2Points[0] != null) {
                int lenght = jointHolder2.transform.childCount;
                //Undo.RecordObject(pipeLine2Points[0].gameObject, "object-null");
                pipeLine2Points[pipeLine2Points.Length - 1] = null;
                pipeLine2Points[0] = null;
                for (int i = lenght; i > 0; i--) {
                    if (!Application.isPlaying) {
                        Undo.DestroyObjectImmediate(jointHolder2.transform.GetChild(i - 1).gameObject);
                    } else {
                        Destroy(jointHolder2.transform.GetChild(i - 1).gameObject);
                    }
                }
            }
        }
    }

    public void DestroyPipeConnections(int index) {
        if (index == 1) {
            int line1Lenght = pipeLine1Start.transform.childCount;
            for (int i = line1Lenght; i > 0; i--) {

                if (!Application.isPlaying) {
                    Undo.DestroyObjectImmediate(pipeLine1Start.transform.GetChild(i - 1).gameObject);
                } else {
                    Destroy(pipeLine1Start.transform.GetChild(i - 1).gameObject);
                }
            }
        } else {
            int line2Lenght = pipeLine2Start.transform.childCount;
            for (int i = line2Lenght; i > 0; i--) {

                if (!Application.isPlaying) {
                    Undo.DestroyObjectImmediate(pipeLine2Start.transform.GetChild(i - 1).gameObject);
                } else {
                    Destroy(pipeLine2Start.transform.GetChild(i - 1).gameObject);
                }
            }
        }
    }
#endif

    public void BuildPipeLine(int lineIndex) {
        if (lineIndex == 1) {
            int line1Lenght = pipeLine1Points.Length;
            Vector3 v0, v1, v2;

            //Debug.Log("array lenght" + line1Lenght);
            v0 = pipeLine1Points[0].transform.position;
            for (int i = 1; i < line1Lenght; i += 2) //we start at 1 and we increment by 2
            {
                v1 = pipeLine1Points[i].transform.position;
                //Debug.Log("First Drawing between " + (i - 1) + "-" + (i));
                //Debug.Log("Vectors before func call  v0 => " + v0 + " .... v1 => " + v1);
                PipePartsCalculation(v0, v1, lineIndex);

                if (i + 1 != line1Lenght)//a hack so  that the algorithm works for even/uneven array size.
                {
                    v2 = pipeLine1Points[i + 1].transform.position;
                    //Debug.Log("Second Drawing between " + (i) + "-" + (i + 1));
                    //Debug.Log("Vectors before func call  v1 => " + v1 + " .... v2 => " + v2);
                    PipePartsCalculation(v1, v2, lineIndex);
                    v0 = v2;
                }
            }
            //----------------------PROPER ROTATION OF JOINTS !!! ------------------//
            for (int i = 1; i < line1Lenght; i++) {
                if (i != line1Lenght - 1) {

                    Vector3 dirToPrevious = pipeLine1Points[i - 1].transform.position - pipeLine1Points[i].transform.position;
                    Vector3 dirToNext = pipeLine1Points[i + 1].transform.position - pipeLine1Points[i].transform.position;

                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i - 1].name + " calc..");
                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i + 1].name + " calc..");

                    FixRotationOfJoints(dirToPrevious, dirToNext, i, lineIndex);
                    //Debug.Log("Calculated for => " + pipeLine1Points[i].name);
                }
            }
            //--------------------------------------------------------------------//
        } else {
            int line2Lenght = pipeLine2Points.Length;
            Vector3 v0, v1, v2;

            //Debug.Log("array lenght" + line2Lenght);
            v0 = pipeLine2Points[0].transform.position;
            for (int i = 1; i < line2Lenght; i += 2) {

                v1 = pipeLine2Points[i].transform.position;
                PipePartsCalculation(v0, v1, lineIndex);

                if (i + 1 != line2Lenght) {
                    v2 = pipeLine2Points[i + 1].transform.position;
                    PipePartsCalculation(v1, v2, lineIndex);
                    v0 = v2;
                }
            }

            //----------------------PROPER ROTATION OF JOINTS !!! ------------------//
            for (int i = 1; i < line2Lenght; i++) {
                if (i != line2Lenght - 1) {

                    Vector3 dirToPrevious = pipeLine2Points[i - 1].transform.position - pipeLine2Points[i].transform.position;
                    Vector3 dirToNext = pipeLine2Points[i + 1].transform.position - pipeLine2Points[i].transform.position;

                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i - 1].name + " calc..");
                    //Debug.Log("Direction from ... " + pipeLine1Points[i].name + " ...to... " + pipeLine1Points[i + 1].name + " calc..");

                    FixRotationOfJoints(dirToPrevious, dirToNext, i, lineIndex);
                    //Debug.Log("Calculated for => " + pipeLine1Points[i].name);
                }
            }
            //--------------------------------------------------------------------//
        }
    }

    void PipePartsCalculation(Vector3 start, Vector3 end, int line) {

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

        while (remainingDistance > 0) {
            int random = Random.Range(1, 3);
            GameObject pipePart;
            //  Debug.Log("Random numbher -> " + random);

            if (random == 1)//small 
            {
                if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                {
                    offset += new Vector3(0, 0, dir.z / 2);

                    posToSet = start + offset;

                    pipePart = CreateSmallPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);

                    pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    offset += new Vector3(0, 0, dir.z / 2);
                } else if (Mathf.Abs(dir.x) == 1)//we are moving along X
                  {
                    offset += new Vector3(dir.x / 2, 0, 0);
                    posToSet = start + offset;
                    pipePart = CreateSmallPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);
                    offset += new Vector3(dir.x / 2, 0, 0);

                } else //we move on Y
                  {
                    offset += new Vector3(0, dir.y / 2, 0);
                    posToSet = start + offset;
                    pipePart = CreateSmallPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);
                    pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    offset += new Vector3(0, dir.y / 2, 0);
                }
                //  Debug.Log("picked small");
                remainingDistance--;
            }

            if (random == 2) {
                if (remainingDistance > 2) {
                    if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                    {
                        offset += new Vector3(0, 0, dir.z);

                        posToSet = start + offset;

                        pipePart = CreateLongPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);

                        pipePart.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                        offset += new Vector3(0, 0, dir.z);
                    } else if (Mathf.Abs(dir.x) == 1)//we are moving along X
                      {
                        offset += new Vector3(dir.x, 0, 0);
                        posToSet = start + offset;
                        pipePart = CreateLongPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);
                        offset += new Vector3(dir.x, 0, 0);
                    } else //we move on Y
                      {
                        offset += new Vector3(0, dir.y, 0);
                        posToSet = start + offset;
                        pipePart = CreateLongPipe(posToSet, line == 1 ? pipeLine1Start.transform : pipeLine2Start.transform);
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

    void FixRotationOfJoints(Vector3 dirToPrevious, Vector3 dirToNext, int jointArrayIndex, int lineIndex) {
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
            } else if (prev.x == 1 && next.z == -1) {
                //Debug.Log("-------------- 180 magic degrees !");
                angleY = 180;
            } else if (prev.x == -1 && next.z == 1) {
                //Debug.Log("-------------- 0 magic degrees !");
                angleY = 0;
            } else if (prev.x == -1 && next.z == -1) {
                //Debug.Log("-------------- 270 magic degrees !");
                //angleY = 270;
                angleX = 180;
            } else if (next.y == 1) {
                angleX = 270;
                angleY = 180;
                if (prev.x == -1) { angleX = 270; angleY = 0; }
            } else {
                angleX = 90;
                angleY = 180;
                if (prev.x == -1) {
                    angleY = 0;
                    angleX = 90;
                }
            }

        } else if (Mathf.Abs(prev.z) == 1)//its coming from forward/back
          {
            if (prev.z == 1 && next.x == 1)    //if we go up
            {
                //Debug.Log("-------------- 90 magic degrees !");
                angleX = 0;
                angleY = 90;
                angleZ = 0;
            } else if (prev.z == 1 && next.x == -1) {
                //Debug.Log("-------------- 0 magic degrees !");
                angleX = 0;
                angleY = 270;
                angleZ = 180;
            } else if (prev.z == -1 && next.x == 1) {
                //Debug.Log("-------------- 180 magic degrees !");
                angleX = 0;
                angleY = 90;
                angleZ = 180;
            } else if (prev.z == -1 && next.x == -1) {
                //Debug.Log("-------------- 270 magic degrees !");
                angleX = 180;
                angleY = 90;
                angleZ = 180;
            } else if (next.y == 1)//maybe needs change ? check others too
              {
                //prev Z == 1 && next.y == 1
                angleX = 270;
                angleZ = 90;
                angleY = 0;
                if (prev.z == -1) {
                    angleX = 270;
                    angleY = 270;
                    angleZ = 0;
                }
            } else// next Y == -1
              {
                //prev.z ==1
                angleX = 90;
                angleY = 90;
                angleZ = 0;
                if (prev.z == -1) { angleX = 90; angleY = 270; angleZ = 0; }
            }
        } else if (Mathf.Abs(prev.y) == 1)//its coming from up/down
          {
            if (prev.y == 1) { angleZ = 270; } 
            else { angleZ = 90; }

            if      (next.x == 1)   { angleY = 90;  } 
            else if (next.x == -1)  { angleY = 270; } 
            else if (next.z == 1)   { angleY = 0;   } 
            else if (next.z == -1)  { angleY = 180; }
        }

        if (lineIndex == 1) {
            pipeLine1Points[jointArrayIndex].transform.localRotation = Quaternion.identity;
            pipeLine1Points[jointArrayIndex].transform.Rotate(new Vector3(angleX, angleY, angleZ));
        } else {
            pipeLine2Points[jointArrayIndex].transform.localRotation = Quaternion.identity;
            pipeLine2Points[jointArrayIndex].transform.Rotate(new Vector3(angleX, angleY, angleZ));
        }
    }

    void ConnectJointsTogether(int index) {
        if (index == 1) {
            steamJointsLine1 = new List<SteamPipeJoint>(jointHolder1.GetComponentsInChildren<SteamPipeJoint>());
            steamJointsLine1.Add(pipeLine1End.GetComponent<SteamPipeJoint>());
            int line1Lenght = steamJointsLine1.Count;
            for (int i = 0; i < line1Lenght; i++) {
                if (steamJointsLine1[i] != steamJointsLine1[line1Lenght - 1]) {
                    steamJointsLine1[i].connectTo = steamJointsLine1[i + 1];
                }
            }
        } else {
            steamJointsLine2 = new List<SteamPipeJoint>(jointHolder2.GetComponentsInChildren<SteamPipeJoint>());
            steamJointsLine2.Add(pipeLine2End.GetComponent<SteamPipeJoint>());
            int line2Lenght = steamJointsLine2.Count;
            for (int i = 0; i < line2Lenght; i++) {
                if (steamJointsLine2[i] != steamJointsLine2[line2Lenght - 1]) { steamJointsLine2[i].connectTo = steamJointsLine2[i + 1]; }
            }
        }
    }

    void ConnectSmallValves() {
        SmallValveSocket[] smallValveSockets = FindObjectsOfType<SmallValveSocket>();
        for (int i = 0; i < smallValveSockets.Length; i++) {
            if (smallValveSockets[i].valveID == valveID) {
                smallValveSockets[i].controlValve = this;

                if (smallValveSockets[i].valveLine == 1) line1Sockets.Add(smallValveSockets[i]);
                else line2Sockets.Add(smallValveSockets[i]);
            }
        }
    }

    void ActivateLine(int index) {
        switch (index) {
            case 1:
                smoke1.Play();
                break;
            case 2:
                smoke2.Play();
                break;
        }
    }

    void StopLine1() {
        steamJointsLine1[0].StopSteamConnection();
    }

    void StopLine2() {
        steamJointsLine2[0].StopSteamConnection();
    }

    void DisableLine(int index) {
        switch (index) {
            case 1:
                smoke1.Stop();
                float distance = Vector3.Distance(transform.position, steamJointsLine1[0].transform.position);
                float waitTime = distance / steamJointsLine1[0].steamParticleSpeed;
                Invoke("StopLine1", waitTime);
                break;
            case 2:
                smoke2.Stop();
                float distance2 = Vector3.Distance(transform.position, steamJointsLine2[0].transform.position);
                float waitTime2 = distance2 / steamJointsLine2[0].steamParticleSpeed;
                Invoke("StopLine2", waitTime2);
                break;
        }
    }

    void SetRotation(float rot) {
        valve.Rotate(new Vector3(0, rot, 0));

        switch ((int)rot) {
            case 0:
                currentState = 0;
                DisableLine(2);
                break;
            case 90:
                currentState = 2;
                ActivateLine(2);
                break;
            case 180:
                currentState = 3;
                DisableLine(1);
                break;
            case 270:
                currentState = 1;
                ActivateLine(1);
                break;
        }
    }

    void Rotate() {
        valve.Rotate(new Vector3(0, -90, 0));

        switch ((int)valve.localEulerAngles.y) {
            case 0:
                currentState = 0;
                DisableLine(2);
                break;
            case 90:
                currentState = 2;
                ActivateLine(2);
                break;
            case 180:
                currentState = 3;
                DisableLine(1);
                break;
            case 270:
                currentState = 1;
                ActivateLine(1);
                break;
        }
    }

    public override void OnInteract() {
        Rotate();
    }


#if UNITY_EDITOR
    void OnDrawGizmos() {
        if (Selection.activeGameObject == null) return;
        if (Application.isPlaying) return;
        if (!draw) return;
        if (cellSizeY == 0 || cellSizeX == 0 || gridWidth == 0 || gridHeight == 0) return;

        //   print("on draw gizmo running !");
        // Debug.Log(SceneView.lastActiveSceneView.rotation.eulerAngles);
        Vector3 eulerRot = SceneView.lastActiveSceneView.rotation.eulerAngles;
        if (eulerRot.x == 90) {

            if (Selection.activeGameObject.transform.root == transform.root) {
                Vector3 pos = transform.position;

                for (float x = pos.x - gridWidth / 2; x <= pos.x + gridWidth / 2; x += cellSizeX) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(new Vector3(x, 0.0f, pos.z - gridHeight / 2),
                                    new Vector3(x, 0.0f, pos.z + gridHeight / 2));
                }

                for (float z = pos.z - gridHeight / 2; z <= pos.z + gridHeight / 2; z += cellSizeY) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(new Vector3(pos.x - gridWidth / 2, 0.0f, z),
                                    new Vector3(pos.x + gridWidth / 2, 0.0f, z));
                }
            }
        } else if (eulerRot.y == 180 || eulerRot.y == 0)//we are drawing a X/Y grid
          {
            // Debug.Log("draw 180 / 0");
            if (Selection.activeGameObject.transform.root == transform.root) {
                Vector3 pos = transform.position;

                for (float x = pos.x - gridWidth / 2; x <= pos.x + gridWidth / 2; x += cellSizeX) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(new Vector3(x, pos.y - gridHeight / 2, pos.z),
                                    new Vector3(x, pos.y + gridHeight / 2, pos.z));
                }

                for (float y = pos.y - gridHeight / 2; y <= pos.y + gridHeight / 2; y += cellSizeY) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(new Vector3(pos.x - gridWidth / 2, y, pos.z),
                                    new Vector3(pos.x + gridWidth / 2, y, pos.z));
                }
            }
        } else {
            // Debug.Log("draw 90 / 270");
            if (Selection.activeGameObject.transform.root == transform.root) {
                Vector3 pos = transform.position;

                for (float z = pos.x - gridWidth / 2; z <= pos.x + gridWidth / 2; z += cellSizeX) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(new Vector3(pos.x, pos.y - gridHeight / 2, z),
                                    new Vector3(pos.x, pos.y + gridHeight / 2, z));
                }

                for (float y = pos.y - gridHeight / 2; y <= pos.y + gridHeight / 2; y += cellSizeY) {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(new Vector3(pos.x, y, pos.z - gridWidth / 2),
                                    new Vector3(pos.x, y, pos.z + gridWidth / 2));
                }
            }
        }

    }
#endif

    /// <summary>
    /// Creates a pipe-joint at the desired position with the desired parent.
    /// </summary>
    /// <param name="localPosition"> position relative to the parent </param>
    /// <param name="parent"> parent of the object;</param>
    /// <returns></returns>
    GameObject CreatePipeLineJoint(Vector3 localPosition, Transform parent) {
        GameObject obj = null;
        if (!Application.isPlaying) {
#if UNITY_EDITOR
            obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeJoint);
#endif
        } else {
            obj = Instantiate(pipeJoint) as GameObject;
        }
        obj.transform.parent = parent;
        obj.transform.localPosition = localPosition;
        //obj.transform.rotation = Quaternion.identity;
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
#endif

        return obj;
    }

    GameObject CreateSmallPipe(Vector3 pos, Transform parent) {
        GameObject obj = null;


        if (!Application.isPlaying) {
#if UNITY_EDITOR
            obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeSmall);
#endif
        } else {
            obj = Instantiate(pipeSmall) as GameObject;
        }
        //obj.transform.rotation = Quaternion.identity;

        obj.transform.position = pos;
        obj.transform.parent = parent;
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
#endif
        return obj;
    }

    GameObject CreateLongPipe(Vector3 pos, Transform parent)
    {
        GameObject obj = null;
        if (!Application.isPlaying) {
#if UNITY_EDITOR
            obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeLong);
#endif
        } else {
            obj = Instantiate(pipeLong) as GameObject;
        }
        obj.transform.position = pos;
        //obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = parent;
#if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
#endif
        return obj;
    }
}
