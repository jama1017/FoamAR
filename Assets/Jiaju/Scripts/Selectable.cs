using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Portalble
{
    public class Selectable : MonoBehaviour
    {

        private bool _isTarget = false;
        private Renderer _renderer;
        private Material _outline_mat;

        private Material[] _normal_mats;
        private Material[] _outline_mats;

        public Material m_vertOutline; // to make more permanent

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = FocusUtils.ObjNormalColor;

            // outline material
            _outline_mat = Instantiate(m_vertOutline);
            _outline_mat.SetFloat("_BodyAlpha", 0.0f);
            _outline_mat.SetFloat("_OutlineWidth", 0.004f);
            _outline_mat.SetColor("_OutlineColor", FocusUtils.ObjRankedColor);

            _normal_mats = new Material[1];
            _normal_mats[0] = _renderer.materials[0];

            _outline_mats = new Material[2];
            _outline_mats[0] = _renderer.materials[0];
            _outline_mats[1] = _outline_mat;
        }


        // Update is called once per frame
        void Update()
        {

        }


        public void SetAsTarget()
        {
            _isTarget = true;
            _renderer.material.color = FocusUtils.ObjTargetColor;
            Debug.Log("HIGHH target set: " + _renderer.material.color);
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



        public void SetHighestRankContour()
        {
            _outline_mats[0] = _renderer.materials[0];

            if (_outline_mats[0].HasProperty("_OutlineColor")) // if there is already an outline (finger enter or grabbing)
            {
                _outline_mats[1].SetColor("_OutlineColor", _outline_mats[0].GetColor("_OutlineColor")); // use existing outline color
            }
            else // otherwise use obj ranked color
            {
                if (_outline_mats[1].GetColor("_OutlineColor") != FocusUtils.ObjRankedColor) 
                {
                    _outline_mats[1].SetColor("_OutlineColor", FocusUtils.ObjRankedColor);
                }
            }

            _renderer.materials = _outline_mats;
        }



        public void RemoveHighestRankContour()
        {
            if(_renderer.materials.Length > 1)
            {
                _normal_mats[0] = _renderer.materials[0];
                _renderer.materials = _normal_mats;
            }
        }

    }
}