using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoSingleton<MessageBox>
{
    public void Start()
    {
        //Dialog.Instance.isPause = true;
        LuaEventCenter.Instance.RegisterFunction("messagebox", "ShowMessageBox", this);
    }

    public void Done()
    {
        Dialog.Instance.isPause = false;
        Destroy(transform.parent.gameObject);
    }
    
    public void ShowMessageBox(string content)
    {
        var go = Instantiate<GameObject>(Resources.Load<GameObject>("MessageBox"),FindObjectOfType<Canvas>().transform);
        go.GetComponentInChildren<Text>().text = content;
    }
}
