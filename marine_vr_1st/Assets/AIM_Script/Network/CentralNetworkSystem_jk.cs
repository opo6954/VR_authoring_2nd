
using UnityEngine;
using System.Collections;
using Photon;
using Leap.Unity;
using UnityStandardAssets.Characters.FirstPerson;

public struct playerInfo2
{
    public bool isCurrNetworkTaskDone;
    public bool[] taskDoneInfo;
};

public class CentralNetworkSystem2 : Photon.PunBehaviour
{

    // Use this for initialization
    //for global task, (shared task status);


    GameObject[] localPlayers = null;


    playerInfo[] localPlayerMetaInfo = null;


    private int totNofPlayers = 2;

    public string[] players;
    private int globalTaskDone = -1;

    private int myClientID = -1;
    private int currPlayerNum = 0;
    public bool isMultiPlayerStart = false;
    private bool isJoinRoom = false;
    private bool isNeedUpdatePlayer = false;

    private int totTaskNum;

    private int multiTaskIndex = 4;

    private int currNetworkTaskIdx = -1;

    string playerName = "MyCharacter";

    /*

0: fire report
1: fire method
2: fire extinguishing
3: fail report
4: assemblying spot
5: passenger escape
    */

    bool isSetGlobalTask = false;

    bool isInitPlayerMetaInfo = false;

    void Start()
    {
        localPlayers = new GameObject[totNofPlayers];
        localPlayerMetaInfo = new playerInfo[totNofPlayers];

        for (int i = 0; i < totNofPlayers; i++)
        {
            localPlayerMetaInfo[i].isCurrNetworkTaskDone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isNetworkTaskDone();



        //여기 최적화 필요
        if (PhotonNetwork.playerList.Length > 0 && isJoinRoom == true)
        {

            localPlayers = GameObject.FindGameObjectsWithTag("Player");


            if (localPlayers.Length > 0)
                localPlayers[0].GetComponent<NetworkSender>().setMyClientID(myClientID);

            //Debug.Log(localPlayers[0].name);
            if (localPlayers.Length > 0)
                GameObject.Find("Canvas_UI").GetComponent<Canvas>().worldCamera = localPlayers[0].GetComponentInChildren<Camera>();

            currPlayerNum = PhotonNetwork.playerList.Length;

            if (isInitPlayerMetaInfo == false)
            {
                ///////////////// modified by JK /////////////////
                //totTaskNum = localPlayers[0].transform.FindChild("FirstPersonCharacter").GetComponent<CentralSystem>().taskManagerNames.Length;

                if (localPlayers.Length > 0)
                    totTaskNum = localPlayers[0].transform.FindChild("Model").FindChild("FirstPersonCharacter").GetComponent<CentralSystem>().taskManagerNames.Length;
                else
                    totTaskNum = 0;
                ///////////////// modified by JK /////////////////

                for (int i = 0; i < totNofPlayers; i++)
                {
                    localPlayerMetaInfo[i].taskDoneInfo = new bool[totTaskNum];
                    for (int j = 0; j < totTaskNum; j++)
                    {
                        localPlayerMetaInfo[i].taskDoneInfo[j] = false;
                    }
                }

                isInitPlayerMetaInfo = true;
            }

            isNeedUpdatePlayer = false;

        }

        //Network play start trigger
        if (isJoinRoom == true)
        {
            if (globalTaskDone == multiTaskIndex && isMultiPlayerStart == false && myClientID > -1)
            {
                int count = 0;
                for (int i = 0; i < localPlayerMetaInfo.Length; i++)
                {
                    if (localPlayerMetaInfo[i].taskDoneInfo[multiTaskIndex] == true)
                        count++;
                }

                if (count == localPlayerMetaInfo.Length)
                {
                    currNetworkTaskIdx = multiTaskIndex;
                    isMultiPlayerStart = true;
                    setOtherPlayerVisible(true);
                }

            }
            else if (isMultiPlayerStart == false)
            {

                setOtherPlayerVisible(false);
            }
        }


        if (globalTaskDone >= 0 && isSetGlobalTask == true && globalTaskDone < totTaskNum)
        {
            Debug.Log("work with " + globalTaskDone);
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                (localPlayers[i].transform.FindChild("FirstPersonCharacter").GetComponent("CentralSystem") as CentralSystem).globalTaskDone = globalTaskDone;
                (localPlayers[i].transform.FindChild("FirstPersonCharacter").GetComponent("CentralSystem") as CentralSystem).isGlobalTaskUpdate = true;
            }
            isSetGlobalTask = false;
        }

    }

    public bool isNetworkTaskDone()
    {
        if (currNetworkTaskIdx < totTaskNum)
        {
            for (int i = 0; i < localPlayerMetaInfo.Length; i++)
            {
                if (localPlayerMetaInfo[i].isCurrNetworkTaskDone == false)
                    return false;
            }

            for (int i = 0; i < localPlayerMetaInfo.Length; i++)
            {
                localPlayerMetaInfo[i].isCurrNetworkTaskDone = false;
            }

            object[] RPCdataObj = new object[2];

            RPCdataObj[0] = (object)myClientID;
            RPCdataObj[1] = (object)currNetworkTaskIdx;

            //gameObject.GetComponent<PhotonView>().RPC("updateClientTaskDone", PhotonTargets.All, RPCdataObj);
            //gameObject.GetComponent<PhotonView>().RPC("changeGlobalTaskDone", PhotonTargets.All, currNetworkTaskIdx + 1);

            ///////////////// modified by JK /////////////////
            gameObject.transform.FindChild("Model").GetComponent<PhotonView>().RPC("updateClientTaskDone", PhotonTargets.All, RPCdataObj);
            gameObject.transform.FindChild("Model").GetComponent<PhotonView>().RPC("changeGlobalTaskDone", PhotonTargets.All, currNetworkTaskIdx + 1);
            ///////////////// modified by JK /////////////////
            

            currNetworkTaskIdx++;

            return true;
        }
        else
            return false;

    }

    public override void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 };

        PhotonNetwork.JoinOrCreateRoom("lhwRoomPOWEROVER", roomOptions, TypedLobby.Default);
        //PhotonNetwork.JoinOrCreateRoom("lhwRoom", roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom() // 아무 player나 방에 join하면 callback되는 함수. --> 나만 실행되는 곳임...
    {
        base.OnJoinedRoom();
        /* [2016-04-01] 다른 코드(아래)로 대체함. */
        /* photonNework기능을 이용 방에 있는 playerList와 PlayerName가져 오기 */
        Debug.Log("Player Num : " + PhotonNetwork.playerList.Length);
        int player_num = PhotonNetwork.playerList.Length;
        //
        GameObject.Find("MainCamera").gameObject.SetActive(false); // 유저가 접속되기 전에 존재하는 메인카메라 끄기.
        GameObject player;

        if (player_num == 1)
        {
            player = PhotonNetwork.Instantiate("Player_Char/Player_3", Vector3.zero, Quaternion.identity, 0); // Player 1 spawn
            player.GetComponent<NetworkSender>().enabled = true;
            //player.GetComponent<NetworkSender>().setMyRole(GameParameter.playerRolePlay.COUNTER);//마지막 task에서의 역할 분담 부분, 일단 걍 함
            player.GetComponent<NetworkSender>().setMyRole(GameParameter.playerRolePlay.FINDER);//마지막 task에서의 역할 분담 부분, 일단 걍 함

        }
        else if (player_num == 2)
        {
            player = PhotonNetwork.Instantiate("Player_Char/Player_2", Vector3.zero, Quaternion.identity, 0); // Player 2 spawn
            player.GetComponent<NetworkSender>().enabled = true;
            //player.GetComponent<NetworkSender>().setMyRole(GameParameter.playerRolePlay.FINDER);//마지막 task에서의 역할 분담 부분, 일단 걍 함
            player.GetComponent<NetworkSender>().setMyRole(GameParameter.playerRolePlay.COUNTER);//마지막 task에서의 역할 분담 부분, 일단 걍 함

        }
        else // player_num > 2
        {
            Debug.LogError("Player number exceed the limit may be error");
            player = null;
            return;
        }

        myClientID = PhotonNetwork.player.ID - 1;
        //굳이 is mine check할 필요 없음, 이 부분은 각 client별로 개별적으로 실행되는 곳임... 0404


        PhotonNetwork.playerName = playerName;
        player.name = playerName;

        Debug.Log("Player_" + player_num.ToString() + " Spawned!");

        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = true;

        Camera camera = player.GetComponentInChildren<Camera>();
        camera.tag = "MainCamera";
        camera.enabled = true;

        AudioListener audiolistner = player.GetComponentInChildren<AudioListener>();
        audiolistner.enabled = true;

        FirstPersonController fp = player.GetComponent<FirstPersonController>();
        fp.enabled = true;

        Network_Player_MotionRecognize mr = player.GetComponentInChildren<Network_Player_MotionRecognize>();
        mr.isMe = true;

        //for the task manager for my client~!~!

        //내 캐릭터는 걍 버립시당

        ///////////////// modified by JK /////////////////
        //player.transform.FindChild("guy").FindChild("male").GetComponent<SkinnedMeshRenderer>().enabled = false;

        //player.transform.FindChild("Model").GetComponent<SkinnedMeshRenderer>().enabled = false;
        player.GetComponentInChildren<IKOrionLeapHandController>().enabled = true;
        player.GetComponentInChildren<LeapServiceProvider>().enabled = true;

        ///////////////// modified by JK /////////////////
        
        player.GetComponentInChildren<ExtinguisherWorking>().enabled = true;
        player.GetComponentInChildren<Pick_event_handler>().enabled = true;
        player.GetComponentInChildren<CentralUISystem>().enabled = true;
        player.GetComponentInChildren<CentralSystem>().enabled = true;
        player.GetComponentInChildren<CallAgent>().enabled = true;
        player.GetComponentInChildren<CountAgent>().enabled = true;

        GameObject.Find("Canvas_UI").GetComponent<Canvas>().worldCamera = Camera.main;

        (gameObject.GetComponent("MatchMaker") as MatchMaker).isJoinRoom = true;
        isJoinRoom = true;
        isSetGlobalTask = true;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    public void setOtherPlayerVisible(bool value)
    {
        for (int i = 0; i < localPlayers.Length; i++)
        {
            if (localPlayers[i].name != playerName)
            {
                localPlayers[i].transform.FindChild("guy").FindChild("male").GetComponent<SkinnedMeshRenderer>().enabled = value;
            }
        }
    }

    [PunRPC]
    public void updateClientTaskDone(object[] RPCdata)
    {
        int clientID;
        int taskNumber;
        clientID = (int)RPCdata[0];
        taskNumber = (int)RPCdata[1];

        localPlayerMetaInfo[clientID].taskDoneInfo[taskNumber] = true;
    }
    [PunRPC]
    public void updateClientLocalNetworkTaskDone(object[] RPCdata)
    {
        int clientID;
        int taskNumber;
        clientID = (int)RPCdata[0];
        taskNumber = (int)RPCdata[1];

        localPlayerMetaInfo[clientID].isCurrNetworkTaskDone = true;
    }


    [PunRPC]
    public void changeGlobalTaskDone(int taskNumber)
    {
        globalTaskDone = taskNumber;
        isSetGlobalTask = true;
    }

}
