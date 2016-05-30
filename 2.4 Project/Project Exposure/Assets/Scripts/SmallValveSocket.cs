using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class SmallValveSocket : MonoBehaviour {

    bool inRange = false;
    GameObject Player;
    PlayerScript playerScript;

    [SerializeField]
    BaseInteractable[] interactables;

    //[HideInInspector]
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

    [Header("If starting with valvehead:")]
    public GameObject socketed = null;

    [Header("Object Canvas")]
    public Canvas objectCanvas;

    void Start () {
        
        objectCanvas = GetComponentInChildren<Canvas>();
        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Assign something to the 'objectCanvas' variable on '" + gameObject.name + "'.");

        sphereColor.a = 1;
        if (Application.isPlaying) {
            playerScript = GameManager.Instance.PlayerScript;

            FindASteamJoint();
			if (socketed != null) {
				PlaceValve(socketed);
			}
        }
    }

   public void FindASteamJoint() {
        colliders = Physics.OverlapSphere(transform.position, radius);
        int cashedLength = colliders.Length;

        if (cashedLength > 0) {
            float potentialShortestDistance = 0;
            float prevShortestDist = float.MaxValue;

            for (int i = 0; i < cashedLength; i++) {
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
                poweredBy.poweredSockets.Add(this);
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
        //--------------This was to pickup without button-----------------------
        //if (playerScript.carriedValve != null && InRange && !socketed) {
        //    PlaceValve(playerScript.carriedValve);
        //} else if (socketed != null && InRange) {
        //    RemoveValve(socketed);
        //} else if
        //----------------------------------------------------------------------
        if (!inRange) {
            GameManager.Instance.Player.GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }

    void PlaceValve(GameObject valve) {
        valve.GetComponent<PickableScript>().Place(this.transform.position + this.transform.up, this.gameObject);
        valve.GetComponent<PickableScript>().clickable = false;
        socketed = valve;
        if (socketed == null) return;
        ActivateInteractables();
    }

    public void ActivateInteractables() {
        if (controlValve.currentState == valveLine) {
         //   print("started with valve" + this.name);
            foreach (BaseInteractable interactable in interactables) {
                interactable.Activate();
            }
        }
    }

    void RemoveValve(GameObject valve) {
        valve.GetComponent<PickableScript>().PickUp();
        valve.GetComponent<PickableScript>().clickable = true;
        socketed = null;
        foreach (BaseInteractable interactable in interactables) {
            interactable.DeActivate();
        }
    }

    public void DeactivateSocket()
    {
        foreach (BaseInteractable interactable in interactables)
        {
            interactable.DeActivate();
        }
    }

    public void Check() {
        if (playerScript.carriedValve != null && inRange && !socketed) {
            PlaceValve(playerScript.carriedValve);
        }
        else if (socketed != null && inRange) {
            RemoveValve(socketed);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            inRange = true;
            if (objectCanvas != null) objectCanvas.enabled = true;
            //if (GameManager.Instance.ClickedObject == this.gameObject) {
            //    Check();
            //}
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            inRange = false;
            if (objectCanvas != null) objectCanvas.enabled = false;
        }
    }
}

#if UNITY_EDITOR
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
#endif