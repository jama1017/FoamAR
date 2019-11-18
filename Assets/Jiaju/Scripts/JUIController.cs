using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.iOS;

public class JUIController : MonoBehaviour {

    public GameObject m_dot;
    private Animator m_dotAnimator;

    public Text m_stateIndicator;

    // Start is called before the first frame update
    void Start()
    {
        m_dotAnimator = m_dot.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Debug.Log(touch.position);
        }

        if (m_dotAnimator.GetCurrentAnimatorStateInfo(0).IsName("dot_fan")) {
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

        }
        else if (isInsideTri(touchPos, corners[2], corners[3], corners[0]))
        {
            m_stateIndicator.text = "Edit";

        }
        else
        {
            m_stateIndicator.text = "Idle";
        }
    }

    private bool checkIfInBound(Vector3[] corners, Vector3 touchPos)
    {
        float max_x = -99999f;
        float min_x = 99999f;
        float max_y = -99999f;
        float min_y = 99999f;

        for (int i = 0; i < corners.Length; i++)
        {
            if (max_x < corners[i].x)
            {
                max_x = corners[i].x;
            }

            if (max_y < corners[i].y)
            {
                max_y = corners[i].y;
            }

            if (min_x > corners[i].x)
            {
                min_x = corners[i].x;
            }

            if (min_y > corners[i].y)
            {
                min_y = corners[i].y;
            }
        }

        if (touchPos.x >= min_x && touchPos.x <= max_x)
        {
            if (touchPos.y >= min_y && touchPos.y <= max_y)
            {
                return true;
            }
        }

        return false;
    }

    public void onDotPressed()
    {
        Debug.Log("dot pressed");
    }

    void DisplayWorldCorners()
    {
        Vector3[] v = new Vector3[4];
        m_dot.GetComponent<RectTransform>().GetWorldCorners(v);

        Debug.Log("World Corners");
        for (var i = 0; i < 4; i++)
        {
            Debug.Log("World Corner " + i + " : " + v[i]);
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

