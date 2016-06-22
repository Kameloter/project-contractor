using UnityEngine;
using System.Collections;

/// <summary>
/// gamelogic script for simple scripting
/// Game class hold functions and variables you can easely use to make some simple actions.
/// </summary>
public class GameLogicIntroLevel : MonoBehaviour {


    public LevelSwitcherScript levelSwitcher;
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
	
    
    // Use this for initialization
	void Start ()
    {
         
        light_1_control = light_1.gameObject.GetComponent<BlinkRedLightControl>();
        light_2_control = light_2.gameObject.GetComponent<BlinkRedLightControl>();


        buttonCompressor.GetComponent<SphereCollider>().enabled = false;
        light_1_control.StartBlinking();

        levelSwitcher.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!activatedButton2) //activate button 2
        {
            if (GameManager.Instance.InteractedObject == buttonWaterTank)
            {

                StartCoroutine("activateButton2", 5f);
                activatedButton2 = true;

            }
        }

        if (!clickedButton2)
        {
            if (GameManager.Instance.InteractedObject == buttonCompressor)
            {
                light_2_control.StopBlinking();
                light_2.enabled = false;
             
                clickedButton2 = true;
                
                levelSwitcher.enabled = true;

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
