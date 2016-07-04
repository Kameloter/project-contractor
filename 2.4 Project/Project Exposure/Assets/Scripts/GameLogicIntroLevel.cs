using UnityEngine;
using System.Collections;

/// <summary>
/// gamelogic script for simple scripting
/// Game class hold functions and variables you can easely use to make some simple actions.
/// </summary>
public class GameLogicIntroLevel : MonoBehaviour {
    GameObject player;

    public GameObject levelSwitcher;
    public GameObject endDoorToOpen;

    public CompressorControlScript CompressorObject;

    public GameObject buttonWaterTank;
    public Light light_1;
    public GameObject buttonCompressor;
    public Light light_2;

    private BlinkRedLightControl light_1_control;
    private BlinkRedLightControl light_2_control;

    bool activatedButton2 = false;
    bool clickedButton2 = false;

    public GameObject waterTankPath;
    public GameObject steamPath;

    //tap here
    public GameObject tapHereWalk, tapHerePump;
    bool tapHereWalkActive = true;

    // Use this for initialization
    void Start ()
    {
        //HEIM FIX
        //This is to ensure the default settings will be used (at least on the second run)
        PlayerPrefs.DeleteAll();

        player = Game.Player;

        light_1_control = light_1.gameObject.GetComponent<BlinkRedLightControl>();
        light_2_control = light_2.gameObject.GetComponent<BlinkRedLightControl>();


        buttonCompressor.GetComponent<SphereCollider>().enabled = false;
        light_1_control.StartBlinking();

        levelSwitcher.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if player moved towards the tapherewalk, remove it.
        if (tapHereWalkActive && Vector3.Distance(player.transform.position, tapHereWalk.transform.position) < 2.0f) {
            tapHereWalk.SetActive(false);
            tapHereWalkActive = false;
        }

        if (!activatedButton2) //activate button 2
        {
            if (GameManager.Instance.InteractedObject == buttonWaterTank)
            {
                Camera.main.GetComponent<CameraControl>().StartCutscene(waterTankPath, true);
                StartCoroutine("activateButton2", 5f);
                activatedButton2 = true;

                //if player tapped the button, remove tapherepump.
                tapHerePump.SetActive(false);
            }
        }

        if (!clickedButton2)
        {
            if (GameManager.Instance.InteractedObject == buttonCompressor)
            {
                Camera.main.GetComponent<CameraControl>().StartCutscene(steamPath, true);
                light_2_control.StopBlinking();
                light_2.enabled = false;
             
                clickedButton2 = true;
                
                levelSwitcher.SetActive(true);

                StartCoroutine("openFinalDoor", 2.0f);
            }
        }
    }

    IEnumerator openFinalDoor(float time)
    {
        yield return new WaitForSeconds(time);
        endDoorToOpen.GetComponent<Animator>().SetTrigger("Open");
        endDoorToOpen.GetComponentInChildren<Light>().gameObject.GetComponent<BlinkRedLightControl>().StartBlinking();
    }


    IEnumerator activateButton2 (float time)
    {
        yield return new WaitForSeconds(time);
        light_1_control.StopBlinking();
        light_1.enabled = false;

        light_2_control.StartBlinking();
        buttonCompressor.GetComponent<SphereCollider>().enabled = true;

        CompressorObject.RunSteamCompressor();
    }
}
