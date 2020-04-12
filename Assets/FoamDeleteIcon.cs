using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamDeleteIcon : FoamIconManager
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void PerformAction()
    {
        base.PerformAction();
        m_data.SceneObjs.Remove(m_data.CurrentSelectionObj);
        GameObject.Destroy(m_data.CurrentSelectionObj);
        m_data.CurrentSelectionObj = null;
    }
}
