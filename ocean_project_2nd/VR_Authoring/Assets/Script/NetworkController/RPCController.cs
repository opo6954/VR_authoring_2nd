using UnityEngine;
using System.Collections;
/*
 * 이 부분에서 모든 RPC를 관리한다.
 * 
 * Server, Client 모두에서 온 RPC를 관리한다.
 * 
 * message가 포함된 함수: scenario 진행 관련 정보만 주고받기
 * 
 * info가 포함된 함수: server에서 client로의 global한 scenario나 task 완료 여부 등을 client한테 일방적으로 보내준다
 * 
 * message format을 몽땅 다 통일하자~!~!~!~!~!~!~!
 * 
 * */
public class RPCController : Photon.PunBehaviour {


    public string localPlayerName = "";

    //client일 경우 localPlayer를 가집니다.
    public ClientManager localClientManager = null;
    public ServerManager localServerManager = null;




    

    //training과 관련있는 message의 send와 receive를 rpc로 정립한다.
    [PunRPC]
    public void sendMessage(string[] parameters)
    {
        MessageProtocol mp = new MessageProtocol(parameters);

        string receiver = mp.receiver;
        
        if (receiver == PhotonNetwork.masterClient.name)//server 받음
        {
            localServerManager.receiveMessage(mp);//server에서 처리
        }
        else if (receiver != PhotonNetwork.masterClient.name)//client받음
        {
            localClientManager.receiveMessage(mp);//client에서 처리
        }
    }
   


    

    

    //message에 포함되지 않는 특수한 message... 아마 다른 류의 state 동기화 등의 message도 여기에 포함될듯

    //turnoff 관련 껍데기 함수
    [PunRPC]
    public void turnOffClientCamera(string playerName)
    {
        //client일 경우
        if (PhotonNetwork.isMasterClient == false)
        {
            localClientManager.turnOffClientCamera_Client(playerName);
        }
        else//server일 경우
        {
            localServerManager.turnOffClientCamera_Server(playerName);
        }
    }

    //prefab name 바꾸는 함수, 모두에 적용
    [PunRPC]
    public void changePlayerPrefabName(string originName, string playerName)
    {
        Debug.Log("change my Prefab Name...");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("In change Name RPC, player length is : " + players.Length);
        Debug.Log("In change Name RPC, network player length: " + PhotonNetwork.playerList.Length);

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].name == originName)
            {
                players[i].name = playerName;
                break;
            }
        }
    }




    //아마 player들의 position, rotattion 외의 다른 부분을 sync 맞춰야 할 때 쓰일 예정인 함수들 밑의 message와는 독립적으로 운영되어야 한다
    //이 부분은 각 character가 자신의 clientstate에 맞춰서 animation을 하는 부분을 sync로 맞출 때에 쓰일 예정임
    [PunRPC]
    public void setState()
    {
    }





    
   
    
   

    


   
}
