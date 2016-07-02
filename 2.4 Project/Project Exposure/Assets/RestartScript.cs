using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used for testing. Restarts the game
/// </summary>
public class RestartScript : MonoBehaviour {

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame()
    {
        GameManager.Instance.SceneManager.SwitchToLevel(0);
    }
}
