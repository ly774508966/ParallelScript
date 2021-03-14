using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public enum Side
{
    Left,Right,Mid
}

public class Character : MonoBehaviour
{
    public string ownner;
    public Side side;

    private SpriteRenderer renderer;
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //Dialog.instance.RegisterFunction(new Action(Test).Method.Name,this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    public void SwitchCharacter(Sprite imag,string name)
    {
        if(name !=ownner)
        {
            renderer.sprite = imag;
            renderer.color = Color.clear;
            renderer.DOColor(Color.white, 0.5f);
        }
        else
        {
            renderer.sprite = imag;
        }

    }

    public void FadeOut()
    {
        Tweener tweener = renderer.DOColor(Color.clear,0.5f);
        tweener.onComplete += () => renderer.sprite = null;
        ownner = string.Empty;
    }
}
