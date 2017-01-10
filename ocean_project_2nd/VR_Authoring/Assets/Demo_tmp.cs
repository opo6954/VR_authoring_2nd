using UnityEngine;
using System.Collections;

public class Demo_tmp : MonoBehaviour {
    bool isFirstTeleport = false;
    bool isSecondTeleport = false;
    public Transform myTransform;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isSecondTeleport == false)
        {
            if(Input.GetKeyDown("r") == true)
            {
            myTransform.transform.position = new Vector3(126.79f, -40.123f, -0.175f);
            myTransform.transform.rotation = Quaternion.Euler(new Vector3(0.0f, -269.031f, 0.0f));

            isSecondTeleport = true;
        }
            if (isFirstTeleport == false)
            {
                if (Input.GetKeyDown("t") == true)
                {
                    myTransform.transform.position = new Vector3(-22.15f, 0.86f, 5.3f);
                    myTransform.transform.rotation = Quaternion.Euler(new Vector3(0.0f,-268.4f,0.0f));

                    isFirstTeleport = true;

                }
            }
        }
	}
}
