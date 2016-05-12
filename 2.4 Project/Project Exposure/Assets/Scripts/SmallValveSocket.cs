using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class SmallValveSocket : MonoBehaviour {

    bool InRange = false;
    GameObject Player;
    PlayerScript playerScript;

    [SerializeField]
    Interactable[] interactables;

    GameObject socketed = null;
    [HideInInspector]
    public BigValve controlValve;

    [Header("Connects to : ")]
    public int valveID;
    public int valveLine;
    [Header("Find sphere properties : ")]
    public float radius;
    public Color sphereColor;

    Collider[] colliders;
    [HideInInspector]
    public SteamPipeJoint poweredBy = null;

    // Use this for initialization
    void Start ()
    {
        sphereColor.a = 1;
        if (Application.isPlaying)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            playerScript = Player.GetComponent<PlayerScript>();

            FindASteamJoint();
        }
    }

   public void FindASteamJoint()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);
        int cashedLength = colliders.Length;

        if (cashedLength > 0)
        {
            float potentialShortestDistance = 0;
            float prevShortestDist = float.MaxValue;

            for (int i = 0; i < cashedLength; i++)
            {
                if (colliders[i].gameObject.GetComponent<SteamPipeJoint>() != null)
                {
                    potentialShortestDistance = Vector3.Distance(transform.position, colliders[i].transform.position);

                    if (potentialShortestDistance < prevShortestDist)
                    {
                        poweredBy = colliders[i].GetComponent<SteamPipeJoint>();
                        prevShortestDist = potentialShortestDistance;
                    }
                }
            }
            if (poweredBy == null)
            {
                Debug.Log(" SMALL VALVE SOCKET WITH NAME \"" + gameObject.name + "\" DOES NOT FIND A STEAM-JOINT TO BE POWERED BY !!!");

            }
            else
            {
                Debug.Log("Valve socket with name \"" + gameObject.name + "\" connected to steam-joint with name \"" + poweredBy.gameObject.name + "\"");
                poweredBy.poweredSocket = this;
            }
              
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying) return;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, radius);
        
    }
#endif
    public void OnCustomEvent()
    {

        if (playerScript.carriedValve != null && InRange && !socketed)
        {
            PlaceValve(playerScript.carriedValve);
        }
        else if (socketed != null && InRange)
        {
            RemoveValve(socketed);
        }
    }

    void PlaceValve(GameObject valve) {
        valve.GetComponent<PickableScript>().Place(this.transform.position + this.transform.up,this.gameObject);
        valve.GetComponent<PickableScript>().clickable = false;
        socketed = valve;
        if(controlValve.currentState == valveLine)
        {
            ActivateInteractables();
        }
        else
        {
            Debug.Log("Line not active! Not doing action.");
        }
     
    }
    public void ActivateInteractables()
    {
        if (socketed == null) return;
        foreach (Interactable interactable in interactables)
        {
            interactable.Activate();
        }

    }
    void RemoveValve(GameObject valve) {
        valve.GetComponent<PickableScript>().PickUp();
        valve.GetComponent<PickableScript>().clickable = true;
        socketed = null;
        foreach (Interactable interactable in interactables) {
            interactable.Deactivate();
        }
    }
    public void DeactivateSocket()
    {
        foreach (Interactable interactable in interactables)
        {
            interactable.Deactivate();
        }
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
        }
    }
}

[CustomEditor(typeof(SmallValveSocket))]
public class ValveEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SmallValveSocket myScript = (SmallValveSocket)target;
        if (GUILayout.Button("Test to find steam pipe joint!"))
        {
            myScript.FindASteamJoint();
        }
    }
}