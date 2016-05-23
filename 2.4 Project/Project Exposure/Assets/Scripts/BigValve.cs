using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[ExecuteInEditMode]
[System.Serializable]
public class BigValve : MonoBehaviour
{

    [Header("Builder Parts")]
    public Object pipeLongWindow;
    public Object pipeLong;
    public Object pipeSmall;
    public Object pipeJoint;
    public GameObject pipeLine1Start;
    public GameObject pipeLine2Start;

    [Header("Pipe-Line-Holders")]
    public GameObject pipeLine1Holder;
    public GameObject pipeLine2Holder;

    RaycastHit hit;
    private Transform valve;

    [Tooltip("0 = OFF , 1 = Line 1 , 2 = Line 2, 3 = INPUT !")]
    public int startState;
    public int currentState = 0;
    bool InRange = false;
    public int valveID;
    bool inRotation = false;

    Quaternion targetRotation;

    SteamPipeJoint[] line1;
    SteamPipeJoint[] line2;

    public ParticleSystem smoke1;
    public ParticleSystem smoke2;


    private List<SmallValveSocket> line1Sockets;
    private List<SmallValveSocket> line2Sockets;

    Quaternion startRot;

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

    [SerializeField]
    public GameObject[] Pipe_Line_1;
    public Vector3[] Pipe_Line_2;

    bool createObjects = false;
    void Awake()
    {
        if (Application.isPlaying)
        {
            line1Sockets = new List<SmallValveSocket>();
            line2Sockets = new List<SmallValveSocket>();
        }


    }
    // Use this for initialization
    void Start()
    {
        if (Application.isPlaying)
        {
            valve = transform.GetChild(0);
            startRot = valve.transform.rotation;


            ConnectSmallValves();
            SetupPipeConnection();
            switch (startState)
            {
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



    public void CreateLine1()
    {
        if (Pipe_Line_1.Length == 0)
        {
            Debug.LogError("Piple-line-1 has 0 objects, make sure you add objects to the line before building,duh.");
            return;
        }

        if (Pipe_Line_1[0] == null)
        {
            Pipe_Line_1[0] = pipeLine1Start;
            for (int i = 1; i < Pipe_Line_1.Length; i++)
            {
                Pipe_Line_1[i] = CreatePipeLineJoint(transform.right, pipeLine1Holder.transform);
            }
        }

    }



    public GameObject AddObjectToLine(int index, int line, Vector3 pos)
    {
        if (line == 1)
        {

            Pipe_Line_1[index] = CreatePipeLineJoint(pos, pipeLine1Holder.transform);
            return Pipe_Line_1[index];
        }
        else
        {
            // Pipe_Line_2[index] = CreatePipeLineJoint(pos, pipeLine1Holder.transform);
            return Pipe_Line_1[index];
        }
    }
    public void DestroyLine1()
    {
        if (Pipe_Line_1[0] != null)
        {
            int lenght = pipeLine1Holder.transform.childCount;
            Pipe_Line_1[0] = null;
            for (int i = lenght; i > 0; i--)
            {
                if (!Application.isPlaying)
                {
                    Undo.DestroyObjectImmediate(pipeLine1Holder.transform.GetChild(i - 1).gameObject);
                }
                else
                {
                    Destroy(pipeLine1Holder.transform.GetChild(i - 1).gameObject);
                }
            }

        }

    }
    public void DestroyPipeLine()
    {
        int line1Lenght = pipeLine1Start.transform.childCount;
        for (int i = line1Lenght; i > 0; i--)
        {

            if (!Application.isPlaying)
            {
                Undo.DestroyObjectImmediate(pipeLine1Start.transform.GetChild(i - 1).gameObject);
            }
            else
            {
                Destroy(pipeLine1Start.transform.GetChild(i - 1).gameObject);
            }
        }
    }

    int pipeLine1TurnsMade = 0;
    public void BuildPipeLine()
    {
        SetupPipeConnection();

        int line1Lenght = Pipe_Line_1.Length;
        Vector3 v0, v1, v2;


        Debug.Log("array lenght" + line1Lenght);
        pipeLine1TurnsMade = 0;
        v0 = Pipe_Line_1[0].transform.position;
        for (int i = 1; i < line1Lenght; i += 2)
        {



            v1 = Pipe_Line_1[i].transform.position;
            Debug.Log("First Drawing between " + (i - 1) + "-" + (i));
            Debug.Log("Vectors before func call  v0 => " + v0 + " .... v1 => " + v1);
            PipePartsCalculation(v0, v1);
            pipeLine1TurnsMade++;
            Debug.Log("Finished drawing (after weird function.");





            if (i + 1 != line1Lenght)
            {
                v2 = Pipe_Line_1[i + 1].transform.position;
                Debug.Log("Second Drawing between " + (i) + "-" + (i + 1));
                Debug.Log("Vectors before func call  v1 => " + v1 + " .... v2 => " + v2);
                PipePartsCalculation(v1, v2);
                pipeLine1TurnsMade++;
                Debug.Log("Finished drawing (after weird function.");
                v0 = v2;
            }






        }
    }

    void PipePartsCalculation(Vector3 start, Vector3 end)
    {

        Debug.Log("FUNCTION  CALCULATE !");
        Vector3 dir = end - start;

        float distance = Mathf.Round(dir.magnitude);
        distance -= 2;
        dir.Normalize();
        Debug.Log("--DIRECTION " + dir);
        Debug.Log("---DISTANCE " + distance);

        float remainingDistance = distance;

        Vector3 offset = dir;
        Vector3 posToSet;

        while (remainingDistance > 0)
        {
            
            int random = Random.Range(1, 3);
            GameObject big;
            GameObject small;
            
            Debug.Log("Random numbher -> " + random);
          
          
            if (random == 1)//small 
            {
                if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                {
                    offset += new Vector3(0, 0, dir.z / 2);

                    posToSet = start + offset;

                    small = CreateSmallPipe(posToSet, pipeLine1Start.transform);

                    small.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                }
                else //we are moving along X
                {
                    offset += new Vector3(dir.x / 2,0,0) ;
                    posToSet = start + offset;
                    small = CreateSmallPipe(posToSet, pipeLine1Start.transform);

                }


                Debug.Log("picked small");
                remainingDistance--;
            }

            if(random == 2)
            {
                if (remainingDistance > 2)
                {

                    if (Mathf.Abs(dir.z) == 1) //we are moving along Z
                    {
                        offset += new Vector3(0, 0, dir.z * 1.5f);

                        posToSet = start + offset;

                        small = CreateLongPipe(posToSet, pipeLine1Start.transform);

                        small.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                    }
                    else //we are moving along X
                    {
                        offset += new Vector3(dir.x * 1.5f, 0, 0);
                        posToSet = start + offset;
                        small = CreateLongPipe(posToSet, pipeLine1Start.transform);

                    }


                    Debug.Log("picked big");
                    remainingDistance-=2;
                }
            }

            Debug.Log("Remaining dist -> " + remainingDistance);
        }


    }

    void SetupPipeConnection()
    {

        //has to change 

        line1 = pipeLine1Holder.GetComponentsInChildren<SteamPipeJoint>();
        int line1Lenght = line1.Length;
        for (int i = 0; i < line1Lenght; i++)
        {
            // print("name order -> " + line1[i].gameObject.name);
            if (line1[i] != line1[line1Lenght - 1])
            {
                //  Debug.Log("set " + line1[i].name + " to " + line1[i + 1].name);
                line1[i].connectTo = line1[i + 1];
            }

        }

        line2 = pipeLine2Holder.GetComponentsInChildren<SteamPipeJoint>();
        int line2Lenght = line2.Length;
        for (int i = 0; i < line2Lenght; i++)
        {

            if (line2[i] != line2[line2Lenght - 1])
            {

                line2[i].connectTo = line2[i + 1];
            }

        }
    }



    void ConnectSmallValves()
    {
        SmallValveSocket[] smallValveSockets = FindObjectsOfType<SmallValveSocket>();
        for (int i = 0; i < smallValveSockets.Length; i++)
        {
            if (smallValveSockets[i].valveID == valveID)
            {
                smallValveSockets[i].controlValve = this;

                if (smallValveSockets[i].valveLine == 1)
                    line1Sockets.Add(smallValveSockets[i]);
                else
                    line2Sockets.Add(smallValveSockets[i]);
            }
        }
    }
    void FixedUpdate()
    {
        //if (inRotation)
        //{

        //    valve.transform.rotation = Quaternion.Slerp(valve.transform.rotation, targetRotation, Time.deltaTime);
        //    while (valve.transform.rotation != targetRotation)
        //    {

        //    }
        //    //inRotation = false;
        //}

    }

    public void OnCustomEvent()
    {
        if (InRange && activated)
        {
            Rotate();
        }
        else if (!InRange && activated)
        {
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }


    void ActivateLine(int index)
    {
        switch (index)
        {
            case 1:
                smoke1.Play();
                break;
            case 2:
                smoke2.Play();
                break;
        }
    }
    void StopLine1()
    {
        line1[0].StopSteamConnection();
    }
    void StopLine2()
    {
        line2[0].StopSteamConnection();
    }
    void DisableLine(int index)
    {
        switch (index)
        {
            case 1:

                smoke1.Stop();

                float distance = Vector3.Distance(transform.position, line1[0].transform.position);
                float waitTime = distance / line1[0].steamParticleSpeed;
                Invoke("StopLine1", waitTime);



                break;
            case 2:

                smoke2.Stop();

                float distance2 = Vector3.Distance(transform.position, line2[0].transform.position);
                float waitTime2 = distance2 / line2[0].steamParticleSpeed;
                Invoke("StopLine2", waitTime2);


                break;
        }
    }
    void SetRotation(float rot)
    {
        valve.Rotate(new Vector3(0, rot, 0));

        switch ((int)rot)
        {
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
    void Rotate()
    {

        valve.Rotate(new Vector3(0, -90, 0));

        switch ((int)valve.localEulerAngles.y)
        {
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

    void OnParticleCollision(GameObject go)
    {

        if (!activated)
        {
            activated = true;
        }


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = true;
            if (GameManager.Instance.ClickedObject == this.gameObject)
            {
                Rotate();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = false;
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //   print("on draw gizmo running !");
        if (Selection.activeGameObject == null) return;
        if (Application.isPlaying) return;
        if (!draw) return;

        if (cellSizeY == 0 || cellSizeX == 0 || gridWidth == 0 || gridHeight == 0) return;

        if (Selection.activeGameObject.transform.root == transform.root)
        {
            Vector3 pos = transform.position;

            for (float x = pos.x - gridWidth / 2; x <= pos.x + gridWidth / 2; x += cellSizeX)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(new Vector3(x, 0.0f, pos.z - gridHeight / 2),
                                new Vector3(x, 0.0f, pos.z + gridHeight / 2));
            }

            for (float z = pos.z - gridHeight / 2; z <= pos.z + gridHeight / 2; z += cellSizeY)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(new Vector3(pos.x - gridWidth / 2, 0.0f, z),
                                new Vector3(pos.x + gridWidth / 2, 0.0f, z));
            }
        }





    }
#endif


    GameObject CreatePipeLineJoint(Vector3 pos, Transform parent)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeJoint);

        obj.transform.position = transform.TransformPoint(pos);
        //obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = parent;
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
        return obj;
    }
    GameObject CreateSmallPipe(Vector3 pos, Transform parent)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeSmall);

        //obj.transform.rotation = Quaternion.identity;

        obj.transform.position = pos;
        obj.transform.parent = parent;
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
        return obj;
    }
    GameObject CreateLongPipe(Vector3 pos, Transform parent)
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(pipeLong);

        obj.transform.position = pos;
        //obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = parent;
        Undo.RegisterCreatedObjectUndo(obj, "Created go");
        return obj;
    }
}

