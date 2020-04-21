 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamRadialManager : MonoBehaviour
{
    //public Sprite m_highlightSprite;
    public Sprite m_centerSprite;
    public SpriteRenderer m_centerText;

    private SpriteRenderer _BGRenderer;
    //private Sprite _originalSprite;

    private bool _isHightlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        _BGRenderer = GetComponent<SpriteRenderer>();
        _BGRenderer.color = FoamUtils.RadialIconBGNormalColor;
        //_originalSprite = _renderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HightlightIcon()
    {
        if (!_isHightlighted)
        {
            _BGRenderer.color = FoamUtils.RadialIconBGHighlightColor;
            m_centerText.sprite = m_centerSprite;
            _isHightlighted = true;
        }
    }

    public void DeHighlightIcon()
    {
        if (_isHightlighted)
        {
            _BGRenderer.color = FoamUtils.RadialIconBGNormalColor;
            _isHightlighted = false;
        }
    }
}
