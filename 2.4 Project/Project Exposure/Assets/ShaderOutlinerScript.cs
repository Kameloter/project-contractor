using UnityEngine;
using System.Collections;


/// <summary>
/// Script that makes an outline around the object when player approaches it.
/// Expects to be attached on object with BaseInteractable and mesh renderer of target object needs to be dragged on the public variable.
/// </summary>
public class ShaderOutlinerScript : MonoBehaviour {
    public enum SizeOfObject { Small, Big }
    public SizeOfObject sizeOfObject;

    BaseInteractable owner;
    /// <summary>
    /// The objects renderer you want to highlight
    /// </summary>
    public MeshRenderer outlinedObjectRenderer;
    Material outlineMaterial;

    public void Start()
    {
        //load the material
        outlineMaterial = new Material(Resources.Load("ObjectOutliner", typeof(Material)) as Material);

        //set shader settings
        outlineMaterial.SetFloat("_Outline", 0.0075f);

        //subscribe to owners events
        owner = GetComponent<BaseInteractable>();
        owner.onTriggerEnterEvent.AddListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.AddListener(RemoveOutlineMaterial);

    }

    /// <summary>
    /// Adds the outline material to the arrays of materials
    /// </summary>
    public void AddOutlineMaterial()
    {
        if (outlinedObjectRenderer == null) { Debug.LogError("Variable 'outlinedObjectRenderer' has not been set.",transform); return; }
        Material[] original = outlinedObjectRenderer.materials; //store a copy of the material ON the object( shared material is THE MATERIAL INSTANCE => affects all object with that mat)

        Material[] newMaterials = new Material[original.Length + 1]; //make a new array with materials and put the old ones + outliner
        for (int i = newMaterials.Length - 1; i >= 0; i--)
        {
            if (i == (newMaterials.Length - 1))//we are at the last one
                newMaterials[i] = outlineMaterial;
            else
                newMaterials[i] = original[i];
        }
        outlinedObjectRenderer.sharedMaterials = newMaterials;
    }
    /// <summary>
    /// Removes the outline material to the arrays of materials
    /// </summary>
    public void RemoveOutlineMaterial() {
        if (outlinedObjectRenderer == null) { Debug.LogError("Variable 'outlinedObjectRenderer' has not been set."); return; }
        Material[] original = outlinedObjectRenderer.materials;
        Material[] newMaterials = new Material[original.Length - 1];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = original[i];
        }

        outlinedObjectRenderer.sharedMaterials = newMaterials;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.RemoveListener(RemoveOutlineMaterial);
    }
}