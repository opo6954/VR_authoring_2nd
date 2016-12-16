using UnityEngine;
using System.Collections;

/*
Lerp를 이용해 player position의 sync를 smooth하게 하도록 함.
*/

public class Network_Player_Position : MonoBehaviour {

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    private Transform pos;
    private PhotonView pv;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.isWriting)
        {
            // We own this player : send the others our data
            stream.SendNext(pos.position);
            stream.SendNext(pos.rotation);
        }
        else
        {
            // Network Player, recieve data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    // Use this for initialization
    void Start () {
        
        pos = this.transform;
        pv = transform.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!pv.isMine)
        {
            pos.position = Vector3.Lerp(pos.position, this.correctPlayerPos, Time.deltaTime * 5);
            pos.rotation = Quaternion.Lerp(pos.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }	
	}
}
