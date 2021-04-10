using System;
using System.Collections.Generic;
using NLua;
using UnityEngine;

public class LuaEventCenter : Singleton<LuaEventCenter>
{
    private Lua lua;
    private List<string> registerList = new List<string>();

    public LuaEventCenter()
    {
        lua = new Lua();
        lua.LoadCLRPackage();
        this.RegisterFunction("print", "LPrint", this);
    }
    
    public void RegisterFunction(string inLuaName,string name,object target)
    {
        if(!registerList.Contains(inLuaName)) lua.RegisterFunction(inLuaName, target, target.GetType().GetMethod(name));
    }

    public void DoString(string code)
    {
        lua.DoString(code);
    }
    /// <summary>
    /// 作为Lua调试使用
    /// </summary>
    /// <param name="content">输出的内容</param>
    public void LPrint(string content)
    {
        Debug.Log(content);
    }
}
