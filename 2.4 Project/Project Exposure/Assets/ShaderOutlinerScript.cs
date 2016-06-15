using UnityEngine;
using System.Collections;

public class ShaderOutlinerScript : MonoBehaviour {
    public enum SizeOfObject { Small, Big }
    public SizeOfObject sizeOfObject;

    BaseInteractable owner;
    public MeshRenderer outlinedObjectRenderer;
    Material outlineMaterial;

    public void Start()
    {

        outlineMaterial = new Material(Resources.Load("ObjectOutliner", typeof(Material)) as Material);

        //if (sizeOfObject == SizeOfObject.Big)
        //{
        //    outlineMaterial.SetFloat("_Outline", 0.0001f);
        //}
        //else
        //{
            outlineMaterial.SetFloat("_Outline", 0.0075f);
        //}

        owner = GetComponent<BaseInteractable>();
        owner.onTriggerEnterEvent.AddListener(AddOutlineMaterial);
        owner.onTriggerExitEvent.AddListener(RemoveOutlineMaterial);

    }

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