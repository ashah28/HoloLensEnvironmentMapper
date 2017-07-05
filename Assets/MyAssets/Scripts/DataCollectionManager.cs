using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectionManager : MonoBehaviour {

    [SerializeField]
    Transform target;

    bool recording = false;

	// Use this for initialization
	void Start () {
        if (!target)
        {
            print("No target transform found");
        }

        //DebugManager.Instance.PrintToRunningLog("Hello");
        //DebugManager.Instance.PrintToInfoLog("World");
    }

    public void OnToggleClick()
    {
        DebugManager.Instance.PrintToInfoLog("Clicked");

        if(recording)
        {

        }
        else
        {

        }

        recording = !recording;
    }
}
