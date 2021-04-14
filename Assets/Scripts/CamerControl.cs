using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CamerControl : MonoBehaviour
{
    public static CamerControl Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LuaEventCenter.Instance.RegisterFunction("shake", "CameraShake", this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CameraShake(float intensity)
    {
        Camera.main.DOShakePosition(intensity, 1, 20);
    }

}
