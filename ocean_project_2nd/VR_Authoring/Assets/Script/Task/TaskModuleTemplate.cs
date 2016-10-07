using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


//module은 항상 1인칭 플레이어한테 붙도록 하자
public class TaskModuleTemplate : MonoBehaviour {

    //이 모듈을 가지고 있는 player임

    Transform myPosition;


    protected PlayerTemplate myPlayerInfo;//선택된 player 종류
    protected UIModuleTemplate myUIInfo;//선택된 UI 종류
    
    public string myTaskName;//customized name
	public string myTaskType;//type of task: pre-defined

	private bool isPropertyEditEnd = false;
	private bool isObjectEditEnd = false;

    
    private bool isTaskStart;
    private bool isTaskEnd;
    private bool isTaskDoing;

    protected int stateIdx = 0;

    /*
     * 시작하는 trigger: isTaskStart --> true로 설정
     * 끝나는 trigger: isTaskDoing --> true로 설정 이거만 하면 됩니다.
     */

    
    public string prevTaskName="";//이전 task
    public string nextTaskName="";//다음 task
    

    private Dictionary<string, object> propertyGroup = new Dictionary<string, object>();
    private Dictionary<string, object> objectGroup = new Dictionary<string, object>();

	//각 task에서 필요한 property 및 object 이름 list
	public string[] propertiesList;
	public string[] objectsList;

    protected List<StateModuleTemplate> myStateList = new List<StateModuleTemplate>();

    //property관련 함수
	//Property, Obj 설정 함수

	public virtual void readyTask()
	{
		Debug.Log ("ready for task");
	}

    public void addProperty(string propertyName, object o)
    {
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

    public void setStartTrigger()
    {
        isTaskStart = true;
    }

    public void setEndTrigger()
    {
        isTaskEnd = true;
    }

    public bool setNextTaskStartTrigger()
    {
        if ( myPlayerInfo.isTaskContains(nextTaskName) == true)
        {
            
            myPlayerInfo.getTask(nextTaskName).setStartTrigger();//start trigger 발동시키기
            return true;
        }

        return false;
    }

     


    public void setMyPlayer(PlayerTemplate player)
    {
        myPlayerInfo = player;
    }
    public PlayerTemplate getMyPlayer()
    {
        return myPlayerInfo;
    }

    public void setMyUI(UIModuleTemplate uimodule)
    {
        myUIInfo = uimodule;
    }

    public Transform getMyPosition()
    {
        return myPosition;
    }

    public GameObject getBackgroundUI()
    {
        return myUIInfo.getUIPrefab("BackgroundForm");
    }


  



    //각종 virtual 함수들

    void Start () {
        TaskInit();
	}


	//task xml 저장
	//document에 붙여라
	public void saveTaskXml(XmlDocument document, XmlElement parent)
	{
		XmlElement element = document.CreateElement ("Task");
		element.SetAttribute ("name", myTaskName);
		element.SetAttribute ("type", myTaskType);
		parent.AppendChild (element);

		Debug.Log (myStateList.Count);

		for (int i = 0; i < myStateList.Count; i++) {
			myStateList [i].saveStateXml (document,element);
		}


		Debug.Log ("XML 포맷으로 저장해라");
	}

	//task xml 불러오기

	public void loadTaskXml()
	{		
		Debug.Log ("XML 포맷 불러오기");
	}


    
    //최초 task가 생성될 때의 초기화
    public virtual void TaskInit()
    {
        
        if(gameObject.transform.childCount>0)
            myPosition = gameObject.transform.GetChild(0);
        
        //이 부분에서 state를 추가합시다

        

    }
    
    //task가 시작하려 할때
    public virtual void TaskStart()
    {
        Debug.Log(myTaskName + " 훈련 시작~!");
        //state에 대한 모든 초기화를 하자

    }

    //task가 수행중일 때의 함수
    public virtual void TaskProcess()
    {
        if (stateIdx < myStateList.Count)
        {

            if (myStateList[stateIdx].isStateEnd == false)//task가 끝나지 않을 경우 실행
            {
				
                myStateList[stateIdx].OnUpdate();



            }
            else
            {
                stateIdx = stateIdx + 1;
            }
        }
        else
        {
            setEndTrigger();
        }

    }


    //task가 끝날 때의 함수
    public virtual void TaskFinish()
    {
        Debug.Log(myTaskName + " 훈련 완료~!");


        if (setNextTaskStartTrigger() == false)
        {
            Debug.Log("다음 task가 존재하지 않습니다 마지막 task로 간주합니다.");
        }




		myUIInfo.deleteUIAll ();




    }
	
	
	void Update () {

        

        if (isTaskStart == true)
        {

            TaskStart();
            isTaskStart = false;
            isTaskDoing = true;
        }
        else if (isTaskDoing == true && isTaskEnd == false)
        {
            TaskProcess();
        }
        else if (isTaskDoing == true && isTaskEnd == true)
        {
            TaskFinish();
            isTaskDoing = false;
        }
	}

}
