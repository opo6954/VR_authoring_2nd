using UnityEngine;
using System.Collections;

public class TurnOffChild : Photon.MonoBehaviour{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    [PunRPC]
    public void turn_on_child(string name)
    {
        gameObject.transform.FindChild(name).gameObject.SetActive(true);
    }
}
