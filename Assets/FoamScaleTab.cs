using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamScaleTab : MonoBehaviour
{
    private Vector3 _prevPos;
    [HideInInspector]
    public Transform m_targetTrans;
    private Vector3 _scaleDir;
    private int _coord;
    private int _dirInt;
    private Quaternion _initRot;

    private FoamScaleParent _parent;
    private bool _isBeingGrabbed = false;
    private int _index = -1;

    private LineRenderer _line = null;
    private MeshFilter _mesh = null;

    // Start is called before the first frame update
    void Start()
    {
        _prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _currPos = transform.position;

        if (_currPos != _prevPos && _line)
        {
            _line.SetPosition(0, m_targetTrans.transform.position);
            _line.SetPosition(1, this.transform.position - this.transform.up * this.transform.localScale[1] / 2);
        }

        if (_currPos != _prevPos && m_targetTrans && _isBeingGrabbed)
        {
            Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?

            Vector3 deltaVector = _currPos - _prevPos;
            Vector3 projectedDeltaVector = Vector3.Project(deltaVector, _dirInt * _scaleDir); // project to direction normal
            float delta = projectedDeltaVector.magnitude;

            float dot = Vector3.Dot(_dirInt * _scaleDir, deltaVector); // to determine the direction of the vector
            if (dot < 0)
            {
                delta = -1 * delta;
            }

            float newY = curBound.size[_coord] * m_targetTrans.localScale[_coord] + delta; // new Height of target transform
            float ratio = newY / curBound.size[_coord]; // ratio for scale

            // update target transform. need to extend this line
            Vector3 newScale = m_targetTrans.localScale;
            for (int i = 0; i < 3; i++)
            {
                if (i == _coord)
                {
                    newScale[i] = ratio;
                }
            }
            m_targetTrans.localScale = newScale;
            m_targetTrans.position += _dirInt * _scaleDir * delta / 2;

            // update location of other tabs
            _parent.UpdateTabsLocation(curBound, _index);
        }
        _prevPos = _currPos;
    }

    public void SetTarget(FoamScaleParent p, int index, Transform tar, Vector3 scaleDir, int coord, int dirInt)
    {
        m_targetTrans = tar;
        _scaleDir = scaleDir;
        _coord = coord;
        _dirInt = dirInt;
        transform.rotation = Quaternion.FromToRotation(transform.up, 1 * dirInt * scaleDir);
        _initRot = transform.rotation;
        _index = index;
        _parent = p;

        _line = Instantiate(_parent.m_linePrefab).GetComponent<LineRenderer>();
        _line.SetPosition(0, m_targetTrans.transform.position);
        _line.SetPosition(1, this.transform.position - this.transform.up * this.transform.localScale[1] / 2);
    }

    public void OnGrabStart()
    {
        _isBeingGrabbed = true;

    }

    public void OnGrabStop()
    {
        Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?
        transform.position = m_targetTrans.position + _dirInt * _scaleDir * (m_targetTrans.localScale[_coord] * curBound.size[_coord] / 2 + FoamUtils.ScaleTabOffset);
        transform.rotation = _initRot;

        //_parent.UpdateTabsLocation(curBound, _index);
        _isBeingGrabbed = false;
    }

    public void CleanUp()
    {
        GameObject.Destroy(_line.gameObject);
    }
}
