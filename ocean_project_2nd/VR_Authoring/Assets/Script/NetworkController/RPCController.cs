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
    public CharactorAnimationController localClientAnimator = null;
    public ServerManager localServerManager = null;

    //training과 관련있는 message의 send와 receive를 rpc로 정립한다.
    [PunRPC]
    public void sendMessage(string[] parameters)
    {
        MessageProtocol mp = new MessageProtocol(parameters);

        string receiver = mp.receiver;

        Debug.Log("On RPC SendMessage...");
        Debug.Log("Sender: " + mp.sender);
        Debug.Log("Receiver: " + receiver);


        //receiver를 통해 message가 제대로 보내졌는지 확인하자.
        if (receiver == PhotonNetwork.playerName)
        {
            if (receiver == PhotonNetwork.masterClient.name)//server 받음
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    Debug.Log("On RPC, " + i.ToString() + "th contents is " + parameters[i]);
                }

                Debug.Log("For localServerManager, " + localServerManager.name);

                localServerManager.receiveMessage(mp);//server에서 처리
            }
            else if (receiver != PhotonNetwork.masterClient.name)//client받음
            {
                localClientManager.receiveMessage(mp);//client에서 처리
            }
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




    //Player의 animatino을 네트워크상에서 맞춰준다
    [PunRPC]
    public void animationSync(int state)
    {
        if (PhotonNetwork.isMasterClient == false)
        {
            CharacterAnimationState cas = CharacterAnimationState.IDLE;

            switch (state)
            {

                case 0:
                    cas = CharacterAnimationState.BUTTON;
                    break;
                case 1:
                    cas = CharacterAnimationState.IDLE;
                    break;
                case 2:
                    cas = CharacterAnimationState.PHONE;
                    break;
                case 3:
                    cas = CharacterAnimationState.TALKING;
                    break;
                case 4:
                    cas = CharacterAnimationState.WALK;
                    break;
            }

            localClientAnimator.triggerAnimation(cas);
        }
    }





    
   
    
   

    


   
}
