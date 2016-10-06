using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ScenarioModuleTemplate {
	
	//task seq가 저장된 task list
	public List<TaskModuleTemplate> taskList = new List<TaskModuleTemplate>();

	//sceanrio의 이름
	private string myScenarioName="";

	//scenario의 난이도
	private int difficulty=0;

	//scenario의 시간
	private double timeout = 0;

	//scenario관련 공통 resource의 설정 및 불러오기
	public void setMyScenarioName(string _myName)
	{
		myScenarioName = _myName;
	}
	public string getMyScenarioName()
	{
		return myScenarioName;
	}
	public void setMyDifficulty(int _difficulty)
	{
		difficulty = _difficulty;
	}
	public int getMyDifficult()
	{
		return difficulty;
	}

	public void setMyPeriodTime(double _time)
	{
		timeout = _time; 
	}

	public double getMyTimeOut()
	{
		return timeout;
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
		
	}







	public ScenarioModuleTemplate()
	{
	}



}
