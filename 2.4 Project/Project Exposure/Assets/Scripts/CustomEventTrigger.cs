using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class for a trigger system to make easy events and actions when a player hits this trigger
/// </summary>
[System.Serializable]
public class CustomEventTrigger : MonoBehaviour {

    /// <summary>
    /// enum with all the avaible actions for the trigger
    /// </summary>
    [SerializeField]
    public enum Action {
        PlaySound, PlayAnimation, PlayCameraPath, ActivateInteractable,
        DeactivateInteractable, ShowTutorial, PlayParticle, StopParticle,
        ActivateLight, DisableLight, ChangeLightValues,
        ChangeImageEffects, ActivateObject, DeactivateObject, ChangeCameraOffset
    };

    /// <summary>
    /// enum to specify when you want to fire the event
    /// </summary>
    [SerializeField]
    public enum OnTrigger {
        OnTriggerEnter, OnTriggerExit, OnTriggerStay
    };

    /// <summary>
    /// enum to specify the fireMode of the event, once, repeating etc
    /// </summary>
    [SerializeField]
    public enum FireType {
        Once, Delayed, Repeat, RepeatDelayed
    };

    /// <summary>
    /// struct with all possible variable for all actions(enum)
    /// show correct viarables in custom event editor script
    /// </summary>
    [System.Serializable]
    public struct CustomEvent {
        public Action action;

        //fire and trigger types
        [SerializeField] public OnTrigger onTrigger;
        [SerializeField] public bool triggerMore;
        [SerializeField] public bool triggered;
        [SerializeField] public FireType fireType;
        [SerializeField] public float delay;
        [SerializeField] public float repeatTime;
        [SerializeField] public float repeatAmount;
        [SerializeField] public bool activated;

        //Gameobject to complete the action
        [SerializeField] public GameObject go;

        // audioclip to play
        [SerializeField] public AudioClip audioClip;

        // play animation
        [SerializeField] public AnimationClip animation;

        // activate / deactivate interactable
        [SerializeField] public BaseActivatable interactable;

        //camera cutscene
        [SerializeField] public GameObject path;
        [SerializeField]
        public bool startAtPlayer;

        //start tutorial
        [SerializeField] public Animator animator;
        [SerializeField]
        public string animationName;


        //particle
        [SerializeField] public ParticleSystem particle;

        //light stuff
        [SerializeField] public Light light;
        [SerializeField] public Color color;
        [SerializeField] public float intensity;
        [SerializeField] public float bounceIntensity;
        [SerializeField] public float range;

        //new camera offset
        [SerializeField] public Vector3 offset;
    }

    /// <summary>
    /// array of events so you can have multiple events per trigger
    /// </summary>
    [SerializeField]
    public CustomEvent[] customEvents;

    /// <summary>
    /// Enumerator to do a action based on the action enum
    /// </summary>
    /// <param name="i">index of the forloop</param>
    /// <param name="waitTime">optional wait time</param>
    IEnumerator DoEvent(int i, float waitTime) {
        yield return new WaitForSeconds(waitTime);

        if (!customEvents[i].triggered) {
            switch (customEvents[i].action) {
                case Action.ActivateInteractable:
                    customEvents[i].interactable.Activate();
                    break;
                case Action.DeactivateInteractable:
                    customEvents[i].interactable.Deactivate();
                    break;
                case Action.PlayAnimation:
                    customEvents[i].go.GetComponent<Animation>().clip = customEvents[i].animation;
                    customEvents[i].go.GetComponent<Animation>().Play();
                    break;
                case Action.PlayCameraPath:
                    Camera.main.GetComponent<CameraControl>().StartCutscene(customEvents[i].path,customEvents[i].startAtPlayer);
                    break;
                case Action.PlaySound:
                    customEvents[i].go.GetComponent<AudioSource>().clip = customEvents[i].audioClip;
                    customEvents[i].go.GetComponent<AudioSource>().Play();
                    break;
                case Action.ShowTutorial:
                    GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.RemoveAllListeners();
                    GameObject.Find("Repeat_Button").GetComponent<Button>().onClick.AddListener(() => { customEvents[i].animator.SetTrigger(customEvents[i].animationName); });
                    customEvents[i].animator.SetTrigger(customEvents[i].animationName);
                    break;
                case Action.ActivateLight:
                    customEvents[i].light.enabled = true;
                    break;
                case Action.DisableLight:
                    customEvents[i].light.enabled = false;
                    break;
                case Action.ChangeLightValues:
                    if (customEvents[i].light.range != 0) {
                        customEvents[i].light.range = customEvents[i].range;
                    }
                    customEvents[i].light.color = customEvents[i].color;
                    customEvents[i].light.intensity = customEvents[i].intensity;
                    customEvents[i].light.bounceIntensity = customEvents[i].bounceIntensity;
                    break;
                case Action.ActivateObject:
                    customEvents[i].go.SetActive(true);
                    break;
                case Action.DeactivateObject:
                    customEvents[i].go.SetActive(false);
                    break;
                case Action.PlayParticle:
                    customEvents[i].particle.Play();
                    break;
                case Action.StopParticle:
                    customEvents[i].particle.Stop();
                    break;
                case Action.ChangeCameraOffset:
                    Camera.main.GetComponent<CameraControl>().offset = customEvents[i].offset;
                    break;
            }
            if (!customEvents[i].triggerMore) {
                customEvents[i].triggered = true;
            }
        }
    }

    /// <summary>
    /// If you trigger the object it loops trough all events and fires them based on firetype
    /// Same for stay and exit trigger events
    /// </summary>
    /// <param name="other">The collider it hit with</param>
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Tags.player)) {
            for (int i = 0; i < customEvents.Length; i++) {
                if (customEvents[i].onTrigger == OnTrigger.OnTriggerEnter) {
                    switch (customEvents[i].fireType) {
                        case FireType.Once:
                            StartCoroutine(DoEvent(i, 0));
                            break;
                        case FireType.Delayed:
                            StartCoroutine(DoEvent(i, customEvents[i].delay));
                            break;
                        case FireType.Repeat:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, 0 + j * customEvents[i].repeatTime));
                            }
                            break;
                        case FireType.RepeatDelayed:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, customEvents[i].delay + j * customEvents[i].repeatTime));
                            }
                            break;
                    }
                }
            }

        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag(Tags.player)) {
            for (int i = 0; i < customEvents.Length; i++) {
                if (customEvents[i].onTrigger == OnTrigger.OnTriggerStay) {
                    switch (customEvents[i].fireType) {
                        case FireType.Once:
                            StartCoroutine(DoEvent(i, 0));
                            break;
                        case FireType.Delayed:
                            StartCoroutine(DoEvent(i, customEvents[i].delay));
                            break;
                        case FireType.Repeat:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, 0 + j * customEvents[i].repeatTime));
                            }
                            break;
                        case FireType.RepeatDelayed:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, customEvents[i].delay + j * customEvents[i].repeatTime));
                            }
                            break;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag(Tags.player)) {
            for (int i = 0; i < customEvents.Length; i++) {
                if (customEvents[i].onTrigger == OnTrigger.OnTriggerExit) {
                    switch (customEvents[i].fireType) {
                        case FireType.Once:
                            StartCoroutine(DoEvent(i, 0));
                            break;
                        case FireType.Delayed:
                            StartCoroutine(DoEvent(i, customEvents[i].delay));
                            break;
                        case FireType.Repeat:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, 0 + j * customEvents[i].repeatTime));
                            }
                            break;
                        case FireType.RepeatDelayed:
                            for (int j = 0; j < customEvents[i].repeatAmount; j++) {
                                StartCoroutine(DoEvent(i, customEvents[i].delay + j * customEvents[i].repeatTime));
                            }
                            break;
                    }
                }
            }
        }
    }
}

