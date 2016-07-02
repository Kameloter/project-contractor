using UnityEngine;
using System.Collections;


/// <summary>
/// Class that can make a light blink. Expects to be attached on a game object with a light
/// </summary>
public class BlinkRedLightControl : MonoBehaviour {

    //refference to the light
    private Light redLight;

    public float lightBlinkSpeed;

    public float minIntensity;
    public float maxIntensity;

	// Use this for initialization
	void Awake ()
    {
        redLight = GetComponent<Light>();
    
	}

    /// <summary>
    /// Makes the light blink.
    /// </summary>
    public void StartBlinking()
    {
        StartCoroutine("blink");
    }
    /// <summary>
    /// Makes the light stop blinking
    /// </summary>
    public void StopBlinking()
    {
        StopCoroutine("blink");
    }
    void OnDestroy()
    {
        //When we destroy , stop coroutine if it wasnt stoppped before.
        StopCoroutine("blink");
    }



    IEnumerator blink()
    {
        //set target
        float targetIntensity = maxIntensity; 
        while (true)//infinite condition
        {
			//lerp to target
            redLight.intensity = Mathf.Lerp(redLight.intensity, targetIntensity, Time.deltaTime * lightBlinkSpeed);


			//change target the moment we are close to the treshold
            if (redLight.intensity < minIntensity + 0.2f)
            {
                targetIntensity = maxIntensity;
            }
            if (redLight.intensity > maxIntensity - 0.2f)
            {
                targetIntensity = minIntensity;
            }
            yield return null;
        }


    }
}
