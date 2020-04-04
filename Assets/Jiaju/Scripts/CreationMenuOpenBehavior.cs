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
    private int m_hash_itemSelectedBool = Animator.StringToHash("ItemSelectedBool");
    private CreateMenuItem m_selectedItem = CreateMenuItem.NULL;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        m_data.CreateMenu.transform.position = m_data.ActiveIndex.transform.position;
        m_data.CreateMenu.SetActive(true);

        m_data.ManiMenu.SetActive(false);

        m_palmPos_init = m_data.ActivePalm.transform.position;
        m_bound_UppL = m_palmPos_init + new Vector3(-m_data.TriggerRadius, m_data.TriggerRadius);
        m_bound_UppR = m_palmPos_init + new Vector3(m_data.TriggerRadius, m_data.TriggerRadius);

        m_bound_LowL = m_palmPos_init + new Vector3(-m_data.TriggerRadius, -m_data.TriggerRadius);
        m_bound_LowR = m_palmPos_init + new Vector3(m_data.TriggerRadius, -m_data.TriggerRadius);

        animator.SetBool(m_hash_itemSelectedBool, false);
        Debug.Log("FOAMFILTER Creation Menu Open State entered");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 palmPos_curr = m_data.ActivePalm.transform.position;

        MenuRegion region = FoamUtils.checkMenuRegion(palmPos_curr, m_palmPos_init, m_bound_UppL, m_bound_UppR, m_bound_LowL, m_bound_LowR, m_data.MiddleRadius);

        switch (region)
        {
            case MenuRegion.UPPER:
                //Debug.Log("FOAMFILTER Create Upp");
                //m_data.CubeRenderer.color = m_data.HoverColor;

                //m_data.CylinderRenderer.color = m_data.NormalColor;
                //m_data.ConeRenderer.color = m_data.NormalColor;
                //m_data.SphereRenderer.color = m_data.NormalColor;

                HighlightSprite("cube");

                m_selectedItem = CreateMenuItem.CUBE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;

            case MenuRegion.RIGHT:
                //Debug.Log("FOAMFILTER Create Right");
                //m_data.CubeRenderer.color = m_data.NormalColor;

                //m_data.CylinderRenderer.color = m_data.HoverColor;

                //m_data.ConeRenderer.color = m_data.NormalColor;
                //m_data.SphereRenderer.color = m_data.NormalColor;
                HighlightSprite("cylinder");

                m_selectedItem = CreateMenuItem.CYLINDER;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;

            case MenuRegion.LOWER:
                //Debug.Log("FOAMFILTER Create Lower");
                //m_data.CubeRenderer.color = m_data.NormalColor;
                //m_data.CylinderRenderer.color = m_data.NormalColor;

                //m_data.ConeRenderer.color = m_data.HoverColor;

                //m_data.SphereRenderer.color = m_data.NormalColor;

                HighlightSprite("cone");

                m_selectedItem = CreateMenuItem.CONE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;

            case MenuRegion.LEFT:
                //Debug.Log("FOAMFILTER Create Left");
                //m_data.CubeRenderer.color = m_data.NormalColor;
                //m_data.CylinderRenderer.color = m_data.NormalColor;
                //m_data.ConeRenderer.color = m_data.NormalColor;

                //m_data.SphereRenderer.color = m_data.HoverColor;

                HighlightSprite("sphere");

                m_selectedItem = CreateMenuItem.SPHERE;
                animator.SetBool(m_hash_itemSelectedBool, true);
                break;

            case MenuRegion.MIDDLE:
                //m_data.CubeRenderer.color = m_data.NormalColor;
                //m_data.CylinderRenderer.color = m_data.NormalColor;
                //m_data.ConeRenderer.color = m_data.NormalColor;
                //m_data.SphereRenderer.color = m_data.NormalColor;

                HighlightSprite("middle");

                m_selectedItem = CreateMenuItem.NULL;            //test
                animator.SetBool(m_hash_itemSelectedBool, false); //test
                break;
        }

        m_data.Selected_createItem = m_selectedItem;
        //Debug.Log("FOAMFILTER selected item in open: " + m_data.Selected_createItem);
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

    private void HighlightSprite(string geoType)
    {
        switch(geoType)
        {
            case "cube":
                m_data.CubeRenderer.sprite = m_data.CubeHighlightSprite;

                m_data.CylinderRenderer.sprite = m_data.CylinderNormalSprite;
                m_data.ConeRenderer.sprite = m_data.ConeNormalSprite;
                m_data.SphereRenderer.sprite = m_data.SphereNormalSprite;

                break;

            case "cylinder":
                m_data.CylinderRenderer.sprite = m_data.CylinderHighlightSprite;

                m_data.CubeRenderer.sprite = m_data.CubeNormalSprite;
                m_data.ConeRenderer.sprite = m_data.ConeNormalSprite;
                m_data.SphereRenderer.sprite = m_data.SphereNormalSprite;
                break;

            case "cone":
                m_data.ConeRenderer.sprite = m_data.ConeHighlightSprite;

                m_data.CylinderRenderer.sprite = m_data.CylinderNormalSprite;
                m_data.CubeRenderer.sprite = m_data.CubeNormalSprite;
                m_data.SphereRenderer.sprite = m_data.SphereNormalSprite;
                break;

            case "sphere":
                m_data.SphereRenderer.sprite = m_data.SphereHighlightSprite;

                m_data.CubeRenderer.sprite = m_data.CubeNormalSprite;
                m_data.ConeRenderer.sprite = m_data.ConeNormalSprite;
                m_data.CylinderRenderer.sprite = m_data.CylinderNormalSprite;
                break;

            case "middle":
                m_data.CubeRenderer.sprite = m_data.CubeNormalSprite;
                m_data.ConeRenderer.sprite = m_data.ConeNormalSprite;
                m_data.CylinderRenderer.sprite = m_data.CylinderNormalSprite;
                m_data.SphereRenderer.sprite = m_data.SphereNormalSprite;
                break;
                
        }
    }
}
