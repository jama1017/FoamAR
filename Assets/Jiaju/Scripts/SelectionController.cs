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
    public GameObject prefab_marker;

    private GestureControl m_leftGC;
    private GestureControl m_rightGC;
    private GameObject m_activeHand;
    private GestureControl m_activeGC;

    // selection specific
    private bool m_isRecorded = false;
    private List<GameObject> m_markers = new List<GameObject>();
    private List<Vector3> m_markers_screenPos = new List<Vector3>();
    private bool m_isMarkerDisplayed = false;
    public SelectionDataManager m_selectionDM;

    // ------ IPs----------
    //public string websocketServer = "172.18.136.107"; //lab computer Brown Guest
    public GameObject WSManager;
    //public string websocketServer = "10.1.77.55";
    public string websocketPort = "8765";

    // selection cylinder
    //private Transform m_cubeTest;
    public Transform m_focusCylinderPrefab;
    private Transform m_focusCylinder;
    private float m_v = 0f;
    private float m_h = 0f;

    public override void OnARPlaneHit(PortalbleHitResult hit)
    {
        base.OnARPlaneHit(hit);

        if (placePrefab != null)
        {
            Transform cen = Instantiate(placePrefab, hit.Pose.position + hit.Pose.rotation * Vector3.up * offset, hit.Pose.rotation);

            // for focus center test purposes
            int num = 2;
            float offset_test = 0.2f;
            for (int i = 1; i < num + 1; i++)
            {
                Instantiate(placePrefab, cen.position - cen.right * offset_test * i, cen.rotation);
                Instantiate(placePrefab, cen.position + cen.right * offset_test * i, cen.rotation);
            }
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

        //test. to be removed
        //m_cubeTest = Instantiate(placePrefab, m_FirstPersonCamera.transform.position + 0.2f * m_FirstPersonCamera.transform.forward, Quaternion.identity);
        //m_cubeTest.gameObject.GetComponent<Collider>().attachedRigidbody.useGravity = false;

        m_focusCylinder = Instantiate(m_focusCylinderPrefab, m_FirstPersonCamera.transform.position + 0.2f * m_FirstPersonCamera.transform.forward, Quaternion.identity);
        m_focusCylinder.gameObject.GetComponent<Collider>().attachedRigidbody.useGravity = false;
        
    }

    private void setupServer()
    {
        // Create web socket
      
        Debug.Log("Connecting" + WSManager.GetComponent<WSManager>().websocketServer);
        string url = "ws://" + WSManager.GetComponent<WSManager>().websocketServer + ":" + websocketPort;
        //webSocket = new WebSocketUnity(url, this);

        // Open the connection
        //webSocket.Open();
        Jetfire.Open2(url);
    }

    private void placementTest()
    {
        //Debug.Log("SELCAM: " + m_FirstPersonCamera.transform.position);
        //m_cubeTest.LookAt(m_FirstPersonCamera.transform);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 touchPos = Input.mousePosition;
        //    Debug.Log(touchPos);
        //    Debug.Log(Camera.main.ScreenToWorldPoint(Vector3.zero));
        //    //Debug.Log(Camera.main.pixelHeight); // screen height in px
        //    //Debug.Log(Camera.main.pixelWidth);  // screen width in px
        //}

       
        //if (Input.touchCount > 0)
        //{
        //    Vector3 touchPos = Input.GetTouch(0).position;
        //    Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0); // clean up

        //    Vector2 distance_vec = touchPos - center;

        //    m_v = distance_vec.y / 10000f;
        //    m_h = distance_vec.x / 10000f;

        //    Debug.Log("SEM: m_v:" + m_v);
        //    Debug.Log("SEM: m_h:" + m_h);

        //    //Debug.Log("SEM touchPos: " + touchPos);
        //    //Debug.Log("SEM touchPos world: " + Camera.main.ScreenToWorldPoint(touchPos));
        //    //Debug.Log("SEM touchPos zero: " + Camera.main.ScreenToWorldPoint(center));

        //    //Debug.Log("SEM touchPos distance: " + Vector3.Distance(Camera.main.ScreenToWorldPoint(touchPos), Camera.main.ScreenToWorldPoint(center)).ToString("F3"));
        //    //Debug.Log(Camera.main.pixelHeight); // screen height in px
        //    //Debug.Log(Camera.main.pixelWidth);  // screen width in px
        //}

        if (m_markers_screenPos.Count > 0) {
            Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            Vector3 focusPos = m_markers_screenPos[m_markers_screenPos.Count - 1];

            Vector2 distance_vec = focusPos - center;

            m_v = distance_vec.y / 10000f;
            m_h = distance_vec.x / 10000f;

            Debug.Log("SEM m_v: " + m_v);
            Debug.Log("SEM m_h: " + m_h);
        }

        //Vector3 cameraOffset = screenToWorldSpace(m_canvas, m_touchPosTest);

        float fac = m_focusCylinderPrefab.gameObject.GetComponent<Renderer>().bounds.size[1];

        m_focusCylinder.position = m_FirstPersonCamera.transform.position + fac * m_FirstPersonCamera.transform.forward + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;
        m_focusCylinder.LookAt(m_focusCylinder.position - (m_FirstPersonCamera.transform.position - m_focusCylinder.position));
        m_focusCylinder.Rotate(90.0f, 0.0f, 0.0f, Space.Self);

        //setting scale
        float initial_width = Camera.main.pixelWidth / (2 * 10000f); //change back to 2
        m_focusCylinder.localScale = new Vector3(initial_width, m_focusCylinder.localScale[1], initial_width);

        //Vector2 newPos = worldToUISpace(m_canvas, m_FirstPersonCamera.transform.position);
        //Debug.Log("SELCAM: " + newPos);
        //GameObject new_marker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);
        //new_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
        ////m_markers.Add(new_marker);
        ///
        Debug.Log("FOCUSED num: " + m_selectionDM.FocusedObjects.Count);

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        recordGrabLoc();

        placementTest();

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
        //    prefab_marker.GetComponent<RectTransform>().position = movePos;
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
                //Debug.Log("FOCUS grabbing");
                m_isRecorded = true;
                Vector3 grabPos = Grab.Instance.GetGrabbingObject().gameObject.transform.position;
                //Debug.Log("FOCUS WORLD POSITION" + grabPos);
                //Debug.Log("FOCUS CAMERA " + Camera.main.WorldToScreenPoint(grabPos));
                //Debug.Log("FOCUS CANVAS " + worldToUISpace(m_canvas, grabPos));

                Vector2 newPos = worldToUISpace(m_canvas, grabPos);
                GameObject new_marker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);
                new_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
                m_markers.Add(new_marker);
                m_markers_screenPos.Add(worldToScreenSpace(grabPos));
                new_marker.SetActive(m_isMarkerDisplayed);
                
                if (Jetfire.IsConnected2())
                {
                    string message = "grabbed at," + newPos + "," + worldToScreenSpace(grabPos);
                    Jetfire.SendMsg2(message);
                    Debug.Log("JETFIRE HAHA");
                }
            }
        }

        if (!Grab.Instance.IsGrabbing)
        {
            m_isRecorded = false;
        }
    }


    private Vector3 worldToScreenSpace(Vector3 worldPos)
    {
        return Camera.main.WorldToScreenPoint(worldPos);
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

    //private Vector3 screenToWorldSpace(Canvas parentCanvas, Vector2 screenPos)
    //{
    //    Vector2 movePos;

    //    //Convert the screenpoint to ui rectangle local point
    //    //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, null, out movePos);

    //    //Convert the local points to world point
    //    //return parentCanvas.transform.TransformPoint(movePos);
    //    return movePos;
    //}

    public void toggleMarkerVisibility()
    {
        m_isMarkerDisplayed = !m_isMarkerDisplayed;
        for (int i = 0; i < m_markers.Count; i++)
        {
            m_markers[i].SetActive(m_isMarkerDisplayed);
        }
    }

    public float UnitsPerPixel()
    {
        var p1 = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var p2 = Camera.main.ScreenToWorldPoint(Vector3.right);
        return Vector3.Distance(p1, p2);
    }

    public float PixelsPerUnit()
    {
        return 1 / UnitsPerPixel();
    }
}
