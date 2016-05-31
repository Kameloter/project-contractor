using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CustomEvent : MonoBehaviour {

    public UnityEvent MyCustomEvent;


    public void OnCustomEvent()
    {
        MyCustomEvent.Invoke();
    }
}
