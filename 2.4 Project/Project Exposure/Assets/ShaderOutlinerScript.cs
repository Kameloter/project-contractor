using UnityEngine;
using System.Collections;

public class ShaderOutlinerScript : MonoBehaviour {


    BaseInteractable owner;
    MeshRenderer meshRenderer;
    Material outlineMaterial;



    public void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        outlineMaterial = Resources.Load("ObjectOutliner", typeof(Material)) as Material;
        owner = GetComponent<BaseInteractable>();
        owner.onTriggerEnterEvent.AddListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.AddListener(RemoveOutlineMaterial);
    }

    public void AddOutlineMaterial() {
        Material original = meshRenderer.material; //store a copy of the material ON the object( shared material is THE MATERIAL INSTANCE => affects all object with that mat)

        Material[] newMaterials = new Material[2]; //make a new array with materials
        newMaterials[0] = original;
        newMaterials[1] = outlineMaterial;

        meshRenderer.sharedMaterials = newMaterials;
    }

    public void RemoveOutlineMaterial() {
        Material original = meshRenderer.materials[0];
        Material[] newMaterials = new Material[1];
        newMaterials[0] = original;

        meshRenderer.sharedMaterials = newMaterials;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.RemoveListener(RemoveOutlineMaterial);
    }
}