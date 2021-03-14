using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using NLua;
using DG.Tweening;
using System.Reflection;
using System.IO;

public class Dialog : MonoSingleton<Dialog>
{
    public float waitTime = 0.2f;
    private Text dialogBox;
    private Text nameBox;
    private string[] buffer;
    
    private int currentLine =0;
    private bool canNext = true;
    public bool isPause = false;
    private float varTime = 0.2f;
    private SelectPannel select;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogBox = transform.GetChild(0).Find("Dialog").GetComponent<Text>();
        nameBox = transform.GetChild(0).Find("Name").GetComponent<Text>();


        LuaEventCenter.Instance.RegisterFunction("w", "WaitForWhile", this);
        LuaEventCenter.Instance.RegisterFunction("speaker", "SetSpeaker", this);
        LuaEventCenter.Instance.RegisterFunction("branch", "Branch", this);
        LuaEventCenter.Instance.RegisterFunction("load", "LoadScript", this);
        
        FileStream fileStream = new FileStream(Application.streamingAssetsPath+@"\" + "Start.txt", FileMode.Open, FileAccess.Read);
        TextReader textReader = new StreamReader(fileStream);
        buffer = WordsAnalyze.ClipLine(textReader.ReadToEnd());
        
        StartCoroutine(PlayText(buffer[currentLine]));
        currentLine++;
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
    /// <param name="Rawtext">切片后的字符串</param>
    /// <returns></returns>
    IEnumerator PlayText(string Rawtext)
    {
        canNext = false;
        List<Mark> mark;
        string text = WordsAnalyze.RemoveKeyWords(Rawtext, out mark);
        dialogBox.text = string.Empty;
        if (text.Length != 0)
        {
            for (int i = 0; i < text.Length; i++)
            {
                dialogBox.text += text[i];
                foreach (var job in mark)
                {
                    if (job.Key == i)
                    {
                        LuaEventCenter.Instance.DoString(job.Value);
                    }
                }

                yield return new WaitForSeconds(varTime);

                varTime = waitTime;
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
    /// 文字等待
    /// </summary>
    /// <param name="time">时间</param>
    public void WaitForWhile(float time)
    {
        varTime = time;
    }
    /// <summary>
    /// 设置发言人
    /// </summary>
    /// <param name="speaker">发言人名字</param>
    public void SetSpeaker(string speaker)
    {
        nameBox.text = speaker;
    }


    /// <summary>
    /// 加载其他脚步
    /// </summary>
    /// <param name="filename">文件名称(需要后缀名)</param>
    public void LoadScript(string filename)
    {
        FileStream fileStream = new FileStream(Application.streamingAssetsPath + @"\" + filename,FileMode.Open,FileAccess.Read);
        TextReader textReader = new StreamReader(fileStream);
        buffer = WordsAnalyze.ClipLine(textReader.ReadToEnd());
        currentLine = 0;
        StartCoroutine(PlayText(buffer[currentLine++]));//执行加载好后的语句
    }

    /// <summary>
    /// 分支
    /// </summary>
    /// <param name="choise">选项</param>
    public void Branch(params string[] choise)
    {
        select = FindObjectOfType<SelectPannel>();
        isPause = true;
        foreach (var item in choise)
        {

            GameObject go = Instantiate(select.selectButton, select.transform);
            go.GetComponentInChildren<Text>().text = item;
            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                LuaEventCenter.Instance.DoString($"choise = '{item}'");
                StartCoroutine(PlayText(buffer[currentLine]));//选择完之后直接跳到分支语句
                isPause = false;
                select.FinishSelect();
            });
        }
    }

}
