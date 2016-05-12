using UnityEngine;
using System.Collections;

public class GeneratorScript : MonoBehaviour {

    public GameObject line1Path;

    bool waterActivated = false;
    bool heatActivated = false;
    bool startedParticle = false;

    float waterTimer = 0.0f;
    float heatTimer = 0.0f;

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update() {
        heatTimer += Time.deltaTime;
        waterTimer += Time.deltaTime;

        if (waterTimer > 0.5f) {
            waterActivated = false;
        }
        if (heatTimer > 0.5f) {
            heatActivated = false;
        }

        if (startedParticle && !(heatActivated && waterActivated)) {
            GetComponentInChildren<ParticleSystem>().Stop();
            print("dsadasda");
            startedParticle = false;


            float distance = Vector3.Distance(transform.GetChild(0).position, line1Path.transform.GetChild(0).position);
            float waitTime = distance / GetComponentInChildren<ParticleSystem>().startSpeed;
            Invoke("Deactivate", waitTime);
        }
    }

    void Deactivate() {
        line1Path.transform.GetChild(0).GetComponent<SteamPipeJoint>().StopSteamConnection();
    }

    void OnParticleCollision(GameObject go) {
        if (go.CompareTag(Tags.particleHeat)) {
            heatTimer = 0.0f;
            heatActivated = true;
        }
        if (go.CompareTag(Tags.particleWater)) {
            waterTimer = 0.0f;
            waterActivated = true;
        }

        if (heatActivated && waterActivated && !startedParticle) {
            GetComponentInChildren<ParticleSystem>().Play();
            startedParticle = true;
        }

    }
}
