using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulationObjPastingBehavior : StateMachineBehaviour
{

    private Transform _copiedObj;
    private FoamDataManager _data;

    private bool _isReleased = false;

    private Renderer _copiedRenderer;
    private Color _copiedOGColor;

    private int _animCount = 0;
    private int _transStep = 0;
    private Vector3 _initalScale;

    private int _hash_objMenuClosedBool = Animator.StringToHash("ObjMenuClosedBool");


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _data = GameObject.FindGameObjectWithTag("foamDM").GetComponent<FoamDataManager>();
        animator.SetBool(_hash_objMenuClosedBool, false);

        _data.StateIndicator.GetComponent<Text>().text = "Pinch to Place";

        _copiedObj = null;
        _animCount = 0;
        _transStep = 0;


        _copiedObj = Instantiate(_data.CurrentSelectionObj.transform, _data.ObjCreatedPos, Quaternion.identity);
        _copiedObj.gameObject.name = _copiedObj.gameObject.name.Replace("(Clone)", "").Trim();

        // grabbing stuff?

        _copiedRenderer = _copiedObj.gameObject.GetComponent<Renderer>();
        _copiedRenderer.material.color = _data.ObjManiOGColor;
        _copiedOGColor = _copiedRenderer.material.color;

        _initalScale = _copiedObj.transform.localScale;
        _copiedObj.transform.localScale = Vector3.zero;

        _isReleased = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_copiedObj && !_isReleased)
        {
            _transStep = FoamUtils.AnimateWaveTransparency(_copiedRenderer, _transStep);
        }

        // play scale animation first
        if (_copiedObj && _animCount < FoamUtils.ObjCreatedAnimTime)
        {
            _animCount = FoamUtils.AnimateGrowSize(_animCount, _initalScale, _copiedObj, _data.ObjCreatedPos);
            return;
        }

        if (_copiedObj)
        {
            if (!_isReleased)
            {
                _copiedObj.position = _data.ObjCreatedPos;
            }

            if (_data.ActiveGC.bufferedGesture() == "pinch")
            {
                _isReleased = true;
                Debug.Log("copied-------------" + _copiedObj.gameObject.name);

                if (_copiedObj.gameObject.name == "FoamCone")
                {
                    GameObject.Destroy(_copiedObj.gameObject);
                    _copiedObj = Instantiate(_data.ConePrefab, _data.ObjCreatedPos, Quaternion.identity);
                }

                _copiedRenderer.material.color = _copiedOGColor;
                //grabbing stuff?

                _data.SceneObjs.Add(_copiedObj.gameObject);
                Debug.Log("!!--!! num obj in scene: " + _data.SceneObjs.Count);
                animator.SetBool(_hash_objMenuClosedBool, true);
            }
        }
        else
        {
            animator.SetBool(_hash_objMenuClosedBool, true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
