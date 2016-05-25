using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TemperatureScript))]
public class MoveableScript : BaseInteractable {


    //public
    public bool needsToBeActivated = false;
    public bool continuous = false;

    [Header("Movable:")]
    public Transform movableObject;
    public Transform endPoint;
    public Transform startPoint;
    public float moveSpeed = 0;

    [Header("State options:")]
    int currentState;

    [Tooltip("  0 => Inactive , 1 => open , 2 => closed")]
    [SerializeField]
    private int startState;

    //private
    private Vector3 moveDirection;
    private Transform currentDestination;

    bool activated = false;

    public TemperatureScript temperatureScript;

	// Use this for initialization
	public override void Start () {
        base.Start();
        currentState = startState;
        SetDestination(startPoint);

        temperatureScript = GetComponent<TemperatureScript>();
        if (temperatureScript == null) {
            temperatureScript = GetComponentInChildren<TemperatureScript>();
        }

        print("super");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            DeActivate();
        }

        if (temperatureScript.temperatureState != TemperatureScript.TemperatureState.Frozen) {
            if (needsToBeActivated) {
                if (activated) {
                    if (continuous) {
                        MoveContinuous();
                    }
                    else {
                        Move();
                    }
                }
            }
            else {
                if (continuous) {
                    MoveContinuous();
                }
                else {
                    Move();
                }
            }
        }
	}

    void Move() {
        if (currentState != 0) {
            movableObject.GetComponent<Rigidbody>().MovePosition(movableObject.position + moveDirection * moveSpeed * Time.deltaTime);

            if (currentState == 2) {
                SetDestination(endPoint);
            }

            if (currentState == 1) {
                SetDestination(startPoint);
            }

            if (Vector3.Distance(movableObject.position, currentDestination.position) < 0.1f) {
                currentState = 0;
                moveDirection = Vector3.zero;
                movableObject.position = currentDestination.position;
            }
        }
    }

    void MoveContinuous() {
        movableObject.GetComponent<Rigidbody>().MovePosition(movableObject.position + moveDirection * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(movableObject.position, currentDestination.position) < 0.1f) {
            SetDestination(currentDestination == startPoint ? endPoint : startPoint);
        }
    }

    void SetDestination(Transform dest) {
        currentDestination = dest;
        moveDirection = (currentDestination.position - movableObject.position).normalized;
    }

    public override void Activate() {
        currentState = 2;
        activated = true;
    }

    public override void DeActivate() {
        currentState = 1;
        activated = false;
    }

    void OnDrawGizmos() {
        if (movableObject == null || startPoint == null || endPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPoint.position, movableObject.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPoint.position, movableObject.localScale);

    }
}
