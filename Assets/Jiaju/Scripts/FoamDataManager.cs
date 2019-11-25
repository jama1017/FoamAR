using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamDataManager : MonoBehaviour
{
    public GameObject m_createMenu;
    public GameObject m_maniMenu;

    // Start is called before the first frame update
    void Start()
    {
        m_createMenu.SetActive(false);
        m_maniMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
