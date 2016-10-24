﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/*
 * State module을 위한 template, State는 UIForm과 1대1로 매칭될 수 있습니다. 
 * Init, Process, Goal, Res로 구성
 * 
 * 
 * Init: State가 시작될 때의 초기 수행
 * Process: Goal이 false이면 계속 수행
 * Goal: State를 완료하기 위한 조건
 * Res: State가 종료될 때의 수행작업, Goal이 true이면 계속 수행
 * 
 *  
 * 
 * 
 * //state, task간의 property나 object의 이름을 똑같이 통일하자
 * */

public class StateModuleTemplate {

    private bool isStateStart = false;
    private bool isStateDoing = false;
    public bool isStateEnd = false;

    

    protected GameObject myUIInfo;//선택된 UI 종류
    protected TaskModuleTemplate myModuleInfo;//나의 윗단인 moduleINfo임
    protected string myStateName;
    
	public Transform myPosition;//현재 player의 위치
    public PlayerTemplate myPlayerInfo;//현재 player 정보


    private Dictionary<string, string> joystickMappingTable = new Dictionary<string, string>();

	 
    //property 설정요소와 obj 설정 요소 존재
    private Dictionary<string, object> propertyGroup = new Dictionary<string, object>();
    private Dictionary<string, object> objectGroup = new Dictionary<string, object>();


    //joystick 매핑 정보 
    private void mappingJoystickButton()
    {
        joystickMappingTable.Add("a", "joystick button 0");
        joystickMappingTable.Add("z", "joystick button 1");
        joystickMappingTable.Add("x", "joystick button 2");
        joystickMappingTable.Add("b", "joystick button 3");
        joystickMappingTable.Add("lb", "joystick button 4");
        joystickMappingTable.Add("rb", "joystick button 5");
        joystickMappingTable.Add("lt", "joystick button 6");
        joystickMappingTable.Add("rt", "joystick button 7");

    }
    
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


    ///////////State에서의 Ui 관리

	public void setUI(GameObject _UIModule)
    {
        myUIInfo = _UIModule;
    }
	public void turnOnMyUI()
	{
		myUIInfo.gameObject.SetActive (true);
	}
	public void turnOffMyUI()
	{
		myUIInfo.gameObject.SetActive (false);
	}

    public void setMyPlayer(PlayerTemplate player)
    {
        myPlayerInfo = player;
    }

    public void setMyModule(TaskModuleTemplate module)
    {
        myModuleInfo = module;
    }

    public void setMyPosition(Transform _myPos)
    {
        myPosition = _myPos;
    }

	//XML 저장 및 로드 함수

	public void saveStateXml(XmlDocument document, XmlElement parent)
	{
		//state 종류도 기억해야 할듯
		//state에서 저장할 부분:
		XmlElement element = document.CreateElement("State");
		XmlElement propElement = document.CreateElement ("Properties");
		

		element.SetAttribute ("name", myStateName);

		foreach (KeyValuePair<string, object> kv in propertyGroup) {

			string typeName = kv.Value.ToString ();




			if (typeName.Length - 2 > 0) {
				string test = typeName.Substring (typeName.IndexOf(".")+1);



				//1차원 배열
				if (test.LastIndexOf ("[") == test.IndexOf ("[") && test.LastIndexOf("[") > 0 ) {



					if (test.Contains ("String")) {
						string[] strArray = (string[])kv.Value;

						string compactStr = "";

						for (int i = 0; i < strArray.Length; i++) {
							if (i == 0)
								compactStr = strArray [i];
							else
								compactStr += "," + strArray [i];
						}

						propElement.SetAttribute (kv.Key, compactStr);
					} else if (test.Contains ("Int32")) {
						int[] intArray = (int[])kv.Value;

						string compactStr = "";

						for (int i = 0; i < intArray.Length; i++) {
							if (i == 0)
								compactStr = intArray [i].ToString();
							else
								compactStr += "," + intArray [i].ToString();
						}
						propElement.SetAttribute (kv.Key, compactStr);
					}
				}
				//2차원 배열
				else if (test.LastIndexOf("[") > test.IndexOf("[")){
					if (test.Contains ("String")) {
						string[][] strArray = (string[][])kv.Value;

						string compactStr = "";

						for (int i = 0; i < strArray.GetLength (0); i++) {
							
							for (int j = 0; j < strArray [i].GetLength (0); j++) {
								if (i == 0 && j == 0)
									compactStr = strArray [i] [j];
								else
									compactStr += "," + strArray [i] [j];
							}
							compactStr = compactStr + " /";
						}

						propElement.SetAttribute (kv.Key, compactStr);

					} else if (test.Contains ("Int32")) {
						int[][] intArray = (int[][])kv.Value;

						string compactStr = "";

						for (int i = 0; i < intArray.GetLength (0); i++) {

							for (int j = 0; j < intArray [i].GetLength (0); j++) {
								if (i == 0 && j == 0)
									compactStr = intArray [i] [j].ToString();
								else
									compactStr += "," + intArray [i] [j].ToString();
							}
							compactStr = compactStr + " /";
						}

						propElement.SetAttribute (kv.Key, compactStr);
					}

				} else {
					//[] 안 들어 있을시 array 아닌 걍 string이므로 그대로 저장하기

					propElement.SetAttribute (kv.Key, kv.Value.ToString ());					
				}
			} else {
				
				propElement.SetAttribute (kv.Key, kv.Value.ToString ());					
			}

			//propElement.SetAttribute (kv.Key, kv.Value.ToString ());			







		}

        XmlElement objElement = document.CreateElement("Objects");

        foreach (KeyValuePair<string, object> kv in objectGroup) {
			//obj는 그 obj의 gameobject의 이름으로 저장하자, 이름은 중복하면 안된다는 가정을 하자
			objElement.SetAttribute (kv.Key, getObject<GameObject> (kv.Key).name);
		}

		element.AppendChild (propElement);
		element.AppendChild (objElement);
		parent.AppendChild (element);


	}
	public void loadStateXml()
	{
		Debug.Log ("State를 xml로 저장하는 부분");
	}



    ///////////////Utility 함수들
    //특정 object에 가까이 가면 true 리턴, 아닐 시 false 리턴
    public bool amISeeObject(GameObject target, float shout_angle = 10.0f, float shout_range = 15.0f)
    {
        float distance = (target.transform.position - myPosition.position).magnitude;
        float angle = Vector3.Dot((target.transform.position - myPosition.position).normalized, Camera.main.transform.forward.normalized);
		if (shout_range <= 0) {
			shout_range = 5.0f;
		}
		if (shout_angle <= 0) {
			shout_angle = 3.0f;
		}


        if (distance < shout_range && angle < shout_angle)
        {
			
            return true;
        }
        return false;
    }

    //1인칭 화면 잠구기, 움직임 X, 시야 변경 X
    public void lockFPSScreen(bool enableLock)
    {
        if (enableLock == true)
        {
            setActiveGameComponent( myPlayerInfo.transform.GetChild(0).name, "FirstPersonController", false);
        }
        else
        {
            setActiveGameComponent(myPlayerInfo.transform.GetChild(0).name, "FirstPersonController", true);
        }
    }
	//1인칭 움직임 잠구기, 움직임 X, 시야 변경 O
	public void lockFPSmoveScreen(bool enableLock)
	{
		if (enableLock == true) {

			myPlayerInfo.transform.GetChild (0).transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().m_WalkSpeed = 0.0f;

		} else {
			myPlayerInfo.transform.GetChild (0).transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().m_WalkSpeed = myPlayerInfo.myWalkSpeed;
		}
	}

    //주어진 gameObject의 component를 끄고 켜기
    public static void setActiveGameComponent(string gameObjectName, string gameComponentName, bool value)
    {
        ((MonoBehaviour)(GameObject.Find(gameObjectName).transform.GetComponent(gameComponentName))).enabled = value;

    }

    //입력함수

    public bool isKeyDown(string keyName)
    {
        bool isKeyPressed = false;
         
        if (myPlayerInfo.isJoystick == true)
        {
            //조이스틱에 대한 control
            isKeyPressed = Input.GetKeyDown(joystickMappingTable[keyName]);
            
            
        }
        else if (myPlayerInfo.isLeapMotion == true)
        {
            //Leap motion의 제스처에 대한 control
        }
        else
        {
            isKeyPressed = Input.GetKeyDown(keyName);
            //일반 키보드에 대한 control
        }

        return isKeyPressed;
    }


	/*
	 * public PlaySoundsState(TaskModuleTemplate _myModule, GameObject _UI)
	{
		


	}
	 * */

    ///////////////State의 실행 조건 및 순서를 위한 함수들.... 건드리지 맙시다
	/// 
	/// 
	//생성자

	public StateModuleTemplate(TaskModuleTemplate _myModule)
	{
		setMyModule(_myModule);
		setMyPosition(myModuleInfo.getMyPosition());
		setMyPlayer(myModuleInfo.getMyPlayer());
        mappingJoystickButton();
	}




	// Update is called once per frame
	public void OnUpdate () { 

        

        if (isStateStart == false)
        {
            Init();
            isStateStart = true;
            isStateDoing = true;
        }
        else
        {
            bool flags = Goal();

            if(flags == true && isStateDoing == true)
            {
                
                Res();
                isStateDoing = false;
                isStateEnd = true;
            }
			else if (flags == false && isStateDoing == true && isStateEnd == false)
			{
				Process();
			}
        }
	    
	}
	//관련 설정 함수

	public virtual void setProperty(Dictionary<string, object> properties)
	{
		
	}

	public virtual void setObject(Dictionary<string, object> objects)
	{
		
	}

    //초기화, 1번만 수행
    public virtual void Init()
    {
        
        Debug.Log(myStateName +  " state 시작");
		turnOnMyUI ();
    }
    //Goal이 false일때 계속 수행하는 작업
    public virtual void Process()
    {
    }
    //State를 종료하기 위한 조건
    public virtual bool Goal()
    {
        return false;
    }
    //Goal을 만날 때의 수행되는 것, 
    //1번만 수행
    public virtual void Res()
    {
        Debug.Log(myStateName + " state 종료");
		turnOffMyUI ();
    }

}
