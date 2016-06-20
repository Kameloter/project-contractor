using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectableHudScript : MonoBehaviour {
    [Header("Textfields")]
    //public Text divider;  
    //public Text collectables;

    public Text found; //will show the amount of collectables found this level
    public Text total; //will show the total amount of collectables in a level

    public void Start() {
        if (referenceErrorCheck()) return; //invalid references
        UpdateHudFound();
        UpdateHudTotal();
    }

    public void OnCollectCollectable(int value) {
        GameManager.Instance.IncreaseCollectables(value);
        UpdateHudFound();
    }

    public void UpdateHudFound() {
        //The amount of collectables found this level is stored in the player script
        found.text = GameManager.Instance.PlayerScript.collectables.ToString();
    }

    public void UpdateHudTotal() {
        //The amount of total collectables available this level is stored in the scenestats script
        total.text = GameManager.Instance.SceneStats.CollectablesAvailable.ToString();
    }

    /// <summary>
    /// Checks the references.
    /// </summary>
    /// <returns>True if errors encountered</returns>
    public bool referenceErrorCheck() {
        bool error = false;
        if (found == null) { Debug.LogError("Collectable HUD: Cannot find 'found'-textfield."); error = true; }
        if (total == null) { Debug.LogError("Collectable HUD: Cannot find 'total'-textfield."); error = true; }
        return error;
    }
}