using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble;
using Portalble.Functions.Grab;

public class FocusCylinder : MonoBehaviour
{
    private SelectionDataManager _selectionDM;

    // Start is called before the first frame update
    void Start()
    {
        _selectionDM = GameObject.FindGameObjectWithTag("selectionDM").GetComponent<SelectionDataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("FOCUSED on object " + other.tag);

        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            _selectionDM.FocusedObjects.Add(other.gameObject);

            if (!Grab.Instance.IsGrabbing)
            {
                HighlightObjColor(other);
            }
            // when grabbing
            else
            {
                ChangeObjToOGColor(other);
            }


            //GameObject lineObj = Instantiate(_selectionDM.FocusInkPrefab);
            //LineRenderer line = lineObj.GetComponent<LineRenderer>();
            //line.positionCount = 2;

            //FocusUtils.UpdateLinePos(line, other, _selectionDM.ActivePalm);
            //_selectionDM.FocusedObjectToLine.Add(other.gameObject, lineObj);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            // draw line on every collider
            //LineRenderer line = _selectionDM.FocusedObjectToLine[other.gameObject].GetComponent<LineRenderer>();
            if (!Grab.Instance.IsGrabbing)
            {
                HighlightObjColor(other);
            }
            // when grabbing
            else
            {
                ChangeObjToOGColor(other);
            }
            // on highest ranked collider
            //GameObject num_one = FocusUtils.rankFocusedObjects(_selectionDM.FocusedObjects, _selectionDM.FirstPersonCamera.transform.position);
            //LineRenderer line = _selectionDM.FocusedObjectToLine[num_one].GetComponent<LineRenderer>();

            //FocusUtils.UpdateLinePos(line, other, _selectionDM.ActivePalm);
            Debug.Log("LINEE COLOR size: " + _selectionDM.FocusedObjectToColor.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            _selectionDM.FocusedObjects.Remove(other.gameObject);

            ChangeObjToOGColor(other);


            //_selectionDM.FocusedObjectToColor.Remove(other);
            //GameObject.Destroy(_selectionDM.FocusedObjectToLine[other.gameObject]);
            //_selectionDM.FocusedObjectToLine.Remove(other.gameObject);
        }
        //material.color = Color.blue
    }

    private void ChangeObjToOGColor(Collider other)
    {
        Renderer objRenderer = other.gameObject.GetComponent<Renderer>();
        Color objOGColor = objRenderer.material.color;

        if (objOGColor == _selectionDM.ObjTargetFocusedColor || objOGColor == _selectionDM.ObjTargetColor)
        {
            objRenderer.material.color = _selectionDM.ObjTargetColor;
        }
        else
        {
            objRenderer.material.color = _selectionDM.ObjNormalColor;
        }
    }

    private void HighlightObjColor(Collider other)
    {
        Renderer objRenderer = other.gameObject.GetComponent<Renderer>();
        Color objOGColor = objRenderer.material.color;

        if (objOGColor == _selectionDM.ObjTargetColor || objOGColor == _selectionDM.ObjTargetFocusedColor)
        {
            objRenderer.material.color = _selectionDM.ObjTargetFocusedColor;
        }
        else
        {
            objRenderer.material.color = _selectionDM.ObjFocusedColor;
        }
    }
}
