using System.Collections;
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
    private bool _useSelectionAid = true;

    private List<GameObject> _sceneObjects = new List<GameObject>();

    // depth cue related;
    private float _farDis = 0.3f;       // to be adjusted based on user preference. far distance to be reached
    private float _nearDis = 0.1f;      // to be adjusted based on user preference. near distance to be reached

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

    public void updateActiveObjects()
    {
        if (!ActiveHand) // if no hand
        {
            _activeIndex = null;
            _activeThumb = null;
            _activePalm = null;
            _activeGC = null;
            return;
        }

        _activeIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        _activeThumb = ActiveHand.transform.GetChild(0).GetChild(2).gameObject;
        _activePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
        _activeGC = ActiveHand.GetComponent<GestureControl>();
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

    public bool UseSelectionAid
    {
        get { return _useSelectionAid; }
        set { _useSelectionAid = value; }
    }

    public List<GameObject> SceneObjects
    {
        get { return _sceneObjects; }
    }

    //public HashSet<int> TargetObjIDs
    //{
    //    get { return _targetObjIDs; }
    //}

    public float NearDis
    {
        get { return _nearDis; }
    }

    public float FarDis
    {
        get { return _farDis; }
    }
}
