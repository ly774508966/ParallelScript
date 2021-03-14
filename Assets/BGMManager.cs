using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BGMManager : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        LuaEventCenter.Instance.RegisterFunction("switchBGM", "SwitchBGM", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchBGM(string name)
    {
        if(name != string.Empty)
        {
            foreach (var item in clips)
            {
                if (item.name == name)
                {
                    var twnner = DOTween.To(() => source.volume, x => source.volume = x, 0, 1);
                    twnner.onComplete += () =>
                    {
                        source.clip = item;
                        DOTween.To(() => source.volume, x => source.volume = x, 1, 1);
                        source.Play();
                    };

                }
            }
        }
        else
        {
            var twnner = DOTween.To(() => source.volume, x => source.volume = x, 0, 1);
            twnner.onComplete += () =>
            {
                source.clip = null;
                source.Stop();
            };
        }

    }

}
