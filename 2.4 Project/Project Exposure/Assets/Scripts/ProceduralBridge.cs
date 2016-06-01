using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode][System.Serializable]
public class ProceduralBridge : BaseActivatable {

    public GameObject bridgePart;

    public Transform leftPart;
    public Transform rightPart;
    public GameObject holder;

    public float fuckingPartSize;


    float distanceBetweenParts = 0;
    float wholePartsCount = 0;

  

    [SerializeField]
    GameObject obstacle;

    // Use this for initialization
    public override void Start() {
        base.Start();

    }

    void Build(float distance,Vector3 dir) {

        Vector3 offset = Vector3.zero;
        Vector3 posToSet = Vector3.zero;


        while (distance >= fuckingPartSize)
        {
            offset += dir * (fuckingPartSize / 2);
            posToSet = rightPart.transform.position + offset;

            GameObject part = (GameObject)Instantiate(bridgePart,rightPart.transform.position,Quaternion.identity);
            part.transform.parent = rightPart.transform;
            part.transform.position = posToSet;
            offset += dir * (fuckingPartSize / 2);

            if (Mathf.Abs(dir.z) == 1)
            {
                part.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
            }

            distance -= fuckingPartSize;
        }
       
     
        print("Bridge constructed!");
    }


    void Destroy() {
        int cachedLenght = rightPart.transform.childCount;
        print("LENGTH" + cachedLenght);
        for (int i = cachedLenght; i > 0; i--) {
            if (!Application.isPlaying)
                DestroyImmediate(rightPart.transform.GetChild(i - 1).gameObject);
            else
                Destroy(rightPart.transform.GetChild(i - 1).gameObject);

        }

        obstacle.SetActive(true);
        print("Bridge deleted!");
    }


    public override void Activate() {
        base.Activate();
        RaycastHit hit;
        Vector3 dir;
        dir = leftPart.position - rightPart.position;
        dir.Normalize();

        Debug.DrawRay(rightPart.position, dir * 100, Color.red, 5);
        if (Physics.Raycast(rightPart.position, dir, out hit, 100)) {
            
            if (hit.transform.name == "Left") {
                distanceBetweenParts = Vector3.Distance(rightPart.localPosition, leftPart.localPosition);
               // distanceBetweenParts = Mathf.Round(distanceBetweenParts);
                Build(distanceBetweenParts,dir);
                print("Distance " + distanceBetweenParts);
                print("Direction " + dir);

                FixObstacle(distanceBetweenParts,dir);
            }
        }
    }


    void FixObstacle(float dist,Vector3 dir)
    {
        if (Mathf.Abs(dir.x) == 1)
        {
            Debug.Log("LEFT - RIGHT");
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            obstacle.transform.localPosition = rightPart.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(0, 1, dist / 2 * dir.x);

            obstacleCollider.size = new Vector3(2, obstacleCollider.size.y, dist);

        }
        else
        {
            Debug.Log("UP - DOWN");
            NavMeshObstacle obstacleCollider = obstacle.GetComponent<NavMeshObstacle>();

            obstacle.transform.localPosition = rightPart.transform.localPosition;
            obstacle.transform.localPosition += new Vector3(dist / 2 * -dir.z, 1,0); 

            obstacleCollider.size = new Vector3(dist, obstacleCollider.size.y, 2);

        }
        obstacle.SetActive(false);

    }


    public override void Deactivate() {
        base.Deactivate();
        Destroy();
    }
    void OnDrawGizmos()
    {
        if (leftPart == null || rightPart == null) return;
        if (Selection.activeGameObject.transform.root != transform.root) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(leftPart.position, leftPart.localScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, rightPart.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rightPart.position, rightPart.localScale);
    }
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