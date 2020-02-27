using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDataManager : MonoBehaviour
{
    private List<GameObject> _focusedObjects;

    // Start is called before the first frame update
    void Start()
    {
        _focusedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> FocusedObjects
    {
        get { return _focusedObjects; }
    }
}
