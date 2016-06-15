using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class SmallValveSocket : BaseInteractable {
    GameObject Player;
    PlayerScript playerScript;

    [SerializeField]
    BaseActivatable[] interactables;

    [HideInInspector]
    public BigValve controlValve;
	[Header("ValveHolder")]
	public Transform valveHolder;

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

    public GameObject optionalPath;
    public bool StartAtPlayer = true;
    bool playedCamera = false;

    [HideInInspector]
    public ParticleSystem particle;

    void Start() {
        sphereColor.a = 1;
        if (Application.isPlaying) {
            playerScript = GameManager.Instance.PlayerScript;
            FindASteamJoint();
            if (socketed != null) {
                PlaceValve(socketed);
            }
        }

        particle = GetComponentInChildren<ParticleSystem>();
        if (particle == null) Debug.LogError("No particle in "+ gameObject.name, transform);
    }

    public void FindASteamJoint() {
        poweredBy = null;
        colliders = Physics.OverlapSphere(transform.position, radius);
        int cashedLength = colliders.Length;

        if (cashedLength > 0) {
            float potentialShortestDistance = 0;
            float prevShortestDist = float.MaxValue;

            for (int i = 0; i < cashedLength; i++) {
                if (colliders[i].gameObject.GetComponent<SteamPipeJoint>() != null) {
                    potentialShortestDistance = Vector3.Distance(transform.position, colliders[i].transform.position);

                    if (potentialShortestDistance < prevShortestDist) {
                        poweredBy = colliders[i].GetComponent<SteamPipeJoint>();
                        prevShortestDist = potentialShortestDistance;
                    }
                }
            }
            if (poweredBy == null) {
                Debug.Log(" SMALL VALVE SOCKET WITH NAME \"" + gameObject.name + "\" DOES NOT FIND A STEAM-JOINT TO BE POWERED BY !!!");

            } else {
                Debug.Log("Valve socket with name \"" + gameObject.name + "\" connected to steam-joint with name \"" + poweredBy.gameObject.name + "\"");
                poweredBy.AddToListItem(this);
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        if (Application.isPlaying) return;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif

    void PlaceValve(GameObject valve) {
        if (socketed != null) return;
		valve.GetComponent<PickableScript>().Place(valveHolder.position, this.gameObject);
        valve.GetComponent<PickableScript>().clickable = false;
        socketed = valve;
        particle.Stop();
        ActivateInteractables();
    }

    public void ActivateInteractables() {
        if (controlValve.currentState == valveLine) {
            if (optionalPath != null && !playedCamera)
            {
                Camera.main.GetComponent<CameraControl>().StartCutscene(optionalPath, StartAtPlayer);
                playedCamera = true;
            }
            foreach (BaseActivatable interactable in interactables) {
                interactable.Activate();
            }
        }
    }

    void RemoveValve(GameObject valve) {
        valve.GetComponent<PickableScript>().PickUp();
        valve.GetComponent<PickableScript>().clickable = true;
        socketed = null;
        foreach (BaseActivatable interactable in interactables) {
            interactable.Deactivate();
        }
        if (controlValve.currentState == valveLine) {
            particle.Play();
        }
    }

    public void DeactivateSocket() {
        foreach (BaseActivatable interactable in interactables) {
            if (interactable == null) { Debug.LogError("Interactable missing from SVS => " + gameObject.name); return; }
            interactable.Deactivate();
        }
    }

    void Check() {
        if (playerScript.carriedValve != null && playerInRange && !socketed) {
            PlaceValve(playerScript.carriedValve);
        } else if (socketed != null && playerInRange && playerScript.carriedValve == null) {
            RemoveValve(socketed);
        }
    }

    public override void OnInteract() {
        Check();
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