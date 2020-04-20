using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationMenuOpenBehavior : StateMachineBehaviour
{
    private FoamDataManager _data;
    private Vector3 m_palmPos_init;
    private Vector3 m_bound_UppL;
    private Vector3 m_bound_UppR;
    private Vector3 m_bound_LowL;
    private Vector3 m_bound_LowR;
    private int m_hash_itemSelectedBool = Animator.StringToHash("ItemSelectedBool");
    private CreateMenuItem m_selectedItem = CreateMenuItem.NULL;

    private FoamRadialManager _currSelectedOption = null; // for highlighting purpose only


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        _data.CreateMenu.transform.position = _data.ActiveIndex.transform.position;
        _data.CreateMenu.SetActive(true);

        _data.ManiMenu.SetActive(false);

        m_palmPos_init = _data.ActivePalm.transform.position;
        m_bound_UppL = m_palmPos_init + new Vector3(-_data.TriggerRadius, _data.TriggerRadius);
        m_bound_UppR = m_palmPos_init + new Vector3(_data.TriggerRadius, _data.TriggerRadius);

        m_bound_LowL = m_palmPos_init + new Vector3(-_data.TriggerRadius, -_data.TriggerRadius);
        m_bound_LowR = m_palmPos_init + new Vector3(_data.TriggerRadius, -_data.TriggerRadius);

        animator.SetBool(m_hash_itemSelectedBool, false);
        //Debug.Log("FOAMFILTER Creation Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 palmPos_curr = _data.ActivePalm.transform.position;

        MenuRegion region = FoamUtils.checkMenuRegion(palmPos_curr, m_palmPos_init, m_bound_UppL, m_bound_UppR, m_bound_LowL, m_bound_LowR, _data.MiddleRadius);

        switch (region)
        {
            case MenuRegion.UPPER:
                //Debug.Log("FOAMFILTER Create Upp");

                HighlightSprite(CreateMenuItem.CUBE);

                m_selectedItem = CreateMenuItem.CUBE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;


            case MenuRegion.RIGHT:
                //Debug.Log("FOAMFILTER Create Right");

                HighlightSprite(CreateMenuItem.CYLINDER);

                m_selectedItem = CreateMenuItem.CYLINDER;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;


            case MenuRegion.LOWER:
                //Debug.Log("FOAMFILTER Create Lower");

                HighlightSprite(CreateMenuItem.CONE);

                m_selectedItem = CreateMenuItem.CONE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;


            case MenuRegion.LEFT:
                //Debug.Log("FOAMFILTER Create Left");

                HighlightSprite(CreateMenuItem.SPHERE);

                m_selectedItem = CreateMenuItem.SPHERE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;


            case MenuRegion.MIDDLE:

                HighlightSprite(CreateMenuItem.NULL);

                m_selectedItem = CreateMenuItem.NULL;            //test
                animator.SetBool(m_hash_itemSelectedBool, false); //test
                break;
        }

        _data.Selected_createItem = m_selectedItem;
        //Debug.Log("FOAMFILTER selected item in open: " + _data.Selected_createItem);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

    private void HighlightSprite(CreateMenuItem geoType)
    {
        int geoNum = (int) geoType;

        if (_currSelectedOption)
        {
            _currSelectedOption.DeHighlightIcon();
        }

        if (geoNum == (int)CreateMenuItem.NULL)
        {
            _data.CreationCenterRenderer.GetComponent<FoamRadialCenterManager>().DeHighlightCenter();
            return;
        }

        _currSelectedOption = _data.CreationRenderers[geoNum].GetComponent<FoamRadialManager>();
        _currSelectedOption.HightlightIcon();
    }
}
