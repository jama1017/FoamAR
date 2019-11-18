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

    private FoamState m_prevFoamState = FoamState.STATE_IDLE;
    private GestureControl m_leftGC;
    private GestureControl m_rightGC;
    private JUIController m_jui;
    private GameObject m_activeHand;
    private GestureControl m_activeGC;
    private bool m_isPrimCreated = false;
    private bool m_isMenuShown = false;

	public override void OnARPlaneHit(PortalbleHitResult hit)
	{
		base.OnARPlaneHit(hit);

		if (placePrefab != null)
		{
			Instantiate(placePrefab, hit.Pose.position + hit.Pose.rotation * Vector3.up * offset, hit.Pose.rotation);
		}
	}

    protected override void Start()
    {
        base.Start();
        Debug.Log("initializing Foam AR Controlelr.cs");

        m_leftGC = m_leftHand.GetComponent<GestureControl>();
        m_rightGC = m_rightHand.GetComponent<GestureControl>();
        m_jui = JUIController.GetComponent<JUIController>();

        m_activeGC = m_rightGC;
        m_activeHand = m_rightHand;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("________ my ARController");

        switch(m_jui.FoamState)
        {
            case FoamState.STATE_CREATE:
                this.handleCreate();
                break;

            case FoamState.STATE_MANIPULATE:
                break;

            case FoamState.STATE_IDLE:
                break;

            default:
                break;
        }
    }

    private void handleCreate()
    {
        Debug.Log("CREATING");
        if(!Grab.Instance.IsGrabbing)
        {
            if (m_activeGC.bufferedGesture() == "pinch")
            {
                if (!m_isMenuShown)
                {

                }
            }
                Debug.Log("Foam Triggering Creating Menu");
        }
    }
}