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
        }
        //material.color = Color.blue
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InteractableObj") // ignore ARPlane prefab
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.white;
            _selectionDM.FocusedObjects.Remove(other.gameObject);
        }
        //material.color = Color.blue
    }
}
