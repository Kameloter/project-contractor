using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigValve : MonoBehaviour {

    public GameObject line1Path;
    public GameObject line2Path;

    RaycastHit hit;
    private Transform valve;
    public Transform rayPointer;
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

    bool activated = false;

    void Awake()
    {
        line1Sockets = new List<SmallValveSocket>();
        line2Sockets = new List<SmallValveSocket>();

    }
    // Use this for initialization
    void Start () {

        valve = transform.GetChild(0);
        ConnectSmallValves();
        SetupPipeConnection();
        DisableLine(1);
        DisableLine(2);

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

    void OnMouseDown()
    {
        if (InRange && activated)
            Rotate();
    }
    void ActivateLine(int index)
    {
        switch(index)
        {
            case 1:


                smoke1.Play();
                //for (int i = 0; i < line1.Length; i++)
                //{
                //    line1[i].DrawConnection();
                //}

                //foreach (SmallValveSocket socket in line1Sockets)
                //{
                //    socket.ActivateInteractables();
                //}

                break;
            case 2:

                smoke2.Play();
                //for (int i = 0; i < line1.Length; i++)
                //{
                //    line2[i].DrawConnection();
                //}
                //foreach (SmallValveSocket socket in line2Sockets)
                //{
                //    socket.ActivateInteractables();
                //}
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

                //for (int i = 0; i < line1.Length; i++)
                //{
                //    line1[i].DeleteConnection();
                //}
                //foreach (SmallValveSocket socket in line1Sockets)
                //{
                //    socket.DeactivateSocket();
                //}
                break;
            case 2:
                smoke2.Stop();
                float distance2 = Vector3.Distance(transform.position, line2[0].transform.position);
                float waitTime2 = distance2 / line2[0].steamParticleSpeed;
                Invoke("StopLine2", waitTime2);

                //for (int i = 0; i < line2.Length; i++)
                //{
                //    line2[i].DeleteConnection();
                //}
                //foreach(SmallValveSocket socket in line2Sockets)
                //{
                //    socket.DeactivateSocket();
                //}
                break;
        }
    }
    void Rotate()
    {
        // targetRotation = Quaternion.Euler(0, 90, 0) * valve.transform.rotation;
        // inRotation = true;
        valve.transform.Rotate(new Vector3(0, 90, 0));
        if(Physics.Raycast(rayPointer.transform.position,rayPointer.transform.forward,out hit,1f))
        {
            switch(hit.transform.name)
            {
                case "Input":
                    currentState = 3;
                    DisableLine(2);
                    break;

                case "Line1":
                    currentState = 1;
                    ActivateLine(1);
                    break;

                case "Line2":
                    currentState = 2;
                    ActivateLine(2);
                    break;

                case "Off":
                    currentState = 0;
                    DisableLine(1);
                    break;
            }
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
