using UnityEngine;
using System.Collections;

/// <summary>
/// A button that activates any BaseActiavatable
/// </summary>
public class GameObjectButtonScript : BaseInteractable {

    //Array for possigle activatavbles
    [SerializeField]
    BaseActivatable[] activatables;

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

	/// <summary>
	/// When interracted with object , activate all Activatables linked to it.
	/// </summary>
    public override void OnInteract() {
        base.OnInteract();
        TriggerActivatables();
    }
}
