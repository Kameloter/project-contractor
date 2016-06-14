using UnityEngine;
using System.Collections;

/// <summary>
/// controls the temperature of the gameobject this is attached t
/// </summary>
public class TemperatureScript : MonoBehaviour {
    //different temperature states
    public enum TemperatureState { Frozen, Neutral, Hot
    }
    //current temperatureState
    public TemperatureState temperatureState = TemperatureState.Neutral;

    public bool changeable = true;

    //different materials for different states
    public Material frozenMaterial;
    public Material neutralMaterial;
    public Material hotMaterial;

    Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
        ChangeMaterial();
    }

    /// <summary>
    /// method to change the current state
    /// </summary>
    /// <param name="state">state to change to</param>
    public void ChangeState(TemperatureState state) {
        if (changeable) {
            temperatureState = state;
            ChangeMaterial();
        }
    } 


    /// <summary>
    /// method to change the material
    /// </summary>
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

    /// <summary>
    /// change state when hit by a particle
    /// </summary>
    /// <param name="go"></param>
    void OnParticleCollision(GameObject go) {
        print(go.name);
        if (go.CompareTag("FreezeParticle")) {
            print("test2");
            ChangeState(TemperatureState.Frozen);
        }
    
    }
}
