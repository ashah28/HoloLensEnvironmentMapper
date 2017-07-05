using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataCollectionManager : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    StreamWriter sr;

    bool recording = false;
    string fileName = "data";
    string filePath;
    float recStartTime = 0;

    // Use this for initialization
    void Start () {
        if (!target)
        {
            print("No target transform found");
        }

        InitFileToWrite();

        if (File.Exists(filePath + ".txt"))
            DebugManager.Instance.PrintToInfoLog("File created successfully: " + filePath);
        else
            DebugManager.Instance.PrintToInfoLog("Failed to create:" + filePath);

        //DebugManager.Instance.PrintToRunningLog("Hello");
        //DebugManager.Instance.PrintToInfoLog("World");
    }

    void Update()
    {
        if(recording)
            WriteToDisk();
    }

    void OnDisable()
    {
        if (sr != null)
            sr.Dispose();
    }

    public void OnToggleClick()
    {
        recording = !recording;
        DebugManager.Instance.PrintToInfoLog("Recording: " + recording);

        //when the recording starts
        if (recording)
        {
            InvokeRepeating("WritePositionToScreen", 0, 0.2f);
            recStartTime = Time.time;
        }
        //when the recording stops
        else
        {
            CancelInvoke("WritePositionToScreen");
            if (sr != null)
                sr.Dispose();
            DebugManager.Instance.PrintToInfoLog("Rec. duration:" + (Time.time - recStartTime));
        }
    }

    void WritePositionToScreen()
    {
        string data = (target.position) + " :: " + target.eulerAngles;
        DebugManager.Instance.PrintToRunningLog(data);
    }

    void InitFileToWrite()
    {
        try
        {
            string currentFileName = fileName + " " + String.Format("{0:MM-dd,hh-mm-ss}", System.DateTime.Now);

            //editor
            if (Application.isEditor)
                filePath = Application.persistentDataPath + "/Data/" + currentFileName;
            //deployed app
            else
                filePath = Application.persistentDataPath + "/" + currentFileName;

            if (File.Exists(filePath + ".txt"))
            {
                Debug.Log(filePath + " already exists. Incrementing it.");
                InitFileToWrite();
            }
            else
            {
                sr = File.CreateText(filePath + ".txt");
                sr.AutoFlush = true;
            }
        }
        catch(Exception e)
        {
            DebugManager.Instance.PrintToInfoLog("Exception:" + e);
        }
    }

    void WriteToDisk()
    {
        string rowData = (Time.time - recStartTime) + " : " + transform.position + " -- " + transform.eulerAngles;
        sr.WriteLine(rowData);
    }
}
