using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
//scenario 자체는 걍 module만 있고 xml로만 저장할 수 있도록 하자
public class ScenarioModuleTemplate { 

	//scenarioSeq의 instance를 가지고 오자sadf
 
    private Transform myPosition;

	private ScenarioController myParent;

    //본 scenario에서의 진행하고 있는 task의 idx를 말함
    public int currTaskIdx = 0;
    	
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

    public ScenarioController getMyParent()
    {
        return myParent;
    }

    


	
    //scenario가 가지고 있는 task를 trigger한다.
	
	public void triggerTask()
	{
        taskList[currTaskIdx].triggerState();
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
