using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionImageScript : MonoBehaviour {
    [Header("References")]
    [Tooltip("The GameObject with Image(script) on a Canvas.")]
    public Image helpImage;

    [Tooltip("The sprite to show.")]
    public Sprite sprite;

    [Header("Behaviour")]
    public bool useTriggerExit = true;
    public bool pausesGameplay = false;
    public bool showUntilClick = false;

    [Tooltip("Time to show sprite (0 = infinit, make sure to disable it some other way).")]
    public float showTime = 0.0f;

    [Header("Debug Info")]
    [ReadOnly][SerializeField] bool activatable = true;

    bool holdingDown;

    void Start() {
        GetImageObject();
    }

    void Update() {
        if (pausesGameplay || showUntilClick) {
            //AnyKeyUp workaround
            if (helpImage.enabled && Input.anyKey) {
                //Key was down
                holdingDown = true;
            }
            if (!Input.anyKey && holdingDown) {
                //Key was released
                holdingDown = false;
                Time.timeScale = 1;
                DisableImage();
            }
        }
    }
	
    /// <summary>
    /// Checks if there is a reference to the image script, if there isn't it tries to make one.
    /// </summary>
    void GetImageObject() {
        //Check if there is an image specified via the inspector, if not try to find it automatically.
        if (helpImage == null) {
            if (GameObject.Find("HelpImage") == null) Debug.LogError("Couldn't find Image(script) named 'HelpImage', make sure its somewhere (active) or specify it via the inspector");
            else {
                Debug.LogWarning("HelpImage found automatically by script.");
                helpImage = GameObject.Find("HelpImage").GetComponent<Image>();
            }
        }
        helpImage.enabled = false;
    }

    void SetSprite() {
        if (sprite != null) helpImage.sprite = sprite;
        else Debug.LogError("No sprite set to " + gameObject.name);
    }

    void OnTriggerEnter(Collider hit) {
        if (activatable) EnableImage();
        if (showTime > 0) Invoke("DisableImage", showTime);
        if (pausesGameplay) Time.timeScale = 0;                 
    }

    void OnTriggerExit(Collider hit) {
        if (useTriggerExit) DisableImage();
    }

    void EnableImage() {
        SetSprite();
        activatable = false;
        helpImage.enabled = true;
    }

    public void DisableImage() {
        activatable = false;
        helpImage.enabled = false;
        Invoke("Reactivatable", 0.5f);
    }

    void Reactivatable() {
        activatable = true;
    }
}