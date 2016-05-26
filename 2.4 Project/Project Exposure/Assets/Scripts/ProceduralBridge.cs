using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode][System.Serializable]
public class ProceduralBridge : BaseInteractable {

    public GameObject bridgePart;
    public GameObject leftPart;
    public GameObject rightPart;
    public GameObject holder;

    Transform leftTr;
    Transform rightTr;


    float distanceBetweenParts = 0;
    float wholePartsCount = 0;
    bool bridgeBuilt = false;

    [SerializeField]
    GameObject obstacle;

	// Use this for initialization
	void Start ()
    {
        leftTr = leftPart.transform;
        rightTr = rightPart.transform;

        if(Application.isPlaying)
        {
            if (holder.transform.childCount > 0)
                bridgeBuilt = true;
        }
    }

	void Build()
    {
        Debug.Log(gameObject.name + "I AM GETTING BUILT MOTHERFUCKER !");
        for (int i = 0; i < wholePartsCount; i++)
        {
            Vector3 direction = leftTr.position - rightTr.position;
            direction.Normalize();
            GameObject part = (GameObject)Instantiate(bridgePart, new Vector3(rightTr.position.x + ((i + 0.5f) * bridgePart.transform.lossyScale.x) * direction.x, rightTr.position.y, rightTr.position.z + ((i + 0.5f) * bridgePart.transform.lossyScale.z) * direction.z), Quaternion.identity);
            part.transform.parent = holder.transform;
            // bridgeParts.Add(part);
            //yield return new WaitForSeconds(0.15f);
        }
        obstacle.SetActive(false);
        //print("Bridge constructed!");
    }


    void Destroy()
    {
        if(bridgeBuilt)
        {

            int cachedLenght = holder.transform.childCount;
            print("LENGTH"+cachedLenght);
            for (int i = cachedLenght;i > 0; i--)
            {
                if(!Application.isPlaying)
                    DestroyImmediate(holder.transform.GetChild(i - 1).gameObject);
                else
                    Destroy(holder.transform.GetChild(i - 1).gameObject);
                
            }

            obstacle.SetActive(true);
            bridgeBuilt = false;
            print("Bridge deleted!");
        }
    }


    public override void Activate() {
        if (bridgeBuilt) { print("bridge already built!!!!!"); return; }

        RaycastHit hit;
        if (Physics.Raycast(rightTr.position, leftTr.position - rightTr.position, out hit, 1000)) {
            if (hit.transform.name == "Left") {
                bridgeBuilt = true;
                distanceBetweenParts = Vector3.Distance(rightTr.position, leftTr.position);
                wholePartsCount = Mathf.Ceil(distanceBetweenParts);
                wholePartsCount /= bridgePart.transform.lossyScale.x;
                Build();
            }
        }
    }

    public override void DeActivate() {
        Destroy();
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
            myScript.DeActivate();
        }

    }
}
#endif