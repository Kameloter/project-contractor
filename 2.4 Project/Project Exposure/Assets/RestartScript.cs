using UnityEngine;
using System.Collections;

public class RestartScript : MonoBehaviour {

    
    public void RestartGame()
    {
        GameManager.Instance.SceneManager.SwitchToLevel(0);
    }
}
