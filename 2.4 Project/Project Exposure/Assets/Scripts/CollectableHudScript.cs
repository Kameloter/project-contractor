using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectableHudScript : MonoBehaviour {
    [Header("Textfields")]
    public Text divider;
    public Text collectables;
    public Text found;
    public Text total; //will show the total amount of collectables in a level

    public void Start() {
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

    
}