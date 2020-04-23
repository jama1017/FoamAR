using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamScaleParent : MonoBehaviour
{
    public Transform m_scaleTabPrefab;
    private Transform m_targetTrans = null;

    private Transform upTab;
    private Vector3 upTab_prevPos;

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (!upTab) return;

        upTab.position = Vector3.MoveTowards(upTab.position, upTab.position + m_targetTrans.up * 2.0f, 0.001f);

        float delta = Vector3.Distance(upTab_prevPos, upTab.position);
        Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?

        float newY = curBound.size.y * m_targetTrans.localScale[1] + delta;
        float oldY = curBound.size.y * m_targetTrans.localScale[1];

        float ratio = newY / (curBound.size.y);

        m_targetTrans.localScale = new Vector3(m_targetTrans.localScale[0], ratio, m_targetTrans.localScale[2]);
        m_targetTrans.position = m_targetTrans.position + m_targetTrans.up * delta / 2;
        //checked += "delta: " + delta.ToString("F6") + "\n";
        //checked +=

        //Debug.Log()
        upTab_prevPos = upTab.position;
    }

    public void SetTarget(Transform tar)
    {
        m_targetTrans = tar;
    }

    public void SetUpTabs()
    {
        if (!m_targetTrans) { return; }
        Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?

        float offset = 0.05f;

        // up
        upTab = Instantiate(m_scaleTabPrefab, m_targetTrans.position + m_targetTrans.up * (m_targetTrans.localScale[1] * curBound.size.y / 2 + offset), Quaternion.Euler(m_targetTrans.up.x, m_targetTrans.up.y, m_targetTrans.up.z));
        upTab_prevPos = upTab.position;
        // down
        Instantiate(m_scaleTabPrefab, m_targetTrans.position - m_targetTrans.up * (m_targetTrans.localScale[1] * curBound.size.y / 2 + offset), Quaternion.Euler(-m_targetTrans.up.x, -m_targetTrans.up.y, -m_targetTrans.up.z));
    }
}
