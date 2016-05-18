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

    void Start() {
    }

    void Update() {
    }

    public override void Activate() {
        _active = true;
        DrawLaser(transform.position);
    }

    public override void DeActivate() {
        _active = false;
        DestroyLaser();
    }

    public void OnCustomEvent() {
        if (InRange) {
            print("clicked and inrange");
            ActivateLaser();
        }
        else {
            print("clicked and NOT inrange");
            GameObject.FindGameObjectWithTag(Tags.player).GetComponent<NavMeshAgent>().SetDestination(this.transform.position);
            GameManager.Instance.ClickedObject = this.gameObject;
            print(GameManager.Instance.ClickedObject.name);
        }
    }

    void ActivateLaser() {
        if (state == 7) {
            state = 0;
        }
        else {
            state++;
        }
        print(state);

        Vector3 rotation = new Vector3(0, state * 45, 0);
        this.transform.eulerAngles = rotation;

        if (_active) DrawLaser(transform.position);
    }

    void DestroyLaser() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    void DrawLaser(Vector3 startPoint) {

        //kill your overdate kids
        DestroyLaser();

        RaycastHit hit;
        Vector3 RayDir = transform.forward;

        for (int i = 1; i < 100; i++) {  //Max 100 bounces
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                if (hit.collider.CompareTag(Tags.mirror)) {
                    AddLineRenderer(startPoint, hit.point, i.ToString());
                    Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
                    Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal

                    RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);    //calculate reflected ray direction
                    Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);       //reflected laser
                    startPoint = hit.point;
                } else {
                    AddLineRenderer(startPoint, hit.point, i.ToString());
                    if (hit.collider.GetComponent<TemperatureScript>() != null){
                        hit.collider.gameObject.GetComponent<TemperatureScript>().ChangeState(TemperatureScript.TemperatureState.Hot);
                    }
                    break;                                                          //break out of the for loop to prevent multiple end lasors.
                }
            }
        }

    }

    void AddLineRenderer(Vector3 startPoint, Vector3 endPoint, string name = "") {
        GameObject lineRendererParent = new GameObject();
        lineRendererParent.transform.SetParent(this.transform);
        lineRendererParent.name = "las0r " + name;
        LineRenderer lineRenderer = lineRendererParent.AddComponent<LineRenderer>();
        BoxCollider col = lineRendererParent.AddComponent<BoxCollider>();
      //  col.center = 

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




