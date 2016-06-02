using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RotatableScript))]
public class LaserEmitter : BaseActivatable{
    [Header("Laser")]
    public Transform laserSpawn;

    Material material;

    [SerializeField] bool _active = false;

    Vector3[] points = new Vector3[100];
    Vector3[] oldPoints = new Vector3[100];

    bool update = false;
    int index = 0;

    void Awake() {
        material = Resources.Load("Lazor") as Material;
    }

    void Update() {
        if (_active) {
            CheckLaser(laserSpawn.position);
            GetComponent<Animator>().SetBool("Active", true);
        }
        else {
            GetComponent<Animator>().SetBool("Active", true);
        }
    }

    public override void Activate() {
        base.Activate();
        _active = true;
       // CheckLaser(transform.position);
    }

    public override void Deactivate() {
        base.Deactivate();
        _active = false;
        DestroyLaser();
    }

    void DestroyLaser() {
        for (int i = 0; i < transform.childCount; i++) { //start at 1 to not remove the body
            if (transform.GetChild(i).gameObject.name.Contains("las0r"))
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    void CheckLaser(Vector3 startPoint) {
        RaycastHit hit;
        Vector3 RayDir = transform.forward;

        points[0] = startPoint;

        for (int i = 1; i < 100; i++) {  //Max 100 bounces
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                if (hit.collider.CompareTag(Tags.mirror)) {
                    //Debug.DrawLine(startPoint, hit.point, Color.red);                 //laser
                    //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow);  //normal

                    RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);       //calculate reflected ray direction
                    //Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);        //reflected laser

                    startPoint = hit.point;
                    points[i] = hit.point;

                    index++;

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
                    break;     //break out of the for loop to prevent multiple end lasors.
                }
            }
        }

        //if something changed, update the lasor
        if (update) RedrawLaser();

        index = 0;
        update = false;

    }

    void RedrawLaser() {
        DestroyLaser();
        for (int i = 0; i < index + 1; i++) {
            AddLineRenderer(points[i], points[i + 1], i.ToString());
        }
        points.CopyTo(oldPoints, 0);  //copy points to OldPoints array
    }

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