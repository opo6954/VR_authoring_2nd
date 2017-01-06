using UnityEngine;
using System.Collections;

/*fire report task를 위한 특수 state
 * 필요한 property:

 * Report_Count: 총 질의 개수

 * Report_Question: list of string으로 구성
일단은 Report_Question[0]: 화재 장소, Report_Question[1]: 화재 종류, Report_Question[2]: 화재 크기

 * Report_Answer: list of list of string으로 구성
Report_Answer[0]: ans[0]: 선장실, ans[1]: 기계실, ans[2]: 객실
Report_Answer[1]: ans[0]: 일반 화재, ans[1]: 유류화재, ans[2]: 전기화재
Report_Answer[2]: ans[0]: 대형, ans[1]: 중형, ans[2]: 소형

* Report_True_Answer: 실제 정답을 나타낼 list of int로 구성
trueAns[0]: 1
trueAns[1]: 2
trueAns[2]: 0

 * 필요한 object:
 * 없음
 * */
//fire report 일을 위한 특수 state

public class QuestioningState : StateModuleTemplate {
	
    public QuestioningState()
	{
		MyStateName = "QuestioningState";
	}

}
