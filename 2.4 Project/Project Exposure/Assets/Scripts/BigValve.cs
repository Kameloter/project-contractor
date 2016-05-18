using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class BigValve : MonoBehaviour {

    public GameObject line1Path;
    public GameObject line2Path;

    RaycastHit hit;
    private Transform valve;
    public Transform rayPointer;
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
            switch(startState)
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
            //DisableLine(1);
            //DisableLine(2);

        }
    }

    void SetupPipeConnection()
    {
        line1 = line1Path.GetComponentsInChildren<SteamPipeJoint>();
        int line1Lenght = line1.Length;
        for (int i = 0; i < line1Lenght; i++)
        {
           // print("name order -> " + line1[i].gameObject.name);
            if(line1[i] != line1[line1Lenght - 1])
            {
              //  Debug.Log("set " + line1[i].name + " to " + line1[i + 1].name);
                line1[i].connectTo = line1[i + 1];
            }
            
        }

        line2 = line2Path.GetComponentsInChildren<SteamPipeJoint>();
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
            if(smallValveSockets[i].valveID == valveID)
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


    public void OnCustomEvent() {
        if (InRange && activated) {
            Rotate();
        }
        else if (!InRange && activated) {
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }

    void OnMouseDown()
    {
        //if (InRange && activated)
        //    Rotate();
    }

    void ActivateLine(int index)
    {
        switch(index)
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

        switch((int)valve.localEulerAngles.y)
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

    void OnParticleCollision(GameObject go) {
      
            if (!activated) {
                activated = true;
            }
        

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InRange = true;
            if (GameManager.Instance.ClickedObject == this.gameObject) {
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
}

[CustomEditor(typeof(BigValve))]
public class BigValveEditor : Editor
{


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BigValve myScript = (BigValve)target;

    }
}