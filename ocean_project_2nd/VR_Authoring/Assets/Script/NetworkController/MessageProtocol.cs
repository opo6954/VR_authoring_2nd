using UnityEngine;
using System.Collections;
/*
 * MessageProtocol
 * Server와 Client 사이의 message protocol 및 format을 담당함
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
 * Sender: Server, Client
 * Receiver: Server, Client
 * Message Type: State 
 * Name: ClientState이름
 * Parameter: clientState에서 쓰일 대사나 obj 이름 등의 저작도구로부터의 정보
 * ExtraInfo: 성공여부 Server->Client로 갈 때는 0, Client->Server로 갈 때 성공일시 1, 실패일 시 0인데 일단 0인 경우는 버그 등으로 멈췄을 때 보내줘야 할 듯
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
 * 
 * 
 * 
 *  
 * 
 * 
 * */




public class MessageProtocol {

    public enum MESSAGETYPE
    {
        CONNECT,
        WAIT,
        STATE,
        END
    }

    public MESSAGETYPE type;
    public string sender = "";
    public string receiver = "";
    public string name;
    public string[] parameter;
    public object extraInfo;

    public MessageProtocol(MESSAGETYPE _type, string _sender, string _receiver, string _name, string[] _parameter, object _extraInfo)
    {
        type = _type;
        sender = _sender;
        receiver = _receiver;
        name = _name;
        parameter = new string[_parameter.Length];
        _parameter.CopyTo(parameter, _parameter.Length);
        extraInfo = _extraInfo;
    }

}
