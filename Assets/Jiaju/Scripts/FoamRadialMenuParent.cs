using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamRadialMenuParent : MonoBehaviour
{
    protected FoamRadialCenterManager m_optionCenter;
    protected List<FoamRadialManager> m_options = new List<FoamRadialManager>();
    protected FoamRadialManager m_currentSelectedOption = null;

    // Start is called before the first frame update
    void Start()
    {
        m_options.Add(this.transform.GetChild(0).GetComponent<FoamRadialManager>());
        m_options.Add(this.transform.GetChild(1).GetComponent<FoamRadialManager>());
        m_options.Add(this.transform.GetChild(2).GetComponent<FoamRadialManager>());
        m_options.Add(this.transform.GetChild(3).GetComponent<FoamRadialManager>());
        m_optionCenter = this.transform.GetChild(4).GetComponent<FoamRadialCenterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        string check = "";

        //for (int i = 0; i < m_options.Count; i++)
        //{
        //    check += i.ToString() + ": " + m_options[i].gameObject.transform.localPosition.ToString() + "\n";
        //}

        check += "parent: " + this.gameObject.transform.position;

        Debug.Log(check);
    }

    public void HighlightSprite(int which)
    {
        if (m_currentSelectedOption)
        {
            m_currentSelectedOption.DeHighlightIcon();
        }

        if (which == -1)
        {
            m_optionCenter.DeHighlightCenter();
            return;
        }

        m_currentSelectedOption = m_options[which];
        m_currentSelectedOption.HightlightIcon();
    }
}
