using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    [HideInInspector]
    public GameObject carriedValve;
    public bool carryingValve = false;

    [Header("Player Stats")]
    [Tooltip("Collected Collectables")] public int collectables = 0;      

}
