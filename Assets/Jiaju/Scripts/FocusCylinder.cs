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
        //Debug.Log("FOCUSED on object " + other.tag);

        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            _selectionDM.FocusedObjects.Add(other.gameObject);

            if (!Grab.Instance.IsGrabbing && _selectionDM.UseSelectionAid)
            {
                HighlightObjColor(other);
            }

            // when grabbing
            else
            {
                ChangeObjToOGColor(other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            if (!Grab.Instance.IsGrabbing && _selectionDM.UseSelectionAid)
            {
                HighlightObjColor(other);
            }
            // when grabbing
            else
            {
                ChangeObjToOGColor(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            _selectionDM.FocusedObjects.Remove(other.gameObject);

            ChangeObjToOGColor(other);
            other.gameObject.GetComponent<Selectable>().RemoveHighestRankContour();
        }
    }

    private void ChangeObjToOGColor(Collider other)
    {
        other.gameObject.GetComponent<Selectable>().DeHighlight();
    }

    private void HighlightObjColor(Collider other)
    {
        other.gameObject.GetComponent<Selectable>().Highlight();
    }
}
