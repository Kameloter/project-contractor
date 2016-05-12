using UnityEngine;
using System.Collections;
using UnityEditor;


public class Interactable : MonoBehaviour {
    
    public enum TypeOfInteractables
    {
        Movable, Rotatable
    }
    
    public TypeOfInteractables typeOfInteractable;
    
    [Header("Movable:")]
    public Transform movableObject;
    public Transform endPoint;
    public Transform startPoint;
    public float moveSpeed = 0;

    [Header("State options:")]
    public int currentState;

    [Tooltip("  0 => Inactive , 1 => open , 2 => closed")]
    [SerializeField]
    private int startState;



    [Header("Rotatable:")]
    public float degrees;
    public Vector3 axisToRotate = Vector3.zero;


    private Vector3 moveDirection;
    private Transform currentDestination;
    void Awake()
    {
        currentState = startState;
    }
    public virtual void FixedUpdate()
    {

        switch (typeOfInteractable) {
            case TypeOfInteractables.Movable:
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
                break;

            case TypeOfInteractables.Rotatable:
                switch (currentState) {
                    case 1:
                        this.transform.rotation = Quaternion.AngleAxis(degrees, axisToRotate);
                        break;
                        
                    case 2:
                        this.transform.rotation = Quaternion.AngleAxis(0, axisToRotate);
                        break;
                }
                break;

        }
        //should handle when an object started rotating 

        //should handle when an object started moving 

        //should not update object ALL the time.
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentState = 1;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentState = 2;
        }
    }

    void SetDestination(Transform dest)
    {
        currentDestination = dest;
        moveDirection = (currentDestination.position - movableObject.position).normalized;
    }
    public virtual void Movable(int state)
    {
        //moves object
        print("Door state - > " + state);
        currentState = state;

    }

    public void Rotatable(int rotateState)
    {
        print("Rotation state -> " + rotateState);
        currentState = rotateState;
    }
    public virtual void RotateAroundAxis(float angle,Vector3 axis)
    {
        //rotates object.
    }



    public virtual void Activate()
    {
        SendMessage(typeOfInteractable.ToString(), 1);
    }
    public virtual void Deactivate()
    {
        SendMessage(typeOfInteractable.ToString(), 2);
    }
}



//[CustomEditor(typeof(Interactable))]
//public class MyScriptEditor : Editor
//{
//    public Interactable.TypeOfInteractables myEnum;

//    override public void OnInspectorGUI()
//    {
//        var myScript = target as Interactable;

//        myEnum = (Interactable.TypeOfInteractables)EditorGUILayout.EnumPopup("Type of interactable:", myEnum);
  
//        switch(myEnum)
//        {
//            case Interactable.TypeOfInteractables.Moveable:
//            myScript.startPoint = EditorGUILayout.ObjectField(myScript.startPoint, typeof(Transform), true) as Transform;
//            myScript.endPoint = EditorGUILayout.ObjectField(myScript.endPoint, typeof(Transform), true) as Transform;
//                break;

//            case Interactable.TypeOfInteractables.Rotateable:
//                myScript.degrees = EditorGUILayout.FloatField(myScript.degrees);
//                myScript.axisToRotate = EditorGUILayout.Vector3Field("Rotate around axis : ", myScript.axisToRotate);
//                break;

//        }




//    }
//}