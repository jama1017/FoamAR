using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandScale : ICommand
{
    private GameObject _target;
    private Vector3 _prevScale;
    private Vector3 _afterScale;

    public CommandScale(GameObject target, Vector3 prevScale, Vector3 afterScale)
    {
        _target = target;
        _prevScale = prevScale;
        _afterScale = afterScale;
    }

    public void Undo()
    {
        // need to redraw the tabs
        _target.transform.localScale = _prevScale;
    }

    public void Redo()
    {
        _target.transform.localScale = _afterScale;

    }
}
