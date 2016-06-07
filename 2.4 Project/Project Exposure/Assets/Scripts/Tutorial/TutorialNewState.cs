using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// This script is used to set the EmptyState of the tutorial to be transparent.
/// </summary>
public class TutorialNewState : StateMachineBehaviour {
    Image image;
    Canvas canvas;
    

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        image = animator.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);

        if (canvas == null) canvas = image.canvas;
        canvas.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        image = animator.GetComponent<Image>();
        image.color = Color.white;

        canvas.enabled = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
