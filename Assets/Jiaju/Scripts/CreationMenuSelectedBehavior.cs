using System.Collections;
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

    private Renderer _primRenderer;
    private Color _primOGColor;

    private float offset = 0.13f;

    private int _animCount = 0;
    private int _animTime = 50;
    private int _transStep = 0;
    private Vector3 _initalScale;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        m_data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();

        Debug.Log("FOAMFILTER MENU ITEM SELECTED IS: " + m_data.Selected_createItem);

        m_prim = null;
        _animCount = 0;
        _transStep = 0;

        Vector3 prim_pos = m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward;

        switch (m_data.Selected_createItem)
        {
            case CreateMenuItem.CUBE:
                //Debug.Log("FOAMFILTER CUBE ITEM CREATED");
                m_prim = Instantiate(m_data.CubePrefab, prim_pos, Quaternion.identity);
                break;

            case CreateMenuItem.SPHERE:
                //Debug.Log("FOAMFILTER SPHERE ITEM CREATED");
                m_prim = Instantiate(m_data.SpherePrefab, prim_pos, Quaternion.identity);
                break;

            case CreateMenuItem.CYLINDER:
                //Debug.Log("FOAMFILTER CYLINDER ITEM CREATED");
                m_prim = Instantiate(m_data.CylinderPrefab, prim_pos, Quaternion.identity);
                break;

            case CreateMenuItem.CONE:
                //Debug.Log("FOAMFILTER CONE ITEM CREATED");
                m_prim = Instantiate(m_data.ConePrefab, prim_pos, Quaternion.identity);
                break;

            default:
                break;
        }

        if (m_prim)
        {
            m_prim.gameObject.GetComponent<Grabable>().enabled = false; // grabbing?
            m_prim_child = m_prim.GetChild(0);
            m_prim_child.gameObject.SetActive(false); // is it for grabbing??

            _primRenderer = m_prim.gameObject.GetComponent<Renderer>();
            _primOGColor = _primRenderer.material.color;

            _initalScale = m_prim.transform.localScale;
            m_prim.transform.localScale = Vector3.zero;
            //Debug.Log("MODELABLE obj count: " + m_data.SceneObjs.Count);
        }

        m_isReleased = false;
        m_data.CreateMenu.SetActive(false);

    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

        // change transparency
        if (m_prim && !m_isReleased)
        {
            Color curC = _primRenderer.material.color;
            float newA = FoamUtils.SinWave(_transStep);
            _primRenderer.material.color = new Color(curC.r, curC.g, curC.b, newA);
            _transStep++;
        }

        // play scale animation first
        if (_animCount < _animTime)
        {
            float newX = FoamUtils.LinearMap(_animCount, 0, _animTime, 0.0f, _initalScale.x);
            float newY = FoamUtils.LinearMap(_animCount, 0, _animTime, 0.0f, _initalScale.y);
            float newZ = FoamUtils.LinearMap(_animCount, 0, _animTime, 0.0f, _initalScale.z);

            m_prim.localScale = new Vector3(newX, newY, newZ);
            m_prim.position = m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward;
            _animCount++;
            return;
        }

        if (m_prim)
        {
            if (!m_isReleased)
            {
                m_prim.position = m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward;
            }

            //if (m_data.ActiveGC.bufferedGesture() == "pinch" || Input.GetKey(KeyCode.DownArrow))
            if (m_data.ActiveGC.bufferedGesture() == "pinch")
            {
                Debug.Log("FOAMFILTER ITEM PLACED: " + m_prim.gameObject.name);
                m_isReleased = true;

                if (m_prim.gameObject.name == "FoamCone(Clone)")
                {
                    GameObject.Destroy(m_prim.gameObject);
                    m_prim = Instantiate(m_data.ConePrefab, m_data.ActivePalm.transform.position + offset * m_data.ActivePalm.transform.forward, Quaternion.identity);
                    Debug.Log("FOAMFILTER cone recreated: ");
                }

                _primRenderer.material.color = _primOGColor;

                m_prim.gameObject.GetComponent<Grabable>().enabled = true; // grabbing
                m_prim_child.gameObject.SetActive(true); // grabbing

                m_data.SceneObjs.Add(m_prim.gameObject);
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
