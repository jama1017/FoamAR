using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandMove : ICommand
{
    private GameObject _target;
    private Vector3 _prevPos;
    private Quaternion _prevRot;
    private Vector3 _afterPos;
    private Quaternion _afterRot;

    public CommandMove(GameObject target)
    {
        _target = target;
    }

    public void Undo()
    {
        _target.transform.position = _prevPos;
        _target.transform.rotation = _prevRot;
    }

    public void Redo()
    {
        _target.transform.position = _afterPos;
        _target.transform.rotation = _afterRot;
    }
}
