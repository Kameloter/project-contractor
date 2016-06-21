using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[System.Serializable]
public class BigValve : BaseInteractable {

    [Header("Valve Parts")]
    public Object pipeLong;
    public Object pipeLongWindow;
    public Object pipeSmall;
    public Object pipeJoint;
    public GameObject pipeLine1Start;
    public GameObject pipeLine1End;
    public GameObject pipeLine2Start;
    public GameObject pipeLine2End;
    public GameObject jointHolder1; 
    public GameObject jointHolder2;
    [SerializeField]
    public GameObject[] pipeLine1Points;
    [SerializeField]
    public GameObject[] pipeLine2Points;

    [Header("Steam particles ")]
    public ParticleSystem smoke1;
    public ParticleSystem smoke2;

    [Header("Control states and ID !")]
    [Range(0, 2)]
    [Tooltip("0 = OFF , 1 = Line 1 , 2 = Line 2")]
    public int startState;

    [Tooltip("The current state of the valve")]
    [ReadOnly]
    public int currentState = 0;

    bool InRange = false;
    public int valveID;

    Quaternion targetRotation;

    List<SteamPipeJoint> steamJointsLine1 = null;
    List<SteamPipeJoint> steamJointsLine2 = null;

    private RaycastHit hit;

    /// <summary>
    /// A list with all the SVS (small valve sockets) in Pipe-Line-1
    /// </summary>
    private List<SmallValveSocket> pipeLine1SmallVaveSockets;
    /// <summary>
    /// A list with all the SVS (small valve sockets) in Pipe-Line-1
    /// </summary>
    private List<SmallValveSocket> pipeLine2SmallValveSockets;

    [Header("GRID ")]
    public bool draw = false;

    public float cellSizeY;
    public float cellSizeX;

    public float gridWidth;
    public float gridHeight;

    bool createObjects = false;

    [HideInInspector]
    public bool isPowered = true;

    public override void Awake() {
        if (Application.isPlaying) {
            pipeLine1SmallVaveSockets = new List<SmallValveSocket>();
            pipeLine2SmallValveSockets = new List<SmallValveSocket>();
        }
        base.Awake();
    }

    void Start() {
        if (Application.isPlaying) {
            ConnectSmallValves();
            ConnectJointsTogether(1);
            ConnectJointsTogether(2);

            //if (pipeLine1Points[1] != null)
            //    BuildPipeLine(1);

            //if (pipeLine2Points[1] != null)
            //    BuildPipeLine(2);

            ActivateLine(startState);
        }
    }


    //VLAD: Probably a design error. First time writing that complicated editor script. Since i need the
    // build functions both in this class and in the editor class i implemented them here so i can use them
    // in both classes. Editor script has access to its target(this script). 
    // The drawback of this is that i have to use the pre-processor tags if UNITY_EDIOTR to wrap
    // every EDITOR utily so when building the game we have no issues.
    
    
#if UNITY_EDITOR
    

    

  

   

   
#endif

    

    

   

    void ConnectJointsTogether(int index)
    { 
        if (index == 1)
        {
            steamJointsLine1 = new List<SteamPipeJoint>(jointHolder1.GetComponentsInChildren<SteamPipeJoint>());
            steamJointsLine1.Add(pipeLine1End.GetComponent<SteamPipeJoint>());
            int line1Lenght = steamJointsLine1.Count;
            for (int i = 0; i < line1Lenght; i++)
            {
                if (steamJointsLine1[i] != steamJointsLine1[line1Lenght - 1])
                {
                    steamJointsLine1[i].connectTo = steamJointsLine1[i + 1];
                }
            }
        }
        else {
            steamJointsLine2 = new List<SteamPipeJoint>(jointHolder2.GetComponentsInChildren<SteamPipeJoint>());
            steamJointsLine2.Add(pipeLine2End.GetComponent<SteamPipeJoint>());
            int line2Lenght = steamJointsLine2.Count;
            for (int i = 0; i < line2Lenght; i++)
            {
                if (steamJointsLine2[i] != steamJointsLine2[line2Lenght - 1]) { steamJointsLine2[i].connectTo = steamJointsLine2[i + 1]; }
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

                if (smallValveSockets[i].valveLine == 1) pipeLine1SmallVaveSockets.Add(smallValveSockets[i]);
                else pipeLine2SmallValveSockets.Add(smallValveSockets[i]);
            }
        }
    }

    void ActivateLine(int index)
    {
        switch (index)
        {
            case 1:
                currentState = 1;
                DisableLine(2);
                smoke1.Play();
                break;
            case 2:
                currentState = 2;
                DisableLine(1);
                smoke2.Play();
                break;
             default:

                break;
        }
    }

    //Stop pipe-flow
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

    public override void actionOnTriggerEnter(Collider player) {
        if (isPowered)
        base.actionOnTriggerEnter(player);
    }

    public override void OnInteract() {
        if(currentState == 0 || currentState == 1)
        {
            ActivateLine(2);
        }
        else
        {
            ActivateLine(1);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        if (Application.isPlaying) return; //if we are in play mode , dont care about this grid (no play mode pipe editing)
        if (!draw) return; //if we do not wish to draw => return
        if (Selection.activeGameObject == null) return; //if there is no selected object => return (no error spam in Console)

        if (cellSizeY == 0 || cellSizeX == 0 || gridWidth == 0 || gridHeight == 0)//in case we f*cked up and some of grid sizes are 0 => safely return
        {            
            Debug.LogError(" Some of the grid sizes are 0 safely returning.....!");
            return;
        }                                                                                 

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

}
