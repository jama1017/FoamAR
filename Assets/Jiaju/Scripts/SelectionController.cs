using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble;
using Portalble.Functions.Grab;


public class SelectionController : PortalbleGeneralController
{
    public Transform placePrefab;
    public float offset = 0.01f;

    public Canvas m_canvas;
    public GameObject prefab_marker;

    // selection specific
    private bool m_isRecorded = false;
    private List<GameObject> m_markers = new List<GameObject>();
    private List<Vector3> m_markers_screenPos = new List<Vector3>();
    private bool m_isMarkerDisplayed = false;
    public SelectionDataManager m_selectionDM;
    private LineRenderer m_guideLine;

    // websocket
    public GameObject WSManager;
    public string websocketPort = "8765";

    // selection cylinder
    public Transform m_focusCylinderPrefab;
    private Transform m_focusCylinder;
    private Vector3 m_focusCylinderCenterPos = Vector3.zero;
    private float m_v = 0f;
    private float m_h = 0f;

    protected override void Start()
    {
        base.Start();
        SetupServer();

        //m_focusCylinder = Instantiate(m_focusCylinderPrefab, m_FirstPersonCamera.transform.position + 0.2f * m_FirstPersonCamera.transform.forward, Quaternion.identity);
        m_focusCylinder = Instantiate(m_focusCylinderPrefab);
        m_focusCylinder.gameObject.GetComponent<Collider>().attachedRigidbody.useGravity = false;
        m_guideLine = Instantiate(m_selectionDM.FocusInkPrefab).GetComponent<LineRenderer>();
        m_guideLine.positionCount = 2;
        m_guideLine.gameObject.SetActive(false);
    }

    private void SetupServer()
    {
        // Create web socket
        Debug.Log("Connecting" + WSManager.GetComponent<WSManager>().websocketServer);
        string url = "ws://" + WSManager.GetComponent<WSManager>().websocketServer + ":" + websocketPort;

        Jetfire.Open2(url);
    }

    private void UpdateFocusCylinder()
    {
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
            //Vector3 focusPos = m_markers_screenPos[m_markers_screenPos.Count - 1];
            Vector3 focusPos = FocusUtils.calcFocusCenter(m_markers_screenPos);

            Vector2 distance_vec = focusPos - center;

            m_v = distance_vec.y / 10000f;
            m_h = distance_vec.x / 10000f;

            Debug.Log("SEM m_v: " + m_v);
            Debug.Log("SEM m_h: " + m_h);
        }

        float fac = m_focusCylinder.transform.localScale[1] * 1.34f; //1.2f if y is 0.5. 1.31f is y is 0.3. 2.f is y is 0.1

        m_focusCylinder.position = m_FirstPersonCamera.transform.position + fac * m_FirstPersonCamera.transform.forward + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;
        m_focusCylinder.LookAt(m_focusCylinder.position - (m_FirstPersonCamera.transform.position - m_focusCylinder.position));
        m_focusCylinder.Rotate(90.0f, 0.0f, 0.0f, Space.Self);

        m_focusCylinderCenterPos = m_FirstPersonCamera.transform.position + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;


        //setting scale
        float initial_width = Camera.main.pixelWidth / (2 * 10000f);
        m_focusCylinder.localScale = new Vector3(initial_width, m_focusCylinder.localScale[1], initial_width);

        UpdateCylinderRadius(initial_width);
    }

    private void UpdateCylinderRadius(float maxWidth)
    {
        float dis = Vector3.Distance(m_selectionDM.ActiveIndex.transform.position, m_selectionDM.ActiveThumb.transform.position);
        if (dis > maxWidth) dis = maxWidth;
        m_focusCylinder.localScale = new Vector3(dis, m_focusCylinder.localScale[1], dis);

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        RecordGrabLoc();
        UpdateFocusCylinder();
        AidSelection();

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

    private void AidSelection()
    {
        GameObject num_one = FocusUtils.rankFocusedObjects(m_selectionDM.FocusedObjects, m_focusCylinderCenterPos);

        if (!num_one)
        {
            m_guideLine.gameObject.SetActive(false);
            return;
        }

        m_guideLine.gameObject.SetActive(true);
        //LineRenderer line = m_selectionDM.FocusedObjectToLine[num_one].GetComponent<LineRenderer>();

        FocusUtils.UpdateLinePos(m_guideLine, num_one.GetComponent<Collider>(), m_selectionDM.ActivePalm);
    }

    private void RecordGrabLoc()
    {
        if (m_selectionDM.ActiveGC.bufferedGesture() == "pinch")
        {
            if (Grab.Instance.IsGrabbing && !m_isRecorded)
            {
                //Debug.Log("FOCUS grabbing");
                m_isRecorded = true;
                Vector3 grabPos = Grab.Instance.GetGrabbingObject().gameObject.transform.position;

                Vector2 newPos = FocusUtils.worldToUISpace(m_canvas, grabPos);
                GameObject new_marker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);

                new_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
                m_markers.Add(new_marker);
                m_markers_screenPos.Add(FocusUtils.worldToScreenSpace(grabPos));
                new_marker.SetActive(m_isMarkerDisplayed);
                
                if (Jetfire.IsConnected2())
                {
                    string message = "grabbed at," + newPos + "," + FocusUtils.worldToScreenSpace(grabPos);
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

    public void toggleMarkerVisibility()
    {
        m_isMarkerDisplayed = !m_isMarkerDisplayed;
        for (int i = 0; i < m_markers.Count; i++)
        {
            m_markers[i].SetActive(m_isMarkerDisplayed);
        }
    }

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
}
