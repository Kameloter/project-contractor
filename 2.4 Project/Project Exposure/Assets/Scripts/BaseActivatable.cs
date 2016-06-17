using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for every object that needs to be actiated by an Interactable type of object.
/// Every derrived class can override function Activate/Deactive to add more behaviour.
/// Reason for making a base class is because we want to to polymorph all of our activatables under 1 base class.
/// </summary>
public class BaseActivatable : MonoBehaviour {
    
    public virtual void Start() {}


    /// <summary>
    /// This function stores a refference of the last activated object. (Might be used by designers in the GameLogic scripts)
    /// Since this needs to happen for every object we put it in the base function and base.Activate needs to be called in derrived classses.
    /// </summary>
    public virtual void Activate() {
        //save the list activated object so you can use it in the gamelogic scripts
        GameManager.Instance.ActivatedObject = this.gameObject;
    }


    /// <summary>
    /// This function stores a refference of the last deactivated object. (Might be used by designers in the GameLogic scripts)
    /// Since this needs to happen for every object we put it in the base function and base.Activate needs to be called in derrived classses.
    /// </summary>
    public virtual void Deactivate() {
        //save the list deactivated object so you can use it in the gamelogic scripts
        GameManager.Instance.DeactivatedObject = this.gameObject;
    }
}
