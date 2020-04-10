using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modelable : MonoBehaviour
{

    private int _indexColliderCount = 0;
    private int _indexDwellCount = 0;
    private FoamDataManager _data;
    private Renderer _renderer;
    private Color _originalColor;

    private int _dwellThreshold = 150;

    // Start is called before the first frame update
    void Start()
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();
        _renderer = this.GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnChildTriggerEnter(Collider other)
    {
        if (!_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationState"))
        {
            return;
        }

        if (other.transform.parent.name == "index")
        {
            _indexColliderCount++;

            if (_indexColliderCount > 0)
            {
                SetAsSelected();
            }
        }
        //Debug.Log("MODELABLE index count: " + _indexColliderCount);
    }


    public void OnChildTriggerStay(Collider other)
    {
        if (!_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationState")) { return; }

        if (other.transform.parent.name == "index")
        {
            _indexDwellCount++;

            //Debug.Log("ICONN obj self stay count: " + _indexDwellCount);
        }
    }


    public void OnChildTriggerExit(Collider other)
    {
        if (!_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationState")) { return; }

        if (other.transform.parent.name == "index")
        {
            _indexColliderCount--;

            if (_indexColliderCount <= 0)
            {
                _indexColliderCount = 0;
                _indexDwellCount = 0;
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
        if (!_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsName("ManipulationState")) { return; }

        //Debug.Log("MODELABLE: Select");

        if (_data.CurrentSelectionObj)
        {
            _data.CurrentSelectionObj.GetComponent<Modelable>().Deselect();
        }

        _renderer.material.color = FoamUtils.ObjManiSelectedColor;
        _data.CurrentSelectionObj = this.gameObject;
    }

    public void Deselect()
    {
        //Debug.Log("MODELABLE: Deselect");
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
}
