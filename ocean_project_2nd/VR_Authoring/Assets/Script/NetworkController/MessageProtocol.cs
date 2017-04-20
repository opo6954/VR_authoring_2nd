using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
 * 
 * 이렇게만 해줘도 될듯d
 * 
 * Parameter: clientState에서 쓰일 대사나 obj 이름 등의 저작도구로부터의 정보
 * 파라미터의 구성:
 * [0]: clientState의 이름
 * [1]: 파라미터 이름
 * [2]: 파라미터 내용
 * [3]: 파라미터 이름
 * [4]: 파라미터 내용
 * ...
  * [k]: 오브젝트 이름
 * [k+1]: 오브젝트 내용
 * ...
 
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
        return "MSG " + MessageProtocol.getStrFromMsgType(type) + " FROM " + sender + " TO " + receiver;
    }

    public static string getStrFromMsgType(MESSAGETYPE mp)
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

    public static MESSAGETYPE getMsgTypeFromStr(string str)
    {
        switch (str)
        {
            case "CONNECT":
                return MESSAGETYPE.CONNECT;
            case "ROLEINFO":
                return MESSAGETYPE.ROLEINFO;
            case "CLIENTSTATE":
                return MESSAGETYPE.CLIENTSTATE;
            case "TRAININGINFO":
                return MESSAGETYPE.TRAININGINFO;
            case "TRAININGEND":
                return MESSAGETYPE.TRAININGEND;
            default:
                Debug.Log("Wrong with get type");
                return MESSAGETYPE.CONNECT;
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
        for (int i = 0; i < packingMessages.Length; i++)
        {
            Debug.Log(i.ToString() + "th Message Contents: " + packingMessages[i]);
        }

        type = getMsgTypeFromStr(packingMessages[0]);
        sender = packingMessages[1];
        receiver = packingMessages[2];
        numOfParameter = int.Parse(packingMessages[3]);

        parameters = new string[numOfParameter];


        for (int i = 0; i < numOfParameter; i++)
        {
            parameters[i] = packingMessages[4 + i];
        }
    }

    //message 만드는 생성자들

    //packing array로부터 messageprotocol만들기
    public MessageProtocol(string[] _packingMessages)
    {
        packingMessages = new string[_packingMessages.Length];

        for (int i = 0; i < _packingMessages.Length; i++)
        {
            packingMessages[i] = _packingMessages[i];
        }
        unpackingMessage();
    }

    public MessageProtocol(MESSAGETYPE _type, int _numOfParameter, string[] _parameter)
    {
        numOfParameter = _numOfParameter;

        packingMessages = new string[4 + numOfParameter];



        packingMessages[0] = getStrFromMsgType(_type);

        packingMessages[1] = "";//sender

        packingMessages[2] = "";//receiver

        packingMessages[3] = numOfParameter.ToString();

        for (int i = 0; i < numOfParameter; i++)
        {
            packingMessages[4 + i] = _parameter[i];
        }
    }



    public MessageProtocol(MESSAGETYPE _type, string _sender, string _receiver, int _numOfParameter, string[] _parameter)
    {

        sender = _sender;
        receiver = _receiver;

        numOfParameter = _numOfParameter;
        packingMessages = new string[4 + numOfParameter];

        packingMessages[0] = getStrFromMsgType(_type);

        packingMessages[1] = sender;//sender

        packingMessages[2] = receiver;//receiver

        packingMessages[3] = numOfParameter.ToString();

        for (int i = 0; i < numOfParameter; i++)
        {
            packingMessages[4 + i] = _parameter[i];
        }
    }

    public MessageProtocol(MESSAGETYPE _type, string clientStateName, List<string> propertyList, Dictionary<string, string> propertyGroup, List<string> objectList, Dictionary<string, string> objectGroup)
    {
        List<string> tmpParameters = new List<string>();

        tmpParameters.Add(clientStateName);

        for (int i = 0; i < propertyList.Count; i++)
        {
            string propertyName = propertyList[i];
            string propertyValue = propertyGroup[propertyName];

            tmpParameters.Add(propertyName);
            tmpParameters.Add(propertyValue);
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            string objectName = objectList[i];
            string objectValue = objectGroup[objectName];

            tmpParameters.Add(objectName);
            tmpParameters.Add(objectValue);
        }

        string [] tmpParametersArray = tmpParameters.ToArray();

        numOfParameter = tmpParameters.Count;

        packingMessages = new string[4 + numOfParameter];

        packingMessages[0] = getStrFromMsgType(_type);
        packingMessages[1] = "";
        packingMessages[2] = "";
        packingMessages[3] = numOfParameter.ToString();

        

        for (int i = 0; i < numOfParameter; i++)
        {
            packingMessages[4 + i] = tmpParametersArray[i];
        }

    }



    public void setSenderPlayer(string _senderPlayer)
    {
        packingMessages[1] = _senderPlayer;
    }

    public void setReceiverPlayer(string _receiverPlayer)
    {
        packingMessages[2] = _receiverPlayer;
    }


    public string[] getpackingMessages()
    {
        if (packingMessages[1] == "" || packingMessages[2] == "")
        {
            Debug.Log("Neither sender or receiver configure...");
        }

        return packingMessages;
    }
    public string getClientStateName()
    {
        if (type == MESSAGETYPE.CLIENTSTATE)
        {
            return getParameterValue(0);
        }
        return null;
    }

    public Dictionary<string, string> getParameterGroupForClientState()
    {
        if (type == MESSAGETYPE.CLIENTSTATE)
        {
            Dictionary<string, string> myDic = new Dictionary<string, string>();

            for (int i = 0; i < numOfParameter; i++)
            {
                if (4 + i + 1 < packingMessages.Length)
                {
                    myDic.Add(packingMessages[4 + i], packingMessages[4 + i + 1]);
                }
            }

            return myDic;
        }

        return null;
    }
}

