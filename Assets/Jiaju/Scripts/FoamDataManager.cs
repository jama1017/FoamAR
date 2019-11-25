using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamDataManager : MonoBehaviour
{
    public GameObject CreateMenu;
    public GameObject ManiMenu;
    public GameObject ActiveHand;
    public GameObject ActiveIndex;
    public GameObject ActivePalm;
    private float _triggerRadius = 2.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        CreateMenu.SetActive(false);
        ManiMenu.SetActive(false);

        ActiveIndex = ActiveHand.transform.GetChild(1).GetChild(2).gameObject;
        ActivePalm = ActiveHand.transform.GetChild(5).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float TriggerRadius
    {
        get { return _triggerRadius; }
        set { _triggerRadius = value; }
    }

}
