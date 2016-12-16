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
        Debug.Log("Init Client...");

        Initialize();

        ClientStateExample cse = new ClientStateExample();

        cse.setParameter1("r");
        cse.setParameter2(20);

        clientStateController.setCurrClientState(cse);


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
    }

    //network 입장 종료 후 callback 불림
    //server한테도 알려주기
    public void Callback_initNetwork(string playerName)
    {
        Debug.Log("Callback, my name is " + playerName);

        Initialize();
    }

    //network 관련 함수, server로의 message를 보내거나 받는다 이 부분을 통해서만 server와 통신이 가능하다
    [PunRPC]
    void MsgReceiver()
    {
    }
    [PunRPC]
    void MsgSender()
    {
    }
}
