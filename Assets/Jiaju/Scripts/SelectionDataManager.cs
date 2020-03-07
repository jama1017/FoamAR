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
    private HashSet<int> _targetObjIDs = new HashSet<int>();

    // depth cue related;
    private float _farDis = 0.3f;       // to be adjusted based on user preference. far distance to be reached
    private float _nearDis = 0.1f;      // to be adjusted based on user preference. near distance to be reached
    private readonly float _nearHandDis = 0.11f; // to be adjusted based on user preference.
    private readonly float _farHandDis = 0.21f;  // to be adjusted based on user preference.
    private readonly float _farAlpha = 0.2f;
    private readonly float _nearAlpha = 1.0f;


    //colors for user task and selection aid
    private Color _objNormalColor = new Color(150f / 255f, 100f / 255f, 0f, 1f);
    private Color _objFocusedColor = new Color(1f, 200f / 255f, 0f, 1f);
    private Color _objTargetColor = new Color(18f / 255f, 20f / 255f, 125f / 255f, 1f);
    private Color _objTargetFocusedColor = new Color(85f / 255f, 180f / 255f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        _focusedObjects = new List<GameObject>();

        //ActiveHand = this.

        _activeIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        _activeThumb = ActiveHand.transform.GetChild(0).GetChild(2).gameObject;
        _activePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
        _activeGC = ActiveHand.GetComponent<GestureControl>();

        _objNormalColor.a = _farAlpha;
        _objFocusedColor.a = _farAlpha;
        _objTargetColor.a = _farAlpha;
        _objTargetFocusedColor.a = _farAlpha;
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

    public Color ObjNormalColor
    {
        get { return _objNormalColor; }
    }

    public Color ObjFocusedColor
    {
        get { return _objFocusedColor; }
    }

    public Color ObjTargetColor
    {
        get { return _objTargetColor; }
    }

    public Color ObjTargetFocusedColor
    {
        get { return _objTargetFocusedColor; }
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

    public HashSet<int> TargetObjIDs
    {
        get { return _targetObjIDs; }
    }

    public float NearDis
    {
        get { return _nearDis; }
    }

    public float FarDis
    {
        get { return _farDis; }
    }

    public float NearHandDis
    {
        get { return _nearHandDis; }
    }

    public float FarHandDis
    {
        get { return _farHandDis; }
    }

    public float FarAlpha
    {
        get { return _farAlpha; }
    }

    public float NearAlpha
    {
        get { return _nearAlpha; }
    }
}
