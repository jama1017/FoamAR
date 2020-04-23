using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamScaleTab : MonoBehaviour
{
    private Vector3 _prevPos;
    public Transform m_targetTrans;
    private Vector3 _scaleDir;
    private int _coord;
    private int _dirInt;

    // Start is called before the first frame update
    void Start()
    {
        _prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TabUpdate()
    {

        Vector3 _currPos = transform.position;
        if (_currPos != _prevPos && m_targetTrans)
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
            m_targetTrans.localScale = new Vector3(m_targetTrans.localScale[0], ratio, m_targetTrans.localScale[2]);

            m_targetTrans.position = m_targetTrans.position + _dirInt * _scaleDir * delta / 2;

        }
        _prevPos = _currPos;
    }

    public void SetTarget(Transform tar, Vector3 scaleDir, int coord, int dirInt)
    {
        m_targetTrans = tar;
        _scaleDir = scaleDir;
        _coord = coord;
        _dirInt = dirInt;
    }

    public void OnGrabStop()
    {
        Bounds curBound = m_targetTrans.GetComponent<MeshFilter>().sharedMesh.bounds; // or just mesh?
        transform.position = m_targetTrans.position + _dirInt * _scaleDir * (m_targetTrans.localScale[_coord] * curBound.size[_coord] / 2 + FoamUtils.ScaleTabOffset);
    }
}
