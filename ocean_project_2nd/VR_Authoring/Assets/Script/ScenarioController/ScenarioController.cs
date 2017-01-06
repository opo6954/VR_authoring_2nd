using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * 저작도구를 통해 만들어진 xml를 parsing하여 scenario형태로 변환된 정보를 관리 및 진행시키는 함수
 * Server에서 돌린다.
 * 일단 지금은 multi-scenario를 넣어도 동작하도록 즉 scenarioModuleTemplate이 List 형태로 저장되고 List를 순회하면서 scenario 진행한다.
 * 
 * */


public class ScenarioController : MonoBehaviour{
	
    //scenario template이 담긴 list임 이 list를 기준으로 flow가 흘러감
     
	private List<ScenarioModuleTemplate> scenarioSeq = new List<ScenarioModuleTemplate> ();

    public int currScanarioIdx = 0;

    ServerManager serverManager = null; 

	//scenario를 list에 넣는 함수, 단 중복된 scenario 이름을 가질 수 없다
	public bool insertScenario(ScenarioModuleTemplate scenario)
	{
        scenario.MyController = this;
		for (int i = 0; i < scenarioSeq.Count; i++) {
			if (scenarioSeq [i].MyScenarioName != scenario.MyScenarioName) {
				return false;
			}
		}
		scenarioSeq.Add (scenario); 

		return true;
	}

	public bool removeScenario(string scenarioName)
	{
		int idx=-1;
		for (int i = 0; i < scenarioSeq.Count; i++) {
			if (scenarioSeq [i].MyScenarioName == scenarioName) {
				idx = i;
			}
		}

		//list에 없을시 remove 못함, false
		if (idx < 0)
			return false;
		
		//List에 있을시 remove 수행
		scenarioSeq.RemoveAt (idx);
		return true;
	}
    public void triggerNextScenario()
    {
        currScanarioIdx++;

        triggerScenario();
    }
	
	public void triggerScenario()
	{
        if (currScanarioIdx < scenarioSeq.Count)
        {
            ServerLogger.Instance().addText("The scenario " + scenarioSeq[currScanarioIdx].MyScenarioName + " is triggered...");

            serverManager.passScenarioInfo(scenarioSeq[currScanarioIdx].MyScenarioName);
            scenarioSeq[currScanarioIdx].triggerTask();
        }
        else
        {
            currScanarioIdx = 0;

            //training END임
            //Server한테 training END임을 보여줘야 함

            serverManager.setTrainingEnd();
            
            

        }
	}

    public void setServer(ServerManager _serverManager)
    {
        serverManager = _serverManager;
    }
    public ServerManager getServer()
    {
        if (serverManager != null)
            return serverManager;
        Debug.Log("No server Found...");
        return null;
    }

    void Start()
    {

    }

    //이 부분에서 scenario 관련 core가 돌아가도록 하자
    //Scenario controller에서 task의 process를 넣는 queue 존재, 이 queue를 계속 확인하자

    void Update()
    {
    }
}
