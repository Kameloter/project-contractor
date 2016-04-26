using UnityEngine;
using System.Collections;
/// <summary>
/// This script is attached to the main camera and it follows the target Game Object provided.
/// </summary>
public class CameraControl : MonoBehaviour
{

    public GameObject targetToFollow;
    private Vector3 nextCamPos;
    //Camera Rotating
    [HideInInspector]
    public float startTime;
    [HideInInspector]
    public float length;
    private float completed;
    [HideInInspector]
    public Quaternion currentRotation;
    [HideInInspector]
    public Quaternion newRotation;

    //Camera Position
    Vector3 camSpeed = Vector3.zero;
    Vector3 target;
    public Vector3 offset;

    //Camera Shake


    [HideInInspector]
    public float shake = 0f;
    [Header("Camera Shake")]
    public float shakeAmount = 75f;
    public float decreaseFactor = 1.0f;
    [HideInInspector]
    public Vector3 lastPosBeforeShake; //Used to produce camera shake .. 

    //Camera Zoom
    public float myFieldOfView;


    void Start()
    {
        currentRotation = this.transform.rotation;
        newRotation = this.transform.rotation;
      //  offset = new Vector3(15, 10, 0);
    }

    void FixedUpdate()
    {
        Rotating();
        Zooming();
        ApplyPosition();
    }

    void ApplyPosition()
    {
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;
        transform.position = Vector3.Lerp(transform.position, nextCamPos, Time.deltaTime * 1);
    }

    void Rotating()
    {
        float distCovered = (Time.time - startTime) * 0.5f;
        float completed = distCovered / length;


        transform.position = Vector3.SmoothDamp(transform.position,
                                       target + offset,
                                       ref camSpeed, 0.5f);

        if (completed > 1)
        {
            length = 0;
            currentRotation = newRotation;
        }
    }

    void Zooming()
    {
        // float distance = Vector3.Distance(big.transform.position, small.transform.position);
        //  float zoomValue = 0;

        //if (distance > myZoomValue)
        //{
        //    zoomValue = myZoomValue;
        //}
        //else {
        //    zoomValue = distance;
        //}

        float fov = Camera.main.fieldOfView;
        Camera.main.fieldOfView = Mathf.Lerp(fov, myFieldOfView, Time.deltaTime * 2.5f);
    }


}
