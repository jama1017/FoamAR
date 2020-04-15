using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.iOS;

public enum FoamState
{
    STATE_CREATE,
    STATE_MANIPULATE,
    STATE_IDLE,
    STATE_BUSY
}

public class JUIController : MonoBehaviour {

    public GameObject m_dot;
    private Animator m_dotAnimator;
    private Image _dotRenderer;

    public Text m_stateIndicator;
    public Sprite m_fan_cre;
    public Sprite m_fan_mani;
    public Sprite m_fan_og;
    private FoamState _foamState = FoamState.STATE_IDLE;

    public FoamState FoamState
    {
        get { return _foamState; }
        set { _foamState = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_dotAnimator = m_dot.GetComponent<Animator>();
        _dotRenderer = m_dot.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Debug.Log(touch.position);
        }

        AnimatorStateInfo info = m_dotAnimator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("dot_fan")) {
            checkTouch();
            checkMouseClick();
        }
    }

    private void checkTouch()
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPos = Input.GetTouch(0).position;
            checkTouchRegion(touchPos);
        }
    }

    private void checkMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Input.mousePosition;
            checkTouchRegion(touchPos);
        }
    }

    private void checkTouchRegion(Vector3 touchPos)
    {
        Vector3[] corners = new Vector3[4];
        m_dot.GetComponent<RectTransform>().GetWorldCorners(corners);

        if (isInsideTri(touchPos, corners[0], corners[1], corners[2]))
        {
            m_stateIndicator.text = "Create";
            _dotRenderer.sprite = m_fan_cre;
            _foamState = FoamState.STATE_CREATE;

        }
        else if (isInsideTri(touchPos, corners[2], corners[3], corners[0]))
        {
            m_stateIndicator.text = "Edit";
            _foamState = FoamState.STATE_MANIPULATE;

        }
        else
        {
            m_stateIndicator.text = "Idle";
            _foamState = FoamState.STATE_IDLE;
        }
    }

    private bool isInsideTri(Vector3 s, Vector3 a, Vector3 b, Vector3 c)
    {
        float as_x = s.x - a.x;
        float as_y = s.y - a.y;

        bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

        if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab) return false;

        if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab) return false;

        return true;
    }
}

