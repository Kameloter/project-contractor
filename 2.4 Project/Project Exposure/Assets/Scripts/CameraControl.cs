using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// This script is attached to the main camera and it follows the target Game Object provided.
/// </summary>
public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// Event called when camera cutscene end so we can use that event in gamelogic scripts and other scripts
    /// </summary>
    public static UnityEvent OnCameraPathEnd = new UnityEvent();

    /// <summary>
    /// Event called when camera cutscene Starts so we can use that event in gamelogic scripts and other scripts
    /// </summary>
    public static UnityEvent OnCameraPathStart = new UnityEvent();

    /// <summary>
    /// Camera variables
    /// </summary>
    [SerializeField] GameObject targetToFollow;
    //Position of targetToFollow
    Vector3 target;
    //calculated position for camera for this frame used to check collision 
    private Vector3 nextCamPos;
    // the offset of the camera related to the tragetToFollow
    public Vector3 offset;

    //speed of camera rotation and movement
    [SerializeField] float cameraSpeed = 3.0f;
    [SerializeField] float cameraRotationSpeed = 2.0f;
    //Are we playing a cutscene?
    [HideInInspector]public bool playCutscene = false;
    //GuiButtonObject to skip a cutscene 
    GameObject SkipButtonObject;
    GameObject overviewObject;

    //reference to splinecontroler
    SplineController splineController;
  
    void Start() {
        SetStartPos();
        SkipButtonObject = GameObject.FindGameObjectWithTag(Tags.skipButton);
        overviewObject = GameObject.Find("ZoomOutButton");
        if (SkipButtonObject == null) {
            Debug.LogError("Button not found");
            return;
        }
        if (overviewObject == null) {
            Debug.LogError("ZoomOut Button not found");
            return;
        }
        splineController = GetComponent<SplineController>();
        if (splineController == null) {
            Debug.LogError("SplineController not found");
            return;
        }

        Button skipButton = SkipButtonObject.GetComponent<Button>();
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(() => { SkipCutScene(); });
        SkipButtonObject.SetActive(false);
    }

    /// <summary>
    /// if we are not playing a cutscene we update the camera as usual
    /// </summary>
    void Update()
    {
        if (!playCutscene) {
            ApplyPosition();
        }
    }

    /// <summary>
    /// function to set the start position of the camera so that it doesnt start at the position of the camera in the editor
    /// This is to remove the camera lerp at the start so you dont have to wait
    /// </summary>
    void SetStartPos() {
        //set position of the camera
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;

        CheckForWall(target + new Vector3(0, offset.y, 0), ref nextCamPos);
        transform.position = nextCamPos;

        //set rotation of camera
        Vector3 lookPos = target - transform.position;
        if (offset.z != 0) {
            lookPos.x = 0;
        }
        if (offset.x != 0) {
            lookPos.z = 0;
        }
        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = rot;
    }

    /// <summary>
    /// Same as the setstartpos but now we lerp in the update to make smooth camera movement
    /// </summary>
    void ApplyPosition()
    {
        //set camera pos
        target = targetToFollow.transform.position;
        nextCamPos = target + offset;
        CheckForWall(target + new Vector3(0,offset.y,0), ref nextCamPos);
        //RaycastHit hit = new RaycastHit();
       // if (Physics.Raycast(nextCamPos, this.transform.right,out hit,0.5f)) {
       //     nextCamPos -= this.transform.right.normalized * 0.5f;
       // }
        transform.position = Vector3.Slerp(transform.position, nextCamPos, Time.deltaTime * cameraSpeed);

        //set camera rotation
        Vector3 lookPos = target - transform.position;
        if (offset.z != 0)
        {
            lookPos.x = 0;
        } 
        if (offset.x != 0)
        {
            lookPos.z = 0;
        }

        Quaternion rot = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * cameraRotationSpeed);
    }

    /// <summary>
    /// check if you hit a wall with a raycast
    /// if we do change x and z of the next posistion to the place you hit the wall
    /// </summary>
    /// <param name="fromObject">Position above the player</param>
    /// <param name="toTarget">the calculated next camera pos</param>
    void CheckForWall(Vector3 fromObject, ref Vector3 toTarget)
    {
        RaycastHit wallhit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallhit))
        {
            toTarget = new Vector3(wallhit.point.x, toTarget.y, wallhit.point.z) - (toTarget - fromObject).normalized * 0.1f;
            Debug.DrawRay(this.transform.position, toTarget - this.transform.position, Color.blue);
        }

        RaycastHit wallhit2 = new RaycastHit();
        if (Physics.Linecast(this.transform.position, toTarget, out wallhit2)) {
            Vector3 newVeccie = Vector3.Cross(wallhit2.normal, Vector3.up).normalized;
            Vector3 direction = toTarget - this.transform.position;
            if (offset.x != 0) {
                if (direction.z > 0) newVeccie *= -1;
            }
            else {
                if (direction.x < 0) newVeccie *= -1;
            }
            Debug.DrawRay(transform.position, direction, Color.yellow);
            toTarget = new Vector3(wallhit2.point.x, toTarget.y, wallhit2.point.z) + wallhit2.normal.normalized / 2.0f + newVeccie / 2.0f;
           // print("wall intersection");
        }
    }

    /// <summary>
    ///  starting the cutscene which means, disabling movement, changing cameraspline variables and settinf the skipbutton active
    /// </summary>
    /// <param name="path">A game object with empy gameobject children as camera spot</param>
    /// <param name="startAtPlayer">Should it start at the player or at the first point</param>
    public void StartCutscene(GameObject path, bool startAtPlayer) {
        splineController.startAtPlayer = startAtPlayer;
        splineController.SplineRoot = path;
        splineController.FollowSpline();
        playCutscene = true;
        SkipButtonObject.SetActive(true);
        overviewObject.SetActive(false);
        OnCameraPathStart.Invoke();
        
    }

    public void StartOverview(GameObject path) {
        splineController.startAtPlayer = true;
        splineController.SplineRoot = path;
        splineController.FollowSpline();
        playCutscene = true;
        SkipButtonObject.SetActive(true);
        overviewObject.SetActive(false);
        OnCameraPathStart.Invoke();
    }

    /// <summary>
    /// skipping the current played cutscene
    /// called when the user clicked the skipbutton
    /// </summary>
    public void SkipCutScene() {
        GetComponent<SplineController>().Skip();
        DisableCutscene();
    }

    /// <summary>
    /// End of the cutscene
    /// Enabling playermovement again, deactive skipbutton, call the OnCameraPathEnd event
    /// </summary>
    public void DisableCutscene() {
        SetStartPos();
        playCutscene = false;
        SkipButtonObject.SetActive(false);
        overviewObject.SetActive(true);
        OnCameraPathEnd.Invoke();
    }
}
