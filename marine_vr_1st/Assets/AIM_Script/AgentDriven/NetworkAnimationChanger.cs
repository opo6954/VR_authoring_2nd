using UnityEngine;
using System.Collections;

public class NetworkAnimationChanger : Photon.MonoBehaviour
{

    public Animator animator;
    public Transform playerPos;
    private Transform pos;

    PhotonView pv;
    AgentFollower af;

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;




    public bool isMoveFirst = false;

    // Use this for initialization
    void Start()
    {
        pos = this.transform;
        pv = gameObject.GetComponent<PhotonView>();
        af = gameObject.GetComponent<AgentFollower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPos != null)
        {
            gameObject.GetComponent<PhotonView>().RPC("setTarget", PhotonTargets.All, playerPos.position);
        }

        if (!pv.isMine && isMoveFirst == true)
        {
            pos.position = Vector3.Lerp(pos.position, this.correctPlayerPos, Time.deltaTime * 5);
            pos.rotation = Quaternion.Lerp(pos.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }


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

    [PunRPC]
    public void setAnimatorValue(int value)
    {
        animator.SetInteger("newParam", value);
    }
    [PunRPC]
    public void setTarget(Vector3 _targetPos)
    {
        af.targetPos = _targetPos;
    }
    [PunRPC]
    public void setMovementOn(bool value)
    {
        af.isTargetOn = value;
    }
}
