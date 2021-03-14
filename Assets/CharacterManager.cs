using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    public Sprite[] Characters;

    private Character[] characters;
    void Start()
    {
        characters = GetComponentsInChildren<Character>();
        LuaEventCenter.Instance.RegisterFunction("character", "SwitchCharacter", this);

    }


    void Update()
    {
        
    }

    public void SwitchCharacter(string name,string side)
    {
        if(name == string.Empty)
        {
            foreach (var item in characters)
            {
                if (item.side.ToString() == side)
                {
                    item.FadeOut();
                }
            }
        }
        else
        {
            foreach (var imag in Characters)
            {
                if (imag.name == name)
                {
                    foreach (var item in characters)
                    {
                        if (item.side.ToString() == side)
                        {
                            item.SwitchCharacter(imag, name);
                        }
                    }
                }
            }
        }


    }
}
