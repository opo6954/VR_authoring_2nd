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

    public string myPlayerName = "";
    ScenarioController sc;
    
    //현재 scenario에 필요한 최소 인원
    public int minPlayerNumber = 1;

    private bool readyTraining = false;
    private bool doingTraining = false;


    //player이름 목록, role과 idx로 bounding됨
    public List<string> playerNameList;
    //player역할 목록, player의 이름과 idx로 bounding됨
    public List<string> playerRoleList;

    //server 부분에 추가
    

    void Awake()
    {        
    }

    void Start()
    {
        
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
        //디버깅 용도로 임의의 state를 넣어보자
        sc = new ScenarioController();//xml정보를 파싱한 scenario를 처리하는 class

        sc.setServer(this);


        ScenarioModuleTemplate tmpScenario = new ScenarioModuleTemplate();//임시 scenario

        TaskModuleTemplate tmpTask = new TaskModuleTemplate();//임시 task

        StateModuleTemplate tmpState = new StateModuleTemplate();//임시 state

        tmpTask.insertState(tmpState);

        tmpScenario.insertTask(tmpTask);

        sc.insertScenario(tmpScenario);

        InvokeRepeating("connectControll", 1.0f, 2.0f);

        ServerLogger.Instance().addText("Server Start");
    }

    public void connectControll()
    {
        if (readyTraining == false)
        {
            if (PhotonNetwork.playerList.Length - 1 < minPlayerNumber)
            {
                ServerLogger.Instance().addText("Lack of plyaers..." + (PhotonNetwork.playerList.Length-1).ToString() + "/" + minPlayerNumber  + ", Wait for others...");
            }
            else
            {
                ServerLogger.Instance().addText("All players are connected... Start Training...");
                CancelInvoke();
                readyTraining = true;
            }
        }
    }

    
    void Update()
    {
        //scenario에 필요한 인원이 모두 모인 경우 
        if (readyTraining == true && doingTraining == false)
        {
            sc.triggerScenario();    

            doingTraining = true;
        }
    }

    //network 입장 종료 후 callback 부름
    public void Callback_initNetwork(string playerName)
    {
        myPlayerName = playerName;
        initialize();

        //실제 scenario 시작
        //scenarioController에 등록된 scenario를 바탕으로 trigger를 진행해야 함
        
    }
    //내구 구조간의 연락 관련

    public void getMessagesFromState(MessageProtocol[] messageSet)
    {
        
    }
    [PunRPC]
    public void sendMessage(MessageProtocol messages)
    {
        
        //message 내용을 바탕으로 client한테 보내면 된다.
    }

    //Network 관련
}
