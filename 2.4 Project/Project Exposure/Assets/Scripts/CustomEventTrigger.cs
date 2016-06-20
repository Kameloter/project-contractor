using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class for a trigger system to make easy events and actions when a player hits this trigger
/// </summary>
public class CustomEventTrigger : MonoBehaviour {
    /// <summary>
    /// enum with all the avaible actions for the trigger
    /// </summary>
   // [SerializeField]
    public enum Action {
        PlaySound, PlayAnimation, PlayCameraPath, ActivateInteractable,
        DeactivateInteractable, ShowTutorial, PlayParticle, StopParticle,
        ActivateLight, DisableLight, ChangeLightValues,
        ChangeImageEffects, ActivateObject, DeactivateObject, ChangeCameraOffset
    };

    /// <summary>
    /// enum to specify when you want to fire the event
    /// </summary>
   // [SerializeField]
    public enum OnTrigger {
        OnTriggerEnter, OnTriggerExit, OnTriggerStay
    };

    /// <summary>
    /// enum to specify the fireMode of the event, once, repeating etc
    /// </summary>
    public enum FireType {
        Once, Delayed, Repeat, RepeatDelayed
    };

    /// <summary>
    /// enum to specify what tutorial you want to show
    /// </summary>
    public enum TypeOfTutorial {
        Geothermal, Valve, Laser, Bridge, Door, Ice, Walking
    };

    /// <summary>
    /// struct with all possible variable for all actions(enum)
    /// show correct viarables in custom event editor script
    /// </summary>
    [System.Serializable]
    public struct CustomEvent {
        //action
        public Action action;

        //fire and trigger types
        public OnTrigger onTrigger;
        public bool triggerMore;
        public bool triggered;
        public FireType fireType;
        public float delay;
        public float repeatTime;
        public float repeatAmount;
        public bool activated;
        public TypeOfTutorial tutorialType;

        //Gameobject to complete the action
        public GameObject go;

        // audioclip to play
        public AudioClip audioClip;

        // play animation
        public AnimationClip animation;

        // activate / deactivate interactable
        public BaseActivatable interactable;

        //camera cutscene
        public GameObject path;
        public bool startAtPlayer;

        //start tutorial
        //[SerializeField] public TutorialSelectorScript tutorialSelector;
        public string tutorialName;

        //particle
        public ParticleSystem particle;

        //light stuff
        public Light light;
        public Color color;
        public float intensity;
        public float bounceIntensity;
        public float range;

        //new camera offset
        public Vector3 offset;
    }

    /// <summary>
    /// array of events so you can have multiple events per trigger
    /// </summary>
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
                    GameManager.Instance.TutorialSelector.ShowTutorial(customEvents[i].tutorialType.ToString());
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

