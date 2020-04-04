using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamDataManager : MonoBehaviour
{
    public GameObject CreateMenu;
    public GameObject ManiMenu;
    public GameObject ActiveHand;
    public GameObject ActiveIndex;
    public GameObject ActivePalm;
    public GestureControl ActiveGC;
    private float _triggerRadius = 2.0f;  // size of bounding region
    private float _middleRadius = 0.023f; // middle region of the menu

    public SpriteRenderer CylinderRenderer;
    public SpriteRenderer CubeRenderer;
    public SpriteRenderer ConeRenderer;
    public SpriteRenderer SphereRenderer;

    public Sprite CubeHighlightSprite;
    public Sprite CylinderHighlightSprite;
    public Sprite ConeHighlightSprite;
    public Sprite SphereHighlightSprite;

    public Sprite CubeNormalSprite;
    public Sprite CylinderNormalSprite;
    public Sprite ConeNormalSprite;
    public Sprite SphereNormalSprite;

    public SpriteRenderer ManiUppRenderer;
    public SpriteRenderer ManiLowRenderer;
    public SpriteRenderer ManiLeftRenderer;
    public SpriteRenderer ManiRightRenderer;

    private Color _normalColor = new Color(1f, 1f, 1f, 0.5f);
    private Color _hoverColor = new Color(1f, 1f, 1f, 1f);

    public Transform CubePrefab;
    public Transform SpherePrefab;
    public Transform CylinderPrefab;
    public Transform ConePrefab;
    private CreateMenuItem _selected_createItem = CreateMenuItem.NULL;
    private ManiMenuItem _selected_maniItem = ManiMenuItem.NULL;

    // Start is called before the first frame update
    void Start()
    {
        CreateMenu.SetActive(false);
        ManiMenu.SetActive(false);

        ActiveIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        ActivePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
        ActiveGC = ActiveHand.GetComponent<GestureControl>();

        CubeNormalSprite = CubeRenderer.sprite;
        ConeNormalSprite = ConeRenderer.sprite;
        CylinderNormalSprite = CylinderRenderer.sprite;
        SphereNormalSprite = SphereRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float TriggerRadius
    {
        get { return _triggerRadius; }
    }

    public float MiddleRadius
    {
        get { return _middleRadius; }
    }

    public Color NormalColor
    {
        get { return _normalColor; }
    }

    public Color HoverColor
    {
        get { return _hoverColor; }
    }

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
}
