using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SelectPannel : MonoBehaviour
{
    public GameObject selectButton;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishSelect()
    {
        foreach (var item in GetComponentsInChildren<Button>())
        {
            Destroy(item.gameObject);
        }
    }
}
