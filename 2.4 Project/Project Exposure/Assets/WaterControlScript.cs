using UnityEngine;
using System.Collections;

public class WaterControlScript : BaseActivatable {

    Animator myAnimator;
    [SerializeField]
    SkinnedMeshRenderer skinnedRenderer;


    // Use this for initialization
    public override void Start()
    {

        myAnimator = GetComponent<Animator>();

        if (skinnedRenderer == null)
        {
            Debug.LogError("Drag TankWater object onto SkinnedRenderer variable AT: " + gameObject.name, transform);
        }
    }

    public override void Activate()
    {
        base.Activate();
        myAnimator.SetTrigger("Work");
        StartCoroutine("interpolateBlendShape", 100);

    }
    IEnumerator interpolateBlendShape(float value)
    {
        while (skinnedRenderer.GetBlendShapeWeight(0) < value - 5.0f) //so it stops at some point
        {
            float blendShapeWeight = skinnedRenderer.GetBlendShapeWeight(0);
            blendShapeWeight = Mathf.Lerp(blendShapeWeight, value, Time.deltaTime);
            skinnedRenderer.SetBlendShapeWeight(0, blendShapeWeight);
            yield return null;
        }
        skinnedRenderer.gameObject.SetActive(false);
        GameManager.Instance.TutorialSelector.ShowTutorial("Geothermal");
    }
    public override void Deactivate()
    {
        base.Deactivate();


    }
 

    
    // Update is called once per frame
    void Update ()
    {
    	if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine("interpolateBlendShape", 100);
        }
	}
}
