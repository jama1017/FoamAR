﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Portalble.Functions.Grab;

public class ManipulationStateBehavior : StateMachineBehaviour
{
    private FoamDataManager _data;
    private int _hash_dwellBool = Animator.StringToHash("DwellBool");
    private int _hash_objMenuClosedBool = Animator.StringToHash("ObjMenuClosedBool");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        _data.ManiMenu.SetActive(false);
        //_data.ObjMenu.SetActive(false);

        animator.SetBool(_hash_objMenuClosedBool, false);

        FoamUtils.IsGlobalGrabbing = true;

        _data.StateIndicator.GetComponent<Text>().text = "Edit";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_data.CurrentSelectionObj)
        {
            if (_data.CurrentSelectionObj.GetComponent<Modelable>().IsHandDwell())
            {
                animator.SetBool(_hash_dwellBool, true);
                //animator.SetTrigger("DwellTrigger");

            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FoamUtils.IsGlobalGrabbing = false;
        if(_data.CurrentSelectionObj)
        {
            Portalble.Functions.Grab.GrabCollider curGC = _data.CurrentSelectionObj.transform.GetChild(0).GetComponent<Portalble.Functions.Grab.GrabCollider>();
            if (curGC)
            {
                curGC.DeGrab();
            }
        }
    }

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
