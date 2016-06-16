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
    [Header("Player Stats")]
    [Tooltip("Collected Collectables")] public int collectables = 0;
}
