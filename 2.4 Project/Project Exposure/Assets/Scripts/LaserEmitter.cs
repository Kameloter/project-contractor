using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEmitter : MonoBehaviour {

    //List<LineRenderer> lineRederers = new List<LineRenderer>();

 //   GameObject lineRendererParent;
    [SerializeField]
    Material mat;

    void Start() {
    }

    void Update() {
        DrawLaser(transform.position);
    }

    void DrawLaser(Vector3 startPoint) {

        //kill your overdate kids
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        RaycastHit hit;
        Vector3 RayDir = transform.forward;

        for (int i = 1; i < 100; i++) {
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                if (hit.collider.CompareTag(Tags.mirror)) {
                    AddLineRenderer(startPoint, hit.point);
                    Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
                    Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal

                    RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);
                    Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);       //reflected laser
                    startPoint = hit.point;
                }
                else {
                    AddLineRenderer(startPoint, hit.point);
                    break;
                }
            }
        }

    }

    void AddLineRenderer(Vector3 startPoint, Vector3 endPoint) {
        GameObject lineRendererParent = new GameObject();
        lineRendererParent.transform.SetParent(this.transform);
        lineRendererParent.name = "las0r";
        LineRenderer lineRenderer = lineRendererParent.AddComponent<LineRenderer>();

        lineRenderer.SetWidth(0.1f, 0.1f);
        lineRenderer.material = mat;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

}




//RaycastHit hit;
//Vector3 RayDir = transform.forward;

//Destroy(lineRendererParent);

//lineRendererParent = new GameObject();
//lineRendererParent.name = "las0r";
//LineRenderer lineRenderer = lineRendererParent.AddComponent<LineRenderer>();


//lineRenderer.SetVertexCount(4);
//lineRenderer.SetWidth(0.1f, 0.1f);
//lineRenderer.material = mat;

//lineRenderer.SetPosition(0, startPoint);

//for (int i = 1; i < 1000; i++) {
//    if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
//        if (hit.collider.CompareTag(Tags.mirror)) {
//            print("index -->" + i);
//            lineRenderer.SetPosition(i,hit.point) ;
//          //  lineRenderer.SetPosition(1, hit.point);
//            //lineRenderer.SetPosition(i, startPoint);
//            Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
//            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal

//            RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);
//            Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);       //reflected laser
//            startPoint = hit.point;
//        }
//    }
//}