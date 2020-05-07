using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble.Functions.Grab;

namespace Portalble
{
    [RequireComponent(typeof(Portalble.Functions.Grab.Grabable))]
    public class Selectable : MonoBehaviour
    {

        private bool _isTarget = false;
        private Renderer _renderer;
        private Material _outline_mat;

        //private Material[] _normal_mats;
        private Material[] _outline_mats;

        public float _outline_width = 0.003f; // for regularly sized

        private Vector3 _preSnapPos = new Vector3(0.0f, 0.0f, 0.0f);
        private bool _isSnapped = false;

        public Material m_vertOutline; // to make more permanent

        public bool IsSmallObj = false;
        private bool IsColliderReset = true;

        private Transform _grabCollider;
        private Vector3 _grabColliderOGScale;

        private bool _isDuringGrabbingProcess = false;

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = FocusUtils.ObjNormalColor;

            // outline material
            _outline_mat = Instantiate(m_vertOutline);
            _outline_mat.SetFloat("_BodyAlpha", 0.0f);
            _outline_mat.SetFloat("_OutlineWidth", _outline_width);
            _outline_mat.SetColor("_OutlineColor", FocusUtils.ObjRankedColor);

            //_normal_mats = new Material[1];
            //_normal_mats[0] = _renderer.materials[0];

            _outline_mats = new Material[2];
            _outline_mats[0] = _renderer.materials[0];
            _outline_mats[1] = _outline_mat;

            _grabCollider = this.transform.GetChild(0);
            _grabColliderOGScale = _grabCollider.localScale;
        }


        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Grabable>().IsBeingGrabbed())
            {
                RemoveHighestRankContour();
            }

            if (IsSmallObj && !_isDuringGrabbingProcess)
            {
                _grabCollider.localScale = _grabColliderOGScale;
            }
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



        public void SetHighestRankContour()
        {
            _outline_mats[0] = _renderer.materials[0];

            if (_renderer.materials.Length <= 1) // if only one material, add
            {
                Debug.Log("DOTT new added");
                _renderer.materials = _outline_mats;
                return;
            }

            // if two material and second is not dotted, skip
            if (_renderer.materials[1].HasProperty("_OutlineColor") && !_renderer.materials[1].HasProperty("_OutlineDot")) // if there is already an outline (finger enter or grabbing)
            {
                Debug.Log("DOTT skipped");
                return;
            }

            // if two material and second is dotted, update
            Debug.Log("DOTT last called");
            _outline_mats[1].SetFloat("_OutlineWidth", _outline_width);
            _renderer.materials = _outline_mats;
        }

        public void SetSnapped()
        {
            _isSnapped = true;
            _isDuringGrabbingProcess = true;
            _preSnapPos = this.transform.position;

            if (IsSmallObj)
            {
                _grabCollider.transform.localScale = FocusUtils.SmallObjSnapExpandedScale;
                IsColliderReset = false;
            }
        }


        public void RemoveHighestRankContour()
        {
            if (_renderer.materials.Length > 1)
            {
                //_normal_mats[0] = _renderer.materials[0];
                //_renderer.materials = _normal_mats;
                if (_renderer.materials[1].HasProperty("_OutlineColor") && !_renderer.materials[1].HasProperty("_OutlineDot")) // if there is already an outline (finger enter or grabbing)
                {
                    return;
                }

                Debug.Log("DOTT removed");
                Material mat = _renderer.materials[0];
                _renderer.materials = new Material[] { mat };

                //_outline_mats[0] = _renderer.materials[0];
                //if (_renderer.materials[1].HasProperty("_OutlineDot"))
                //{
                //    _outline_mats[1].SetFloat("_OutlineWidth", 0.0f);
                //    _renderer.materials = _outline_mats;
                //}
            }
        }

        public Vector3 GetSnappedPosition()
        {
            Vector3 res = new Vector3(0.0f, 0.0f, 0.0f);

            if (_isSnapped)
            {
                res = _preSnapPos;
            }
            else
            {
                res = this.transform.position;
            }

            _isSnapped = false;

            return res;
        }


        //public void ResetColliderSize()
        //{
        //    if (!Grab.Instance.IsGrabbing)
        //    {
        //        if (IsSmallObj && !IsColliderReset)
        //        {
        //            Debug.Log("COLLIDERSIZE: called in select");
        //            _grabCollider.localScale = _grabColliderOGScale;
        //            IsColliderReset = true;
        //        }
        //    }
        //}

        public void ResetColliderSizeToOG()
        {
            if (IsSmallObj)
            {
                Debug.Log("COLLIDERSIZE: called OG");
                _grabCollider.localScale = _grabColliderOGScale;
                _isDuringGrabbingProcess = false;
            }
        }
    }

}