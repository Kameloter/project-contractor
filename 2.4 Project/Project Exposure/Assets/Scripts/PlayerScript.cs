using UnityEngine;
using System.Collections;

/// <summary>
/// calss holding player stats/info
/// </summary>
public class PlayerScript : MonoBehaviour {
    //the valve carried by the player
    [HideInInspector]
    public GameObject carriedValve;
    public bool carryingValve = false;

    //amount of collectables
    [Header("Player Stats")][Tooltip("Collected Collectables")]
    [SerializeField] int _collectables = 0; //total collectables collected during level

    public int Collectables {
        get { return _collectables;  }
        set { _collectables = value; }
    }

    void OnEnable() {
        GameManager.OnCollectableCollect.AddListener(IncreaseCollectables);
    }

    void OnDisable() {
        GameManager.OnCollectableCollect.RemoveListener(IncreaseCollectables);
    }

    void IncreaseCollectables() {
        _collectables += 1;
    }
}
