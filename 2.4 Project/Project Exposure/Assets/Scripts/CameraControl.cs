using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// This script is attached to the main camera and it follows the target Game Object provided.
/// </summary>
public class CameraControl : MonoBehaviour
{
    public GameObject targetToFollow;
    private Vector3 nextCamPos;
    //Camera Rotating
 //   [HideInInspector]
   // public float startTime;
  //  [HideInInspector]
  //  public float length;
   // private float completed;
    [HideInInspector]
    public Quaternion currentRotation;
    [HideInInspector]
    public Quaternion newRotation;

    //Camera Position
    Vector3 camSpeed = Vector3.zero;
    Vector3 target;
    public Vector3 offset;

    //camere spline
    //public float lerp = 0;
    //public GameObject path;
    //List<Transform> pathList = new List<Transform>();


    bool playCutscene = false;
    //int index = 0;
    //Transform currentPos;

    //public float speed = 1.0F;
    //private float startTime;
    //private float journeyLength;



    void Start()
    {
      
    //    PlayCutscene(path);
        currentRotation = this.transform.rotation;
        newRotation = this.transform.rotation;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
            
        }

        if (!playCutscene) {
            ApplyPosition();
        }
       // }
    }

    public void StartCutscene(GameObject path) {
        GetComponent<SplineController>().SplineRoot = path;
        GetComponent<SplineController>().FollowSpline();
        playCutscene = true;
    }

    void ApplyPosition()
    {
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;
        transform.position = Vector3.Lerp(transform.position, nextCamPos, Time.deltaTime * 1);
    }

    void Rotating()
    {
        //float distCovered = (Time.time - startTime) * 0.5f;
        //float completed = distCovered / length;


        //transform.position = Vector3.SmoothDamp(transform.position,
        //                               target + offset,
        //                               ref camSpeed, 0.5f);

        //if (completed > 1)
        //{
        // //   length = 0;
        //    currentRotation = newRotation;
        //}
    }

    public void DisableCutscene() {
        this.transform.rotation = currentRotation;
        playCutscene = false;
    }

    //public void PlayCutscene(GameObject path) {
    ////    print("playcutscene");
    //    CalculatePath(path);
    //    SetVariables(this.transform, 0);
    //    playCutscene = true;
       
        
    //}

    //void CalculatePath(GameObject path) {
    //    print(path.transform.childCount);
    //    for (int i = 0; i < path.transform.childCount; i++) {
    //        pathList.Add(path.transform.GetChild(i));
    //    }

    //    //foreach (Transform trans in pathList) {
    //    //    print(trans.name);
    //    //}
    //}

    //void Cutscene() {
    //    float distCovered = (Time.time - startTime) * speed;
    //    float fracJourney = distCovered / journeyLength;
    //    this.transform.position = Vector3.Lerp(currentPos.position, pathList[index].position, fracJourney);
    //    if (Vector3.Distance( currentPos.position, pathList[index].position) <= 0.5f) {
    //        index++;
    //        SetVariables(this.transform,index);
    //    }
    //}

    //void SetVariables(Transform pCurrentPos, int pIndex) {
    //    startTime = Time.time;
    //    currentPos = pCurrentPos;
    //    journeyLength = Vector3.Distance(currentPos.position, pathList[pIndex].position);
    //}
}
