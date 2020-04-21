using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamRadialMenuParent : MonoBehaviour
{
    protected FoamRadialCenterManager m_optionCenter;
    protected List<FoamRadialManager> m_options = new List<FoamRadialManager>();
    protected FoamRadialManager m_currentSelectedOption = null;

    protected Vector3 m_palmPos_init = Vector3.zero; // projected space
    protected Vector3 m_menu_center = Vector3.zero;
    //protected Vector2 m_palmPos_curr = Vector3.zero;
    protected float inner_radius = 0.02795f;
    protected float outer_radius = 1.0f;

    private Vector3 m_bound_UppL;
    private Vector3 m_bound_UppR;
    private Vector3 m_bound_LowL;
    private Vector3 m_bound_LowR;


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

    }

    public MenuRegion RegionDetection(Vector3 palmPos_curr)
    {
        Vector3 local_curr = Vector3.ProjectOnPlane(this.transform.InverseTransformPoint(palmPos_curr), Vector3.forward) - m_palmPos_init;

        MenuRegion region = FoamUtils.checkMenuRegion(local_curr, m_menu_center, m_bound_UppL, m_bound_UppR, m_bound_LowL, m_bound_LowR, inner_radius);

        switch (region)
        {
            case MenuRegion.UPPER:
                this.HighlightSprite(0);
                break;


            case MenuRegion.RIGHT:
                this.HighlightSprite(1);
                break;


            case MenuRegion.LOWER:
                this.HighlightSprite(2);
                break;


            case MenuRegion.LEFT:
                this.HighlightSprite(3);
                break;


            case MenuRegion.MIDDLE:
                this.HighlightSprite(-1);

                break;
        }

        return region;
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



    public void RecordPalmPosInit(Vector3 init)
    {
        m_palmPos_init = Vector3.ProjectOnPlane(this.transform.InverseTransformPoint(init), Vector3.forward);
        m_menu_center = Vector3.ProjectOnPlane(this.transform.InverseTransformPoint(this.transform.position), Vector3.forward);

        //location detection
        m_bound_UppL = m_menu_center + Vector3.up * outer_radius - Vector3.right * outer_radius;
        m_bound_UppR = m_menu_center + Vector3.up * outer_radius + Vector3.right * outer_radius;
                                             
        m_bound_LowL = m_menu_center - Vector3.up * outer_radius - Vector3.right * outer_radius;
        m_bound_LowR = m_menu_center - Vector3.up * outer_radius + Vector3.right * outer_radius;
    }
}