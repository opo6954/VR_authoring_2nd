using UnityEngine;
using System.Collections;

/*
 * Fire report에 필요한 property 및 object
 * 
 * Property:
 * 
 * * Report_Count: 총 질의 개수

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
 * 
 * */


public class FireReport : TaskModuleTemplate
{

	GameObject defaultForm = null;
	GameObject questionForm = null;

	public FireReport()
	{
		myTaskType = "FireReport";

	}


    
    public override void TaskInit()
    {
        base.TaskInit();

		propertiesList = new string[] {"Approach_Distance", "Approach_Angle", "Patrol_Contents","Report_Count", "Report_Question", "Report_Answer", "Report_TrueAns", "Select_Button-Info", "Move_Button_Info" };
		objectsList = new string[] { "Approach_to_Object"};

    }


    public override void TaskProcess()
    {
        base.TaskProcess();
    }

	public override void readyTask()
	{
		

		ApproachObjState a = new ApproachObjState (this);
		a.setProperty (getProperties ());
		a.setObject (getObjects ());
		QuestioningState b = new QuestioningState (this);
		b.setProperty (getProperties ());
		b.setObject (getObjects ());

		myStateList.Add (a);
		myStateList.Add (b);
	}

    public override void TaskStart()
    {
		myUIInfo.loadUIPrefab ("DefaultForm");
		myUIInfo.loadUIPrefab ("ReportForm");

		defaultForm = myUIInfo.getUIPrefab ("DefaultForm");
		questionForm = myUIInfo.getUIPrefab ("ReportForm");

		myStateList [0].setUI (defaultForm);
		myStateList [1].setUI (questionForm);

        base.TaskStart();
    }

    public override void TaskFinish()
    {
        base.TaskFinish();

		Destroy (defaultForm);
		Destroy (questionForm);

		//UIPrefab dic에서 제거해주기
    }
}
