using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[System.Serializable]
public class CustomEventTrigger : MonoBehaviour {

    [SerializeField]
    public enum Action {
        PlaySound, PlayAnimation, PlayCameraPath, ActivateInteractable, DeactivateInteractable, ShowHint, PlayParticle, StopParticle, FocusOnTarget, ActivateLight, DisableLight, ChangeLightValues, ChangeImageEffects, ActivateObject, DeactivateObject
    };

    [SerializeField]
    public enum OnTrigger {
        OnTriggerEnter, OnTriggerExit, OnTriggerStay
    };

    [SerializeField]
    public enum FireType {
        Once, Delayed, Repeat, RepeatDelayed
    };

    bool triggered = false;

    [System.Serializable]
    public struct info {
        public Action action;

        [SerializeField]
        public OnTrigger onTrigger;

        [SerializeField]
        public FireType fireType;

        [SerializeField]
        public float delay;

        [SerializeField]
        public float repeatTime;

        [SerializeField]
        public float repeatAmount;


        [SerializeField]
        public bool activated;

        //bool if activated
        //[SerializeField]
        //public bool activated;

        //gameobject
        [SerializeField]
        public GameObject go;

        // audioclip to play
        [SerializeField]
        public AudioClip audioClip;

        // play animation
        [SerializeField]
        public AnimationClip animation;

        // activate / deactivate interactable
        [SerializeField]
        public BaseActivatable interactable;

        //camera cutscene
        [SerializeField]
        public GameObject path;

        //start hint
        [SerializeField]
        public Image image;

        [SerializeField]
        public Sprite sprite;

        //particle
        [SerializeField]
        public ParticleSystem particle;

        //light stuff
        [SerializeField]
        public Light light;

        [SerializeField]
        public Color color;

        [SerializeField]
        public float intensity;

        [SerializeField]
        public float bounceIntensity;

        [SerializeField]
        public float range;
    }

    [SerializeField]
    public info[] Go;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator DoEvent(int i, float waitTime) {
        yield return new WaitForSeconds(waitTime);

        switch (Go[i].action) {
            case Action.ActivateInteractable:
                Go[i].interactable.Activate();
                break;
            case Action.DeactivateInteractable:
                Go[i].interactable.DeActivate();
                break;
            case Action.PlayAnimation:
                Go[i].go.GetComponent<Animation>().clip = Go[i].animation;
                Go[i].go.GetComponent<Animation>().Play();
                break;
            case Action.PlayCameraPath:
                Camera.main.GetComponent<CameraControl>().StartCutscene(Go[i].path);
                break;
            case Action.PlaySound:
                Go[i].go.GetComponent<AudioSource>().clip = Go[i].audioClip;
                Go[i].go.GetComponent<AudioSource>().Play();
                break;
            case Action.ShowHint:
                //gotyn
                break;
            case Action.ActivateLight:
                Go[i].light.enabled = true;
                break;
            case Action.DisableLight:
                Go[i].light.enabled = false;
                break;
            case Action.ChangeLightValues:
                if (Go[i].light.range != null) {
                    Go[i].light.range = Go[i].range;
                }
                Go[i].light.color = Go[i].color;
                Go[i].light.intensity = Go[i].intensity;
                Go[i].light.bounceIntensity = Go[i].bounceIntensity;
                break;
            case Action.ActivateObject:
                Go[i].go.SetActive(true);
                break;
            case Action.DeactivateObject:
                Go[i].go.SetActive(false);
                break;
            case Action.PlayParticle:
                Go[i].particle.Play();
                break;
            case Action.StopParticle:
                Go[i].particle.Stop();
                break;
            case Action.FocusOnTarget:
                Camera.main.GetComponent<CameraControl>().StartCutscene(Go[i].go);
                break;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!triggered)
        {
            if (other.CompareTag(Tags.player))
            {
                for (int i = 0; i < Go.Length; i++)
                {
                    if (Go[i].onTrigger == OnTrigger.OnTriggerEnter)
                    {
                        switch (Go[i].fireType)
                        {
                            case FireType.Once:
                                StartCoroutine(DoEvent(i, 0));
                                break;
                            case FireType.Delayed:
                                StartCoroutine(DoEvent(i, Go[i].delay));
                                break;
                            case FireType.Repeat:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, 0 + j * Go[i].repeatTime));
                                }
                                break;
                            case FireType.RepeatDelayed:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, Go[i].delay + j * Go[i].repeatTime));
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (!triggered)
        {
            if (other.CompareTag(Tags.player))
            {
                for (int i = 0; i < Go.Length; i++)
                {
                    if (Go[i].onTrigger == OnTrigger.OnTriggerStay)
                    {
                        switch (Go[i].fireType)
                        {
                            case FireType.Once:
                                StartCoroutine(DoEvent(i, 0));
                                break;
                            case FireType.Delayed:
                                StartCoroutine(DoEvent(i, Go[i].delay));
                                break;
                            case FireType.Repeat:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, 0 + j * Go[i].repeatTime));
                                }
                                break;
                            case FireType.RepeatDelayed:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, Go[i].delay + j * Go[i].repeatTime));
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (!triggered)
        {
            if (other.CompareTag(Tags.player))
            {
                for (int i = 0; i < Go.Length; i++)
                {
                    if (Go[i].onTrigger == OnTrigger.OnTriggerExit)
                    {
                        switch (Go[i].fireType)
                        {
                            case FireType.Once:
                                StartCoroutine(DoEvent(i, 0));
                                break;
                            case FireType.Delayed:
                                StartCoroutine(DoEvent(i, Go[i].delay));
                                break;
                            case FireType.Repeat:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, 0 + j * Go[i].repeatTime));
                                }
                                break;
                            case FireType.RepeatDelayed:
                                for (int j = 0; j < Go[i].repeatAmount; j++)
                                {
                                    StartCoroutine(DoEvent(i, Go[i].delay + j * Go[i].repeatTime));
                                }
                                break;
                        }
                    }
                }
            }
        }
        triggered = true;
    }
}

