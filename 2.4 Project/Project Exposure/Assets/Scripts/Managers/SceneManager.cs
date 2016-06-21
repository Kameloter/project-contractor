using UnityEngine;
using System.Collections;

public enum GameState {
    Menu, InGame, Paused
}

public class SceneManager : MonoBehaviour {
    GameState currentState;

    void Awake() {
        DontDestroyOnLoad(GameManager.Instance);
    }

    void Start() {
        currentState = GameManager.Instance.CurrentState;
    }

    public void SetState(GameState newGameState) {
        DisablePreviousState(currentState);
        switch (newGameState) {
            case GameState.Menu:
                break;
            case GameState.InGame:
                break;
            case GameState.Paused:
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
        SetState(GameState.InGame);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Export some data. This should be called just before switching levels.s
    /// </summary>
    public void ExportSaveData() {
        //This should change.
        GameManager.Instance.CollectablesCollected += GameManager.Instance.PlayerScript.Collectables; //Total collectables collected this level
        GameManager.Instance.MaxCollectablesAvailable += gameObject.GetComponent<SceneStats>().CollectablesAvailable; //Total collectables available this level
    }

    public void QuitGame() {
        Application.Quit();
    }
}