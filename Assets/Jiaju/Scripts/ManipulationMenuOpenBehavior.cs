using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulationMenuOpenBehavior : StateMachineBehaviour
{
    private FoamDataManager m_data;
    private Vector3 m_palmPos_init;
    private Vector3 m_bound_UppL;
    private Vector3 m_bound_UppR;
    private Vector3 m_bound_LowL;
    private Vector3 m_bound_LowR;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        m_data.ManiMenu.transform.position = m_data.ActiveIndex.transform.position;
        m_data.ManiMenu.SetActive(true);

        m_data.CreateMenu.SetActive(false);

        m_palmPos_init = m_data.ActivePalm.transform.position;
        m_bound_UppL = m_palmPos_init + new Vector3(-m_data.TriggerRadius, m_data.TriggerRadius);
        m_bound_UppR = m_palmPos_init + new Vector3(m_data.TriggerRadius, m_data.TriggerRadius);

        m_bound_LowL = m_palmPos_init + new Vector3(-m_data.TriggerRadius, -m_data.TriggerRadius);
        m_bound_LowR = m_palmPos_init + new Vector3(m_data.TriggerRadius, -m_data.TriggerRadius);
        Debug.Log("FOAMFILTER Mani Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 palmPos_curr = m_data.ActivePalm.transform.position;

        MenuRegion region = FoamUtils.checkMenuRegion(palmPos_curr, m_palmPos_init, m_bound_UppL, m_bound_UppR, m_bound_LowL, m_bound_LowR, m_data.MiddleRadius);

        //switch (region)
        //{
        //    case MenuRegion.UPPER:
        //        Debug.Log("FOAMFILTER Mani Upp");
        //        m_data.ManiUppRenderer.color = m_data.HoverColor;

        //        m_data.ManiRightRenderer.color = m_data.NormalColor;
        //        m_data.ManiLowRenderer.color = m_data.NormalColor;
        //        m_data.ManiLeftRenderer.color = m_data.NormalColor;
        //        break;

        //    case MenuRegion.RIGHT:
        //        Debug.Log("FOAMFILTER Mani Right");
        //        m_data.ManiUppRenderer.color = m_data.NormalColor;

        //        m_data.ManiRightRenderer.color = m_data.HoverColor;

        //        m_data.ManiLowRenderer.color = m_data.NormalColor;
        //        m_data.ManiLeftRenderer.color = m_data.NormalColor;
        //        break;

        //    case MenuRegion.LOWER:
        //        Debug.Log("FOAMFILTER Mani Lower");
        //        m_data.ManiUppRenderer.color = m_data.NormalColor;
        //        m_data.ManiRightRenderer.color = m_data.NormalColor;

        //        m_data.ManiLowRenderer.color = m_data.HoverColor;

        //        m_data.ManiLeftRenderer.color = m_data.NormalColor;
        //        break;

        //    case MenuRegion.LEFT:
        //        Debug.Log("FOAMFILTER Mani Left");
        //        m_data.ManiUppRenderer.color = m_data.NormalColor;
        //        m_data.ManiRightRenderer.color = m_data.NormalColor;
        //        m_data.ManiLowRenderer.color = m_data.NormalColor;

        //        m_data.ManiLeftRenderer.color = m_data.HoverColor;
        //        break;

        //    case MenuRegion.MIDDLE:
        //        m_data.ManiUppRenderer.color = m_data.NormalColor;
        //        m_data.ManiRightRenderer.color = m_data.NormalColor;
        //        m_data.ManiLowRenderer.color = m_data.NormalColor;
        //        m_data.ManiLeftRenderer.color = m_data.NormalColor;
        //        break;
        //}
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
}
