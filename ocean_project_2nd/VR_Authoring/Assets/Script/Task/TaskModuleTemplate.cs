using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


//module은 항상 1인칭 플레이어한테 붙도록 하자
public class TaskModuleTemplate{

//이 task를 가지고 있는 scenario instance

    string _myTaskName;//customized name
    ScenarioModuleTemplate _myParent;//이 task를 소유한 scenario


    Dictionary<string, object> propertyGroup = new Dictionary<string, object>();
    Dictionary<string, object> objectGroup = new Dictionary<string, object>();

    //각 task에서 필요한 property 및 object 이름 list
    public string[] propertiesList;
    public string[] objectsList;

    protected List<StateModuleTemplate> stateList = new List<StateModuleTemplate>();

    public int currStateIdx = 0;

    

    public string MyTaskName
    {
        get
        {
            return _myTaskName;
        }
        set
        {
            _myTaskName = value;
        }
    }

    public ScenarioModuleTemplate MyParent
    {
        get
        {
            return _myParent;
        }
        set
        {
            _myParent = value;
        }
    }


    //property관련 함수
	//Property, Obj 설정 함수

	public virtual void readyTask()
	{
		Debug.Log ("ready for task");
	}

    public void addProperty(string propertyName, object o)
    {
        if (propertyGroup.ContainsKey(propertyName) == true)
        {
            
        }
        else
            propertyGroup.Add(propertyName, o);
    }
    

    //각 task별로 특정 property를 가져오거나 set할 수 있도록 하기
    public void setProperty(string propertyName, object o)
    {
        if(propertyGroup.ContainsKey(propertyName))
        {
            propertyGroup[propertyName] = o;
        }
    }
    //각 task별로 특정 property를 가져오기
    public T getProperty<T>(string propertyName)
    {
        
        if (propertyGroup.ContainsKey(propertyName))
            return (T)propertyGroup[propertyName];
        
        return default(T);
    }

	public Dictionary<string, object> getProperties()
	{
		return propertyGroup;
	}

    //각 task에 필요한 상호작용 가능한 object 추가하기
    public void addObject(string objectName, object o)
    {
        objectGroup.Add(objectName, o);
    }

    public T getObject<T>(string objectName)
    {
        if (objectGroup.ContainsKey(objectName))
            return (T)objectGroup[objectName];
        return default(T);
    }

	public Dictionary<string, object> getObjects()
	{
		return objectGroup;
	}

    public void insertState(StateModuleTemplate _state)
    {
        _state.MyParent = this;
        stateList.Add(_state);
    }


    //task start됨
    //task가 가지고 있는 state를 탐색하면서 시작함

    public void triggerNextState()
    {
        currStateIdx++;
        triggerState();

    }
    public void triggerState()
    {
        //state가 남아있을 경우 그대로 진행
        if (currStateIdx < stateList.Count)
        {
            ServerLogger.Instance().addText("The state " + stateList[currStateIdx].MyStateName + " is triggered...");
            
            stateList[currStateIdx].initState();
        }
        else//만일 state가 남아있지 않은 경우 다음 task로 넘어가야됨
        {
            currStateIdx = 0;
            
            _myParent.triggerNextTask();
        }
        
    }
}
