using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectableHudScript : MonoBehaviour {
    [Header("Textfields")]
    public Text found; //will show the amount of collectables found this level
    public Text total; //will show the total amount of collectables in a level

    public void Start() {
        if (referenceErrorCheck()) return; //invalid references
        UpdateHudCollectablesFound();
        UpdateHudCollectablesTotal();
    }

    void OnEnable() {
        GameManager.OnCollectableCollect.AddListener(UpdateHudCollectablesFound);
    }

    void OnDisable() {
        GameManager.OnCollectableCollect.RemoveListener(UpdateHudCollectablesFound);
    }

    public void UpdateHudCollectablesFound() { //this level!
        //The amount of collectables found this level is stored in the player script
        found.text = GameManager.Instance.PlayerScript.Collectables.ToString();
    }

    public void UpdateHudCollectablesTotal() { //this level!
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