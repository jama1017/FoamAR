using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble;
using Portalble.Functions.Grab;


public class FoamARController : PortalbleGeneralController
{
	public Transform placePrefab;
	public float offset = 0.01f;
	public GameObject JUIController;
    public GameObject m_leftHand;
    public GameObject m_rightHand;
    public GameObject m_createMenu;
    public GameObject m_maniMenu;

    private GestureControl m_leftGC;
    private GestureControl m_rightGC;
    private JUIController m_jui;
    private GameObject m_activeHand;
    private GestureControl m_activeGC;


    //state machine
    private Animator m_stateMachine;

    private int m_hash_createBool = Animator.StringToHash("CreateStateBool");
    private int m_hash_maniBool = Animator.StringToHash("ManipulationStateBool");
    private int m_hash_idleBool = Animator.StringToHash("IdleStateBool");
    private int m_hash_pinchBool = Animator.StringToHash("PinchBool");
    private List<int> m_uiState_hashes = new List<int>();


    public override void OnARPlaneHit(PortalbleHitResult hit)
	{
		base.OnARPlaneHit(hit);

		if (placePrefab != null)
		{
			Instantiate(m_createMenu.transform, hit.Pose.position + hit.Pose.rotation * Vector3.up * offset, hit.Pose.rotation);
            Debug.Log("FOAMFILTER hit pos" + hit.Pose.position + hit.Pose.rotation * Vector3.up * offset);
            Debug.Log("FOAMFILTER hit rotation" + hit.Pose.rotation);
		}
	}

    protected override void Start()
    {
        base.Start();
        m_leftGC = m_leftHand.GetComponent<GestureControl>();
        m_rightGC = m_rightHand.GetComponent<GestureControl>();
        m_jui = JUIController.GetComponent<JUIController>();

        m_activeGC = m_rightGC;
        m_activeHand = m_rightHand;

        //state machine
        m_stateMachine = this.GetComponent<Animator>();
        m_uiState_hashes.Add(m_hash_idleBool);
        m_uiState_hashes.Add(m_hash_createBool);
        m_uiState_hashes.Add(m_hash_maniBool);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        m_maniMenu.transform.LookAt(Camera.main.transform);
        m_createMenu.transform.LookAt(Camera.main.transform);

        switch (m_jui.FoamState)
        {
            case FoamState.STATE_CREATE:
                this.handleCreate();
                break;

            case FoamState.STATE_MANIPULATE:
                this.handleManipulate();
                break;

            case FoamState.STATE_IDLE:
                this.handleIdle();
                break;

            default:
                break;
        }

        togglePinchBool();

        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    m_stateMachine.SetBool(m_hash_pinchBool, true);
        //}

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    m_stateMachine.SetBool(m_hash_pinchBool, false);
        //}


    }

    private void handleIdle()
    {
        switchStateBool(m_hash_idleBool);

    }

    private void handleCreate()
    {
        switchStateBool(m_hash_createBool);
    }

    private void handleManipulate()
    {
        switchStateBool(m_hash_maniBool);
    }

    //private void toggleActiveMenu()
    //{
    //    if (!Grab.Instance.IsGrabbing)
    //    {
    //        if (m_activeGC.bufferedGesture() == "pinch")
    //        {
    //            if (!m_isMenuShown)
    //            {
    //                m_isMenuShown = true;
    //                m_activeMenu.transform.position = m_activeIndex.transform.position;
    //                m_activeMenu.SetActive(m_isMenuShown);
    //            }
    //        }
    //        else
    //        {
    //            if (m_isMenuShown)
    //            {
    //                m_isMenuShown = false;
    //                m_activeMenu.SetActive(m_isMenuShown);
    //            }
    //        }
    //    }
    //}

    private void switchStateBool(int targetState)
    {
        for (int i = 0; i < m_uiState_hashes.Count; i++)
        {
            if (m_uiState_hashes[i] == targetState)
            {
                m_stateMachine.SetBool(m_uiState_hashes[i], true);

            } else
            {
                m_stateMachine.SetBool(m_uiState_hashes[i], false);

            }
        }
    }

    private void togglePinchBool()
    {
        if (!Grab.Instance.IsGrabbing)
        {
            if (m_activeGC.bufferedGesture() == "pinch")
            {
                m_stateMachine.SetBool(m_hash_pinchBool, true);
                Debug.Log("FOAMFILTER pinch bool is true");
            } else
            {
                m_stateMachine.SetBool(m_hash_pinchBool, false);
                Debug.Log("FOAMFILTER pinch bool is false");
            }
        }
    }
}
