using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationMenuOpenBehavior : StateMachineBehaviour
{
    private FoamDataManager _data;
    private Vector3 m_palmPos_init;
    private Vector3 m_bound_UppL;
    private Vector3 m_bound_UppR;
    private Vector3 m_bound_LowL;
    private Vector3 m_bound_LowR;

    //private int _hash_itemSelectedBool = Animator.StringToHash("ItemSelectedBool");
    private int _hash_toolSelectedInt = Animator.StringToHash("ToolSelectedInt");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        _data.ManiMenu.transform.position = _data.ActiveIndex.transform.position;
        _data.ManiMenu.SetActive(true);
        _data.CreateMenu.SetActive(false);

        m_palmPos_init = _data.ActivePalm.transform.position;
        m_bound_UppL = m_palmPos_init + new Vector3(-_data.TriggerRadius, _data.TriggerRadius);
        m_bound_UppR = m_palmPos_init + new Vector3(_data.TriggerRadius, _data.TriggerRadius);

        m_bound_LowL = m_palmPos_init + new Vector3(-_data.TriggerRadius, -_data.TriggerRadius);
        m_bound_LowR = m_palmPos_init + new Vector3(_data.TriggerRadius, -_data.TriggerRadius);

        //animator.SetBool(_hash_itemSelectedBool, false);
        Debug.Log("FOAMFILTER Mani Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 palmPos_curr = _data.ActivePalm.transform.position;

        MenuRegion region = FoamUtils.checkMenuRegion(palmPos_curr, m_palmPos_init, m_bound_UppL, m_bound_UppR, m_bound_LowL, m_bound_LowR, _data.MiddleRadius);

        switch (region)
        {
            case MenuRegion.UPPER:
                Debug.Log("-----------Mani Upp");

                _data.ManiMenuParent.HighlightSprite(0);

                animator.SetInteger(_hash_toolSelectedInt, 0);

                break;

            case MenuRegion.RIGHT:
                //Debug.Log("-----------Mani RIGHT");

                _data.ManiMenuParent.HighlightSprite(1);

                animator.SetInteger(_hash_toolSelectedInt, 1);

                break;

            case MenuRegion.LOWER:
                //Debug.Log("-----------Mani LOWER");

                _data.ManiMenuParent.HighlightSprite(2);

                break;

            case MenuRegion.LEFT:
                //Debug.Log("-----------Mani LEFT");

                _data.ManiMenuParent.HighlightSprite(3);

                break;

            case MenuRegion.MIDDLE:
                //Debug.Log("-----------Mani MIDDLE");

                _data.ManiMenuParent.HighlightSprite(-1);

                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _data.ManiMenu.SetActive(false);
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
