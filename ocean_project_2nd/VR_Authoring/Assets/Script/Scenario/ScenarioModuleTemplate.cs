using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
//scenario 자체는 걍 module만 있고 xml로만 저장할 수 있도록 하자
public class ScenarioModuleTemplate {

	//scenarioSeq의 instance를 가지고 오자

    private Transform myPosition;

	private ScenarioController myParent;

	//myParent에서의 scenarioList에서의 나의 순서
	private int myScenarioIdx;
	
	//task seq가 저장된 task list
	private List<TaskModuleTemplate> taskList = new List<TaskModuleTemplate>();

	//sceanrio의 이름
	private string myScenarioName="";

	//scenario의 난이도
	private int difficulty=0;

	//scenario의 시간
	private double timeout = 0;

    public Transform MyPosition
    {
        get
        {
            return myPosition;
        }
        set
        {
            myPosition = value;
        }
    }

    public int MyScenarioIdx
    {
        get
        {
            return myScenarioIdx;
        }
        set
        {
            myScenarioIdx = value;
        }
    }

    public string MyScenarioName
    {
        get
        {
            return myScenarioName;
        }
        set
        {
            myScenarioName = value;
        }
    }

    public int MyDifficulty
    {
        get
        {
            return difficulty;
        }
        set
        {
            difficulty = value;
        }
    }

    public double MyTimeout
    {
        get
        {
            return timeout;
        }
        set
        {
            timeout = value;
        }
    }

	public void insertTask(TaskModuleTemplate _task)
	{
        _task.setMyParent(this);

        taskList.Add(_task);


	}






	public void setMyParent(ScenarioController _myParent)
	{
        
		myParent = _myParent;
        myPosition = _myParent.transform.GetChild(0).transform;

	}
    


	//build 관련

	//주어진 task idx 이후의 task를 시작한다. 만일 없을시 제일 처음 task부터 실행한다
	public void triggerTask(int taskIdx=0)
	{
		if (taskIdx > 0) {
			if (taskList.Count < taskIdx) {
				taskList [taskIdx + 1].setStartTrigger ();//다음 task를 실행하기
			} else {
				Debug.Log ("No Next Task Found");

				myParent.triggerScenario (myScenarioIdx + 1);
			}
		} else if (taskIdx == 0) {
			if (taskList.Count > taskIdx) {
                Debug.Log("First Task is triggered " + taskList[taskIdx].myTaskName);
				taskList [taskIdx].setStartTrigger ();//처음 task 실행하기

				
			}
		}
	}







	//scenario xml 저장 및 불러오기(밑단의 task, state의 저장 및 불러오기도 필요함)
	//from scenario to xml
	//xml로 저장시 task단 역시 저장 필요
	public void saveScenario2Xml()
	{
		
		XmlDocument document = new XmlDocument();
		XmlElement element = document.CreateElement ("Scenario");
		element.SetAttribute ("name", myScenarioName);
		element.SetAttribute ("difficulty", difficulty.ToString());
		element.SetAttribute ("Time", timeout.ToString());
		document.AppendChild (element);



		//이 후에 밑단(task, state단) 하자

		for (int i = 0; i < taskList.Count; i++) {
			taskList [i].saveTaskXml (document,element);
		}

		document.Save (myScenarioName + ".xml");

	}
	//from xml to scenario
	//xml로터 불러올 시 task단 역시 제대로 불러와야 한다
	public void loadScenariofromXml(string scenarioName)
	{
		Debug.Log ("load xml");
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.Load (scenarioName + ".xml");

		XmlElement itemListElement = xmldoc ["Scenario"];
		//scenario단에서의 정보
		MyTimeout = double.Parse(itemListElement.GetAttribute ("Time"));
		MyDifficulty = int.Parse(itemListElement.GetAttribute ("difficulty"));
		MyScenarioName = itemListElement.GetAttribute ("name");

		XmlNodeList nodeList = xmldoc.GetElementsByTagName ("Task");

		foreach (XmlNode xnode in nodeList) {


			XmlNodeList xnodeChList = xnode.ChildNodes;

			foreach (XmlNode xnodeCh in xnodeChList) {

				XmlNodeList xnodeChStateList = xnodeCh.ChildNodes;

				foreach (XmlNode xnodeChState in xnodeChStateList) {
					XmlAttributeCollection xac = xnodeChState.Attributes;

					foreach (XmlAttribute xa in xac) {
						Debug.Log (xa.Name + ": " + xa.InnerText);
					}

				}
			}
		}
	}







	public ScenarioModuleTemplate()
	{
	}



}
