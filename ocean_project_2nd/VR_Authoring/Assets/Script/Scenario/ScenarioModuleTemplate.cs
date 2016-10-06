using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Scenario의 module화
public class ScenarioModuleTemplate{

	//scenario에서 가지고 있는 taskList
	private List<TaskModuleTemplate> taskList;

	//scenario 이름
	private string myScenarioName="";

	//Scenario 전체 시간
	private double periodTime;

	//Scenario 전체의 난이도
	private int difficulty;

	public ScenarioModuleTemplate()
	{
		
	}

	public void setMyScenarioName(string myName)
	{
		myScenarioName = myName;
	}
	public string getMyScenarioName()
	{
		return myScenarioName;
	}

	//scenario 불러오기(from xml to class)
	public void loadScenario(string scenarioName)
	{
		
	}
	//scenario 저장하기(from class to xml)
	public void saveScenario(string scenarioName)
	{
		
	}


	
}
