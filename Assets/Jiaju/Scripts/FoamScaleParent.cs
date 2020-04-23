using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamScaleParent : MonoBehaviour
{
    public FoamDataManager m_data;
    public Transform m_scaleTabPrefab;
    private Transform m_targetTrans = null;

    private FoamScaleTab upTab;
    private FoamScaleTab downTab;

    private List<FoamScaleTab> _tabs = new List<FoamScaleTab>();

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (!upTab) return;
        upTab.TabUpdate();
        downTab.TabUpdate();
    }

    public void SetTarget(Transform tar)
    {
        m_targetTrans = tar;
    }

    public void SetUpTabs()
    {
        _tabs = new List<FoamScaleTab>();
        if (!m_targetTrans) { return; }
        Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?

        // up
        upTab = Instantiate(m_scaleTabPrefab, m_targetTrans.position + m_targetTrans.up * (m_targetTrans.localScale[1] * curBound.size.y / 2 + FoamUtils.ScaleTabOffset), Quaternion.Euler(m_targetTrans.up.x, m_targetTrans.up.y, m_targetTrans.up.z)).GetComponent<FoamScaleTab>();
        upTab.SetTarget(m_targetTrans, m_targetTrans.up, 1, 1);
        FoamUtils.CreateObjData(m_data, upTab.gameObject);

        // down
        downTab = Instantiate(m_scaleTabPrefab, m_targetTrans.position - m_targetTrans.up * (m_targetTrans.localScale[1] * curBound.size.y / 2 + FoamUtils.ScaleTabOffset), Quaternion.Euler(-m_targetTrans.up.x, -m_targetTrans.up.y, -m_targetTrans.up.z)).GetComponent<FoamScaleTab>();
        downTab.SetTarget(m_targetTrans, m_targetTrans.up, 1, -1);
        FoamUtils.CreateObjData(m_data, downTab.gameObject);

        _tabs.Add(upTab);
        _tabs.Add(downTab);
    }

    public void DestroyTabs()
    {
        int num = _tabs.Count;

        for (int i = 0; i < num; i++)
        {
            FoamUtils.RemoveObjData(m_data, _tabs[i].gameObject);
            GameObject.Destroy(_tabs[i].gameObject);
        }
    }
}
