using UnityEngine;
using System.Collections;

public class ShaderOutlinerScript : MonoBehaviour {
    public enum SizeOfObject { Small, Big }
    public SizeOfObject sizeOfObject;

    BaseInteractable owner;
    public MeshRenderer outlinedObjectRenderer;
    Material outlineMaterial;

    public void Start() {
        outlineMaterial = new Material(Resources.Load("ObjectOutliner", typeof(Material)) as Material);
        if (sizeOfObject == SizeOfObject.Big) {
            outlineMaterial.SetFloat("_Outline", 0.0001f);
        }
        owner = GetComponent<BaseInteractable>();
        owner.onTriggerEnterEvent.AddListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.AddListener(RemoveOutlineMaterial);
    }

    public void AddOutlineMaterial() {
        if (outlinedObjectRenderer == null) { Debug.LogError("Variable 'outlinedObjectRenderer' has not been set."); return; }
        Material original = outlinedObjectRenderer.material; //store a copy of the material ON the object( shared material is THE MATERIAL INSTANCE => affects all object with that mat)

        Material[] newMaterials = new Material[2]; //make a new array with materials
        newMaterials[0] = original;
        newMaterials[1] = outlineMaterial;

        outlinedObjectRenderer.sharedMaterials = newMaterials;
    }

    public void RemoveOutlineMaterial() {
        if (outlinedObjectRenderer == null) { Debug.LogError("Variable 'outlinedObjectRenderer' has not been set."); return; }
        Material original = outlinedObjectRenderer.materials[0];
        Material[] newMaterials = new Material[1];
        newMaterials[0] = original;

        outlinedObjectRenderer.sharedMaterials = newMaterials;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.RemoveListener(RemoveOutlineMaterial);
    }
}