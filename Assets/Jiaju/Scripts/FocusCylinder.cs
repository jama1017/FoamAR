using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCylinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("FOCUSED on object " + other.tag);

        if (other.tag != "ARPlane") // ignore ARPlane prefab
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
        //material.color = Color.blue
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "ARPlane") // ignore ARPlane prefab
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        //material.color = Color.blue
    }
}
