using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//scenario List를 정의한 class, list 내부의 scenario끼리의 start, end를 담당한다
public class ScenarioController : MonoBehaviour{
	

	private List<ScenarioModuleTemplate> scenarioSeq = new List<ScenarioModuleTemplate> ();

	//scenario를 list에 넣는 함수, 단 중복된 scenario 이름을 가질 수 없다
	public bool insertScenario(ScenarioModuleTemplate scenario)
	{
		for (int i = 0; i < scenarioSeq.Count; i++) {
			if (scenarioSeq [i].MyScenarioName != scenario.MyScenarioName) {
				return false;
			}
		}

		scenario.MyScenarioIdx = scenarioSeq.Count;
        
		Debug.Log ("Scenario Name: " + scenario.MyScenarioName+ " and idx is " + scenario.MyScenarioIdx);
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

	//trigger하는 Scenario로서 밑단의 scenarioModuleTemplate으로부터 불려진다
	public void triggerScenario(int scenarioIdx=0)
	{
		if (scenarioIdx > 0) {
			if (scenarioSeq.Count < scenarioIdx) {
				Debug.Log ("Trigger scenario : " + scenarioSeq [scenarioIdx + 1].MyScenarioName);
				scenarioSeq [scenarioIdx + 1].triggerTask ();
			} else {
				Debug.Log ("No Next Scenario Found");
			}
		} else if(scenarioIdx == 0) {
			if(scenarioSeq.Count > scenarioIdx){

                
				Debug.Log ("First scenario Triger: " + scenarioSeq [scenarioIdx].MyScenarioName);
				scenarioSeq [scenarioIdx].triggerTask ();				
			}
		}
	}


    void Start()
    {

    }

    //이 부분에서 scenario 관련 core가 돌아가도록 하자
    void Update()
    {
        
    }
}
