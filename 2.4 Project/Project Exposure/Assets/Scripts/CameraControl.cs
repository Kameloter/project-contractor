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

    //Camera Position
    Vector3 target;
    public Vector3 offset;

    //camere spline
    //public float lerp = 0;
    //public GameObject path;
    //List<Transform> pathList = new List<Transform>();


    public bool playCutscene = false;
    //int index = 0;
    //Transform currentPos;

    //public float speed = 1.0F;
    //private float startTime;
    //private float journeyLength;



    void Start() {

        SetStartPos();
        //    PlayCutscene(path);
    }

    void FixedUpdate()
    {
        if (!playCutscene) {
            ApplyPosition();
        }
       // }
    }

    public void StartCutscene(GameObject path) {
        FindObjectOfType<PlayerMovement>().BroadcastMessage("StopAgent");
        GetComponent<SplineController>().SplineRoot = path;
        GetComponent<SplineController>().FollowSpline();
        playCutscene = true;
    }

    void SetStartPos() {
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;
        CheckForWall(target + new Vector3(0, offset.y, 0), ref nextCamPos);
        transform.position = nextCamPos;

        Vector3 lookPos = target - transform.position;
        if (offset.z > 0) {
            lookPos.x = 0;
        }
        if (offset.x > 0) {
            lookPos.z = 0;
        }
        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = rot;

    }

    void ApplyPosition()
    {
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;
        CheckForWall(target + new Vector3(0,offset.y,0), ref nextCamPos);
        transform.position = Vector3.Lerp(transform.position, nextCamPos, Time.deltaTime * 3);

        Vector3 lookPos = target - transform.position;
        if (offset.z > 0)
        {
            lookPos.x = 0;
        } 
        if (offset.x > 0)
        {
            lookPos.z = 0;
        }
       // lookPos.y = 0;
        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 2);

    }


    void CheckForWall(Vector3 fromObject, ref Vector3 toTarget)
    {
        RaycastHit wallhit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallhit))
        {
            toTarget = new Vector3(wallhit.point.x, toTarget.y, wallhit.point.z) - (toTarget - fromObject).normalized * 0.1f;
        }
    }

    public void DisableCutscene() {
        FindObjectOfType<PlayerMovement>().BroadcastMessage("ResumeAgent");
        playCutscene = false;
    }
}
