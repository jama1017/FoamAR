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
            other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
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
            other.gameObject.GetComponent<Renderer>().material.color = Color.white;
            _selectionDM.FocusedObjects.Remove(other.gameObject);
            //GameObject.Destroy(_selectionDM.FocusedObjectToLine[other.gameObject]);
            //_selectionDM.FocusedObjectToLine.Remove(other.gameObject);
        }
        //material.color = Color.blue
    }
}
