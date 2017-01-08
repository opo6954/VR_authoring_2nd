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
public class ClientStateModuleTemplate{

    private bool isStateStart = false;
    private bool isStateDoing = false;
    public bool isStateEnd = false; 

    protected string myClientState = "";
    

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
