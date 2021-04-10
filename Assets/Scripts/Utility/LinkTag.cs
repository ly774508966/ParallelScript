using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LinkTag : MonoBehaviour
{
    public Gradient rainbow;
    
    private TMP_Text tmpText;
    private Dictionary<string, List<TMP_LinkInfo>> linksdict = new Dictionary<string, List<TMP_LinkInfo>>();

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        tmpText.ForceMeshUpdate();
    }

    private void Start()
    {
    }

    public void ForceUpdate()
    {
        linksdict.Clear();
        tmpText.ForceMeshUpdate();
        var links = tmpText.textInfo.linkInfo;
        
        
        foreach (var item in links)
        {
            
            if (item.GetLinkID() != String.Empty)
            {
                if(!linksdict.ContainsKey(item.GetLinkID()))
                {
                    List<TMP_LinkInfo> info = new List<TMP_LinkInfo>{item};
                    linksdict.Add(item.GetLinkID(),info);
                }
                else
                {
                    linksdict[item.GetLinkID()].Add(item);
                }
            }
        }
        foreach (var items in linksdict)
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
        for (int i = 0; i < mesh.colors.Length; i++)
        {
            mesh.colors[i] = tmpText.color;
        }
        foreach (var item in linksdict)
        {
            if(item.Key=="Wave") ApllyEffect(item.Key,mesh,TextWobble);
            if(item.Key=="Rainbow") ApllyEffect(item.Key,mesh,Rainbow);
        }
        tmpText.canvasRenderer.SetMesh(mesh);
    }

    void ApllyEffect(string id,Mesh mesh,Action<TMP_LinkInfo,Mesh> effctFunc)
    {
        List<TMP_LinkInfo> Infos;
        if (linksdict.TryGetValue(id, out Infos))
        {
            foreach (var info in Infos)
            {
                effctFunc(info,mesh);
            }
        }
    }
    void TextWobble(TMP_LinkInfo info,Mesh src)
    {
        var vertices = src.vertices;
        for (int i = info.linkTextfirstCharacterIndex; i <info.linkTextfirstCharacterIndex + info.linkTextLength; i++)
        {
            TMP_CharacterInfo c = tmpText.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            Vector3 offset = Wobble(Time.time + i)*2;
            vertices[index] += offset;
            vertices[index + 1] += offset;
            vertices[index + 2] += offset;
            vertices[index + 3] += offset;
        }
        src.vertices = vertices;
    }
    void Rainbow(TMP_LinkInfo info,Mesh src)
    {
        var colors = src.colors;
        var vertices = src.vertices;
        for (int i = info.linkTextfirstCharacterIndex; i <info.linkTextfirstCharacterIndex + info.linkTextLength; i++)
        {
            TMP_CharacterInfo c = tmpText.textInfo.characterInfo[i];
            int index = c.vertexIndex;
            colors[index] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index].x*0.001f, 1f));
            colors[index + 1] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 1].x*0.001f, 1f));
            colors[index + 2] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 2].x*0.001f, 1f));
            colors[index + 3] = rainbow.Evaluate(Mathf.Repeat(Time.time + vertices[index + 3].x*0.001f, 1f));
        }
        src.colors = colors;
    }

    Vector2 Wobble(float time) 
    {
        return new Vector2(Mathf.Sin(time*15f), Mathf.Cos(time*12f));
    }
}
