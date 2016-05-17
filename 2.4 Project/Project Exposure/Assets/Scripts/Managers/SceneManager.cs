using UnityEngine;
using System.Collections;


public enum GameState {
    Menu, InGame, Paused
}

public class SceneManager : MonoBehaviour {
    GameState currentState;

    [Header("Level Stats")]
    [SerializeField][ShowOnly] private int collectablesAvailable = 0; //keeps track of the amount of collectables to be found 
    

    void Awake() {
        DontDestroyOnLoad(GameManager.Instance);

        //Count collectables available this scene
        collectablesAvailable = GameObject.FindGameObjectsWithTag(Tags.collectable).Length;
    }
    
    void Start() {
        currentState = GameManager.Instance.CurrentState;

        //if (currentState == GameState.InGame)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
        //if (currentState == GameState.Menu)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            switch (currentState) {
                case GameState.InGame:
                    SetState(GameState.Paused);
                    break;
                case GameState.Paused:
                    SetState(GameState.InGame);
                    break;
                default:
                    print(string.Format("Current state is '{0}'. If not expected state check build settings !!", currentState));
                    break;
            }
        }
    }

    public void SetState(GameState newGameState) {
        DisablePreviousState(currentState);
        switch (newGameState) {
            case GameState.Menu:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case GameState.InGame:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1; //reset time scale
                break;
            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0; // stop time when paused.
                break;
        }
        currentState = newGameState;
    }


    void DisablePreviousState(GameState previousState) {
        switch (previousState) {
            case GameState.Menu:
                break;
            case GameState.InGame:
                break;
            case GameState.Paused:
                //deactivate pause menu
                break;
        }
    }

    public void UnpauseGame() {
        SetState(GameState.InGame);
    }
    public void PauseGame() {
        SetState(GameState.Paused);
    }

    public void SwitchToLevel(int index) {
        ExportSaveData();
        print("loading level " + index);
        SetState(GameState.InGame);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        print("loaded level " + index);
    }

    /// <summary>
    /// Export some data. This should be called just before switching levels.s
    /// </summary>
    public void ExportSaveData() {
        GameManager.Instance.CollectablesCollected += GameManager.Instance.PlayerScript.collectables; //Total collectables collected this level
        GameManager.Instance.CollectablesCollected += GameManager.Instance.PlayerScript.collectables; //Total collectables available this level
    }

    public void QuitGame() {
        print("Application terminated.");
        Application.Quit();
    }
}

