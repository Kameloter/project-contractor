using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEmitter : Interactable {

    [SerializeField]
    Material mat;

    [HideInInspector]
    public int state = 0;

    Vector3 rotation = Vector3.zero;

    bool _active = false;
    bool InRange = false;

    [ReadOnly] public Vector3[] points = new Vector3[100];
    Vector3[] oldPoints = new Vector3[100];

    bool update = false;
    int index = 0;

    void Start() {
    }

    void Update() {
       if (_active) DrawLaser(transform.position);
    }

    public override void Activate() {
        _active = true;
       // DrawLaser(transform.position);
    }

    public override void DeActivate() {
        print("deactived laser");
        _active = false;
        DestroyLaser(0);
    }

    public void OnCustomEvent() {
        if (InRange) {
          //  print("clicked and inrange");
            ActivateLaser();
        }
        else {
          //  print("clicked and NOT inrange");
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
          //  print(GameManager.Instance.ClickedObject.name);
        }
    }

    void ActivateLaser() {
        if (state == 7) {
            state = 0;
        }
        else {
            state++;
        }

        Vector3 rotation = new Vector3(0, state * 45, 0);
        this.transform.eulerAngles = rotation;

      //  if (_active) DrawLaser(transform.position);
    }

    void DestroyLaser(int index) {
        for (int i = 0; i < transform.childCount; i++) {
            print("name: " + transform.GetChild(i).name);
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void DrawLaser(Vector3 startPoint) {

        RaycastHit hit;
        Vector3 RayDir = transform.forward;

        points[0] = startPoint;

        for (int i = 1; i < 100; i++) {  //Max 100 bounces
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                if (hit.collider.CompareTag(Tags.mirror)) {
                    //  if (points[i] != hit.point) {
                    //  DestroyLaser(i);
                    // AddLineRenderer(startPoint, hit.point, i.ToString());
                    //   }
                    //Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
                    //  Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal

                    RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);    //calculate reflected ray direction
                    //   Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);       //reflected laser
                    startPoint = hit.point;
                    points[i] = hit.point;

                    index++;

                    if (oldPoints[i] == points[i]) {
                        print("ghjk");
                        update = true;
                    }
                }
                else {
                    //   if (points[i] != hit.point) {
                    //DestroyLaser(i);
                    // AddLineRenderer(startPoint, hit.point, i.ToString());
                    //  }
                    points[i] = hit.point;
                    //index++;
                    if (oldPoints[i] == points[i]) {
                        print("ghjk");
                        update = true;
                    }

                    if (hit.collider.GetComponent<TemperatureScript>() != null) {
                        hit.collider.gameObject.GetComponent<TemperatureScript>().ChangeState(TemperatureScript.TemperatureState.Hot);
                    }
                    break;                                                          //break out of the for loop to prevent multiple end lasors.
                }
            }
        }

        DestroyLaser(0);
        for (int i = 0; i < index+1; i++) {
            print("oldPoint: " + oldPoints[i] + "newPoint: " + points[i]);
            AddLineRenderer(points[i], points[i + 1], i.ToString());
        }

        oldPoints = points;

        index = 0;

        update = false;
    }

    void Redraw() {
    }

    void AddLineRenderer(Vector3 startPoint, Vector3 endPoint, string name = "") {
        GameObject lineRendererParent = new GameObject();
        lineRendererParent.transform.SetParent(this.transform);
        lineRendererParent.name = "las0r " + name;
        LineRenderer lineRenderer = lineRendererParent.AddComponent<LineRenderer>();

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
            //  print("In Range TRUE");
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            InRange = false;
            //  print("In Range FALSE");
        }
    }



}




