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
    public Canvas m_canvas;
    public GameObject m_marker;

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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 touchPos = Input.mousePosition;
        //    Vector2 movePos;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, touchPos, null, out movePos);
        //    Debug.Log(movePos);
        //    m_marker.GetComponent<RectTransform>().position = movePos;
        //}

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
                Vector3 grabPos = Grab.Instance.GetGrabbingObject().gameObject.transform.position;
                Debug.Log("FOCUS WORLD POSITION" + grabPos);
                Debug.Log("FOCUS CAMERA " + Camera.main.WorldToScreenPoint(grabPos));
                Debug.Log("FOCUS CANVAS " + worldToUISpace(m_canvas, grabPos));

                Vector2 newPos = worldToUISpace(m_canvas, grabPos);
                m_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
            }
        }

        if (!Grab.Instance.IsGrabbing)
        {
            m_isRecorded = false;
        }
    }

    private Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, null, out movePos);

        //Convert the local point to world point
        //return parentCanvas.transform.TransformPoint(movePos);
        return movePos;
    }
}
