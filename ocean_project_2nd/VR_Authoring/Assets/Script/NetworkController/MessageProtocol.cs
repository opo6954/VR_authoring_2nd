using UnityEngine;
using System.Collections;
/*
 * MessageProtocol
 * Server와 Client 사이의 message protocol 및 format을 담당함
 * 
 * 
 * //connet는 굳이 할 필요 없을 듯
 * MessageProtocol 종류
 * 1. Connect
 * Sender: Server, Client
 * Receiver: Server, Client
 * Message Type: Connect
 * Name: 없음
 * Parameter: client 이름 , client의 scenario에서의 역할
 * ExtraInfo: 없음
 * 
 * 처음 connect한 이후 다시 message protocol을 통해 다시 한 번 server에게 client의 출석 여부 알려주기 혹은 역할 알려주기?
 * 일단 client가 server에게 보내고 server가 client에게 알았다고 답변, 이후 server가 wait을 보냄 이후 server의 state message 받을 때마다 처리하면 될 듯
 * 
 * 
 * 2. Wait
 * Sender: Server
 * Receiver: Client
 * Message Type: Wait
 * Name: 없음
 * Parameter: 없음
 * ExtraInfo: 없음
 * Server로부터의 message가 올 때까지 대기
 * 
 * 3. StateProcess
 * 임무 부여
 * Sender: Server
 * Receiver: Client
 * Message Type: CLIENTSTATE
 * Name: ClientState이름
 * Parameter: clientState에서 쓰일 대사나 obj 이름 등의 저작도구로부터의 정보
 * ExtraInfo: 성공여부 Server->Client로 갈 때는 0, Client->Server로 갈 때 성공일시 1, 실패일 시 0인데 일단 0인 경우는 버그 등으로 멈췄을 때 보내줘야 할 듯
 * 
 * 임무 성공
 * Sender: Client
 * Receiver: Server
 * Message Type: CLIENTSTATE
 * Name: ClientState이름
 * Parameter:
 * [0]: 성공/실패 여부
 * [1]: 파라미터 이름
 * [2]: 파라미터 내용
 * [3]: 파라미터 이름
 * [4]: 파라미터 내용
 * 
 * 이런 식으로 보내주자
 *
 * 
 * 
 * 
 * 
 * 
 * 4. End
 * Sender: Server
 * Receiver: Client
 * Message Type: End
 * Name: 없음
 * Parameter: 점수?
 * ExtraInfo: 없음
 * 
 * 모든 scenario가 끝날 시 종료하는 message, client에서도 관련 답변을 해줘야 할듯
 * 
 * 모든 message를 담당하자(물론 serialize되는거 제외하고
 * 
 * 
 *  
 * 
 * 
 * */
/*
 * 
 * */



//기본적인 message 구성을 다음과 같이 하자
/*server-->client에서의 메시지 구성
     * object[] 형태
     * object[0]: type: int
     * object[1]: sender: string
     * object[2]: receiver: string
     * object[3]: number of parameters: int
     * object[4]~: parameters: object...
     * 
     * */
/*
 * message종류
 * ClientState관련
 * Role관련
 * Connect(client의 server 연결할 때)
 * Info Scenario/Task 정보 관련
 * End(모든 훈련 종료)
 * */

public class MessageProtocol {

    public enum MESSAGETYPE
    {
        CONNECT=0,
        ROLEINFO=1,
        CLIENTSTATE=2,
        TRAININGINFO=3,
        TRAININGEND=4
    }

    public MESSAGETYPE type;
    public string sender;
    public string receiver;
    public int numOfParameter;
    private string[] parameters;//파라미터만 있는 형태



    private string[] packingMessages;//sender, receiver정보가 모두 패킹된 형태

    public override string ToString()
    {
        return "MSG " + MessageProtocol.getTypeNameStr(type) + " FROM " + sender + " TO " + receiver;
    }

    public static string getTypeNameStr(MESSAGETYPE mp)
    {
        switch (mp)
        {
            case MESSAGETYPE.CONNECT:
                return "CONNECT";                
            case MESSAGETYPE.ROLEINFO:
                return "ROLEINFO";
            case MESSAGETYPE.CLIENTSTATE:
                return "CLIENTSTATE";                
            case MESSAGETYPE.TRAININGINFO:
                return "TRAININGINFO";                
            case MESSAGETYPE.TRAININGEND:
                return "TRAININGEND";                
            default:
                return "UNKNOWN";
        }
    }

    public void setParameterValue(int idx, string val)
    {
        parameters[idx] = val;
    }

    public string getParameterValue(int idx)
    {
        if (numOfParameter > idx && idx >= 0)
        {
            return parameters[idx];
        }
        return "";
    }

    public void unpackingMessage()
    {
        type = (MESSAGETYPE)int.Parse(packingMessages[0]);
        sender = packingMessages[1];
        receiver = packingMessages[2];
        numOfParameter = int.Parse(packingMessages[3]);

        for (int i = 0; i < numOfParameter; i++)
        {
            parameters[i] = packingMessages[4 + i];
        }
    }

    //packing array로부터 messageprotocol만들기
    public MessageProtocol(string[] _packingMessages)
    {
        for (int i = 0; i < _packingMessages.Length; i++)
        {
            packingMessages[i] = _packingMessages[i];
        }
        unpackingMessage();
    }

    public MessageProtocol(MESSAGETYPE _type, int numOfParameter, string[] _parameter)
    {
        parameters = new string[4 + numOfParameter];

        parameters[0] = _type.ToString();

        parameters[1] = "";//sender

        parameters[2] = "";//receiver

        parameters[3] = numOfParameter.ToString();

        for (int i = 0; i < numOfParameter; i++)
        {
            parameters[4 + i] = _parameter[i];
        }
    }



    public MessageProtocol(MESSAGETYPE _type, string sender, string receiver, int numOfParameter, string[] _parameter)
    {
        parameters = new string[4 + numOfParameter];

        parameters[0] = _type.ToString();

        parameters[1] = sender;//sender

        parameters[2] = receiver;//receiver

        parameters[3] = numOfParameter.ToString();

        for (int i = 0; i < numOfParameter; i++)
        {
            parameters[4 + i] = _parameter[i];
        }
    }

    public void setSenderPlayer(string _senderPlayer)
    {
        parameters[1] = _senderPlayer;
    }

    public void setReceiverPlayer(string _receiverPlayer)
    {
        parameters[2] = _receiverPlayer;
    }


    public string[] getParameters()
    {
        if (parameters[1] == "" || parameters[2] == "")
        {
            Debug.Log("Neither sender or receiver configure...");
        }

        return parameters;
    }
}
