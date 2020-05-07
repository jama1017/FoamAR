using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble;
using Portalble.Functions.Grab;


public class SelectionController : PortalbleGeneralController
{
    private Transform placePrefab;
    public Transform m_bigFocusObj;
    public Transform m_smallFocusObj;


    public float offset = 0.01f;

    public Canvas m_canvas;
    public GameObject prefab_marker;

    // selection specific
    private bool m_isRecorded = false;
    private List<GameObject> m_markers = new List<GameObject>();
    private Queue<KeyValuePair<Vector2, long>> m_markers_screenPos = new Queue<KeyValuePair<Vector2, long>>();
    private int m_markerQueueLimit = 5;
    private bool m_isMarkerDisplayed = false;
    public SelectionDataManager m_sDM;
    private LineRenderer m_guideLine;    // disabled
    private GameObject m_highestRankedObjMarker;
    private GameObject m_highestRankedObj;
    private bool m_isSnapped = false;
    private float m_maxSnapDis = 0.025f;


    // websocket
    public GameObject WSManager;
    public string websocketPort = "8765";

    // selection cylinder
    public Transform m_focusCylinderPrefab;
    private Transform m_focusCylinder;
    private Renderer m_focusCylinderRenderer;
    private Vector3 m_focusCylinderCenterPos = Vector3.zero;
    private Vector3 m_focusCylinderCenterPos_noOffset = Vector3.zero;
    private float m_v = 0f;
    private float m_h = 0f;

    // test distance calculation
    //private GameObject m_marker2;
    private Portalble.Functions.Grab.Grabable _prevLastSelectedObj = null;

    protected override void Start()
    {
        base.Start();
        SetupServer();

        placePrefab = m_smallFocusObj;

        // set up focus cylinder
        //m_focusCylinder = Instantiate(m_focusCylinderPrefab, m_FirstPersonCamera.transform.position + 0.2f * m_FirstPersonCamera.transform.forward, Quaternion.identity);
        m_focusCylinder = Instantiate(m_focusCylinderPrefab);
        m_focusCylinderRenderer = m_focusCylinder.GetComponent<Renderer>();
        m_focusCylinder.gameObject.GetComponent<Collider>().attachedRigidbody.useGravity = false;
        float initial_width = Camera.main.pixelWidth / (2 * 10000f);
        m_focusCylinder.localScale = new Vector3(initial_width, m_focusCylinder.localScale[1], initial_width);


        m_guideLine = Instantiate(m_sDM.FocusInkPrefab).GetComponent<LineRenderer>();
        m_guideLine.positionCount = 2;
        m_guideLine.gameObject.SetActive(false);

        // marks where the highestRankedObj is
        m_highestRankedObjMarker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);
        m_highestRankedObjMarker.SetActive(false);

        FoamUtils.IsExcludingSelectedObj = false;
        FoamUtils.IsGlobalGrabbing = true;
    }

    private void SetupServer()
    {
        // Create web socket
        Debug.Log("Connecting" + WSManager.GetComponent<WSManager>().websocketServer);
        string url = "ws://" + WSManager.GetComponent<WSManager>().websocketServer + ":" + websocketPort;

        Jetfire.Open2(url);
    }

    protected override void Update()
    {
        base.Update();

        UpdateActiveHandData();

        UpdateFocusCylinder();
        UpdateDepthCues();

        AidSelection();
        RecordGrabLoc();

        ResetSelectionAid();

        if (!Grab.Instance.IsGrabbing && m_sDM.UseSelectionAid) // if not grabbing, enable focus cylinder
        {
            TurnOnSelectionAid();
        }
        else // if grabbing. do not detect candidate objects
        {
            TurnOffSelectionAid();
        }

        // usually hand is 0.3f in front of camera at most
        //Debug.Log("TRANSPP :" + Vector3.Distance(m_FirstPersonCamera.transform.position, ActiveHandTransform.position).ToString("F10"));
    }


    //public void ResetHelperCollider()
    //{
    //    Portalble.Functions.Grab.Grabable grabable = Grab.Instance.LastGrabbedObject;
    //    if (grabable)
    //    {
    //        Selectable select = grabable.gameObject.GetComponent<Selectable>();
    //        if (select) select.ResetColliderSize();

    //    }
    //    //_prevLastSelectedObj = grabable;

    //}


    /// <summary>
    /// Turn on the visuals of selection aid
    /// </summary>
    private void TurnOnSelectionAid()
    {
        if (!m_focusCylinderRenderer.enabled)
        {
            m_focusCylinderRenderer.enabled = true;
        }
        //m_guideLine.gameObject.SetActive(true);
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
        //m_guideLine.gameObject.SetActive(false);
    }


    /// <summary>
    /// updates the location, scale, and rotation of the focus cylinder
    /// </summary>
    private void UpdateFocusCylinder()
    {
        float fac = m_focusCylinder.transform.localScale[1] * 2f; //1.2f if y is 0.5. 1.31f is y is 0.3. 2.f is y is 0.1

        m_focusCylinderCenterPos = m_FirstPersonCamera.transform.position + fac * m_FirstPersonCamera.transform.forward + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;
        m_focusCylinderCenterPos_noOffset = m_FirstPersonCamera.transform.position + m_h * m_FirstPersonCamera.transform.right + m_v * m_FirstPersonCamera.transform.up;

        m_focusCylinder.position = m_focusCylinderCenterPos;
        m_focusCylinder.LookAt(m_focusCylinder.position - (m_FirstPersonCamera.transform.position - m_focusCylinder.position));
        m_focusCylinder.Rotate(90.0f, 0.0f, 0.0f, Space.Self);


        // disabled cylinder radius adjustment
        //UpdateCylinderRadius(initial_width);
    }




    private void UpdateCylinderCenter()
    {
        if (m_markers_screenPos.Count > 0 && m_markers_screenPos.Count <= 5)
        {
            Debug.Log("CYLINDERR q count: " + m_markers_screenPos.Count);
            Vector2 center = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
            Vector2 focusPos = FocusUtils.CalcFocusCenter(m_markers_screenPos);

            Vector2 distance_vec = focusPos - center;

            m_v = distance_vec.y / 10000f;
            m_h = distance_vec.x / 10000f;
        }
    }




    private void UpdateCylinderRadius(float maxWidth)
    {
        if (!ActiveHandManager) return;

        float dis = Vector3.Distance(m_sDM.ActiveIndex.transform.position, m_sDM.ActiveThumb.transform.position);
        if (dis > maxWidth) dis = maxWidth;
        m_focusCylinder.localScale = new Vector3(dis, m_focusCylinder.localScale[1], dis);

    }



    private void AidSelection()
    {

        if (Grab.Instance.IsGrabbing) return;

        m_highestRankedObj = FocusUtils.RankFocusedObjects(m_sDM.FocusedObjects, m_focusCylinderCenterPos, m_sDM, m_canvas);
        
        if (!m_highestRankedObj || !ActiveHandManager)
        {
            m_guideLine.gameObject.SetActive(false);
            m_highestRankedObjMarker.SetActive(false);
            return;
        }

        m_highestRankedObj.GetComponent<Selectable>().SetHighestRankContour();
        DecontourOtherFocusedObjects(m_highestRankedObj.GetInstanceID());

        // ranked object marker
        //m_highestRankedObjMarker.SetActive(true);
        //m_highestRankedObjMarker.GetComponent<RectTransform>().anchoredPosition = FocusUtils.WorldToUISpace(m_canvas, m_highestRankedObj.transform.position);


        // snap target object to hand if close enough
        Vector3 indexThumbPos = FocusUtils.GetIndexThumbPos(m_sDM);

        // snapping object to hand
        if (Vector3.Distance(m_highestRankedObj.transform.position, indexThumbPos) < m_maxSnapDis)
        {
            if (ActiveHandGesture == "pinch")
            {
                if(!Grab.Instance.IsGrabbing && !m_isSnapped)
                {
                    m_highestRankedObj.GetComponent<Selectable>().SetSnapped();
                    m_highestRankedObj.transform.position = indexThumbPos; // might need a different value to ensure collider trigger
                    m_isSnapped = true;
                }
            }
        }
    }

    private void DecontourOtherFocusedObjects(int RankedObjID)
    {
        for (int i = 0; i < m_sDM.FocusedObjects.Count; i++)
        {
            GameObject curr = m_sDM.FocusedObjects[i];
            if (curr.GetInstanceID() != RankedObjID)
            {
                curr.GetComponent<Selectable>().RemoveHighestRankContour();
            }
        }
    }

    private void ResetSelectionAid()
    {
        if (!Grab.Instance.IsGrabbing)
        {
            m_isSnapped = false;
        }

        //ResetHelperCollider();
    }


    private void RecordGrabLoc()
    {
        if (ActiveHandGesture == "pinch")
        {
            if (Grab.Instance.IsGrabbing && !m_isRecorded)
            {
                m_isRecorded = true;
                GameObject grabbedObj = Grab.Instance.GetGrabbingObject().gameObject;
                //Vector3 grabPos = grabbedObj.transform.position;
                Vector3 grabPos = grabbedObj.GetComponent<Selectable>().GetSnappedPosition();

                Vector2 newPos = FocusUtils.WorldToUISpace(m_canvas, grabPos);
                GameObject new_marker = Instantiate(prefab_marker, Vector3.zero, Quaternion.identity, m_canvas.transform);

                new_marker.GetComponent<RectTransform>().anchoredPosition = newPos;
                m_markers.Add(new_marker);
                new_marker.SetActive(m_isMarkerDisplayed);

                if (m_markers_screenPos.Count < m_markerQueueLimit)
                {
                    m_markers_screenPos.Enqueue(new KeyValuePair<Vector2, long>(FocusUtils.WorldToScreenSpace(grabPos), System.DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                }
                else
                {
                    m_markers_screenPos.Dequeue();
                    m_markers_screenPos.Enqueue(new KeyValuePair<Vector2, long>(FocusUtils.WorldToScreenSpace(grabPos), System.DateTimeOffset.Now.ToUnixTimeMilliseconds()));
                }

                UpdateCylinderCenter();

                // transmit data
                if (Jetfire.IsConnected2())
                {
                    string message = "Object grabbed," + newPos + "," + FocusUtils.WorldToScreenSpace(grabPos) + "," + FocusUtils.AddTimeStamp();

                    Color objColor = grabbedObj.GetComponent<Renderer>().material.color;
                    //if (m_sDM.TargetObjIDs.Contains(grabbedObj.GetInstanceID()))
                    if (grabbedObj.GetComponent<Selectable>().IsTarget)
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


    private void UpdateDepthCues()
    {
        //List<GameObject> cuedObjs = m_sDM.SceneObjects;
        List<GameObject> cuedObjs = m_sDM.FocusedObjects;

        int numIter = cuedObjs.Count;

        //Vector3 visPos = m_FirstPersonCamera.transform.position;
        Vector3 visPos = m_focusCylinderCenterPos_noOffset;

        for (int i = 0; i < numIter; i++)
        {
            GameObject curr = cuedObjs[i];
            Renderer currRenderer = curr.GetComponent<Renderer>();
            float objVisDis = Vector3.Distance(curr.transform.position, visPos);

            if (objVisDis > m_sDM.FarDis) // far from far dis
            {
                FocusUtils.UpdateMaterialAlpha(currRenderer, FocusUtils.FarAlpha);

            }
            else if (objVisDis < m_sDM.FarDis && objVisDis > m_sDM.NearDis) // in between far and near dis
            {
                float alpha = FocusUtils.LinearMapReverse(objVisDis, m_sDM.NearDis, m_sDM.FarDis, FocusUtils.FarAlpha, FocusUtils.NearAlpha); // mapping from m_sDM.NearDis - m_sDM.FarDis to m_sDM.FarAlpha - 1.0f to

                // hand distance from obj is usually from 0.09 to 0.20
                alpha += AddHandDistanceAlpha(curr, alpha);

                FocusUtils.UpdateMaterialAlpha(currRenderer, alpha);

            }
            else
            {
                FocusUtils.UpdateMaterialAlpha(currRenderer, FocusUtils.NearAlpha);

            }
        }
    }



    private float AddHandDistanceAlpha(GameObject curr, float alpha)
    {
        float objHandDis = FocusUtils.FarHandDis;

        if (ActiveHandManager) objHandDis = Vector3.Distance(ActiveHandTransform.position, curr.transform.position);
        if (objHandDis > FocusUtils.FarHandDis) objHandDis = FocusUtils.FarHandDis;

        return FocusUtils.LinearMapReverse(objHandDis, FocusUtils.NearHandDis, FocusUtils.FarHandDis, 0.0f, FocusUtils.NearAlpha - alpha); // add hand distance into consideration
    }




    // functional functions
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
        m_sDM.UseSelectionAid = !m_sDM.UseSelectionAid;
    }


    public void TogglePrefabSize(bool isBig)
    {
        if (isBig)
        {
            placePrefab = m_bigFocusObj;
            m_maxSnapDis = 0.05f;
        }
        else
        {
            placePrefab = m_smallFocusObj;
            m_maxSnapDis = 0.025f;
        }
    }


    public override void OnARPlaneHit(PortalbleHitResult hit)
    {
        base.OnARPlaneHit(hit);

        if (placePrefab != null)
        {
            Transform cen = Instantiate(placePrefab, hit.Pose.position + hit.Pose.rotation * Vector3.up * offset, hit.Pose.rotation);
            cen.gameObject.GetComponent<Renderer>().material.color = FocusUtils.ObjNormalColor;
            m_sDM.SceneObjects.Add(cen.gameObject);

            List<Transform> cens = new List<Transform>();

            // for focus center test purposes
            int num = 3; // change this number to change number of prefabs
            float offset_test = placePrefab.transform.localScale[0] * 1.2f;

            for (int i = -num; i < num + 1; i++)
            {
                for (int j = -num; j < num + 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Transform obj = Instantiate(placePrefab, cen.position + i * offset_test * cen.right - j * offset_test * cen.forward, cen.rotation);
                    obj.gameObject.GetComponent<Renderer>().material.color = FocusUtils.ObjNormalColor;
                    cens.Add(obj);
                    m_sDM.SceneObjects.Add(obj.gameObject);
                }
            }

            int idx = Random.Range(0, cens.Count);
            cens[idx].gameObject.GetComponent<Selectable>().SetAsTarget();
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
            m_sDM.ActiveHand = activeHM.gameObject;
        }
        else
        {
            m_sDM.ActiveHand = null;
        }
        
        m_sDM.updateActiveObjects();
    }



}


//if (Input.touchCount > 0)
//{
//    Vector3 touchPos = Input.GetTouch(0).position;
//    Vector3 center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0); // clean up

//    Vector2 distance_vec = touchPos - center;

//    m_v = distance_vec.y / 10000f;
//    m_h = distance_vec.x / 10000f;
//}

//if (Input.GetMouseButtonDown(0))
//{
//    Vector3 touchPos = Input.mousePosition;
//    Vector2 movePos;
//    RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvas.transform as RectTransform, touchPos, null, out movePos);
//    Debug.Log(movePos);
//    prefab_marker.GetComponent<RectTransform>().position = movePos;
//}