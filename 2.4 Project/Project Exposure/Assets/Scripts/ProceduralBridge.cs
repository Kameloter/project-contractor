using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class speaks by itself. 
/// </summary>
[ExecuteInEditMode][System.Serializable]
public class ProceduralBridge : BaseActivatable {
    
    /// <summary>
    /// The part used to build the bridge
    /// </summary>
    [SerializeField]
    GameObject bridgePart;
    //The nav mesh obstacle
    [SerializeField]
    GameObject obstacle;
    //Collider
    [SerializeField]
    GameObject colliderOfBridge;

    public Transform end;
    public Transform start;

    [Header("Editor part !!!!")]
    [Tooltip("From top down perspective the part WIDTH")]
    public float partSize;
    [Tooltip("From top down perspective the bridge HEIGHT(gap to go trough)")]
    public float bridgeWidth;

    //make it here so we can access it it the whole script.
    float distanceBetweenStartEnd = 0;
    //the direction the bridge is getting built in world space.
    Vector3 buidDirection;

    public override void Start() {
        base.Start();
        colliderOfBridge.SetActive(false);
    }

    IEnumerator buildAnimation()
    {
        Vector3 offset = Vector3.zero;
        Vector3 posToSet = Vector3.zero;
        float currentDistance = distanceBetweenStartEnd;
        offset.y = -0.05f;
        while (currentDistance >= partSize)
        {
            offset += buidDirection * (partSize / 2);
            posToSet = start.transform.position + offset;

            GameObject part = (GameObject)Instantiate(bridgePart, start.transform.position, Quaternion.identity);
            part.transform.parent = start.transform;
            part.transform.position = posToSet;
            offset += buidDirection * (partSize / 2);

            if (Mathf.Abs(buidDirection.z) == 1) part.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);

            currentDistance -= partSize;
            yield return new WaitForSeconds(0.15f);
        }
    }

    void Build() {
        if(Application.isPlaying)
        {
            StartCoroutine("buildAnimation");
        }
        else
        {
            Vector3 offset = Vector3.zero;
            Vector3 posToSet = Vector3.zero;
            float currentDistance = distanceBetweenStartEnd;
            offset.y = -0.05f;
            while (currentDistance >= partSize)
            {
                offset += buidDirection * (partSize / 2);
                posToSet = start.transform.position + offset;

                GameObject part = (GameObject)Instantiate(bridgePart, start.transform.position, Quaternion.identity);
                part.transform.parent = start.transform;
                part.transform.position = posToSet;
                offset += buidDirection * (partSize / 2);

                if (Mathf.Abs(buidDirection.z) == 1) part.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);

                currentDistance -= partSize;
            }
        } 
    }

    void Destroy() {
        int cachedLenght = start.transform.childCount;

        for (int i = cachedLenght; i > 0; i--) {
            if (!Application.isPlaying) DestroyImmediate(start.transform.GetChild(i - 1).gameObject);
            else Destroy(start.transform.GetChild(i - 1).gameObject);
        }

        obstacle.SetActive(true);
        colliderOfBridge.SetActive(false);
    }

    public override void Activate() {
        if(partSize == 0 ) { Debug.LogError(" Part size of procedural bridge " + gameObject.name + " is 0 , please set size !"); return; }
       
        RaycastHit hit;
     
        buidDirection = end.position - start.position;
        buidDirection.Normalize();

        if (Physics.Raycast(start.position, buidDirection, out hit, 100)) {
            if (hit.transform.name == end.gameObject.name) {
                distanceBetweenStartEnd = Vector3.Distance(start.localPosition, end.localPosition);
                Build();
                FixNavMeshObstacleAndCollider();
            }
        }
        base.Activate();
    }

    /// <summary>
    /// This function fixes the nav-mesh-obstacle on the procedural bridge so the designer does not have to
    /// worry about it. Also it makes a collider for the bridge so we dont have a separate collider 
    /// for each object.
    /// </summary>
    void FixNavMeshObstacleAndCollider()
    {
        if (Mathf.Abs(buidDirection.x) == 1)
        {
            //ref to the navmeshobstacle
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            //set its pos to the start of the bridge
            obstacle.transform.localPosition = start.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(distanceBetweenStartEnd / 2 * buidDirection.x, 1, 0); // add half distance multiplied by direction

            Vector3 colliderSize = new Vector3(distanceBetweenStartEnd, obstacleCollider.size.y, 2); 
            obstacleCollider.size = colliderSize;
            colliderSize.y = 0.3f;
            colliderSize.z = 2.5f;

            colliderOfBridge.transform.localPosition = start.transform.localPosition; 
            colliderOfBridge.transform.localPosition += new Vector3(distanceBetweenStartEnd / 2 * buidDirection.x, 0, 0);
            colliderOfBridge.GetComponent<BoxCollider>().size = colliderSize;
        }
        else
        {
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            obstacle.transform.localPosition = start.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(0, 1, distanceBetweenStartEnd / 2 * buidDirection.z);

            Vector3 colliderSize = new Vector3(2, obstacleCollider.size.y, distanceBetweenStartEnd);

            obstacleCollider.size = colliderSize;

            colliderSize.y = 0.2f;
            colliderSize.x = 2.5f;
            colliderOfBridge.transform.localPosition = start.transform.localPosition;
            colliderOfBridge.transform.localPosition += new Vector3(0, 0, distanceBetweenStartEnd / 2 * buidDirection.z);
            colliderOfBridge.GetComponent<BoxCollider>().size = colliderSize;
        }
        obstacle.SetActive(false);
        colliderOfBridge.SetActive(true);
    }


    public override void Deactivate() {
        StopCoroutine("buildAnimation");
        base.Deactivate();
        Destroy();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (end == null || start == null || Selection.activeGameObject == null) return;
        if (Selection.activeGameObject.transform.root != transform.root) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(end.position, end.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, start.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(start.position, start.localScale);
    }
#endif
}

//Just some quick way to make buttons in the inspector:)
//Did not feel the need to create new class for JUST that.
#if UNITY_EDITOR
[CustomEditor(typeof(ProceduralBridge))]
public class TestBridge : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProceduralBridge myScript = (ProceduralBridge)target;
        if (GUILayout.Button("Build bridge"))
        {
            myScript.Activate();
        }
        if (GUILayout.Button("Destroy bridge"))
        {
            myScript.Deactivate();
        }
    }
}
#endif