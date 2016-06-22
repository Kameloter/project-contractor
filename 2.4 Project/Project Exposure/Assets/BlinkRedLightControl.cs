using UnityEngine;
using System.Collections;

public class BlinkRedLightControl : MonoBehaviour {

    private Light redLight;
    public float lightBlinkSpeed;

    public float minIntensity;
    public float maxIntensity;

	// Use this for initialization
	void Awake ()
    {
        redLight = GetComponent<Light>();
    
	}

    public void StartBlinking()
    {
        StartCoroutine("blink");
    }
    public void StopBlinking()
    {
        StopCoroutine("blink");
    }


    IEnumerator blink()
    {
        float targetIntensity = maxIntensity; 
        while (true)
        {
            redLight.intensity = Mathf.Lerp(redLight.intensity, targetIntensity, Time.deltaTime * lightBlinkSpeed);

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
