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
       
    }

    public void DisableCutscene() {
        this.transform.rotation = currentRotation;
        playCutscene = false;
    }

    
}
