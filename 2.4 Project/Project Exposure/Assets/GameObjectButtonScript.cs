using UnityEngine;
using System.Collections;

public class GameObjectButtonScript : BaseInteractable {


    [SerializeField]
    BaseActivatable[] activatables;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void TriggerActivatables()
    {
        int index = 0;
        foreach (BaseActivatable activatable in activatables)
        {
            index++;
            if (activatable != null)
            {
                activatable.Activate();
            }
            else
            {
                Debug.LogError("Activatable in " + gameObject.name + " is NULL in collection at -> " + index + ".", transform);
            }
        }
    }

    public override void OnInteract() {
        base.OnInteract();
        TriggerActivatables();
    }
}
