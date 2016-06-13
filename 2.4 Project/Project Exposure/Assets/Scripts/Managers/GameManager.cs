using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance { //singleton
        get {
            if (_instance == null) {
                GameObject gameManager = new GameObject("GameController");
                gameManager.tag = Tags.gameController;

                if (gameManager != null) {
                    _instance = gameManager.AddComponent<GameManager>();
                    //Debug.Log("Game Manager created !");
                } else {
                   // Debug.Log("Manager GameObject not present. Make sure the Tag is proper or the game object Managers exists");
                }
            }
            return _instance;
        }
    }

    private GameState _currentState;
    public GameState CurrentState { get { return _currentState; } }

    //PLAYER         //SerializeField used for debugging purposes.
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private SceneStats _sceneStats;
    [SerializeField] private int _collectablesCollected = 0;        //total collectables the player has collected through the entire game
    [SerializeField] private int _maxCollectablesAvailable = 0;     //total collectables the player *could have* collected through the entire game

    GameObject clickedObject;
    GameObject activatedObject;
    GameObject interactedObject;
    GameObject deactivatedObject;

    Text text;
    Text textLevel;
   // WWW www;

    public float TimeLeft = 180.0f; 
    public float TimeSpentLevel = 0.0f;

    float inactiveTime = 0;

    int score = 0;

    void Start() {
        text = GameObject.Find("Time").GetComponent<Text>();
        textLevel = GameObject.Find("Timer").GetComponent<Text>();
    }

    void OnLevelWasLoaded(int level) {
        text = GameObject.Find("Time").GetComponent<Text>();
        textLevel = GameObject.Find("Timer").GetComponent<Text>();
        TimeSpentLevel = 0;
    }

    /// <summary>
    /// Returns the Player GameObject.
    /// </summary>
    public GameObject Player {
        get {
            if (_player == null) _player = GameObject.FindGameObjectWithTag(Tags.player);
            return _player;
        }
    }

    /// <summary>
    /// Returns the PlayerScript from the Player GameObject.
    /// </summary>
    public PlayerScript PlayerScript { //fast access to playerscript
        get {
            if (_playerScript == null) _playerScript = Player.GetComponent<PlayerScript>();
            return _playerScript;
        }
    }

    /// <summary>
    /// Return the PlayerMovement script from the Player GameObject.
    /// </summary>
    public PlayerMovement PlayerMovement {
        get {
            if (_playerMovement == null) _playerMovement = Player.GetComponent<PlayerMovement>();
            return _playerMovement;
        }
    }

    /// <summary>
    /// Returns the SceneManager.
    /// </summary>
    public SceneManager SceneManager
    { 
        get
        {
            if (_sceneManager == null) _sceneManager = FindObjectOfType<SceneManager>();
            return _sceneManager;
        }
    }

    /// <summary>
    /// Returns the SceneStats script from the UtilityManagers
    /// </summary>
    public SceneStats SceneStats { //fast access to scenestats
        get {
            if (_sceneStats == null) _sceneStats = GameObject.FindGameObjectWithTag(Tags.managers).GetComponent<SceneStats>();
            return _sceneStats;
        }
    }

    public void IncreaseCollectables(int amount = 1) {
        PlayerScript.collectables += amount;
    }

    public int CollectablesCollected {
        get { return _collectablesCollected; }
        set { _collectablesCollected = value; }
    }

    public int Score {
        get { return score; }
        set { score = value; }
    }

    public float TimeNeededForLevel {
        get { return GameObject.Find("UtilityManagers").GetComponent<SceneStats>().TimeNeededForLevel;}
    }

    public float TimeSpentOnLevel {
        get { return TimeSpentLevel; }
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



    public GameObject ActivatedObject
    {
        get { return activatedObject; }
        set { activatedObject = value; }

    }

    public GameObject InteractedObject {
        get { return interactedObject; }
        set { interactedObject = value; }

    }

    public GameObject DeactivatedObject
    {
        get { return deactivatedObject; }
        set { deactivatedObject = value; }

    }



    void Update() {
        //changing timers
        TimeLeft -= Time.deltaTime;
        inactiveTime += Time.deltaTime;
        TimeSpentLevel += Time.deltaTime;

        //if input change inactive timer to 0
        if (Input.GetMouseButton(0)) {
            inactiveTime = 0;
        }

        //if longer inactive than 30s close the game
        if (inactiveTime >= 30) {
            print("termination");
            Application.Quit();
        }

        //if gametime is over save it on the server
        if (TimeLeft <= 0) {
          //  www = new WWW("http://www.serellyn.net/HEIM/php/insertScore.php?"+"userID="+Environment.GetCommandLineArgs()[2]+"&gameID="+Environment.GetCommandLineArgs()[3]+"&score="+score.ToString());
        }

        //debug texts
        text.text = Mathf.Floor((TimeLeft / 60)).ToString("0"+"#':'") + ((int)TimeLeft % 60).ToString("D2");
        textLevel.text = Mathf.Floor((TimeSpentLevel / 60)).ToString("0" + "#':'") + ((int)TimeSpentLevel % 60).ToString("D2");
    }


}