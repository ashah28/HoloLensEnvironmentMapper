using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class DebugManager : Singleton<DebugManager> {

    [SerializeField]
    Text runningLog;

    [SerializeField]
    Text infoLog;

    public DebugManager instance;

	// Use this for initialization
	void Start () {
        if (!runningLog)
            Debug.LogError("Visual log not found");
	}
	
    public void PrintToRunningLog(string message)
    {
        string data = GetCurrentTimestamp() + " : " + message + "\n";
        print(data);
        runningLog.text = data + runningLog.text.Substring(0, Mathf.Min(runningLog.text.Length, 1000));
    }

    public void PrintToInfoLog(string message)
    {
        string data = GetCurrentTimestamp() + " : " + message + "\n";
        print(data);
        infoLog.text = data + infoLog.text.Substring(0, Mathf.Min(infoLog.text.Length, 1000));
    }

    string GetCurrentTimestamp()
    {
        return Time.time.ToString("000.000");
    }
}
