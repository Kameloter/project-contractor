using UnityEngine;
using System.Collections;

/// <summary>
/// Used to autocomplete tags. Less prone to typos.
/// </summary>
public class Tags : MonoBehaviour {
    //----------------- Unity Predifined Tags -----------------
    public const string respawn = "Respawn";
    public const string finish = "Finish";
    public const string editorOnly = "EditorOnly";
    public const string mainCamera = "MainCamera";
    public const string player = "Player";
    public const string gameController = "GameController";
    //---------------------------------------------------------

    //Custom Tags
    public const string valve = "Valve";
    public const string mirror = "Mirror";
    public const string managers = "Managers";
    public const string collectable = "Collectable";
    public const string particleWater = "WaterParticle";
    public const string particleHeat = "HeatParticle";
    public const string particleSteam = "SteamParticle";
    public const string particleFreeze = "FreezeParticle";
    public const string meltable = "Meltable";
    public const string skipButton = "SkipButton";
    public const string scoreScreen = "ScoreScreen";
    public const string levelSwitcher = "LevelSwitcher";
    public const string uiMonitor = "UI Monitor";
    public const string tutorialSelector = "TutorialSelector";
}
