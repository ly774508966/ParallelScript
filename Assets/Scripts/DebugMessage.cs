
using UnityEngine;

public class DebugMessage : Singleton<DebugMessage>
{
    public DebugMessage()
    {
        Application.logMessageReceived += HandleLog; 

    }
    void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            string message = string.Format("condition = {0} \n stackTrace = {1} \n type = {2}", condition, stackTrace, type);
            MessageBox.Instance.ShowMessageBox(message);
        }
    }
}
