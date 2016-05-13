using UnityEngine;
using System.Collections;

public class MovingBridgeScript : MonoBehaviour {

    public Transform movableObject;
    public Transform endPoint;
    public Transform startPoint;
    public float moveSpeed = 0;

    private Vector3 moveDirection;
    private Transform currentDestination;

    public TemperatureScript temperatureScript;
	// Use this for initialization
	void Start () {
        temperatureScript = GetComponent<TemperatureScript>();
        if (temperatureScript == null) {
            temperatureScript = GetComponentInChildren<TemperatureScript>();
        }

        SetDestination(startPoint);
	}
	
	// Update is called once per frame
    void Update() {
        if (temperatureScript.temperatureState != TemperatureScript.TemperatureState.Frozen) {
            print("updating");
            movableObject.GetComponent<Rigidbody>().MovePosition(movableObject.position + moveDirection * moveSpeed * Time.deltaTime);

            if (Vector3.Distance(movableObject.position, currentDestination.position) < 0.1f) {
                SetDestination(currentDestination == startPoint ? endPoint : startPoint);
            }
        }

    }

    void SetDestination(Transform dest) {
        currentDestination = dest;
        moveDirection = (currentDestination.position - movableObject.position).normalized;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPoint.position, movableObject.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPoint.position, movableObject.localScale);

    }
}
