using UnityEngine;
using System.Collections;

/// <summary>
/// class to make any object move between 2 points
/// either continuous or when activated 
/// </summary>
[RequireComponent(typeof(TemperatureScript))]
public class MoveableScript : BaseActivatable
{
    //variables to set the functionality
    [SerializeField]
    bool continuous = false;
    [SerializeField]
    bool needsToBeActivated = false;
    bool activated = false;

    //variables for the movable
    [Header("Movable:")]
    [SerializeField]
    Transform movableObject;
    [SerializeField]
    Transform endPoint;
    [SerializeField]
    Transform startPoint;
    [SerializeField]
    float moveSpeed = 0;


    //state variables
    [Header("State options:")]
    int currentState;

    [Tooltip("  0 => Inactive , 1 => END-POINT , 2 => START-POINT")]
    [SerializeField]
    private int startState;

    // positions
    private Vector3 moveDirection;
    private Transform currentDestination;

    [HideInInspector]
    public TemperatureScript temperatureScript;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        currentState = startState;
        SetDestination(startPoint);

        temperatureScript = GetComponent<TemperatureScript>();

        if (temperatureScript == null) temperatureScript = GetComponentInChildren<TemperatureScript>();
    }

    /// <summary>
    /// check what movement the object should do based on variables
    /// </summary>
    void FixedUpdate()
    {
        if (temperatureScript.temperatureState != TemperatureScript.TemperatureState.Frozen)
        {
            if (needsToBeActivated)
            {
                if (activated)
                {
                    if (continuous) MoveContinuous();
                    else Move();
                }
            }
            else {
                if (continuous) MoveContinuous();
                else Move();
            }
        }
    }

    /// <summary>
    /// moves to the end or startpoint
    /// when reached it stops and waits if currentstate is changed
    /// </summary>
    void Move()
    {
        if (currentState != 0)
        {
            movableObject.GetComponent<Rigidbody>().MovePosition(movableObject.position + moveDirection * moveSpeed * Time.deltaTime);


            if (currentState == 1) SetDestination(endPoint);
            if (currentState == 2) SetDestination(startPoint);



            if (Vector3.Distance(movableObject.position, currentDestination.position) < 0.1f)
            {
                currentState = 0;
                moveDirection = Vector3.zero;
                movableObject.position = currentDestination.position;
            }
        }
    }

    /// <summary>
    /// moving continious between 2 points
    /// </summary>
    void MoveContinuous()
    {
        movableObject.GetComponent<Rigidbody>().MovePosition(movableObject.position + moveDirection * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(movableObject.position, currentDestination.position) < 0.1f)
        {
            SetDestination(currentDestination == startPoint ? endPoint : startPoint);
        }
    }

    /// <summary>
    /// set the destination of the object 
    /// </summary>
    /// <param name="dest">The destination</param>
    void SetDestination(Transform dest)
    {
        currentDestination = dest;
        moveDirection = (currentDestination.position - movableObject.position).normalized;
    }

    public override void Activate()
    {
        activated = true;
        base.Activate();
        currentState = 1;
    }

    public override void Deactivate()
    {
        activated = false;
        base.Deactivate();
        currentState = 2;
    }

    /// <summary>
    /// drawing start and end point in editor
    /// </summary>
    void OnDrawGizmos()
    {
        if (movableObject == null || startPoint == null || endPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPoint.position, movableObject.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(endPoint.position, movableObject.localScale);
    }
}