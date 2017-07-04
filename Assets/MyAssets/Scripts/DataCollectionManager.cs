using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectionManager : MonoBehaviour {

    [SerializeField]
    Transform target;

	// Use this for initialization
	void Start () {
        if (!target)
        {
            print("No target transform found");
        }
        DebugManager.Instance.PrintToRunningLog("Hello");
        DebugManager.Instance.PrintToInfoLog("World");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
