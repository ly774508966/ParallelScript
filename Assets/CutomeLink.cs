using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutomeLink : MonoBehaviour
{
    public Gradient rainbow;
    
    private TMP_Text tmpText;
    //private Mesh mesh;
    //private Vector3[] vertices;
    private List<TMP_LinkInfo> waveLinks = new List<TMP_LinkInfo>();
    void Start()
    {
        Application.targetFrameRate = 60;
        tmpText = GetComponent<TMP_Text>();
        tmpText.ForceMeshUpdate();
        var links = tmpText.textInfo.linkInfo;
        foreach (var item in links)
        {
            print($"{item.GetLinkID()}:{item.GetLinkText()}");
        }

        //StartCoroutine(OrderShowText());
    }

    private void Update()
    {

    }
}
