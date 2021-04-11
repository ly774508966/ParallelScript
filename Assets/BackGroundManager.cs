using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackGroundManager : MonoBehaviour
{
    public Sprite[] backGrounds;


    private new SpriteRenderer renderer;
    void Start()
    {
        LuaEventCenter.Instance.RegisterFunction("background", "SwitchBackGround", this);
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchBackGround(string name)
    {
        foreach (var item in backGrounds)
        {
            if(item.name == name)
            {
                renderer.sprite = item;
                renderer.color = Color.clear;
                renderer.DOColor(Color.white, 0.5f);
            }
        }
    }
}
