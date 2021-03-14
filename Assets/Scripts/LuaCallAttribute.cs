
using System;
[AttributeUsage(AttributeTargets.Method,AllowMultiple = false,Inherited = true)]
public class LuaCallAttribute : Attribute
{
    public LuaCallAttribute()
    {
        
    }
}
