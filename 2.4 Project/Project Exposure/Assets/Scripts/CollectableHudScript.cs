using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectableHudScript : MonoBehaviour {
    [Header("Textfields")]
    public Text divider;
    public Text collectables;
    public Text found;
    public Text total; //will show the total amount of collectables in a level

    public void Awake() {
        UpdateCollectableHud();
    }

    public void OnCollectCollectable(int value) {
        GameManager.Instance.IncreaseCollectables(value);

        UpdateCollectableHud();
    }

    public void UpdateCollectableHud() {
        UpdateFound();
        UpdateTotal();
    }

    public void UpdateFound() {
        found.text = GameManager.Instance.PlayerScript.collectables.ToString();
    }

    public void UpdateTotal() {
        total.text = GameManager.Instance.PlayerScript.collectables.ToString();
    }

    
}