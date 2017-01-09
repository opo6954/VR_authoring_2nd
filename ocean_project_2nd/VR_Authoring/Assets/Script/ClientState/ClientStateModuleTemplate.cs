using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * server에서 message를 받은 후 그 message는 clientstate로 변환, 이후 client단에서 수행된다. 각 client는 단 1개의 clientstate만을 진행할 수 있다.(대기상태 역시 clientstate에 포함됨)
 * client에서 직접 수행되는 process 집합
 * 이 부분을 기존의 state로 대체하는 게 나을 듯
 * 일단 server와의 통신을 기준으로 clientstate를 나누자 server 통신 온 이후로부터 다시 server로 통신을 해야할 때까지의 client가 하는 일이 clientstate라고 정의하자
 * 
 * server와의 통신을 기준으로 clientstate를 나눠보자
 *  
 * 
 * */
public class ClientStateModuleTemplate {

    private bool isStateStart = false;
    private bool isStateDoing = false;
    public bool isStateEnd = false;

    protected ClientManager _cm = null;

    protected string myClientState = "";

    protected List<string> propertyList = new List<string>();
    protected List<string> objectList = new List<string>();

    protected Dictionary<string, string> propertyGroup = new Dictionary<string, string>();
    protected Dictionary<string, string> objectGroup = new Dictionary<string, string>();

    //파라미터가 들어있는 dictionary를 만들어야 겠다
    //property이든 object이든 모두 여기에 저장을 하자
    

    public ClientManager MyClientManager
    {
        get
        {
            return _cm;
        }
        set
        {
            _cm = value;
        }
    }

    
    

    //초기 client State시작후 1번만 불림
    public virtual void Init()
    {
        Debug.Log(myClientState + " client state 시작");
    }
    //client state가 살아있는 동안 계속 불림
    public virtual void Process()
    {
    }
    //Client state의 종료 조건임, 계속 조건 확인함
    public virtual bool Goal()
    {
        return false;
    }
    //Client state가 종료된 후에 하는 일
    public virtual void Res()
    {
        Debug.Log(myClientState + " client state 종료");
        convertRes2Msg();

    }

    //server로부터 온 message Protocol의 여러 파라미터를 바탕으로 clientState에 넣는다

    public virtual void setParameters()
    {
        Debug.Log("Dictionary to single variable");
    }

    public void convertMP2ClientState(Dictionary<string, string> myDic)
    {
        for (int i = 0; i < propertyList.Count; i++)
        {
            if (myDic.ContainsKey(propertyList[i]) == true)
            {
                propertyGroup.Add(propertyList[i],myDic[propertyList[i]]);
            }
        }
        for (int i = 0; i < objectList.Count; i++)
        {
            if (myDic.ContainsKey(objectList[i]) == true)
            {
                objectGroup.Add(objectList[i], myDic[objectList[i]]);
            }

        }

        setParameters();

        Debug.Log(myClientState + " server로부터 온 message Protocol을 본 clientState에 맞게 파라미터 설정하기");
    }

    //server한테 보낼 파라미터를 설정하는 함수
    public virtual void convertRes2Msg()
    {
        //이 부분에서 server로 보낼 information을 message protocol로 변환하고 client manager에서 message를 보내면 됩니다
        Debug.Log(myClientState + " client state 정보 server한테 보내기");
    }


    //ClientState의 수행과정임
    //이 부분이 clientstate의 main과정임
    //clientstate 처리할 때 trigger로 OnUpdate함수를 불러야 함
    //OnUpdate는 clientManager의 Update 함수에서 불리거나 혹은 StateManager를 따로 또 만들어서 거기의 Update에서 부르게 하거나 둘 중 하나임

    public void OnUpdate()
    {
        if (isStateStart == false)
        {
            Init();
            isStateStart = true;
            isStateDoing = true;
        }
        else
        {
            bool flags = Goal();

            if (flags == true && isStateDoing == true)//종료 조건, Goal이 만족 and Init 불려진 후 stateDoing이 true일 경우임
            {
                Res();
                isStateDoing = false;
                isStateEnd = true;
            }
            else if (flags == false && isStateDoing == true && isStateEnd == false)//계속 진행하는 조건, Goal이 false이고 Doing이 계속 되고 있고 StateEnd가 false일 경우 계속 Process 실행합니다
            {
                Process();
            }
        }
    }
}
