using UnityEngine;
using System.Collections;

public class LaserEmitter : MonoBehaviour {
    
    void Start () {
        
	}
	
	void Update () {
        DrawLaser(transform.position);
	}

    void DrawLaser(Vector3 startPoint) {

        ////Working setup
        //RaycastHit hit;
        //if (Physics.Raycast(startPoint, Vector3.forward, out hit)) {
        //    Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
        //    Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal
        //    Debug.DrawLine(hit.point, hit.point + Vector3.Reflect(hit.point - startPoint, hit.normal), Color.blue); //reflected laser
        //}

        RaycastHit hit;
        Vector3 RayDir = transform.forward;
        for (int i = 0; i < 1000; i++) {
            if (Physics.Raycast(startPoint, RayDir, out hit, 1000.0f)) {
                Debug.DrawLine(startPoint, hit.point, Color.red);                //laser
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.yellow); //normal

                RayDir = Vector3.Reflect(hit.point - startPoint, hit.normal);
                Debug.DrawLine(hit.point, hit.point + RayDir, Color.blue);       //reflected laser
                startPoint = hit.point;
            }
        }
    }
}
