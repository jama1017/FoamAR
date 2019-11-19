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

    private FoamState m_prevFoamState = FoamState.STATE_IDLE;
    private GestureControl m_leftGC;
    private GestureControl m_rightGC;
    private JUIController m_jui;
    private GameObject m_activeHand;
    private GestureControl m_activeGC;
    private GameObject m_activeIndex;
    private GameObject m_activeMenu;

    private bool m_isPrimCreated = false;
    private bool m_isMenuShown = false;

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
        Debug.Log("initializing Foam AR Controlelr.cs");

        m_leftGC = m_leftHand.GetComponent<GestureControl>();
        m_rightGC = m_rightHand.GetComponent<GestureControl>();
        m_jui = JUIController.GetComponent<JUIController>();

        m_createMenu.SetActive(false);
        m_maniMenu.SetActive(false);

        m_activeGC = m_rightGC;
        m_activeHand = m_rightHand;
        m_activeIndex = m_activeHand.transform.GetChild(1).GetChild(2).gameObject;
        m_activeMenu = null;
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

        //test
        //if (!m_isMenuShown && m_activeMenu != null)
        //{
        //    m_activeMenu.transform.position = m_activeIndex.transform.position;
        //    //Debug.Log("FOAMFILTER index pos " + m_createMenu.transform.position);
        //    Debug.Log("FOAMFILTER hand pos" + m_activeIndex.transform.position);
        //}
    }

    private void handleIdle()
    {
        // need to handle idle state
        //m_activeMenu = null;
    }

    private void handleCreate()
    {
        Debug.Log("CREATING");

        m_activeMenu = m_createMenu;
        toggleActiveMenu();
    }

    private void handleManipulate()
    {
        m_activeMenu = m_maniMenu;
        toggleActiveMenu();
    }

    private void toggleActiveMenu()
    {
        if (!Grab.Instance.IsGrabbing)
        {
            if (m_activeGC.bufferedGesture() == "pinch")
            {
                if (!m_isMenuShown)
                {
                    m_isMenuShown = true;
                    m_activeMenu.transform.position = m_activeIndex.transform.position;
                    m_activeMenu.SetActive(m_isMenuShown);
                }
            }
            else
            {
                if (m_isMenuShown)
                {
                    m_isMenuShown = false;
                    m_activeMenu.SetActive(m_isMenuShown);
                }
            }
        }
    }
}
