using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portalble
{
    public class Selectable : MonoBehaviour
    {

        private bool _isTarget = false;
        private Renderer _renderer;

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = FocusUtils.ObjNormalColor;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetAsTarget()
        {
            _isTarget = true;
            _renderer.material.color = FocusUtils.ObjTargetColor;
        }

        public void Highlight()
        {
            if (_isTarget)
            {
                _renderer.material.color = new Color(FocusUtils.ObjTargetFocusedColor.r, FocusUtils.ObjTargetFocusedColor.g, FocusUtils.ObjTargetFocusedColor.b, _renderer.material.color.a);
            }
            else
            {
                _renderer.material.color = new Color(FocusUtils.ObjFocusedColor.r, FocusUtils.ObjFocusedColor.g, FocusUtils.ObjFocusedColor.b, _renderer.material.color.a); 
            }
        }

        public void DeHighlight()
        {
            if (_isTarget)
            {
                _renderer.material.color = FocusUtils.ObjTargetColor;
            }
            else
            {
                _renderer.material.color = FocusUtils.ObjNormalColor;
            }
        }

        public bool IsTarget
        {
            get { return _isTarget; }
        }
    }
}