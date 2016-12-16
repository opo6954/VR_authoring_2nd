using UnityEngine;
using System.Collections;

public class NetworkSender : MonoBehaviour
{

    // Use this for initialization

    private int myClientID;

    private GameParameter.playerRolePlay myRole;
    

    CentralNetworkSystem centralNetworkSystemInstance;

    private bool tmp = false;

    void Start()
    {
        centralNetworkSystemInstance = GameObject.Find("NetworkScript").GetComponent<CentralNetworkSystem>();
    }

    public void setMyRole(GameParameter.playerRolePlay _role)
    {
        myRole = _role;
    }

    public GameParameter.playerRolePlay getMyRole()
    {
        return myRole;
    }

    public void setMyClientID(int ID)
    {
        myClientID = ID;

        tmp = true;
    }
    public int getMyClientID()
    {
        return myClientID;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void changeGlobalTaskDone(int taskNumber)
    {
        object[] RPCdataObj = new object[2];

        RPCdataObj[0] = (object)myClientID;
        RPCdataObj[1] = (object)taskNumber;

        centralNetworkSystemInstance.GetComponent<PhotonView>().RPC("updateClientTaskDone", PhotonTargets.All, RPCdataObj);

        if (GameParameter.isSinglePlayer == true)
        {
            centralNetworkSystemInstance.changeGlobalTaskDone(taskNumber);

        }
        else
        {
            centralNetworkSystemInstance.GetComponent<PhotonView>().RPC("changeGlobalTaskDone", PhotonTargets.All, taskNumber);
        }
    }

    public void changeLocalNetworkTaskDone(int taskNumber)
    {
        object[] RPCdataObj = new object[2];

        RPCdataObj[0] = (object)myClientID;
        RPCdataObj[1] = (object)taskNumber;

        centralNetworkSystemInstance.GetComponent<PhotonView>().RPC("updateClientLocalNetworkTaskDone", PhotonTargets.All, RPCdataObj);
    }
    //public void updateClientLocalNetworkTaskDone(object[] RPCdata)

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
