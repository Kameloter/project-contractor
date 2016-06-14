using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// laser class to shoot a laser and reflect of mirrors
/// </summary>
[RequireComponent(typeof(RotatableScript))]
public class LaserEmitter : BaseActivatable{
    //place where the laser starts
    [Header("Laser")]
    [SerializeField] Transform laserSpawn;

    // material for the line renderer
    Material material;

    //if you want to fire laser in the beginning
    [SerializeField] bool _active = false;
    //after you hit correctly the object should it still be useable?
    [SerializeField] bool _reusable = false;

    //two arrays to check if the laser has changed so that we dont have to update the laser constantly
    Vector3[] points = new Vector3[10];
    Vector3[] oldPoints = new Vector3[10];

    //bool to update the laser
    bool update = false;
    int index = 0;

    void Awake() {
        material = Resources.Load("Lazor") as Material;
    }

    /// <summary>
    /// check the laser if it is active and play animation
    /// </summary>
    void Update() {
        if (_active) {
            CheckLaser(laserSpawn.position);
            GetComponent<Animator>().SetBool("Active", true);
        }
        else {
            GetComponent<Animator>().SetBool("Active", false);
        }
    }

    /// <summary>
    /// activate the laser
    /// </summary>
    public override void Activate() {
        base.Activate();
        _active = true;
    }

    /// <summary>
    /// deactivate laser
    /// </summary>
    public override void Deactivate() {
        base.Deactivate();
        _active = false;
        DestroyLaser();
    }

    /// <summary>
    /// destroy all linerenderers to delete the laser
    /// </summary>
    void DestroyLaser() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.name.Contains("las0r"))
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// raycast and then reflect and store the hit in an point array
    /// after that we check if array is same as previous
    /// if yes we update the laser
    /// </summary>
    /// <param name="startPoint">start point of the laser</param>
    void CheckLaser(Vector3 startPoint) {
        RaycastHit hit;
        Vector3 RayDir = transform.forward;

        points[0] = startPoint;

        for (int i = 1; i < 10; i++) {  //Max 10 bounces
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                if (hit.collider.CompareTag(Tags.mirror)) {
                    //Debug.DrawLine(startPoint, hit.point, Color.red);                 //laser
                    //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow);  //normal

                    RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);       //calculate reflected ray direction
                    //Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);        //reflected laser

                    startPoint = hit.point;
                    points[i] = hit.point;

                    index++;

                    //check if points are not the same
                    if (oldPoints[i] != points[i]) {
                        update = true;
                    }
                } else {
                    points[i] = hit.point;
                    if (oldPoints[i] != points[i]) {
                        update = true;
                    }
                    if (hit.collider.GetComponent<TemperatureScript>() != null) {
                        hit.collider.gameObject.GetComponent<TemperatureScript>().ChangeState(TemperatureScript.TemperatureState.Hot);
                    }
                    //if we hit a meltable we make the meltable melt
                    if (hit.collider.CompareTag(Tags.meltable)){
                        hit.collider.gameObject.GetComponent<MeltableScript>().SetMelting(GetComponent<RotatableScript>(), _reusable);
                    }
                    break;     //break out of the for loop to prevent multiple end lasors.
                }
            }
        }

        //if something changed, update the lasor
        if (update) RedrawLaser();

        index = 0;
        update = false;

    }

    /// <summary>
    /// draw the laser with linerenderers but destroying it first
    /// </summary>
    void RedrawLaser() {
        DestroyLaser();
        for (int i = 0; i < index + 1; i++) {
            AddLineRenderer(points[i], points[i + 1], i.ToString());
        }
        points.CopyTo(oldPoints, 0);  //copy points to OldPoints array
    }

    /// <summary>
    /// creating a linerenderer with the specified material and start an end point
    /// </summary>
    /// <param name="startPoint">point where it starts</param>
    /// <param name="endPoint">point where it ends</param>
    /// <param name="name">name of the laser for debug purposes</param>
    void AddLineRenderer(Vector3 startPoint, Vector3 endPoint, string name = "") {
        GameObject lineRendererParent = new GameObject();
        lineRendererParent.transform.SetParent(this.transform);
        lineRendererParent.name = "las0r " + name;
        LineRenderer lineRenderer = lineRendererParent.AddComponent<LineRenderer>();
        lineRenderer.receiveShadows = false;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.material = material;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}