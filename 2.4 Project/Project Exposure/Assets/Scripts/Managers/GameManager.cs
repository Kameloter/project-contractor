using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// This script is a singleton. IT remains throught the WHOLE game and is used to hold important variables
/// in scene transitions. Also it stores usefull references to important game scripts and caches them.
/// </summary>
public class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager Instance { //singleton
        get {
            if (_instance == null) {
                GameObject gameManager = new GameObject("GameController");
                gameManager.tag = Tags.gameController;

                if (gameManager != null) _instance = gameManager.AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private GameState _currentState;
    public GameState CurrentState { get { return _currentState; } }

    //COLLECTABLES
    public static UnityEvent OnCollectableCollect = new UnityEvent();
    public void CollectCollectable() {
        if (OnCollectableCollect != null) OnCollectableCollect.Invoke();
    }

    [Header("Game")]
    [ReadOnly]
    [SerializeField] int _gameScore = 0;
    //PLAYER         //SerializeField used for debugging purposes.
    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Scene")]
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private SceneStats _sceneStats;
    [SerializeField] private SplineInterpolator _splineInterpolator;

    [Header("UI")]
    [SerializeField] private ScoreScreenScript _scoreScreen;
    [SerializeField] private TutorialSelectorScript _tutorialSelector;
    [SerializeField] private MonitorScript _uiMonitor;

    GameObject clickedObject;
    GameObject activatedObject;
    GameObject interactedObject;
    GameObject deactivatedObject;

    //time
    Text gameTimeText;
    //Text levelTimeText;
    WWW www;
    GameObject inactiveScreen;

    [Header("Time")]
    public float gameTimeLeft = 180.0f; 
    public float timeSpentLevel = 0.0f;

    float inactiveTime = 0;

    void Awake() {
        FindObjectRefs();
    }

    void OnLevelWasLoaded(int level) {
        FindObjectRefs();
    }

    void FindObjectRefs()
    {
        gameTimeText = GameObject.Find("GameTimerText").GetComponent<Text>();
        //levelTimeText = GameObject.Find("LevelTimerText").GetComponent<Text>();
        timeSpentLevel = 0;

        _scoreScreen = ScoreScreen;             //ref needed before it disables itself
        _tutorialSelector = TutorialSelector;   //ref needed before it disables itself

        inactiveScreen = GameObject.Find("InactiveScreen");
        inactiveScreen.SetActive(false);
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
    public SceneManager SceneManager { 
        get {
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

    /// <summary>
    /// Returns the ScoreScreenScript from the first GameObject tagged as 'ScoreScreen'.
    /// </summary>
    public ScoreScreenScript ScoreScreen {
        get {
            if (_scoreScreen == null) _scoreScreen = GameObject.FindGameObjectWithTag(Tags.scoreScreen).GetComponent<ScoreScreenScript>();
            return _scoreScreen;
        }
    }
    
    /// <summary>
    /// Returns the MonitorScript.
    /// </summary>
    public MonitorScript UiMonitor {
        get {
            if (_uiMonitor == null) _uiMonitor = GameObject.FindGameObjectWithTag(Tags.uiMonitor).GetComponent<MonitorScript>();
            return _uiMonitor;
        }
    }

    /// <summary>
    /// Returns the TutorialSelectorScript.
    /// This should be called before it turns itself off.
    /// </summary>
    public TutorialSelectorScript TutorialSelector {
        get {
            if (_tutorialSelector == null) _tutorialSelector = GameObject.FindGameObjectWithTag(Tags.tutorialSelector).GetComponent<TutorialSelectorScript>();
            return _tutorialSelector;
        }
    }

    /// <summary>
    /// Score. Based on amount of Stars earned.
    /// </summary>
    public int GameScore {
        get { return _gameScore; }
        set { _gameScore = value; }
    }

    public float TimeNeededForLevel {
        get { return GameObject.Find("UtilityManagers").GetComponent<SceneStats>().TimeNeededForLevel;}
    }

    public float TimeSpentOnLevel {
        get { return timeSpentLevel; }
    }

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
        gameTimeLeft -= Time.deltaTime;
        inactiveTime += Time.deltaTime;
        timeSpentLevel += Time.deltaTime;

        //if input change inactive timer to 0
        if (Input.GetMouseButton(0)) {
            inactiveTime = 0;
            if (inactiveScreen.activeInHierarchy) {
                inactiveScreen.SetActive(false);
            }
        }

        if (inactiveTime >= 20) {
            inactiveScreen.SetActive(true);
        }
        //if longer inactive than 30s close the game
        if (inactiveTime >= 30) {
            Application.Quit();
        }
        //if gametime is over save it on the server
        if (gameTimeLeft <= 0) {
            www = new WWW("http://www.serellyn.net/HEIM/php/insertScore.php?" + "userID=" + Environment.GetCommandLineArgs()[2] + "&gameID=" + Environment.GetCommandLineArgs()[3] + "&score=" + _gameScore.ToString());
        }

        //Timer
        gameTimeText.text = UpdateGameTimerText();
        //levelTimeText.text = Mathf.Floor((timeSpentLevel / 60)).ToString("0" + "#':'") + ((int)timeSpentLevel % 60).ToString("D2");
    }

	/// <summary>
	/// Updgrades the in-game timer text.
	/// </summary>
	/// <returns>The game timer text.</returns>
    public string UpdateGameTimerText() {
        return Mathf.Floor((gameTimeLeft / 60)).ToString("0" + "#':'") + ((int)gameTimeLeft % 60).ToString("D2");
    }
}