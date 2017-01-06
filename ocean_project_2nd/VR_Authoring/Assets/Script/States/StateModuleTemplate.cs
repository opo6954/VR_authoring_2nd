using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/*
 * 
 * NETWORK버전임
 * //공통된 state 정보임
 * 
 * state는 <server>에서만 관장
 * 
 * 이 state 전체가 client에게 넘겨지지 않고 따로 다른 client용 state를 만들어야 할 듯
 * 
 * 저작도구에서 만든 XML과 직접적으로 연동되는 형태임
 * 
 * 이 state는 server가 관장하기 때문에 playerInfo같은 정보는 필요 없음
 * server에서 main flow를 돌릴 때의 state가 이 statemodule를 기반으로 돌림
 * 
 * 
 * State module을 위한 template
 * 
 * 4가지의 함수로 state 현표
 * 
 * Init, Process, Goal, Res로 구성
 * Init: State가 시작될 때의 초기 수행
 * Process: Goal이 false이면 계속 수행
 * Goal: State를 완료하기 위한 조건
 * Res: State가 종료될 때의 수행작업, Goal이 true이면 계속 수행
 * 
 * 중요한 파라미터
 * propertyGroup: 저작 가능한 요소 집합, dictionary로 구성, object형식
 * ojbectGroup: 상호작용 가능한 물체 집합, dictionary로 구성, object형식
 * networkPlayer: 현 state에서 필요한 player집합, dictionary로 구성, string으로 찾기
 * 
 * mainflow를 server에서 어떻게 돌려야 할까
 * 일단 기본으로 돌리는 것은 task 기반으로 돌려짐
 * 이후 task단에서 자신이 가진 state를 돌림
 * 이 때 server에서의 state는 실행될 때 state-->message로 변환을 하고 client한테 보내면 됨 이후 network player 개수 만큼의 완료가 들어올 경우 state 종료됨 다음 state로 넘어감
 * 
 * 
 * /*
 * State말단에서 해줘야 할 것은 state를 바탕으로 clientState를 붙여야 한다.
 * 각 player별로 어떻게 client state가 할당되는 지 관련 table만 만들어서 servermanager한테 건네주면 servermanager가 message를 client한테 보내고 다시 완료 message를 받는 조건에 따라서
 * 다음 state로 이동을 주사한다. 다시 state에서 다음 state trigger를 해줘야겠지?
 * 할당 table
 * CS: ClientState
 * 
 * 
 * Player1 Player2 Player3 Player4
 * CS1      null     CS3     CS3
 * null     CS1      CS2     null
 * null     null     CS7     null
 * ------------종료--------------
     
 * 
 * 
 *  
 * */

public class StateModuleTemplate {

    protected ServerManager _server;
    
    private int playerNumber=-1;



    TaskModuleTemplate _myParentTask;//나의 윗단인 moduleINfo임
    string _myStateName;



    
    //각 state별로 필요한 property, object, networkplayer List로 string 형태로 저장해놓자
    protected List<string> propertyList = new List<string>();
    protected List<string> objectList = new List<string>();

    //필요한 role list임
    protected List<string> playerRoleList = new List<string>();
    
    

    

	 
    //property 설정요소와 obj 설정 요소 존재
    Dictionary<string, object> propertyGroup = new Dictionary<string, object>();
    Dictionary<string, object> objectGroup = new Dictionary<string, object>();
    //player 이름과 role이 포함?

    public string MyStateName
    {
        get
        {
            return _myStateName;
        }
        set
        {
            _myStateName = value;
        }
    }

    public TaskModuleTemplate MyParent
    {
        get
        {
            return _myParentTask;
        }
        set
        {
            _myParentTask = value;
        }
    }

    //본 state가 속한 task를 생성자의 input으로 넣어줘야 함
    public StateModuleTemplate()
    {
    }
    
    //Property, Object, NetworkPlayer List 관리

    public void addPropertyList(string _propertyName)
    {
        propertyList.Add(_propertyName);
    }
    public void addObjectList(string _objectName)
    {
        objectList.Add(_objectName);
    }
    public void addNetworkPlayerList(string _roleName)
    {
        playerRoleList.Add(_roleName);
    }

    public List<string> getPropertyList()
    {
        return propertyList;
    }

    public List<string> getObjectList()
    {
        return objectList;
    }

    public List<string> getNetworkPlayerList()
    {
        return playerRoleList;
    }



    //network player 관리

    //property 및 obj 관리

    //property 추가하기
    public void addProperty(string propertyName, object o)
    {
        if(propertyGroup.ContainsKey(propertyName) == false)
            propertyGroup.Add(propertyName, o);

        return;
    }


    //property 가져오기
    public T getProperty<T>(string propertyName)
    {
        if (propertyGroup.ContainsKey(propertyName) == true)
        {
            
            
            return (T)propertyGroup[propertyName];
        }

        return default(T);  
    }

    //property 확인하기
    public bool isContainProperty(string propertyName)
    {
        if (propertyGroup.ContainsKey(propertyName) == true)
            return true;
        return false;
    }

    //task에서 state으로 property정보를 위임받을 때 사용, List에 있는 얘들만 차례대로 넣는다.
    public virtual void setProperty(Dictionary<string, object> properties)
    {
        foreach (string key in propertyList)
        {
            addProperty(key, properties[key]);
        }
    }


    //object 추가하기
    public void addObject(string objectName, object o)
    {
        if (objectGroup.ContainsKey(objectName) == false)
            objectGroup.Add(objectName, o);

        return;
    }

    //object 가져오기
    public T getObject<T>(string objectName)
    {
        if (objectGroup.ContainsKey(objectName) == true)
        {
            return (T) objectGroup[objectName];
        }


        return default(T);
    }

    //object 확인하기
    public bool isContainObject(string objectName)
    {
        if (objectGroup.ContainsKey(objectName) == true)
            return true;
        return false;
    }

    //task에서 state으로 object 정보를 위임받을 때 사용, List에 있는 얘들만 차례대로 넣는다.
    public virtual void setObject(Dictionary<string, object> objects)
    {
        foreach (string key in objectList)
        {
            addObject(key, objects[key]);
        }
    }
    //설정하는 함수임

    //각 player별로 clientState를 배분한다.
    //걍 clientState에 wait만을 위한 state를 넣는 게 좋을 듯 하다.
    public virtual void buildClientStateTable()
    {
        Debug.Log("각 state별로 clientState를 순서대로 list로 나눠서 줌");
    }
    public virtual void updateClientStateTable()
    {
        Debug.Log("이 전의 clientstate결과가 다음 clientstate의 파라미터에 영향을 끼칠 경우 execute하기 직전에 update를 해야 한다.");
    }

    public void Init()
    {
        //server 설정해주기
        _server = GameObject.FindGameObjectWithTag("ServerUnit").GetComponent<ServerManager>();

        //본 state에 필요한 모든 player의 개수를 설정해야 함
    } 



    //state 시작 부분임
    //각 state별로 clientState를 바꿔야 합니다.
    public virtual void initState()
    {
        ServerLogger.Instance().addText(MyStateName + " is now start...");

        //초기화
        Init();

        //clientStateAssignTable 만들기
        
        ServerLogger.Instance().addText(MyStateName + " is now building Client State Table...");
        buildClientStateTable();//table 만들기
        ServerLogger.Instance().addText(MyStateName + " is now executing Client State Table...");
        _server.executeAssignTable();//servermanager단에서 만들어진 table 처리
    }

    

}
