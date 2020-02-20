﻿using System.Collections;
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

    //public string websocketServer = "172.18.136.107"; //lab computer Brown Guest
    public string websocketServer = "10.1.76.168"; // surface book RISD Misc
    //public string websocketServer = "10.1.77.55";
    public string websocketPort = "8765";

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

        setupServer(); 
    }

    private void setupServer()
    {
        // Create web socket
        Debug.Log("Connecting" + websocketServer);
        string url = "ws://" + websocketServer + ":" + websocketPort;
        //webSocket = new WebSocketUnity(url, this);

        // Open the connection
        // webSocket.Open();
        Jetfire.Open2(url);
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();

        recordGrabLoc();

        //if (Jetfire.IsConnected2())
        //{
        //    Jetfire.SendMsg2("hahahah");
        //    Debug.Log("JETFIRE HAHA");
        //}

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

                if (Jetfire.IsConnected2())
                {
                    Jetfire.SendMsg2("grabbed something");
                    Debug.Log("JETFIRE HAHA");
                }
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
