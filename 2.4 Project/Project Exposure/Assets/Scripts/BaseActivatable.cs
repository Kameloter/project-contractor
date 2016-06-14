using UnityEngine;
using System.Collections;

/// <summary>
/// Class as base for all objects that need to be activatable
/// class holds two overridable functions Activate and Deactivate
/// These functions get called when activating/deactivating a valve or in gamelogic/triggerevent
/// </summary>
public class BaseActivatable : MonoBehaviour {

    public virtual void Start()
    {
    }

    public virtual void Activate() {
        //save the list activated object so you can use it in the gamelogic scripts
        GameManager.Instance.ActivatedObject = this.gameObject;

    }

    public virtual void Deactivate() {
        //save the list deactivated object so you can use it in the gamelogic scripts
        GameManager.Instance.DeactivatedObject = this.gameObject;
    }
}
