using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    //singleton
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject managers = GameObject.FindWithTag("Managers");
                if (managers != null)
                {
                    _instance = managers.AddComponent<GameManager>();
                    Debug.Log("Game Manager created !");
                }
                else
                    Debug.Log("Manager game object is not present. Make sure tag is proper or the game object Managers exists");
            }
            return _instance;
        }
    }

    private static GameManager _instance;
    private GameState _currentState;
    public GameState CurrentState
    {
        get { return _currentState; }
    }




}
