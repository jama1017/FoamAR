using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamDataManager : MonoBehaviour
{
    public Camera FirstPersonCamera;

    public GameObject CreateMenu;
    public GameObject ManiMenu;
    public GameObject ObjMenu;

    public GameObject ActiveHand;
    public GameObject ActiveIndex;
    public GameObject ActivePalm;
    public GestureControl ActiveGC;
    private float _triggerRadius = 2.0f;  // size of bounding region
    private float _middleRadius = 0.023f; // middle region of the menu

    // creation menu renderer and sprite
    public SpriteRenderer CylinderRenderer;
    public SpriteRenderer CubeRenderer;
    public SpriteRenderer ConeRenderer;
    public SpriteRenderer SphereRenderer;
    public SpriteRenderer CreationCenterRenderer;

    public Sprite CubeHighlightSprite;
    public Sprite CylinderHighlightSprite;
    public Sprite ConeHighlightSprite;
    public Sprite SphereHighlightSprite;

    public Sprite CreationCenterSprite_Cube;
    public Sprite CreationCenterSprite_Cylinder;
    public Sprite CreationCenterSprite_Cone;
    public Sprite CreationCenterSprite_Sphere;
    public Sprite CreationCenterSprite_Middle;

    private List<SpriteRenderer> _creationRenderers = new List<SpriteRenderer>();
    private List<Sprite> _creationHighlightSprites = new List<Sprite>();
    private List<Sprite> _creationNormalSprites = new List<Sprite>();
    private List<Sprite> _creationCenterSprites = new List<Sprite>();


    public SpriteRenderer ManiUppRenderer;
    public SpriteRenderer ManiLowRenderer;
    public SpriteRenderer ManiLeftRenderer;
    public SpriteRenderer ManiRightRenderer;

    //private Color _normalColor = new Color(1f, 1f, 1f, 0.78f);
    //private Color _hoverColor = new Color(1f, 1f, 1f, 1f);
    public Material FoamMaterial;
    private Color _objManiOGColor;


    public Transform CubePrefab;
    public Transform SpherePrefab;
    public Transform CylinderPrefab;
    public Transform ConePrefab;
    private CreateMenuItem _selected_createItem = CreateMenuItem.NULL;
    private ManiMenuItem _selected_maniItem = ManiMenuItem.NULL;


    private List<GameObject> _sceneObjs = new List<GameObject>();
    private GameObject _currentSelectionObj = null;
    private Animator _stateMachine = null;

    private Vector3 _objCreatedPos;


    // Start is called before the first frame update
    void Start()
    {
        CreateMenu.SetActive(false);
        ManiMenu.SetActive(false);
        ObjMenu.SetActive(false);

        ActiveIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        ActivePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
        ActiveGC = ActiveHand.GetComponent<GestureControl>();

        // creation renderers
        _creationRenderers.Add(CubeRenderer);
        _creationRenderers.Add(CylinderRenderer);
        _creationRenderers.Add(ConeRenderer);
        _creationRenderers.Add(SphereRenderer);

        // creation normal sprites
        for (int i = 0; i < _creationRenderers.Count; i++)
        {
            _creationNormalSprites.Add(_creationRenderers[i].sprite);
            _creationRenderers[i].color = FoamUtils.IconNormalColor;
        }
        CreationCenterRenderer.color = FoamUtils.IconNormalColor;

        // creation highlight sprites
        _creationHighlightSprites.Add(CubeHighlightSprite);
        _creationHighlightSprites.Add(CylinderHighlightSprite);
        _creationHighlightSprites.Add(ConeHighlightSprite);
        _creationHighlightSprites.Add(SphereHighlightSprite);

        // creation center sprites
        _creationCenterSprites.Add(CreationCenterSprite_Cube);
        _creationCenterSprites.Add(CreationCenterSprite_Cylinder);
        _creationCenterSprites.Add(CreationCenterSprite_Cone);
        _creationCenterSprites.Add(CreationCenterSprite_Sphere);
        _creationCenterSprites.Add(CreationCenterSprite_Middle);

        _objCreatedPos = Vector3.zero;

        _objManiOGColor = FoamMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        // check state first
       _objCreatedPos = ActivePalm.transform.position + FoamUtils.ObjCreatedOffset * ActivePalm.transform.forward;
    }

    public float TriggerRadius
    {
        get { return _triggerRadius; }
    }

    public float MiddleRadius
    {
        get { return _middleRadius; }
    }

    //public Color NormalColor
    //{
    //    get { return _normalColor; }
    //}

    //public Color HoverColor
    //{
    //    get { return _hoverColor; }
    //}

    public CreateMenuItem Selected_createItem
    {
        get { return _selected_createItem; }
        set { _selected_createItem = value; }
    }

    public ManiMenuItem Selected_maniItem
    {
        get { return _selected_maniItem; }
        set { _selected_maniItem = value; }
    }

    public List<SpriteRenderer> CreationRenderers
    {
        get { return _creationRenderers; }
    }

    public List<Sprite> CreationNormalSprites
    {
        get { return _creationNormalSprites; }
    }

    public List<Sprite> CreationHighlightSprites
    {
        get { return _creationHighlightSprites; }
    }

    public List<Sprite> CreationCenterSprites
    {
        get { return _creationCenterSprites; }
    }

    public List<GameObject> SceneObjs
    {
        get { return _sceneObjs; }
    }

    public GameObject CurrentSelectionObj
    {
        set { _currentSelectionObj = value; }
        get { return _currentSelectionObj; }
    }

    public Animator StateMachine
    {
        set { _stateMachine = value; }
        get { return _stateMachine; }
    }


    public Vector3 ObjCreatedPos
    {
        get { return _objCreatedPos; }
    }


    public Color ObjManiOGColor
    {
        get { return _objManiOGColor; }
    }
}
