using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

/// <summary>
/// 文字特效函数接口
/// </summary>
public interface ITextEffect
{
    /// <summary>
    /// 处理字符网格
    /// </summary>
    /// <param name="info">link标签信息</param>
    /// <param name="src">网格</param>
    void HandleEffect(TMP_LinkInfo info, Mesh src);
}

/// <summary>
/// TMP文字自定义特效
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class TMPEffect : MonoBehaviour
{
    private TMP_Text tmpText;
    private readonly Dictionary<string, List<TMP_LinkInfo>> linkDir = new Dictionary<string, List<TMP_LinkInfo>>();
    private Dictionary<string,ITextEffect> effectsDir = new Dictionary<string,ITextEffect>();
    
    private void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        GenerateEffects();
    }
    
    /// <summary>
    /// 读取实现ITextEffect接口是类并实例化添加到effectsDir里
    /// </summary>
    private void GenerateEffects()
    {
        var effects = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t =>t.GetInterfaces().Contains(typeof(ITextEffect))).ToArray();
        foreach (var effect in effects)
        {
            ConstructorInfo ctor = effect.GetConstructor(new Type[0]);
            var textInterface = ctor.Invoke(null) as ITextEffect;
            effectsDir.Add(effect.Name,textInterface);
        }
    }

    /// <summary>
    /// 更新Link标签信息
    /// </summary>
    public void ForceUpdate()
    {
        linkDir.Clear();
        tmpText.ForceMeshUpdate();
        var links = tmpText.textInfo.linkInfo;
        
        foreach (var item in links)
        {
            if (item.GetLinkID() != String.Empty)
            {
                if(!linkDir.ContainsKey(item.GetLinkID()))
                {
                    List<TMP_LinkInfo> info = new List<TMP_LinkInfo>{item};
                    linkDir.Add(item.GetLinkID(),info);
                }
                else
                {
                    linkDir[item.GetLinkID()].Add(item);
                }
            }
        }
        foreach (var items in linkDir)
        {
            foreach (var item in items.Value)
            {
                print($"{items.Key} {item.GetLinkText()}");
            }
        }
    }
    private void Update()
    {
        tmpText.ForceMeshUpdate();
        Mesh mesh = tmpText.mesh;
        
        foreach (var item in linkDir)
        {
            ApllyEffect(item.Key,mesh,effectsDir[item.Key]);
        }
        tmpText.canvasRenderer.SetMesh(mesh);
    }
    
    /// <summary>
    /// 应用效果器
    /// </summary>
    /// <param name="id">link的值</param>
    /// <param name="mesh">TMP的网格</param>
    /// <param name="effectFunc">效果类的接口</param>
    void ApllyEffect(string id,Mesh mesh,ITextEffect effectInterface)
    {
        if (linkDir.TryGetValue(id, out var linkInfos))
        {
            foreach (var info in linkInfos)
            {
                effectInterface.HandleEffect(info,mesh);
            }
        }
    }
}
