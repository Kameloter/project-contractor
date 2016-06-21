using UnityEngine;
using System.Collections;

public static class Game {
    static public GameObject Player {
        get {
            return GameManager.Instance.Player;;
        }
    }

    static public float TimeLeft {
        get { return GameManager.Instance.TimeLeft; }
    }

    static public float TimeNeededForLevel {
        get { return GameManager.Instance.TimeNeededForLevel; }
    }

    static public float TimeSpentOnLevel {
        get { return GameManager.Instance.TimeSpentOnLevel; }
    }

    static public GameObject LastActivatedObject
    {
        get { return GameManager.Instance.ActivatedObject; }
    }

    static public GameObject LastDeactivatedObject
    {
        get { return GameManager.Instance.DeactivatedObject; }
    }

    static public GameObject LastInteractedObject {
        get { return GameManager.Instance.InteractedObject; }
    }

    static public int Score {
        get { return GameManager.Instance.GameScore; }
        set { GameManager.Instance.GameScore = value; }
    }

    static public GameObject GetGameObject(string GameObjectName) {
        return GameObject.Find(GameObjectName);
    }

    static public GameObject GetGameObjectWithTag(string tag) {
        return GameObject.FindGameObjectWithTag(tag);
    }

    static public void ActivateActivatable (GameObject activatable) {
        activatable.GetComponent<BaseActivatable>().Activate();
    }

    static public void ActivateActivatable(BaseActivatable activatable) {
        activatable.Activate();
    }

    static public void DeactivateActivatable(GameObject activatable) {
        activatable.GetComponent<BaseActivatable>().Deactivate();
    }

    static public void DeactivateInteractable(BaseActivatable activatable) {
        activatable.Deactivate();
    }

    static public void PlayCameraPath (GameObject path,bool pStartAtPlayer) {
        Camera.main.GetComponent<CameraControl>().StartCutscene(path, pStartAtPlayer);
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
