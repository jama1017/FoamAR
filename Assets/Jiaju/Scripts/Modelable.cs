using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble.Functions.Grab;

public class Modelable : MonoBehaviour
{

    private int _indexColliderCount = 0;
    private int _indexDwellCount = 0;
    private FoamDataManager _data;
    private Renderer _renderer;
    private Color _originalColor;

    private int _dwellThreshold = 220; // to add a special case for cone
    private bool _isBeingSelected = false;
    private float _lowAlpha = 0.5f;
    private int _dwellEffectLowCutoff = 30;

    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();
        _renderer = this.GetComponent<Renderer>();
        _originalColor = _renderer.material.color;

        if (this.gameObject.name == "FoamCone" || this.gameObject.name == "FoamCone(Clone)")
        {
            _dwellThreshold = 170;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckIsInManipulationState()) { return; }
        //if (!CheckIsInManipulationState()) { return; }

        if (_indexDwellCount >= _dwellEffectLowCutoff && _isBeingSelected)
        {
            Color curC = _renderer.material.color;
            float alp = 0.0f;

            int mappedDwellCount = _indexDwellCount - _dwellEffectLowCutoff;
            int mappedDwellThreshold = _dwellThreshold - _dwellEffectLowCutoff;

            if (mappedDwellCount < mappedDwellThreshold / 2)
            {
                alp = FoamUtils.LinearMapReverse(mappedDwellCount, 0, mappedDwellThreshold / 2, _lowAlpha, 1.0f);
            }
            else
            {
                alp = FoamUtils.LinearMap(mappedDwellCount, mappedDwellThreshold / 2, mappedDwellThreshold, _lowAlpha, 1.0f);
            }

            _renderer.material.color = new Color(curC.r, curC.g, curC.b, alp);
        }
        else
        {
            if (_isBeingSelected)
            {
                _renderer.material.color = FoamUtils.ObjManiSelectedColor;
            }
        }

    }


    public void OnChildTriggerEnter(Collider other)
    {
        if (!CheckIsInManipulationState()) { return; }

        if (!other.transform.parent) return;
        if (other.transform.parent.name == "index")
        {
            _indexColliderCount++;

            if (_indexColliderCount > 0 && !Grab.Instance.IsGrabbing)
            {
                if (!CheckIsInManipulationState()) { return; }
                SetAsSelected();
            }
        }
        //Debug.Log("MODELABLE index count: " + _indexColliderCount);
    }


    public void OnChildTriggerStay(Collider other)
    {
        if (!CheckIsInManipulationState()) { return; }

        if (!other) return;
        if (!other.transform.parent) return;
        if (other.transform.parent.name == "index")
        {
            _indexDwellCount++;

            Debug.Log("------!!! threshold" + _dwellThreshold);
            Debug.Log("------!!! obj num" + _data.SceneObjs.Count);

        }
    }


    public void OnChildTriggerExit(Collider other)
    {
        if (!CheckIsInManipulationState()) { return; }

        if (!other.transform.parent) return;
        if (other.transform.parent.name == "index")
        {
            _indexColliderCount--;

            if (_indexColliderCount <= 0)
            {
                _indexColliderCount = 0;
                _indexDwellCount = 0;
                if(_isBeingSelected)
                {
                    _renderer.material.color = FoamUtils.ObjManiSelectedColor;
                }
            }
        }
        //Debug.Log("MODELABLE index count: " + _indexColliderCount);

    }


    public int IndexColliderCount
    {
        get { return _indexColliderCount; }
    }


    public void SetAsSelected()
    {
        if (_data.CurrentSelectionObj)
        {
            _data.CurrentSelectionObj.GetComponent<Modelable>().Deselect();
        }

        _isBeingSelected = true;
        _renderer.material.color = FoamUtils.ObjManiSelectedColor;
        _data.CurrentSelectionObj = this.gameObject;

        FoamUtils.CurrentSelectionObjID = this.gameObject.GetInstanceID();
    }


    public void Deselect()
    {
        _isBeingSelected = false;
        _renderer.material.color = _originalColor;
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

    //private bool CheckIsInRightState()
    //{
    //    AnimatorStateInfo info = _data.StateMachine.GetCurrentAnimatorStateInfo(0);

    //    if (info.IsName("ManipulationState") || info.IsName("ToolMoveSelected")) {
    //        return true;
    //    }

    //    return false;
    //}

    private bool CheckIsInManipulationState()
    {
        AnimatorStateInfo info = _data.StateMachine.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("ManipulationState"))
        {
            return true;
        }

        return false;
    }
}
