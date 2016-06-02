using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (AlignRotationScript))]
public class InteractCanvasScript : MonoBehaviour {
    public enum ImageType { ValvePlacement, ValveRotate, Other }

    BaseInteractable owner; //The object that has 
    AlignRotationScript alignScript;

    [Header("References (auto)")]
    public Canvas objectCanvas;
    public Image imageObject;
    

    [Header("Interaction Image Type")]
    public ImageType imageType;
    [ReadOnly] public Sprite interactionSprite;

    SpriteState spriteState = new SpriteState();
    Button button;

    void Start() {
        owner = transform.root.GetComponent<BaseInteractable>();
        if (owner == null) owner = transform.root.GetComponentInChildren<BaseInteractable>();
        if (owner == null) Debug.LogError("Cannot find owner.");

        objectCanvas = GetComponentInChildren<Canvas>();
        if (objectCanvas != null) objectCanvas.enabled = false;
        else Debug.LogError("Assign something to the 'objectCanvas' variable on '" + gameObject.name + "'.");

        alignScript = GetComponent<AlignRotationScript>();

        imageObject = GetComponentInChildren<Image>();
        button = imageObject.gameObject.GetComponent<Button>();

        if      (imageType == ImageType.ValvePlacement) {
            //set the button transition to spriteswap
            button.transition = Selectable.Transition.SpriteSwap;

            //set the proper sprites
            spriteState.highlightedSprite = Resources.Load("ValveSprite_Pressed", typeof(Sprite)) as Sprite;
            spriteState.pressedSprite = Resources.Load("ValveSprite_Pressed", typeof(Sprite)) as Sprite;
            button.spriteState = spriteState;

            //set the normal sprite
            interactionSprite = Resources.Load("ValveSprite", typeof(Sprite)) as Sprite;
        }
        else if (imageType == ImageType.ValveRotate   ) {
            button.transition = Selectable.Transition.ColorTint;
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.red;
            colorBlock.pressedColor = Color.red;
            colorBlock.colorMultiplier = 1;
            button.colors = colorBlock;
            interactionSprite = Resources.Load("RotateSprite", typeof(Sprite)) as Sprite;
        }
        else if (imageType == ImageType.Other         ) { /*Image should be specified in inspector*/ }
        imageObject.sprite = interactionSprite;

        owner.onTriggerEnterEvent.AddListener(ActivateCanvas);
        owner.onTriggerExitEvent.AddListener(DeactivateCanvas);
    }

    void ActivateCanvas() {
        objectCanvas.enabled = true;
        alignScript.Align();
    }
    void DeactivateCanvas() {
        objectCanvas.enabled = false;
    }

    void OnDestroy() {
        owner.onTriggerEnterEvent.RemoveListener(ActivateCanvas);
        owner.onTriggerExitEvent.RemoveListener(DeactivateCanvas);
    }
}
