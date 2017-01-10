using UnityEngine;
using System.Collections;

public class Demo_tmp : MonoBehaviour {
    bool isTeleport = false;
    public Transform myTransform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isTeleport == false)
        {
            myTransform.transform.position = new Vector3(126.79f, -40.123f, -0.175f);
            myTransform.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -269.031f, 0.0f));

            isTeleport = true;
        }
	}
}
