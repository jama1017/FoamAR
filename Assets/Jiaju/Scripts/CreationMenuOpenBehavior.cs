using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationMenuOpenBehavior : StateMachineBehaviour
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

        m_data.CreateMenu.transform.position = m_data.ActiveIndex.transform.position;
        m_data.CreateMenu.SetActive(true);

        m_data.ManiMenu.SetActive(false);

        m_palmPos_init = m_data.ActivePalm.transform.position;
        m_bound_UppL = m_palmPos_init - new Vector3(m_data.TriggerRadius, -m_data.TriggerRadius);
        m_bound_UppR = m_palmPos_init - new Vector3(-m_data.TriggerRadius, -m_data.TriggerRadius);

        m_bound_LowL = m_palmPos_init - new Vector3(m_data.TriggerRadius, m_data.TriggerRadius);
        m_bound_LowR = m_palmPos_init - new Vector3(-m_data.TriggerRadius, m_data.TriggerRadius);
        Debug.Log("FOAMFILTER Creation Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 palmPos_curr = m_data.ActivePalm.transform.position;

        // upper tri
        if (isInsideTri(palmPos_curr, m_bound_UppL, m_bound_UppR, m_palmPos_init)) {
            Debug.Log("FOAMFILTER INSIDE UPPER TRI");

        // right tri
        } else if (isInsideTri(palmPos_curr, m_bound_UppR, m_bound_LowR, m_palmPos_init))
        {
            Debug.Log("FOAMFILTER INSIDE RIGHT TRI");

        // lower tri
        } else if (isInsideTri(palmPos_curr, m_bound_LowR, m_bound_LowL, m_palmPos_init))
        {
            Debug.Log("FOAMFILTER INSIDE LOWER TRI");

        // left tri
        } else
        {
            Debug.Log("FOAMFILTER INSIDE LEFT TRI");
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

    private bool isInsideTri(Vector3 s, Vector3 a, Vector3 b, Vector3 c)
    {
        float as_x = s.x - a.x;
        float as_y = s.y - a.y;

        bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

        if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab) return false;
        if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab) return false;

        return true;
    }
}
