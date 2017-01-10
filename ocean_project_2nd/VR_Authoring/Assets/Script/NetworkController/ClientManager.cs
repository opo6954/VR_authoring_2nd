using UnityEngine;
using System.Collections;
/*
 * master client를 제외한 다른 client들은 clientmanager에 따라 진행됨
 * master client에서는 servermanager script를 off해놓자
 * 일단 training에 따른 역할 선택할 수 있도록 하자-->HMD 시야 한 가운데에 점을 만들고 그 점 있는 곳에 joystick으로 버튼을 누르면 선택 가능하도록 하자
 * 다른 client가 들어올 때까지 대기하자
 * 다른 client가 들어올 때 마다 다른 player 전용 prefab init하고(물론 여기서 내 player prefab과 다른 player prefab이 같을 필요는 없음
 * 아마 다른 player prefab에서는 간단한 모션(무전기 하고 있는 모습을 구름 형식(?)으로 표시하자)과 위치, animation에 대한 sync가 필요하다-->이 부분은 일단 기존 구현된 것 활용해보자
 * 이후 master client로부터 message를 받아서 처리할 수 있는 구조 만들기d
 * */
//client쪽 관리
public class ClientManager : Photon.PunBehaviour {

    public string myPlayerName;
     
    ClientStateController clientStateController = null;
    PlayerTemplate myPlayerInfo = null;
    RPCController rpcController=null;
    
    void Awake()
    {
   
    }

    void Start()
    {
        
         



        /*
         * 
         * 
         * 
         * 
         * 
        // For Client Debugging... LHW
         * 
         * 
         * 
         * 
         * 
        */

        
        /*
        Debug.Log("Init Client...");

        Initialize();

        ClientStateExample cse = new ClientStateExample();

        cse.setParameter1("r");
        cse.setParameter2(20);

        cse.MyClientManager = this;
         
        

        clientStateController.setCurrClientState(cse);
        */


    }

    void Update()
    {
        
    }

    //client 초기화부분 joystick input 정보 등을 초기화할 수 있음
    //client일 시 자신만의 player 소환해야 함
    void Initialize()
    {
        myPlayerInfo = gameObject.GetComponent<PlayerTemplate>();


        InputDeviceSettings.Instance().mappingJoystickButton();

        clientStateController = gameObject.AddComponent<ClientStateController>(); 

        GameObject canvas = GameObject.FindGameObjectWithTag("Server_Canvas");

        if(canvas != null)
            canvas.SetActive(false);//server를 위한 canvas는 제거

        
        //server한테 connect message 보내기
        sendMessage(new MessageProtocol(MessageProtocol.MESSAGETYPE.CONNECT, PhotonNetwork.playerName, PhotonNetwork.masterClient.name, 1, new string[1] { PhotonNetwork.playerName }));
    }

    //network 입장 종료 후 callback 불림
    //server한테도 알려주기
    public void Callback_initNetwork(string playerName, RPCController _rpcController)
    {
        Debug.Log("Callback, my name is " + playerName);
        //rpc controller 등록
        rpcController = _rpcController;

        Initialize();
    }


    public void setScenarioNameText(string text)
    {
        Debug.Log("Current scenario is " + text);
    }

    public void setTaskNameText(string text)
    {
        Debug.Log("Current task is " + text);
    }

    


    /*
     * parameter의 구성:
     *      * object[] 형태
     * object[0]: type: int
     * object[1]: sender: string
     * object[2]: receiver: string
     * object[3]: name: string(clientstate의 이름이 들어감)
     * object[4]: number of parameters: int
     * object[5]~: parameters: object...
     * */



    //clientstate가 완료될 시 완료된 flag와 보낼 파라미터를 가지고 message 보내기
    public void finishClientState()
    {
        //LHWLHWLHW
    }

    //messageProtocol로부터 clientstate만들기
    public void buildClientState(MessageProtocol mp)
    {
        string clientStateName = mp.getClientStateName();

        ClientStateModuleTemplate csmt = System.Activator.CreateInstance(System.Type.GetType(clientStateName)) as ClientStateModuleTemplate;


        //clientstate에 필요한 파라미터를 messageprotocol로부터 가져온다.
        //message protocol로부터 파라미터를 읽어서 clientstate에 저장한다.
        csmt.convertMP2ClientState(mp.getParameterGroupForClientState());


        //이후 그 clientstate를 실행한다
        clientStateController.setCurrClientState(csmt);

    //이 부분이 제일 중요함d
    }
    //역할 정보 설정기능
    public void controlRoleInfo(MessageProtocol mp)
    {
        string[] roleList = new string[mp.numOfParameter - 1];

        for (int i = 0; i < mp.numOfParameter - 1; i++)
        {
            roleList[i] = mp.getParameterValue(i + 1);
        }
        StartCoroutine("chooseRoleInfo", roleList);
    }
    //server로부터 scenario 정보나 task 정보를 받아서 표시하기
    public void controlTrainInfo(MessageProtocol mp)
    {
        string infoType = mp.getParameterValue(0);

        //null 아닐경웅
        if(infoType != null)
        {
            if (infoType == "Scenario")
            {
                setScenarioNameText(mp.getParameterValue(1));
            }

            else if (infoType == "Task")
            {
                setTaskNameText(mp.getParameterValue(1));
            }

        }
        
    }
    //모든 training 종료
    public void finishiTraining(MessageProtocol mp)
    {
        int score = int.Parse(mp.getParameterValue(0));
        myPlayerInfo.MyScore = score;

        Debug.Log("Training end... my final score is " + score.ToString());
        //이 부분에서 score 화면 보여주기 해야함
    }

    //현재 clientstate 종료될 시 sendMessage로 server한테 완료 flag와 보낼 정보 설정해서 보내기
    public void passClientStateInfo(MessageProtocol mp)
    {
        sendMessage(mp);
    }

    
    
    








   

    public void turnOffClientCamera_Client(string playerName)
    {
        //모든 player 가져오기

        Debug.Log("In TurnOffClientCamera_Client...");

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("현재 player: " + players.Length);

        Debug.Log("my name is " + myPlayerName);
        Debug.Log("my network name is " + PhotonNetwork.playerName);

        Debug.Log("In turnOffRPC, network player length: " + PhotonNetwork.playerList.Length);
        

        //다른 player의 관련 요소 끄기
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetPhotonView().name != myPlayerName)
            {
                //client Manager 끄기
                players[i].GetComponent<ClientManager>().enabled = false;
                //Player Template 끄기
                players[i].GetComponent<PlayerTemplate>().enabled = false;

                Transform fpsController = players[i].transform.FindChild("FPSController");

                //Character Controller 끄기
                fpsController.GetComponent<CharacterController>().enabled = false;
                //오디오 소스 끄기
                fpsController.GetComponent<AudioSource>().enabled = false;

                //1인칭 Controller 끄기
                fpsController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

                //1인칭 Character 끄기
                fpsController.FindChild("FirstPersonCharacter").gameObject.SetActive(false);

            }
        }
    }


    //role 고르는 부분인데 일단은 걍 r버튼 누르면 제일 처음 role을 선택하는 거로 하자
    IEnumerator chooseRoleInfo(string[] roleList)
    {
        Debug.Log("In the choose role process...");

        while (true)
        {
            if (Input.GetKey("r") == true)
            {
                myPlayerInfo.MyRoleName = roleList[0];

                Debug.Log("My role is choosing...");
                Debug.Log("My role name is " + myPlayerInfo.MyRoleName);

                //이 부분에서 message 보내자
                sendMessage(new MessageProtocol(MessageProtocol.MESSAGETYPE.ROLEINFO, PhotonNetwork.playerName, PhotonNetwork.masterClient.name, 2, new string[2] { PhotonNetwork.playerName, myPlayerInfo.MyRoleName}));

                yield break;
            }
            yield return null;
        }
    }


    //message 보내기 rpc 호출하면 됨
    public void sendMessage(MessageProtocol mp)
    {
        //client에서의 message관련 logging...
        Debug.Log("Send message...");
        Debug.Log(mp.ToString());
        rpcController.photonView.RPC("sendMessage", PhotonTargets.All, mp.getParameters());
    }



    //message 받기 이후 message type에 따라 처리하면 됨
    public void receiveMessage(MessageProtocol mp)
    {
        Debug.Log("Receive message...");
        Debug.Log(mp.ToString());

        switch (mp.type)
        {
            case MessageProtocol.MESSAGETYPE.CONNECT:
                //client-->server 통신이기 때문에 필요 없음 만일 일로 msg가 오면 잘못 온거임
                Debug.Log("Client doesn't get the connect message.. please check your code...");
                break;
            case MessageProtocol.MESSAGETYPE.CLIENTSTATE:
                //새로운 clientState를 만들기
                //이 부분 구현하

                buildClientState(mp);

                break;
            case MessageProtocol.MESSAGETYPE.ROLEINFO:
                //server로부터 role List를 받게 됨

                controlRoleInfo(mp);

                

                break;
            case MessageProtocol.MESSAGETYPE.TRAININGEND:
                finishiTraining(mp);
                //이 부분 구현하기
                break;
            case MessageProtocol.MESSAGETYPE.TRAININGINFO:
                controlTrainInfo(mp);
                //이 부분 구현하기
                break;
        }
    }

    //특정 action에 등장하는 animation을 sync한다
    public void actionAnimationSync(CharacterAniState aniState)
    {
        int state=-1;
        switch(aniState)
        {
            case CharacterAniState.TALKING:
                state = 0;
                break;
            case CharacterAniState.PHONE:
                state = 1;
                break;
            case CharacterAniState.WALK:
                state = 2;
                break;
        }
        rpcController.photonView.RPC("animationSync", PhotonTargets.All, state);
    }




    /*
    *     [PunRPC]
   public void sendRoleInfo(string[] roleInfo)
   {
       string type = roleInfo[0];

       if (type == "connect")
       {
           if (PhotonNetwork.isMasterClient == false)
           {
               string sender = roleInfo[1];
               string receiver = roleInfo[2];

               if (PhotonNetwork.playerName == receiver)
               {
                   int numberofRole = int.Parse(roleInfo[3]);

                   string[] roleList = new string[numberofRole];

                   for (int i = 0; i < numberofRole; i++)
                   {
                       roleList[i] = roleInfo[i + 4];
                   }

                   localClientManager.chooseRoleInfo(roleList);
               }
           }
       }
       else if (type == "request")
       {
           string sender = roleInfo[1];
           string receiver = roleInfo[2];

           if (PhotonNetwork.isMasterClient == true)
           {
               string playerName = roleInfo[3];
               string roleName = roleInfo[4];

               localServerManager.addRolePlayer(playerName, roleName);
           }
       }
   }
    * */

    /*
     * 이 부분은 TRAININGINFO에서 처리해야 함
     *  [PunRPC]
    public void sendScenarioInfoToClient(object[] parameters)
    {
        //client한테만 해당
        if (PhotonNetwork.isMasterClient == false)
        {

            string sender = parameters[0].ToString(); ;
            string scenarioName = parameters[1].ToString();

            localClientManager.setScenarioNameText(scenarioName);
        }
    }

    [PunRPC]
    public void sendTaskInfoToClient(object[] parameters)
    {
        //client한테만 해당
        if (PhotonNetwork.isMasterClient == false)
        {
            string sender = parameters[0].ToString();
            string taskName = parameters[1].ToString();

            localClientManager.setTaskNameText(taskName);
        }
    }

     * */


    //network 관련 함수, server로의 message를 보내거나 받는다 이 부분을 통해서만 server와 통신이 가능하다

    //Server-->Client로의 message를 수신한다.

   
}
