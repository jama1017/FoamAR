﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamIconManager : MonoBehaviour
{

    public FoamDataManager m_data;
    public Sprite m_bg_highlightSprite;
    public Sprite m_rippleSprite;
    public SpriteRenderer m_bg_spriteRenderer;
    public GameObject m_ripple;

    private Sprite _bg_originalSprite;
    private int _indexDwellCount = 0;
    private int _indexColliderCount = 0;
    private int _dwellThreshold = 80;

    private bool _isActive = true;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //_spriteRenderer = this.GetComponent<SpriteRenderer>();
        m_bg_spriteRenderer.color = FoamUtils.IconNormalColor;
        _bg_originalSprite = m_bg_spriteRenderer.sprite;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (_indexColliderCount == 0)
        {
            m_bg_spriteRenderer.sprite = _bg_originalSprite;
        }

        if (_isActive && _indexDwellCount > 10)
        {
            float scal = FoamUtils.LinearMap(_indexDwellCount, 0, _dwellThreshold, 0.0f, 1.0f);
            m_ripple.transform.localScale = new Vector3(scal, scal, 1.0f);

        }
        else
        {
            m_ripple.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

        }
    }


    void OnTriggerEnter(Collider other)
    {

        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationObjMenuOpen")) { return; }

        if (other.transform.parent.name == "index") {

            _indexColliderCount++;

            if (_isActive)
            {
                m_bg_spriteRenderer.sprite = m_bg_highlightSprite;
            }      
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationObjMenuOpen")) { return; }

        if (other.transform.parent.name == "index")
        {
            _indexDwellCount++;
            //Debug.Log("ICONN: " + _indexDwellCount);
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationObjMenuOpen")) { return; }

        if (other.transform.parent.name == "index")
        {
            if (_isActive)
            {
                m_bg_spriteRenderer.sprite = _bg_originalSprite;
            }

            _indexColliderCount--;

            if (_indexColliderCount <= 0)
            {
                _indexColliderCount = 0;
                _indexDwellCount = 0;
            }
        }
    }


    public virtual void PerformAction()
    {
        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationObjMenuOpen")) { return; }

        m_bg_spriteRenderer.sprite = _bg_originalSprite;
        _indexDwellCount = 0;
        _indexColliderCount = 0;

        Debug.Log("ICONN: action performed");
    }


    public bool IsHandDwell()
    {
        if (_indexDwellCount > _dwellThreshold)
        {
            _indexDwellCount = 0;
            return true;
        }

        return false;
    }


    public void ActivateIcon() {
        _isActive = true;
        if (_indexColliderCount > 0)
        {
            //_spriteRenderer.sprite = m_highlightSprite;
        }
    }


    public void DeactivateIcon()
    {
        _isActive = false;
        m_bg_spriteRenderer.sprite = _bg_originalSprite;
        _indexDwellCount = 0;
        _indexColliderCount = 0;
    }


    public int IndexColliderCount
    {
        get { return _indexDwellCount; }
    }
}
