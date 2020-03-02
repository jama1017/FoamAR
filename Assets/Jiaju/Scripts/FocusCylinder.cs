using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Renderer objRenderer = other.gameObject.GetComponent<Renderer>();
            Color objOGColor = objRenderer.material.color;

            _selectionDM.FocusedObjectToColor.Add(other, objOGColor);

            if (objOGColor == Color.white)
            {
                objRenderer.material.color = Color.yellow;
            }
            else
            {
                objRenderer.material.color = Color.red;
            }


            _selectionDM.FocusedObjects.Add(other.gameObject);

            //GameObject lineObj = Instantiate(_selectionDM.FocusInkPrefab);
            //LineRenderer line = lineObj.GetComponent<LineRenderer>();
            //line.positionCount = 2;

            //FocusUtils.UpdateLinePos(line, other, _selectionDM.ActivePalm);
            //_selectionDM.FocusedObjectToLine.Add(other.gameObject, lineObj);
        }
        //material.color = Color.blue
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            // draw line on every collider
            //LineRenderer line = _selectionDM.FocusedObjectToLine[other.gameObject].GetComponent<LineRenderer>();


            // on highest ranked collider
            //GameObject num_one = FocusUtils.rankFocusedObjects(_selectionDM.FocusedObjects, _selectionDM.FirstPersonCamera.transform.position);
            //LineRenderer line = _selectionDM.FocusedObjectToLine[num_one].GetComponent<LineRenderer>();

            //FocusUtils.UpdateLinePos(line, other, _selectionDM.ActivePalm);
            Debug.Log("LINEE dic size: " + _selectionDM.FocusedObjectToLine.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            //Renderer objRenderer = other.gameObject.GetComponent<Renderer>();

            other.gameObject.GetComponent<Renderer>().material.color = _selectionDM.FocusedObjectToColor[other];

            _selectionDM.FocusedObjectToColor.Remove(other);
            _selectionDM.FocusedObjects.Remove(other.gameObject);
            //GameObject.Destroy(_selectionDM.FocusedObjectToLine[other.gameObject]);
            //_selectionDM.FocusedObjectToLine.Remove(other.gameObject);
        }
        //material.color = Color.blue
    }
}
