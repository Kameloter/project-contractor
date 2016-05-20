using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEmitter : Interactable {

    [SerializeField] Material mat;
    [HideInInspector] public int state = 0;

    Vector3 rotation = Vector3.zero;

    bool _active = false;
    bool InRange = false;

    [ReadOnly] public Vector3[] points = new Vector3[100];
    Vector3[] oldPoints = new Vector3[100];

    bool update = false;
    int index = 0;

    void Update() {
        if (_active) CheckLaser(transform.position);
    }

    public override void Activate() {
        _active = true;
        CheckLaser(transform.position);
    }

    public override void DeActivate() {
        _active = false;
        DestroyLaser(0);
    }

    public void OnCustomEvent() {
        if (InRange) {
            ActivateLaser();
        } else {
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
        }
    }

    void ActivateLaser() {
        if (state == 7) {
            state = 0;
        } else {
            state++;
        }

        Vector3 rotation = new Vector3(0, state * 45, 0);
        this.transform.eulerAngles = rotation;
    }

    void DestroyLaser(int index) {
        for (int i = 1; i < transform.childCount; i++) { //start at 1 to not remove the nose
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
        DestroyLaser(0);
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
        lineRenderer.material = mat;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = true;
            if (GameManager.Instance.ClickedObject == this.gameObject) {
                ActivateLaser();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
        }
    }
}