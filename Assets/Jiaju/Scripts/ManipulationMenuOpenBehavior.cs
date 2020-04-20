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
    private int m_hash_itemSelectedBool = Animator.StringToHash("ItemSelectedBool");

    private FoamRadialManager _currSelectedOption = null; // for highlighting purpose only

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

        animator.SetBool(m_hash_itemSelectedBool, false);
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

                HighlightSprite(ManiMenuItem.MOVE);

                break;

            case MenuRegion.RIGHT:
                //Debug.Log("-----------Mani RIGHT");
                HighlightSprite(ManiMenuItem.SCALE);

                break;

            case MenuRegion.LOWER:
                //Debug.Log("-----------Mani LOWER");
                HighlightSprite(ManiMenuItem.ONE);

                break;

            case MenuRegion.LEFT:
                //Debug.Log("-----------Mani LEFT");
                HighlightSprite(ManiMenuItem.TWO);

                break;

            case MenuRegion.MIDDLE:
                //Debug.Log("-----------Mani MIDDLE");
                HighlightSprite(ManiMenuItem.NULL);

                break;
        }
    }

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

    private void HighlightSprite(ManiMenuItem toolType)
    {
        int toolNum = (int)toolType;

        if (_currSelectedOption)
        {
            _currSelectedOption.DeHighlightIcon();
        }

        if (toolNum == (int)ManiMenuItem.NULL)
        {
            _data.ManiCenterRenderer.GetComponent<FoamRadialCenterManager>().DeHighlightCenter();
            return;
        }
        
        _currSelectedOption = _data.ManipulateRenderers[toolNum].GetComponent<FoamRadialManager>();
        _currSelectedOption.HightlightIcon();
    }
}
