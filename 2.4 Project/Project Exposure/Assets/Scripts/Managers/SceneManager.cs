using UnityEngine;
using System.Collections;

public enum GameState {
    Menu, InGame, Paused
}

/// <summary>
/// This script controls the scene transitioning between levels / menu.
/// </summary>
public class SceneManager : MonoBehaviour {
    GameState currentState;

    void Awake() {
        DontDestroyOnLoad(GameManager.Instance);
    }

    void Start() {
        currentState = GameManager.Instance.CurrentState;
    }

	/// <summary>
	/// Sets the game state.
	/// </summary>
	/// <param name="newGameState">New game state.</param>
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

	/// <summary>
	/// Unpauses the game.
	/// </summary>
    public void UnpauseGame() {
        SetState(GameState.InGame);
    }

	/// <summary>
	/// Pauses the game.
	/// </summary>
    public void PauseGame() {
        SetState(GameState.Paused);
    }

	/// <summary>
	/// Swithes the level to the provided index.
	/// </summary>
	/// <param name="index">Index of level in build settings.</param>
    public void SwitchToLevel(int index) {
        ExportSaveData();
        SetState(GameState.InGame);
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    /// <summary>
    /// Export some data. This should be called just before switching levels.s
    /// </summary>
    public void ExportSaveData() {
        //get score from scorescreen
        GameManager.Instance.GameScore += GameManager.Instance.ScoreScreen.LevelScore;
    }

	/// <summary>
	/// Shuts down the game.
	/// </summary>
    public void QuitGame() {
        Application.Quit();
    }
}