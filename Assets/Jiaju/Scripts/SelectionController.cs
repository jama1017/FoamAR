using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble;
using Portalble.Functions.Grab;


public class SelectionController : PortalbleGeneralController
{
    public Transform placePrefab;
    public float offset = 0.01f;

    public GameObject m_leftHand;
    public GameObject m_rightHand;

    private GestureControl m_leftGC;
    private GestureControl m_rightGC;
    private GameObject m_activeHand;
    private GestureControl m_activeGC;

    private bool m_isRecorded = false;

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
        m_leftGC = m_leftHand.GetComponent<GestureControl>();
        m_rightGC = m_rightHand.GetComponent<GestureControl>();

        // to update these
        m_activeGC = m_rightGC;
        m_activeHand = m_rightHand;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        recordGrabLoc();

        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    Debug.Log("Down");
        //}

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    Debug.Log("Up");
        //}
    }

    private void recordGrabLoc()
    {
        if (m_activeGC.bufferedGesture() == "pinch")
        {
            if (Grab.Instance.IsGrabbing && !m_isRecorded)
            {
                Debug.Log("FOCUS grabbing");
                m_isRecorded = true;
            }
        }

        if (!Grab.Instance.IsGrabbing)
        {
            m_isRecorded = false;
        }
    }
}
