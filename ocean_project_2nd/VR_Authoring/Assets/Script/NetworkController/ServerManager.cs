using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * master client가 대신 server의 역할을 수행
 * 다른 일반 client에서는 servermanager script를 off해놓기
 * server역할의 client가 할 일은 주어진 scenarioController with xml parsed 정보에 대해서 main flow를 진행할 수 있도록 하자
 * 각 단계가 넘어갈 때마다 다른 client에게 message를 보내서 처리하도록 하자(우선 단계 등을 server측에서 선택)
 * 그리고 server의 카메라는 일단 없애고 검은 창에 다른 client message만 표시해서 디버깅 쉽게 할 수 있도록 하자d
 * 
 * 
 * 
 * 
 *  
 * */

public class ServerManager : Photon.PunBehaviour {

    //각 player가 진행해야 할 clientState를 table 형태로 가지고 있음
    

    //각 player이름이 key가 되고 value는 clientState의 일렬 형태로 구성됨
    //여기에서의 clientStateAssignTable 내에서 모두 완료될 시 종료됨

    //key는 role 이름임
    public Dictionary<string, List<MessageProtocol>> clientStateAssignTable;

    //key는 role 이름, object는 그 role에 대해서 저장된 정보들
    public Dictionary<string, Dictionary<string, object>> clientTrainInfo;

    public Dictionary<string, int> trainingScoreList;


    //key는 role이름, value는 done이 되었는지 안되었는지 확인
    public Dictionary<string, bool> clientStateDoneFlags;


    private StateModuleTemplate currStateModule = null;

    public void setCurrState(StateModuleTemplate stateModule)
    {
        currStateModule = stateModule;
    }



    //xml에 의거한 role list
    public List<string> roleList = new List<string>();
    public List<string> remainRoleList = new List<string>();
    
    int roleNumber;

    //key는 role이름, value는 playerName임
    public Dictionary<string, string> playerRoleList;

    public string myPlayerName = "";
    ScenarioController sc;

    //network rpc 담당
    RPCController rpcController = null;
    
    //현재 scenario에 필요한 인원
    //일단 현재 minPlayerNumber만큼의 client의 연결만을 허락하자
    public int minPlayerNumber; 


    private bool readyTraining = false;
    private bool doingTraining = false;

    //이 부분은 table의 지금까지 진행된 공통된 순서를 말함
    //한 raw에서 모두 진행이 되었다면 idx는 올라감
    public int currTableIdx = 0;
    

    //////////////////////FOR Debuggin////////////////////

    private string[] debugRoleList;
    


    //player이름 목록, role과 idx로 bounding됨
//    public List<string> playerNameList;
    //player역할 목록, player의 이름과 idx로 bounding됨

    

    //server 부분에 추가
    

    void Awake()
    {        
        //////////////////////////////////////////////////////For Debugging...

        debugRoleList = new string[4] {"First_Extinguisher", "Extinguish_Leader", "Second_Extinguisher", "Supporter"};


        for (int i = 0; i < 4; i++)
        {
            roleList.Add(debugRoleList[i]);
            remainRoleList.Add(debugRoleList[i]);
        }






    }

    void Start()
    {
        minPlayerNumber = 1;

        playerRoleList = new Dictionary<string, string>();

        //일단 임시로 player이름과 role을 다음과 같이 매핑시키자

        playerRoleList.Add("First_Extinguisher", "Client(1)");
        playerRoleList.Add("Extinguisher_Leader", "Client(2)");
        playerRoleList.Add("Second_Extinguisher", "Client(3)");
        playerRoleList.Add("Supporter", "Client(4)");
        
    }
    /*
     * 만일 server에서의 카메라 움직임을 넣을려면 시야를 기준으로 상하좌우 움직일 수 있도록만 하자
     * */

    public void initialize()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Server_Canvas");
        if (canvas != null)
        {
            canvas.GetComponent<Canvas>().worldCamera = Camera.main;
        }


        //server에서의 main flow를 일컫음
        /*
         * 
         * 
         * 
         * 
         * DEBUGGING용 임의의 STATE를 넣음...
         * 그 대상은 MethodLearnState임
         * 
         * 
         * 
         * 
         * 
         * 
        */
        //디버깅 용도로 임의의 state를 넣어보자
        sc = new ScenarioController();//xml정보를 파싱한 scenario를 처리하는 class

        sc.setServer(this);


        ScenarioModuleTemplate tmpScenario = new ScenarioModuleTemplate();//임시 scenario

        TaskModuleTemplate tmpTask = new TaskModuleTemplate();//임시 task

        MethodLearnState tmpState = new MethodLearnState();//임시 state

        tmpState.addProperty("Select_Button", "x");
        tmpState.addProperty("VideoName", "video1,video2,video3,video4");
        tmpState.addProperty("isVdeo", true.ToString());
        tmpState.addProperty("PartCount", "4");
        tmpState.addObject("FireExtinguisher_Segment", "Fire");
        tmpState.addNetworkPlayerList("First_Extinguisher");

        tmpTask.insertState(tmpState);
        
        tmpScenario.insertTask(tmpTask);

        sc.insertScenario(tmpScenario);

        InvokeRepeating("connectControll", 1.0f, 2.0f);

        ServerLogger.Instance().addText("Server Start");
        /*
         * Player 이름은 걍 A,B,C,D로 디버깅상에서 하자
         * */
        
    }

    //만들어진 assignTable을 바탕으로 client한테 message 보내기
    //state를 통해 만들어진 table을 돌면서 client한테 message 보내는 함수임
    //executeAssignTable이 실행되는 경우는 각 state에서 buildTable을 하고 나서임
    //buildtable은 각 state별로 다를 수 있는 데 executeAssignTable은 공통되도록 만들어진 table을 실행하는 거니까 build 완료후 공통으로 message를 따라서 실행 가능함
    //일단 임의로 한 state를 만들고 이후에 수행하자

    public void executeAssignTable()
    {
        Debug.Log("In executing AssignTable...");

        bool checkAvailableTableIdx = false;

        //이 전의 결과를 바탕으로 update하는데 이전 결과는 clientInfo dicionary에 (파라미터 이름, 파라미터 값) 형식으로 저장이 되어 있으니까 필요하면 갖다가 써서 table의 파라미터를 각 state별로 수정해야 함
        currStateModule.updateClientStateTable();
        
        foreach (string key in clientStateAssignTable.Keys)
        {
            Debug.Log("In clientStateAssignTable with " + key);
            Debug.Log("Count of " + key + " in table: " + clientStateAssignTable[key].Count);

            if (clientStateAssignTable[key].Count > 0)
            {
                //원래 clientStateTemplate으로 해야되는데 걍 다른 사람들이 구현하고 있으니까 EquipmentOrderClientState로 대신 하자...

                //currTableIdx가 가능한 범위일 경우 clientstate만들기
                if (clientStateAssignTable[key].Count > currTableIdx)
                {
                    checkAvailableTableIdx = true;

                    MessageProtocol mp = clientStateAssignTable[key][currTableIdx] as MessageProtocol;

                    //만일 role에 해당하는 player가 존재할 시
                    if (playerRoleList.ContainsKey(key) == true)
                    {
                        string playerName = playerRoleList[key];

                        mp.setSenderPlayer(PhotonNetwork.playerName);
                        mp.setReceiverPlayer(playerName);

                        //해당하는 client한테 message를 보냄

                        sendMessage(mp);

                        ServerLogger.Instance().addText("Sending Message to Client" + playerName + "with state name" + "equipmentOrder...");
                    }
                }
            }
            else
            {
                //만일 존재하지 않을 경우
                //true로 넣기만 하면 됨
                clientStateDoneFlags[key] = true;
            }
        }
        //만일 false일 경우 가능한 messageProtocol이 없다는 의미
        //-->다음 state로 넘어가야함
        //어떻게 trigger를 줘야 하나...
        if (checkAvailableTableIdx == false)
        {
            clearAssignTable();
            currStateModule.MyParent.triggerNextState();
        }


    }
    
    
    

    public bool addAssignTable(string roleName, MessageProtocol mp)
    {
        if (playerRoleList.ContainsKey(roleName) == true)
        {
            clientStateAssignTable[roleName].Add(mp);

            return true;
        }
        Debug.Log("No such roleName " + roleName + " exist...");
        return false;
    }

    public void clearAssignTable()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if (PhotonNetwork.playerList[i].isMasterClient == false)
            {
                clientStateAssignTable[PhotonNetwork.playerList[i].name].Clear();
            }
        }
    }


    public void connectControll()
    {
        if (readyTraining == false)
        {
            //player가 모두 접속한 경우
            if (PhotonNetwork.playerList.Length - 1 < minPlayerNumber)
            {
                ServerLogger.Instance().addText("Lack of plyaers..." + (PhotonNetwork.playerList.Length-1).ToString() + "/" + minPlayerNumber  + ", Wait for others...");
            }
            else if (remainRoleList.Count > debugRoleList.Length - minPlayerNumber)//모든 player가 접속은 했지만 role 정보는 모두 결정되지 않은 경우임
            {
                ServerLogger.Instance().addText("All players are connected... but role is not determined...");
                ServerLogger.Instance().addText("Current role setting: ");

                //지금까지 player들에 의해 결정된 role 정보를 보여준다.
                for (int i = 0; i < roleList.Count; i++)
                {
                    if (playerRoleList.ContainsKey(roleList[i]) == true)
                    {
                        ServerLogger.Instance().addText("Player Name: " + playerRoleList[roleList[i]] + "; role Name: " + roleList[i]);
                    }
                }
            }
            else//player의 role이 모두 결정된 경우, training 시작함
            {
                ServerLogger.Instance().addText("All players are connected... All roles are assigned... Start Training...");
                CancelInvoke();
                
                //XML 기반의 roleList 초기화하기
                //일단 XML과의 연동 부분은 추후에 하자 데모를 해서 보여주는 것이 일단 가장 중요하기 때문에
                

                //clientStateAssignTable 및 playerNameList 초기화하기

                Debug.Log("Photonnetwork의 playerList 이름");
                for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
                {
                    Debug.Log(i.ToString() + "th player name is " + PhotonNetwork.playerList[i].name);

                }


                clientStateAssignTable = new Dictionary<string, List<MessageProtocol>>();
                clientStateDoneFlags = new Dictionary<string, bool>();
                trainingScoreList = new Dictionary<string, int>();

                


                clientTrainInfo = new Dictionary<string, Dictionary<string,object>>();

                foreach(string key in playerRoleList.Keys)
                {
                    clientStateAssignTable.Add(key, new List<MessageProtocol>());
                    clientStateDoneFlags.Add(key, false);//모두 false로 맞추기
                    clientTrainInfo.Add(key, new Dictionary<string, object>());
                    trainingScoreList.Add(key, 80);//임시로 점수는 80점으로 맞춰주자

                }

                readyTraining = true;
            } 
        }
    } 

    /*
     * server 0
     * p1 1
     * p2 2
     * 
     * 
     * */

    
    void Update()
    {
        //scenario에 필요한 인원이 모두 모인 경우 
        //readyTraining이 true일 때부터 state 시작이니까 readyTraining을 모든 client에서 role을 받아오면 readyTraining을 true로 놓으면 될듯
        if (readyTraining == true && doingTraining == false)
        {
            ServerLogger.Instance().addText("Scenario Trigger is start...");
            sc.triggerScenario();

            doingTraining = true;
        }
    }

    //network 입장 종료 후 callback 부름
    public void Callback_initNetwork(string playerName, RPCController _rpcController)
    {
        rpcController = _rpcController;

        myPlayerName = playerName;
        initialize();

        //실제 scenario 시작
        //scenarioController에 등록된 scenario를 바탕으로 trigger를 진행해야 함
    }

    public void addRolePlayer(string playerName, string roleName)
    {
        if (playerRoleList.ContainsKey(roleName) == true)
        {
            Debug.Log("Exist same role...");
            //다시 client한테 rpc보내야함
        }
        else
        {
            playerRoleList.Add(roleName, playerName);
            remainRoleList.Remove(roleName);
        }
    }

    public string getRoleInfoByPlayerName(string name)
    {
        foreach (KeyValuePair<string, string> items in playerRoleList)
        {
            if (items.Value == name)
            {
                return items.Key;
            }
        }
        return "";
    }


    //server단에서 client 모두 off시키는 함수
    public void turnOffClientCamera_Server(string playerName)
    {
        Debug.Log("In server through RPC");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetActive(false);
        }
    }

    //message 보내기 rpc 호출하면 됨
    public void sendMessage(MessageProtocol mp)
    {
        ServerLogger.Instance().addText("Send message...");
        ServerLogger.Instance().addText(mp.ToString());
        rpcController.photonView.RPC("sendMessage", PhotonTargets.All, mp.getParameters());
    }
    //message 받기 이후 message type에 따라 처리하면 됨
    //client-->server의 모든 message를 받는 local 함수임
    public void receiveMessage(MessageProtocol mp)
    {
        ServerLogger.Instance().addText("Receive message...");
        ServerLogger.Instance().addText(mp.ToString());


        switch (mp.type)
        {
                //connect되었다는 message가 client로부터 올 경우임--> 바로 roleInfo를 선택하도록 하기
            case MessageProtocol.MESSAGETYPE.CONNECT:                
                string[] remainRoleListArr = remainRoleList.ToArray();
                int roleLength = remainRoleListArr.Length;

                string connectPlayerName = mp.getParameterValue(0);

                string[] parameterSet = new string[1 + roleLength];

                parameterSet[0] = connectPlayerName;

                for(int i=0;i < roleLength; i++)
                {
                    parameterSet[i+1] = remainRoleListArr[i];
                }

                
                //client로부터 connect 받으면 바로 roleList 넘겨주고 선택하게 하기
                sendMessage(new MessageProtocol(MessageProtocol.MESSAGETYPE.ROLEINFO, PhotonNetwork.playerName, connectPlayerName, 1 + remainRoleListArr.Length, parameterSet));
                break;
                 
            case MessageProtocol.MESSAGETYPE.CLIENTSTATE:
                //이 부분이 clientstate정보 받을 때의 상황임
                //server에서의 clientstate받기
                //아마 clientstate를 완료했다는 message만을 받을듯

                //이 부분이 client로부터 해당 clientstate가 완료되었다는 msg를 받게 됨
                //완료되었다는 msg를 받게 되면 clientstatedoneflag에서 해당 role을 true로 함
                //이후 update에서 모든 role이 true일 경우 currIdx를 올리고 다시 executeTable을 수행한다.

                //파라미터의 첫 번째는 미션 완료 여부가 포함됨

                if (bool.Parse(mp.getParameterValue(0)) == true)
                {
                    string roleInfo = getRoleInfoByPlayerName(mp.sender);

                    /*
                     * client로부터 온 파라미터를 저장한다.
                     * client로부터 온 msg는 성공 여부 and (파라미터 이름, 파타미터 값) 형태의 짝 형식으로 들어온다 이거를 그대로 저장하면 된다
                        */
                    //그리고 이 부분에서 관련 정보를 저장해야되는데
                    //그냥 바로 들어온 파라미터를 저장을 하자
                    for (int i = 1; i < mp.numOfParameter-1; i++)
                    {
                        clientTrainInfo[roleInfo].Add(mp.getParameterValue(i), mp.getParameterValue(i + 1));
                    }

                    if(roleInfo != "")
                    {
                        clientStateDoneFlags[roleInfo] = true;
                    }

                    bool consistentCheck = true;

                    foreach (bool value in clientStateDoneFlags.Values)
                    {
                        if (value == false)
                        {
                            consistentCheck = false;
                        }
                    }

                    //만일 모든 role에서 현재 current단계에서 완료가 될 경우
                    //currIdx를 올리고 다시 executeTable을 수행한다
                    if (consistentCheck == true)
                    {
                        //모두 다시 false로 reset하기
                        foreach (string roleNameFlag in clientStateDoneFlags.Keys)
                        {
                            clientStateDoneFlags[roleNameFlag] = false;
                        }

                        //현재 단계 올리고
                        currTableIdx++;

                        //다시 table을 실행한다.
                        executeAssignTable();
                    }
                }

                break;
            case MessageProtocol.MESSAGETYPE.ROLEINFO:
                //server-->client로부터 선택한 role 받기 
                  
                string playerName = mp.getParameterValue(0);
                string roleName = mp.getParameterValue(1);
                addRolePlayer(playerName, roleName);

                ServerLogger.Instance().addText("Player " + playerName + " 's role is selected; " + roleName);
                ServerLogger.Instance().addText("Now role info selection info is: ");

                for (int i = 0; i < roleList.Count; i++)
                {
                    if (playerRoleList.ContainsKey(roleList[i]) == true)
                    {
                        ServerLogger.Instance().addText("Player Name: " + playerRoleList[roleList[i]] + "; role Name: " + roleList[i]);
                    }
                }
                break;

            case MessageProtocol.MESSAGETYPE.TRAININGEND:
                //server-->client msg만 있어서 상관 없음
                Debug.Log("Wrong msg.... TRAININGEND Cannot reach server...");
                break;

            case MessageProtocol.MESSAGETYPE.TRAININGINFO:
                //server-->client msg만 있어서 상관 없음
                Debug.Log("Wrong msg... TRAININGINFO Cannot reach server...");
                break;
        }
    }

    //모든 training종료임
    //종료될 때 모든 client한테 각각의 훈련 점수를 보내줘야 함
    public void setTrainingEnd()
    {
        foreach (string roleName in playerRoleList.Keys)
        {
            MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.TRAININGEND, PhotonNetwork.playerName, playerRoleList[roleName], 1, new string[] { trainingScoreList[roleName].ToString() });
            sendMessage(mp);

        }
    }


    /*
     * Server에서 Client로의 현재 진행중인 scenario 넘겨주기
     * Scenario 넘기는 부분은 scenario가 trigger될 때 이 함수 불러야 함
     * */

    public void passScenarioInfo(string scenario)
    {
        foreach (string roleName in playerRoleList.Keys)
        {
            MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.TRAININGINFO, PhotonNetwork.playerName, playerRoleList[roleName], 2, new string[] {"Scenario", scenario});
        }
    }

    /*
     * Server에서 Client로의 현재 진행중인 task 넘겨주기
     * task 넘기는 부분은 task가 trigger될 때 이 함수 불러야 함
     * */

    public void passTaskInfo(string task)
    {
        foreach(string roleName in playerRoleList.Keys)
        {
            MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.TRAININGINFO, PhotonNetwork.playerName, playerRoleList[roleName], 2, new string[] {"Task",task});
            sendMessage(mp);
        }
    }




    
}
