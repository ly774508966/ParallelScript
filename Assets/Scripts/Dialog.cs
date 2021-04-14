using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using TMPro;

public class Dialog : MonoSingleton<Dialog>,IPause
{
    
    
    [SerializeField]private float textAnimateTime = 0.2f;
    
    private TMP_Text tmpText;
    private TMPEffect tmpEffect;
    private Text nameBox;
    
    private string[] buffer;
    private int currentLine;
    private bool canNext = true;
    private bool isPause;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tmpText = transform.GetChild(0).Find("Dialog").GetComponent<TMP_Text>();
        nameBox = transform.GetChild(0).Find("Name").GetComponent<Text>();
        tmpEffect = GetComponentInChildren<TMPEffect>();
        
        
        LuaEventCenter.Instance.RegisterFunction("speaker", "SetSpeaker", this);
        LuaEventCenter.Instance.RegisterFunction("branch", "Branch", this);
        LuaEventCenter.Instance.RegisterFunction("load", "LoadScript", this);

        LoadScript("Start.txt");
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown&&canNext&&!isPause)
        {
            StartCoroutine(PlayText(buffer[currentLine]));
            currentLine++;
        }
    }

    /// <summary>
    /// 显示文字的协程
    /// </summary>
    IEnumerator PlayText(string Rawtext)
    {
        canNext = false;
        
        string text = TokenAnalyze.RemoveKeyWords(Rawtext, out var mark);
        
        tmpEffect.SetText(text);//更新文字信息
        float waitTime = textAnimateTime/ (tmpText.textInfo.characterCount+1);//计算单个文字出现时间

        if (text.Length != 0)
        {
            for (int i = 0; i < tmpText.textInfo.characterCount; i++)
            {
                LuaEventCenter.Instance.DoString($"w={waitTime}");//设置等待时间
                foreach (var job in mark)
                {
                    if (job.Key == i)
                    {
                        LuaEventCenter.Instance.DoString(job.Value);
                        
                    }
                }
                yield return new WaitForSeconds((float)LuaEventCenter.Instance.GetNumber("w"));
                
                tmpText.maxVisibleCharacters = i;
                
            }
        }
        else
        {
            foreach (var job in mark)
            {
                LuaEventCenter.Instance.DoString(job.Value);
            }
        }
        canNext = true;
    }
    
    /// <summary>
    /// 设置发言人名称
    /// </summary>
    public void SetSpeaker(string speaker)
    {
        nameBox.text = speaker;
    }


    /// <summary>
    /// 加载其他脚本
    /// </summary>
    /// <param name="filename">文件名称(需要后缀名)</param>
    public void LoadScript(string filename)
    {
        FileStream fileStream = new FileStream(Application.streamingAssetsPath + @"\" + filename,FileMode.Open,FileAccess.Read);
        TextReader textReader = new StreamReader(fileStream);
        buffer = TokenAnalyze.ClipLine(textReader.ReadToEnd());
        currentLine = 0;
        StartCoroutine(PlayText(buffer[currentLine++]));//执行加载好后的语句
    }

    /// <summary>
    /// 分支
    /// </summary>
    /// <param name="choise">选项s</param>
    public void Branch(params string[] choise)
    {
        var select = FindObjectOfType<SelectPannel>();
        Pause();
        foreach (var item in choise)
        {

            GameObject go = Instantiate(select.selectButton, select.transform);
            go.GetComponentInChildren<Text>().text = item;
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                LuaEventCenter.Instance.DoString($"choise = '{item}'");
                TokenAnalyze.RemoveKeyWords(buffer[currentLine], out var mark);
                LuaEventCenter.Instance.DoString(mark[0].Value);
                Resume();
                select.FinishSelect();
            });
        }
    }
    
    

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }
}
