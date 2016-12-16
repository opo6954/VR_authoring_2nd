using UnityEngine;
using System.Collections;
/*
 * Client State를 받아서 Client State를 윗단에서 실행하는 함수임
 * server로부터 온 clientState를 받는 역할만 하자
 * clientState를 받아와서 실행하는 역할
 * 
 * 
 * */
//clientstate만을 다루자
public class ClientStateController : MonoBehaviour{

    private ClientStateModuleTemplate currClientState = null;

    
    //server로부터 client state 정보 받아야 함 그리고 client state로 변환해야
    //일단 message를 적절히 정의해야하는데...


    /*
     * 이 부분은 message type중에서 State인 경우에만 변환을 합니다.
     * */
    
    ClientState message2ClientState(MessageProtocol _message)
    { 
        
        return new ClientState();
    }

    MessageProtocol clientState2Message()
    {
        return null;
    }

     

    public void setCurrClientState(ClientStateModuleTemplate csm)
    {
        if (currClientState != null)
            Debug.Log("Current ClientState is on...");
        else
            currClientState = csm;
    }

    void Start()
    {
        Debug.Log("ClientStateController Start~!");
    }

    void Update()
    {
        if (currClientState != null)
        {
            if (currClientState.isStateEnd == false)
                currClientState.OnUpdate();//client state의 process 시작
            else
                currClientState = null;
        }
    }
}
