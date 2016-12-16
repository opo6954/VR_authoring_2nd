using UnityEngine;
using System.Collections;

public class Enable_Audio_Source : Photon.MonoBehaviour{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [PunRPC]
    public void enableAudio()
    {
        gameObject.GetComponent<AudioSource>().enabled = true;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
