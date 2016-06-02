using UnityEngine;
using System.Collections;

public class IndicationJitterScript : MonoBehaviour {
    public Transform start, end;
    Vector3 startPosition, endPosition; //cache position

    public float speed = 1.0f;

    float startTime;
    float distance;

    void Start() {
        startPosition = start.position;
        endPosition = end.position;

        distance = Vector3.Distance(startPosition, endPosition); //distance from start to end
        transform.position = start.position;  //start at startposition
    }

    void Update() {
        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(Time.time, 1.0f));
    }
}