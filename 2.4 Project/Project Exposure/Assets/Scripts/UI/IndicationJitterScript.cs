using UnityEngine;
using System.Collections;

/// <summary>
/// This script makes the indication allign its position properly so it does not jitter.
/// </summary>
public class IndicationJitterScript : MonoBehaviour {
    public Transform start, end;
    Vector3 startPosition, endPosition; //cache position
    public float speed = 1.0f;
    private Canvas parentCanvas; //to check whether its active.

    void Start() {
        parentCanvas = transform.parent.GetComponent<Canvas>();
        startPosition = start.position;
        endPosition = end.position;

        transform.position = start.position;  //start at startposition
    }

    void Update() {
        if(parentCanvas.enabled) transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(Time.time, 1.0f / speed) * speed);
    }
}