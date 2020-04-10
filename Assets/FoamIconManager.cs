using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamIconManager : MonoBehaviour
{

    public FoamDataManager m_data;
    public Sprite m_highlightSprite;
    private Sprite _originalSprite;
    private SpriteRenderer _spriteRenderer;
    private int _indexDwellCount = 0;
    private int _indexColliderCount = 0;
    private int _dwellThreshold = 100;

    private bool _isActive = true;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = FoamUtils.IconNormalColor;
        _originalSprite = _spriteRenderer.sprite;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (_indexColliderCount == 0)
        {
            _spriteRenderer.sprite = _originalSprite;
        }
    }


    void OnTriggerEnter(Collider other)
    {

        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationObjMenuOpen")) { return; }

        if (other.transform.parent.name == "index") {

            _indexColliderCount++;

            if (_isActive)
            {
                _spriteRenderer.sprite = m_highlightSprite;
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
                _spriteRenderer.sprite = _originalSprite;
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

        _spriteRenderer.sprite = _originalSprite;
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
            _spriteRenderer.sprite = m_highlightSprite;
        }
    }


    public void DeactivateIcon()
    {
        _isActive = false;
        _spriteRenderer.sprite = _originalSprite;
        _indexDwellCount = 0;
        _indexColliderCount = 0;
    }


    public int IndexColliderCount
    {
        get { return _indexDwellCount; }
    }
}
