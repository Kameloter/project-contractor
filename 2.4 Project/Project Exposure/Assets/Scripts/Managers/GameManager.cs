using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

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

    //PLAYER         //SerializeField used for debugging purposes.
    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private PlayerMovement _playerMovement;

    [Header("Scene")]
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private SceneStats _sceneStats;
    [SerializeField] private int _collectablesCollected = 0;        //total collectables the player has collected through the entire game
    [SerializeField] private int _maxCollectablesAvailable = 0;     //total collectables the player *could have* collected through the entire game
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
    Text text;
    Text textLevel;
    //WWW www;

    [Header("Time")]
    public float TimeLeft = 180.0f; 
    public float TimeSpentLevel = 0.0f;

    float inactiveTime = 0;

    int score = 0;

    void Awake() {
        FindObjectRefs();
    }

    void OnLevelWasLoaded(int level) {
        FindObjectRefs();
    }

    void FindObjectRefs()
    {
        text = GameObject.Find("Time").GetComponent<Text>();
        textLevel = GameObject.Find("Timer").GetComponent<Text>();
        TimeSpentLevel = 0;

        _scoreScreen = ScoreScreen;             //ref needed before it disables itself
        _tutorialSelector = TutorialSelector;   //ref needed before it disables itself
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
    /// Collectables collected through the entire game.
    /// </summary>
    public int CollectablesCollected {
        get { return _collectablesCollected; }
        set { _collectablesCollected = value; }
    }

    /// <summary>
    /// Used to set max available collectables in the entire game played so far.
    /// </summary>
    public int MaxCollectablesAvailable {
        get { return _maxCollectablesAvailable; }
        set { _maxCollectablesAvailable = value; }
    }

    /// <summary>
    /// Score. Based on amount of Stars earned.
    /// </summary>
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