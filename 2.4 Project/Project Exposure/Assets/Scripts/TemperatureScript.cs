using UnityEngine;
using System.Collections;

public class TemperatureScript : MonoBehaviour {

    public enum TemperatureState { Frozen, Neutral, Hot
    }

    public TemperatureState temperatureState = TemperatureState.Neutral;

    public bool changeable = true;

    public Material frozenMaterial;
    public Material neutralMaterial;
    public Material hotMaterial;

    Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
        ChangeMaterial();
        
    }
    void Update() {
        //if (Input.GetKeyDown(KeyCode.I)) {
        //    ChangeState(TemperatureState.Frozen);
        //}
        //if (Input.GetKeyDown(KeyCode.O)) {
        //    ChangeState(TemperatureState.Neutral);
        //}
        //if (Input.GetKeyDown(KeyCode.P)) {
        //    ChangeState(TemperatureState.Hot);
        //}
    }

    public void ChangeState(TemperatureState state) {
        if (changeable) {
            temperatureState = state;
            ChangeMaterial();
        }
    } 

    void ChangeMaterial() {
        switch (temperatureState) {
            case TemperatureState.Frozen:
                rend.material = frozenMaterial;
                break;
            case TemperatureState.Neutral:
                rend.material = neutralMaterial;
                break;
            case TemperatureState.Hot:
                rend.material = hotMaterial;
                break;
        }
    }

    void OnParticleCollision(GameObject go) {
        print(go.name);
        if (go.CompareTag("FreezeParticle")) {
            print("test2");
            ChangeState(TemperatureState.Frozen);
        }
    
    }
}
