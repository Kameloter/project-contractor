using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode][System.Serializable]
public class ProceduralBridge : BaseActivatable {


    
    [SerializeField]
    GameObject bridgePart;
    [SerializeField]
    GameObject obstacle;
    [SerializeField]
    GameObject colliderHolder;

    public Transform leftSide;
    public Transform rightSide;


    [Header("Editor part !!!!")]
    public float partSize;
    float distanceBetweenParts = 0;
    float wholePartsCount = 0;
    Vector3 dir;

   

    // Use this for initialization
    public override void Start() {
        base.Start();
        colliderHolder.SetActive(false);
    }
    IEnumerator buildAnimation()
    {
        Vector3 offset = Vector3.zero;
        Vector3 posToSet = Vector3.zero;
        float currentDistance = distanceBetweenParts;
        offset.y = -0.05f;
        while (currentDistance >= partSize)
        {
            offset += dir * (partSize / 2);
            posToSet = rightSide.transform.position + offset;

            GameObject part = (GameObject)Instantiate(bridgePart, rightSide.transform.position, Quaternion.identity);
            part.transform.parent = rightSide.transform;
            part.transform.position = posToSet;
            offset += dir * (partSize / 2);

            if (Mathf.Abs(dir.z) == 1)
            {
                part.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            }

            currentDistance -= partSize;
           // Debug.Log("Builded part");
            yield return new WaitForSeconds(0.15f);
        }
      //  Debug.Log("loop coroutine finish");
      //  print("Bridge constructed!");
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
            float currentDistance = distanceBetweenParts;
            offset.y = -0.05f;
            while (currentDistance >= partSize)
            {
                offset += dir * (partSize / 2);
                posToSet = rightSide.transform.position + offset;

                GameObject part = (GameObject)Instantiate(bridgePart, rightSide.transform.position, Quaternion.identity);
                part.transform.parent = rightSide.transform;
                part.transform.position = posToSet;
                offset += dir * (partSize / 2);

                if (Mathf.Abs(dir.z) == 1)
                {
                    part.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
                }

                currentDistance -= partSize;
                Debug.Log("Builded part");
            }
            Debug.Log("loop coroutine finish");
            print("Bridge constructed!");
        } 
    }


    void Destroy() {
        int cachedLenght = rightSide.transform.childCount;
        print("LENGTH" + cachedLenght);
        for (int i = cachedLenght; i > 0; i--) {
            if (!Application.isPlaying)
                DestroyImmediate(rightSide.transform.GetChild(i - 1).gameObject);
            else
                Destroy(rightSide.transform.GetChild(i - 1).gameObject);

        }

        obstacle.SetActive(true);
        colliderHolder.SetActive(false);
        print("Bridge deleted!");
    }


    public override void Activate() {
        if(partSize == 0 ) { Debug.LogError(" Part size of procedural bridge " + gameObject.name + " is 0 , please set size !"); return; }
       
        RaycastHit hit;
     
        dir = leftSide.position - rightSide.position;
        dir.Normalize();

       // Debug.DrawRay(rightPart.position, dir * 100, Color.red, 5);
        if (Physics.Raycast(rightSide.position, dir, out hit, 100)) {
            
            if (hit.transform.name == "Left") {
                distanceBetweenParts = Vector3.Distance(rightSide.localPosition, leftSide.localPosition);
                Build();
                FixObstacle();
            }
        }
        
        base.Activate();
    }


    void FixObstacle()
    {
        if (Mathf.Abs(dir.x) == 1)
        {
            Debug.Log("LEFT - RIGHT");
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            obstacle.transform.localPosition = rightSide.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(distanceBetweenParts / 2 * dir.x, 1, 0);
            Vector3 colliderSize = new Vector3(distanceBetweenParts, obstacleCollider.size.y, 2);
            obstacleCollider.size = colliderSize;
            colliderSize.y = 0.3f;
            colliderSize.z = 2.5f;
            colliderHolder.transform.localPosition = rightSide.transform.localPosition;
            colliderHolder.transform.localPosition += new Vector3(distanceBetweenParts / 2 * dir.x, 0, 0);
            colliderHolder.GetComponent<BoxCollider>().size = colliderSize;

        }
        else
        {
            Debug.Log("UP - DOWN");
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            obstacle.transform.localPosition = rightSide.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(0, 1, distanceBetweenParts / 2 * dir.z);

           Vector3 colliderSize = new Vector3(2, obstacleCollider.size.y, distanceBetweenParts);

            obstacleCollider.size = colliderSize;

            colliderSize.y = 0.2f;
            colliderSize.x = 2.5f;
            colliderHolder.transform.localPosition = rightSide.transform.localPosition;
            colliderHolder.transform.localPosition += new Vector3(0, 0, distanceBetweenParts / 2 * dir.z);
            colliderHolder.GetComponent<BoxCollider>().size = colliderSize;

        }
        obstacle.SetActive(false);
        colliderHolder.SetActive(true);

    }


    public override void Deactivate() {
        StopCoroutine("buildAnimation");
        base.Deactivate();
        Destroy();
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (leftSide == null || rightSide == null || Selection.activeGameObject == null) return;
        if (Selection.activeGameObject.transform.root != transform.root) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftSide.position, leftSide.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, rightSide.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rightSide.position, rightSide.localScale);
    }
#endif
}

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