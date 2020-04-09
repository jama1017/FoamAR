using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamIconManager : MonoBehaviour
{

    public Sprite m_highlightSprite;
    private Sprite _originalSprite;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = FoamUtils.IconNormalColor;
        _originalSprite = _spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.name == "Hand_l" || other.transform.parent.parent.name == "Hand_r")
        {
            if (other.transform.parent.name == "index")
            {
                _spriteRenderer.sprite = m_highlightSprite;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.parent.name == "Hand_l" || other.transform.parent.parent.name == "Hand_r")
        {
            if (other.transform.parent.name == "index")
            {
                _spriteRenderer.sprite = _originalSprite;

            }
        }
    }
}
