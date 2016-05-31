using UnityEngine;
using System.Collections;

public static class Game {

    static public GameObject Player {
        get {
            return GameManager.Instance.Player;;
        }
    }

    static public int Collectables {
        get { return GameManager.Instance.CollectablesCollected; }
    }

    static public float TimeLeft {
        get { return GameManager.Instance.TimeLeft; }
    }

    static public int Score {
        get { return GameManager.Instance.Score; }
        set { GameManager.Instance.Score = value; }
    }

    //clickedObject

    //activatedItem

    static public GameObject GetGameObject(string GameObjectName) {
        return GameObject.Find(GameObjectName);
    }

    static public GameObject GetGameObjectWithTag(string tag) {
        return GameObject.FindGameObjectWithTag(tag);
    }

    static public void ActivateInteractable (GameObject Interactable) {
        Interactable.GetComponent<BaseInteractable>().Activate();
    }

    static public void ActivateInteractable(BaseInteractable Interactable) {
        Interactable.Activate();
    }

    static public void DeactivateInteractable(GameObject Interactable) {
        Interactable.GetComponent<BaseInteractable>().DeActivate();
    }

    static public void DeactivateInteractable(BaseInteractable Interactable) {
        Interactable.DeActivate();
    }

    static public void PlayCameraPath (GameObject path) {
        Camera.main.GetComponent<CameraControl>().StartCutscene(path);
    }

    static public void PlaySound(GameObject Source) {
        Source.GetComponent<AudioSource>().Play();
    }

    static public void PlaySound(GameObject Source,AudioClip sound) {
        Source.GetComponent<AudioSource>().clip = sound;
        Source.GetComponent<AudioSource>().Play();
    }

    static public void ActivateLight (Light light) {
        light.enabled = true;
    }

    static public void DeactivateLight(Light light) {
        light.enabled = false;
    }

    static public void ChangeLightValues(Light light, Color color, float intensity, float bounceIntensity) {
         light.color = color;
         light.intensity = intensity;
         light.bounceIntensity = bounceIntensity;
    }

    static public void ActivateObject(GameObject gameObject) {
        gameObject.SetActive(true);
    }

    static public void DeactivateObject(GameObject gameObject) {
        gameObject.SetActive(false);
    }

    static public void PlayParticle(GameObject particleObject) {
        particleObject.GetComponent<ParticleSystem>().Play() ;
    }

    static public void PlayParticle(ParticleSystem particleSystem) {
       particleSystem.Play();
    }

    static public void StopParticle(GameObject particleObject) {
        particleObject.GetComponent<ParticleSystem>().Stop();
    }

    static public void StopParticle(ParticleSystem particleSystem) {
        particleSystem.Stop();
    }
}
