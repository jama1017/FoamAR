using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamRadialManager : MonoBehaviour
{
    public Sprite m_highlightSprite;
    public Sprite m_centerSprite;
    public SpriteRenderer m_centerText;

    private SpriteRenderer _renderer;
    private Sprite _originalSprite;

    private bool _isHightlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = FoamUtils.IconNormalColor;
        _originalSprite = _renderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HightlightIcon()
    {
        if (!_isHightlighted)
        {
            _renderer.sprite = m_highlightSprite;
            m_centerText.sprite = m_centerSprite;
            _isHightlighted = true;
        }
    }

    public void DeHighlightIcon()
    {
        if (_isHightlighted)
        {
            _renderer.sprite = _originalSprite;
            _isHightlighted = false;
        }
    }
}
