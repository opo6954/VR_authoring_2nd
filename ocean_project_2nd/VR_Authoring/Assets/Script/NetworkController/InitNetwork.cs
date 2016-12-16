using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*
 * 네트워크 초기화임
 * 이름은 다른 scene에서 결정하기(키보드를 사용하기 위해서임)
 * 일단 server 혹은 master client는 server역할을 하고
 * master client는 가장 먼저 들어온 사람으로 배정하자
 * 물론 이 과정 전에 xml 읽어오는 과정이 필요하겠지
 * 이름 설정한 후 역할도 부여하고 이후 main scenario flow를 network상에서 진행시켜야 합네당
 * 
 * Client와 Master client 모두의 접속이 포함됨
 * 
 * 
 * */

//component와의 dependent를 연결하는 형태
public class InitNetwork : Photon.PunBehaviour{

    string _gameVersion = "1";
    public int maxPlayer = 4;

    static string myPlayerName = "";

    void Awake()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Informational;//log 하는 정도 체크임
        PhotonNetwork.autoJoinLobby = false;//로비 자동 입장 여부

        PhotonNetwork.automaticallySyncScene = true;//모든 client의 자동 sync 여부

        Debug.developerConsoleVisible = true;


    }
    //이름 입력, 버튼 누르는 것은 걍 키보드로 입력받자
    //이름 입력하고 직업 선택한 후 버튼을 누르면 connect되도록 하자
    //이 부분은 hmd로 구현을 해야될텐데 음...
    void Start()
    {
        GameObject sn = GameObject.Find("SendingName");
        if (sn != null)
        {
            IntroManager im = sn.transform.GetComponent<IntroManager>();

            myPlayerName = im.playerName;


            GameObject.Destroy(sn);

            Connect();

        }
        else
        {
            myPlayerName = "Client";//걍 일단 master로 하자

            Connect();

        }
    }

    //Playerprefs local에 저장하는것임

    //이 부분이 실제로 photon cloud에 연결되는 부분임
    void Connect()
    {
        Debug.Log("연결부분...");
        
        PhotonNetwork.ConnectUsingSettings(_gameVersion);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Master랑 연결됨...");

        PhotonNetwork.JoinOrCreateRoom("marine room", new RoomOptions() { maxPlayers = byte.Parse(this.maxPlayer.ToString()) }, null);
        
    }

    

    public override void OnJoinedRoom()
    {
        Debug.Log("Join the room");
        
        //player의 이름 설정인데 중복될 경우 뒤에 숫자 붙이기
        PhotonPlayer[] p = PhotonNetwork.playerList;

        string playerNameModified = myPlayerName;
        
        while (true)
        {
            int cnt = 0;
            int idx = 1;
            
            for (int i = 0; i < p.Length; i++)
            {
                //일단 이렇게만 하자, 원래 계속 중복되는 이름을 찾아야 하는데...
                cnt = i;
                if (p[i].name == playerNameModified)
                {
                    Debug.Log("same");
                    playerNameModified = myPlayerName + "(" + idx.ToString() + ")";
                    Debug.Log(playerNameModified);
                    idx++;
                    break;
                }
            }
            if (cnt == p.Length-1)
            {
                myPlayerName = playerNameModified;
                break;
            }
        }
        
        PhotonNetwork.playerName = myPlayerName;

        Debug.Log("my player name is " + PhotonNetwork.playerName);

        for (int i = 0; i < p.Length; i++)
        {
            //만일 masterClient일 경우
            if(p[i].isMasterClient)
            {
                Debug.Log("master clinet name is " + p[i].name);
            }
        }

        GameObject.FindWithTag("MainCamera").SetActive(false);

        if (PhotonNetwork.isMasterClient == true)
        {
            //server
            
            Debug.Log("Server setting...");

            GameObject server = GameObject.Instantiate(Resources.Load<GameObject>("Player_Network/Player_Server"));
            server.GetComponent<ServerManager>().Callback_initNetwork(PhotonNetwork.playerName);
            
        }
        else
        {
            //client
            
            Debug.Log("Client setting...");

            //player는 network상에서 prefab을 소환해야하니까 일단 photonnetwork로 instantiate를 해야함
            //이후에 player를 훝어서 걍 꺼놓는 거로 하자 그거는 일단 기본 flow를 만든 후에 하자

            GameObject player = PhotonNetwork.Instantiate("Player_Network/Player_Own_Network", Vector3.zero, Quaternion.identity, 0);
            player.GetComponent<ClientManager>().Callback_initNetwork(PhotonNetwork.playerName);            
        }
    }
}
