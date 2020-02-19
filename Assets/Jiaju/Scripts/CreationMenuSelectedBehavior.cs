﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble.Functions.Grab;

public class CreationMenuSelectedBehavior : StateMachineBehaviour
{

    private FoamDataManager m_data;
    private int m_hash_actionBool = Animator.StringToHash("ActionPerformedBool");
    private int m_hash_itemSelectedBool = Animator.StringToHash("ItemSelectedBool");
    private Transform m_prim = null;
    private Transform m_prim_child = null;
    private bool m_isReleased =  false;

    private float offset = 0.2f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        m_data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        Debug.Log("FOAMFILTER MENU ITEM SELECTED IS: " + m_data.Selected_createItem);

        m_prim = null;

        switch (m_data.Selected_createItem)
        {
            case CreateMenuItem.CUBE:
                Debug.Log("FOAMFILTER CUBE ITEM CREATED");
                m_prim = Instantiate(m_data.CubePrefab, m_data.ActivePalm.transform.position, Quaternion.identity);
                break;

            case CreateMenuItem.SPHERE:
                Debug.Log("FOAMFILTER SPHERE ITEM CREATED");
                m_prim = Instantiate(m_data.SpherePrefab, m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward, Quaternion.identity);
                break;

            case CreateMenuItem.CYLINDER:
                Debug.Log("FOAMFILTER CYLINDER ITEM CREATED");
                m_prim = Instantiate(m_data.CylinderPrefab, m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward, Quaternion.identity);
                break;

            case CreateMenuItem.CONE:
                Debug.Log("FOAMFILTER CONE ITEM CREATED");
                m_prim = Instantiate(m_data.ConePrefab, m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward, Quaternion.identity);
                break;

            default:
                break;
        }

        if (m_prim)
        {
            m_prim.gameObject.GetComponent<Grabable>().enabled = false;
            m_prim_child = m_prim.GetChild(0);
            m_prim_child.gameObject.SetActive(false);
        }

        m_isReleased = false;
        m_data.CreateMenu.SetActive(false);

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        if (m_prim)
        {
            //Debug.Log("FOAMFILTER IN SELECTED STATE UPDATE");

            if (!m_isReleased)
            {
                Debug.Log("FOAMFILTER UPDATING ITEM");
                m_prim.position = m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward;
                m_prim.rotation = m_data.ActivePalm.transform.rotation;
            }


            if (m_data.ActiveGC.bufferedGesture() == "pinch" || Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("FOAMFILTER ITEM PLACED");
                m_isReleased = true;
                m_prim.gameObject.GetComponent<Grabable>().enabled = true;
                m_prim_child.gameObject.SetActive(true);

                animator.SetBool(m_hash_actionBool, true);
            }   
        }
        else
        {
            animator.SetBool(m_hash_actionBool, true);
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        Debug.Log("EXITING CREATION MENU SELECTED STATE");
        animator.SetBool(m_hash_itemSelectedBool, false);
        m_prim = null;
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