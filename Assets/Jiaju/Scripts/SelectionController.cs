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
    private Renderer m_focusCylinderRenderer;
    private float m_v = 0f;
    private float m_h = 0f;

    // test distance calculation
    private GameObject m_marker1;
    private GameObject m_marker2;

    private HashSet<int> m_targetObjIDs = new HashSet<int>();
    //private bool m_useSelectionAid = true;

    protected override void Start()
    {
        base.Start();
        SetupServer();

        //m_focusCylinder = Instantiate(m_focusCylinderPrefab, m_FirstPersonCamera.transform.position + 0.2f * m_FirstPersonCamera.transform.forward, Quaternion.identity);
        m_focusCylinder = Instantiate(m_focusCylinderPrefab);
        m_focusCylinder.gameObject.GetComponent<Collider>().attachedRigidbody.useGravity = false;
        m_focusCylinderRenderer = m_focusCylinder.gameObject.GetComponent<Renderer>();

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

        //m_marker1 = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);
        //m_marker2 = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);
    }

    /// <summary>
    /// updates the location, scale, and rotation of the focus cylinder
    /// </summary>
    private void UpdateFocusCylinder()
    {
        //if (Input.touchCount > 0)
        //{
        //    Vector3 touchPos = Input.GetTouch(0).position;
        //    Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0); // clean up

        //    Vector2 distance_vec = touchPos - center;

        //    m_v = distance_vec.y / 10000f;
        //    m_h = distance_vec.x / 10000f;
        //}

        if (m_markers_screenPos.Count > 0) {
            Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
            Vector3 focusPos = FocusUtils.CalcFocusCenter(m_markers_screenPos);

            Vector2 distance_vec = focusPos - center;

            m_v = distance_vec.y / 10000f;
            m_h = distance_vec.x / 10000f;

            Debug.Log("SEM m_v: " + m_v);
            Debug.Log("SEM m_h: " + m_h);
        }

        float fac = m_focusCylinder.transform.localScale[1] * 2f; //1.2f if y is 0.5. 1.31f is y is 0.3. 2.f is y is 0.1

        m_focusCylinder.position = m_FirstPersonCamera.transform.position + fac * m_FirstPersonCamera.transform.forward + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;
        m_focusCylinder.LookAt(m_focusCylinder.position - (m_FirstPersonCamera.transform.position - m_focusCylinder.position));
        m_focusCylinder.Rotate(90.0f, 0.0f, 0.0f, Space.Self);

        //m_focusCylinderCenterPos = m_FirstPersonCamera.transform.position + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;
        m_focusCylinderCenterPos = m_FirstPersonCamera.transform.position + fac * m_FirstPersonCamera.transform.forward + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;


        //setting scale
        float initial_width = Camera.main.pixelWidth / (2 * 10000f);
        m_focusCylinder.localScale = new Vector3(initial_width, m_focusCylinder.localScale[1], initial_width);

        UpdateCylinderRadius(initial_width);
    }

    private void UpdateCylinderRadius(float maxWidth)
    {
        if (!ActiveHandManager) return;

        float dis = Vector3.Distance(m_selectionDM.ActiveIndex.transform.position, m_selectionDM.ActiveThumb.transform.position);
        if (dis > maxWidth) dis = maxWidth;
        m_focusCylinder.localScale = new Vector3(dis, m_focusCylinder.localScale[1], dis);

    }

    protected override void Update()
    {
        base.Update();

        UpdateActiveHandData();

        RecordGrabLoc();
        UpdateFocusCylinder();
        AidSelection();

        if (!Grab.Instance.IsGrabbing && m_selectionDM.UseSelectionAid) // if not grabbing, enable focus cylinder
        {
            TurnOnSelectionAid();
        }
        else // if grabbing. do not detect candidate objects
        {
            TurnOffSelectionAid();
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 touchPos = Input.mousePosition;
        //    Vector2 movePos;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, touchPos, null, out movePos);
        //    Debug.Log(movePos);
        //    prefab_marker.GetComponent<RectTransform>().position = movePos;
        //}
    }

    /// <summary>
    /// Turn on the visuals of selection aid
    /// </summary>
    private void TurnOnSelectionAid()
    {
        if (!m_focusCylinderRenderer.enabled)
        {
            m_focusCylinderRenderer.enabled = true;
        }

        m_guideLine.gameObject.SetActive(true);
    }

    /// <summary>
    /// Turn off the visuals of selection aid
    /// </summary>
    private void TurnOffSelectionAid()
    {
        if (m_focusCylinderRenderer.enabled)
        {
            m_focusCylinderRenderer.enabled = false;
        }

        m_guideLine.gameObject.SetActive(false);
    }

    private void AidSelection()
    {
        GameObject num_one = FocusUtils.RankFocusedObjects(m_selectionDM.FocusedObjects, m_focusCylinderCenterPos, m_canvas);
        
        if (!num_one || !ActiveHandManager)
        {
            m_guideLine.gameObject.SetActive(false);
            return;
        }

        //m_marker1.GetComponent<RectTransform>().anchoredPosition = FocusUtils.WorldToUISpace(m_canvas, m_focusCylinderCenterPos);
        //m_marker2.GetComponent<RectTransform>().anchoredPosition = FocusUtils.WorldToUISpace(m_canvas, num_one.transform.position);

        m_guideLine.gameObject.SetActive(true);

        FocusUtils.UpdateLinePos(m_guideLine, num_one.GetComponent<Collider>(), m_selectionDM.ActivePalm);
    }

    private void RecordGrabLoc()
    {
        if (ActiveHandGesture == "pinch")
        {
            if (Grab.Instance.IsGrabbing && !m_isRecorded)
            {
                //Debug.Log("FOCUS grabbing");
                m_isRecorded = true;
                GameObject grabbedObj = Grab.Instance.GetGrabbingObject().gameObject;
                Vector3 grabPos = grabbedObj.transform.position;

                Vector2 newPos = FocusUtils.WorldToUISpace(m_canvas, grabPos);
                GameObject new_marker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);

                new_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
                m_markers.Add(new_marker);
                m_markers_screenPos.Add(FocusUtils.WorldToScreenSpace(grabPos));
                new_marker.SetActive(m_isMarkerDisplayed);
                
                if (Jetfire.IsConnected2())
                {
                    string message = "Object grabbed," + newPos + "," + FocusUtils.WorldToScreenSpace(grabPos) + "," + FocusUtils.AddTimeStamp();

                    Color objColor = grabbedObj.GetComponent<Renderer>().material.color;
                    if (m_targetObjIDs.Contains(grabbedObj.GetInstanceID()))
                    {
                        message += ", target obj";
                    }
                    else
                    {
                        message += ", normal obj";
                    }

                    Jetfire.SendMsg2(message);
                    Debug.Log("JETFIREE" + objColor);
                }
            }
        }

        if (!Grab.Instance.IsGrabbing)
        {
            m_isRecorded = false;
        }
    }

    public void ToggleMarkerVisibility()
    {
        m_isMarkerDisplayed = !m_isMarkerDisplayed;
        for (int i = 0; i < m_markers.Count; i++)
        {
            m_markers[i].SetActive(m_isMarkerDisplayed);
        }
    }


    public void ToggleUseSelectionAid()
    {
        m_selectionDM.UseSelectionAid = !m_selectionDM.UseSelectionAid;
    }


    public override void OnARPlaneHit(PortalbleHitResult hit)
    {
        base.OnARPlaneHit(hit);

        if (placePrefab != null)
        {
            Transform cen = Instantiate(placePrefab, hit.Pose.position + hit.Pose.rotation * Vector3.up * offset, hit.Pose.rotation);
            cen.gameObject.GetComponent<Renderer>().material.color = m_selectionDM.ObjNormalColor;
            List<Transform> cens = new List<Transform>();

            // for focus center test purposes
            int num = 2;
            float offset_test = placePrefab.transform.localScale[0] * 1.2f;

            for (int i = -num; i < num + 1; i++)
            {
                for (int j = -num; j < num + 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Transform obj = Instantiate(placePrefab, cen.position + i * offset_test * cen.right - j * offset_test * cen.forward, cen.rotation);
                    obj.gameObject.GetComponent<Renderer>().material.color = m_selectionDM.ObjNormalColor;
                    cens.Add(obj);
                }
            }

            int idx = Random.Range(0, cens.Count);
            cens[idx].gameObject.GetComponent<Renderer>().material.color = m_selectionDM.ObjTargetColor;
            m_targetObjIDs.Add(cens[idx].gameObject.GetInstanceID());
        }
    }

    public void ToggleTimeStamp(bool isStart)
    {
        FocusUtils.ToggleTimeStamp(isStart);
    }

    private void UpdateActiveHandData()
    {
        HandManager activeHM = this.ActiveHandManager;
        if (activeHM)
        {
            m_selectionDM.ActiveHand = activeHM.gameObject;
        }
        else
        {
            m_selectionDM.ActiveHand = null;
        }
        
        m_selectionDM.updateActiveObjects();
    }
}
