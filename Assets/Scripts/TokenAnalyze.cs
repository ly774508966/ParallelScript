using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// 记号
/// </summary>
public struct Mark
{
    public int Key;
    public string Value;

    public Mark(int key,string value)
    {
        Key = key;
        Value = value;
    }
        

}
/// <summary>
/// 词法分析
/// </summary>
public class TokenAnalyze
{
    /// <summary>
    /// 读取Token流
    /// </summary>
    /// <param name="content"></param>
    /// <returns>要读取的字符串</returns>
    public static Dictionary<int, string> ReadToken(string content)
    {
        var mark = Regex.Matches(content, @"{|}");
        Dictionary<int, string> token = new Dictionary<int, string>();
        Stack<int> markStack = new Stack<int>();

        for (int i = 0; i < mark.Count; i++)
        {
            if (mark[i].Value == "{")
            {
                markStack.Push(mark[i].Index);
            }
            else if (mark[i].Value == "}")
            {
                int start = markStack.Pop();
                string buffer = content.Substring(start, mark[i].Index - start+1);
                token.Add(start, buffer);
            }
        }
        return token;
    }

    /// <summary>
    /// 把KeyWords去掉
    /// </summary>
    /// <param name="text">需要除关键字的字符串</param>
    /// <param name="mark">返回的标记位置以及关键词</param>
    /// <returns>除去关键字的结果</returns>
    public static string RemoveKeyWords(string text,out List<Mark> mark)
    {
        string tempString = text;
        var token = ReadToken(text);
        mark = new List<Mark>();
        while (token.Count!=0)
        {
            token = ReadToken(tempString);
            foreach (var item in token)
            {
                tempString = tempString.Remove(item.Key, item.Value.Length);
                mark.Add(new Mark(item.Key,item.Value.Substring(1, item.Value.Length-2)));
                break;
            }
        }

        return tempString;
    }
    /// <summary>
    /// 把文本文件的字符切分成一行一行的
    /// </summary>
    public static string[] ClipLine(string text)
    {
        return Regex.Split(text, @"\n");
    }
}
