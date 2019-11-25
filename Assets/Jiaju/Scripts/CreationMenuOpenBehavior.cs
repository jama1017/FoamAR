using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationMenuOpenBehavior : StateMachineBehaviour
{
    private FoamDataManager m_data;
    private GameObject m_activeHand;
    private GameObject m_activeIndex;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();
        m_activeHand = GameObject.FindGameObjectWithTag("hand_r"); // right hand only right now
        m_activeIndex = m_activeHand.transform.GetChild(1).GetChild(2).gameObject;

        m_data.m_createMenu.transform.position = m_activeIndex.transform.position;
        m_data.m_createMenu.SetActive(true);
        Debug.Log("Creation Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
