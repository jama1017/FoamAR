﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDataManager : MonoBehaviour
{
    public GameObject ActiveHand;
    public Camera FirstPersonCamera;
    public GameObject FocusInkPrefab;
    private List<GameObject> _focusedObjects;
    private GameObject _activeIndex;
    private GameObject _activeThumb;
    private GameObject _activePalm;
    private GestureControl _activeGC;
    private Dictionary<GameObject, GameObject> _focusedObjectToLine = new Dictionary<GameObject, GameObject>();
    private Dictionary<Collider, Color> _focusedObjectToColor = new Dictionary<Collider, Color>();
    
    // Start is called before the first frame update
    void Start()
    {
        _focusedObjects = new List<GameObject>();

        _activeIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        _activeThumb = ActiveHand.transform.GetChild(0).GetChild(2).gameObject;
        _activePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
        _activeGC = ActiveHand.GetComponent<GestureControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> FocusedObjects
    {
        get { return _focusedObjects; }
    }

    public GameObject ActiveIndex
    {
        get { return _activeIndex; }
    }

    public GameObject ActiveThumb
    {
        get { return _activeThumb; }
    }

    public GameObject ActivePalm
    {
        get { return _activePalm; }
    }

    public GestureControl ActiveGC
    {
        get { return _activeGC; }
    }

    public Dictionary<GameObject, GameObject> FocusedObjectToLine
    {
        get { return _focusedObjectToLine; }
    }

    public Dictionary<Collider, Color> FocusedObjectToColor
    {
        get { return _focusedObjectToColor; }
    }
}
