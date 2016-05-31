using UnityEngine;
using System.Collections;

public class RotationTest : MonoBehaviour {
    public Transform targetPoint;
    Quaternion newRotation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        float angle = Vector3.Angle(transform.up, targetPoint.right);
        Debug.Log(angle);

        Vector3 direction = targetPoint.position - transform.position;
      

        newRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation, Time.deltaTime * 5f);

    }


    void FixRotation()
    {


    }
}
