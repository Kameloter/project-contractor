using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Class that holds game variables used by designers.
/// </summary>
public static class Game {


	private static List<GameObject> cachedObjects = new List<GameObject> ();
		 
	/// <summary>
	/// The main player of the game
	/// </summary>
    static public GameObject Player {
        get {
            return GameManager.Instance.Player;;
        }
    }

	/// <summary>
	/// Time left
	/// </summary>
	/// <value>The game time left.</value>
    static public float GameTimeLeft {
        get { return GameManager.Instance.gameTimeLeft; }
    }

	/// <summary>
	/// The time needed to complete the level (approximate)
	/// </summary>
	/// <value>The time needed for level.</value>
    static public float TimeNeededForLevel {
        get { return GameManager.Instance.TimeNeededForLevel; }
    }

	/// <summary>
	/// The time spent by the player on the current level
	/// </summary>
    static public float TimeSpentOnLevel {
        get { return GameManager.Instance.TimeSpentOnLevel; }
    }

	/// <summary>
	/// The last activated object. (bridge,door,lasor, etc etc)
	/// </summary>
	/// <value>The last activated object.</value>
    static public GameObject LastActivatedObject
    {
        get { return GameManager.Instance.ActivatedObject; }
    }

	/// <summary>
	/// Last deactivated object ( bridge , door , lasor , etc etc)
	/// </summary>
    static public GameObject LastDeactivatedObject
    {
        get { return GameManager.Instance.DeactivatedObject; }
    }

	/// <summary>
	/// The last object the player interacted with.
	/// </summary>
	/// <value>The last interacted object.</value>
    static public GameObject LastInteractedObject {
        get { return GameManager.Instance.InteractedObject; }
    }

	/// <summary>
	/// The score (get / set)
	/// </summary>
	/// <value>The score.</value>
    static public int Score {
        get { return GameManager.Instance.GameScore; }
        set { GameManager.Instance.GameScore = value; }
    }

	/// <summary>
	/// 
	/// </summary>
	/// <returns>The game object.</returns>
	/// <param name="GameObjectName">Game object name.</param>
    static public GameObject GetGameObject(string GameObjectName) { //CHANGE TO A DICTIONARY SO WE CACHE OBJECTS !! - VLAD.
        return GameObject.Find(GameObjectName);
    }

	static public GameObject GetGameObjectWithTag(string tag) {//CHANGE TO A DICTIONARY SO WE CACHE OBJECTS !! - VLAD.
        return GameObject.FindGameObjectWithTag(tag);
    }

	/// <summary>
	/// Activate an object which is typeof BaseActivatable(bridge door lasor , etc)
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void ActivateActivatable (GameObject activatable) {
        activatable.GetComponent<BaseActivatable>().Activate();
    }
	/// <summary>
	/// Activate an object which is typeof BaseActivatable(bridge door lasor , etc)
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void ActivateActivatable(BaseActivatable activatable) {
        activatable.Activate();
    }
	/// <summary>
	/// DeActivate an object which is typeof BaseActivatable(bridge door lasor , etc)
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void DeactivateActivatable(GameObject activatable) {
        activatable.GetComponent<BaseActivatable>().Deactivate();
    }
	/// <summary>
	/// DeActivate an object which is typeof BaseActivatable(bridge door lasor , etc)
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void DeactivateInteractable(BaseActivatable activatable) {
        activatable.Deactivate();
    }
	/// <summary>
	/// Plays a camera path with the provided path array.
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void PlayCameraPath (GameObject path,bool pStartAtPlayer) {
        Camera.main.GetComponent<CameraControl>().StartCutscene(path, pStartAtPlayer);
    }
	/// <summary>
	/// Plays a sound.
	/// </summary>
	/// <param name="activatable">Activatable.</param>
    static public void PlaySound(GameObject Source) {
        Source.GetComponent<AudioSource>().Play();
    }
	/// <summary>
	/// Plays a sound.
	/// </summary>
    static public void PlaySound(GameObject Source,AudioClip sound) {
        Source.GetComponent<AudioSource>().clip = sound;
        Source.GetComponent<AudioSource>().Play();
    }
	/// <summary>
	/// Activates a light
	/// </summary>
    static public void ActivateLight (Light light) {
        light.enabled = true;
    }
	/// <summary>
	/// Activates a light
	/// </summary>
    static public void DeactivateLight(Light light) {
        light.enabled = false;
    }
	/// <summary>
	/// Changes a lihts properties
	/// </summary>
    static public void ChangeLightValues(Light light, Color color, float intensity, float bounceIntensity) {
         light.color = color;
         light.intensity = intensity;
         light.bounceIntensity = bounceIntensity;
    }
	/// <summary>
	/// Activates the object.
	/// </summary>
	/// <param name="gameObject">Game object.</param>
    static public void ActivateObject(GameObject gameObject) {
        gameObject.SetActive(true);
    }
	/// <summary>
	/// Deactivates the object.
	/// </summary>
	/// <param name="gameObject">Game object.</param>
    static public void DeactivateObject(GameObject gameObject) {
        gameObject.SetActive(false);
    }
	/// <summary>
	/// Plays the particle.
	/// </summary>
	/// <param name="particleObject">Particle object.</param>
    static public void PlayParticle(GameObject particleObject) {
        particleObject.GetComponent<ParticleSystem>().Play() ;
    }
	/// <summary>
	/// Plays the particle
	/// </summary>
	/// <param name="particleSystem">Particle system.</param>
    static public void PlayParticle(ParticleSystem particleSystem) {
       particleSystem.Play();
    }
	/// <summary>
	/// Stops the particle.
	/// </summary>
	/// <param name="particleObject">Particle object.</param>
    static public void StopParticle(GameObject particleObject) {
        particleObject.GetComponent<ParticleSystem>().Stop();
    }
	/// <summary>
	/// Stops the particle.
	/// </summary>
	/// <param name="particleSystem">Particle system.</param>
    static public void StopParticle(ParticleSystem particleSystem) {
        particleSystem.Stop();
    }
}
