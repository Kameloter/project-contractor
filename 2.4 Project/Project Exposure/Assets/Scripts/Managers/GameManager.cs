using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    //singleton
    public static GameManager Instance {
        get {
            if (_instance == null) {
                GameObject gameManager = new GameObject("GameController");
                gameManager.tag = Tags.gameController;

                if (gameManager != null) {
                    _instance = gameManager.AddComponent<GameManager>();
                    Debug.Log("Game Manager created !");
                } else {
                    Debug.Log("Manager GameObject not present. Make sure the Tag is proper or the game object Managers exists");
                }
            }
            return _instance;
        }
    }

    private static GameManager _instance;
    private GameState _currentState;

    public GameState CurrentState {
        get { return _currentState; }
    }

    //PLAYER  //SerializeField used for debugging purposes.
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private PlayerScript _playerScript;
    [SerializeField]
    private int _collectablesCollected = 0; //total collectables the player has collected through the entire game
    [SerializeField]
    private int _maxCollectablesAvailable = 0; //total collectables the player *could have* collected through the entire game
    GameObject clickedObject;

    public GameObject Player {
        get {
            if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
            return _player;
        }
    }

    public PlayerScript PlayerScript { //fast access to playerscript
        get {
            if (_playerScript == null) _playerScript = Player.GetComponent<PlayerScript>();
            return _playerScript;
        }
    }

    public void IncreaseCollectables(int amount = 1) {
        PlayerScript.collectables += amount;
    }

    public int CollectablesCollected {
        get { return _collectablesCollected; }
        set { _collectablesCollected = value; }
    }

    public int MaxCollectablesAvailable {
        get { return _maxCollectablesAvailable; }
        set { _maxCollectablesAvailable = value; }
    }

    //private bool clickedOnObject = false;

    public GameObject ClickedObject {
        get { return clickedObject; }
        set { clickedObject = value; }
    }
}