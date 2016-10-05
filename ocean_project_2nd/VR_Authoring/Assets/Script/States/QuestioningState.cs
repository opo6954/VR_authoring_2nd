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

	GameObject backgroundUI;

	int currQuesIdx=0;//지금 풀고 있는 문제 인덱스
	int currAnsIdx=0;//현재 선택된 답 인덱스
	int currAnsNum=0;//현재 문제에서 답 개수
	int correctNum=0;//맞은 문제 개수

	string selectButton="";
	string moveButton="";


	public QuestioningState(TaskModuleTemplate _myModule, GameObject _UI) : base(_myModule, _UI)
	{

	}



	public override void setProperty (System.Collections.Generic.Dictionary<string, object> properties)
	{
		addProperty ("Report_Count", properties["Report_Count"]);
		addProperty ("Report_Question", properties["Report_Question"]);
		addProperty ("Report_Answer", properties["Report_Answer"]);
		addProperty ("Report_TrueAns", properties["Report_TrueAns"]);

		addProperty ("Select_Button_Info", properties ["Select_Button_Info"]);
		addProperty ("Move_Button_Info", properties ["Move_Button_Info"]);

		selectButton = getProperty<string> ("Select_Button_Info");
		moveButton = getProperty<string> ("Move_Button_Info");
		 
	}

	public override void setObject (System.Collections.Generic.Dictionary<string, object> objects)
	{
		base.setObject (objects);
	}

	public override void Init ()
	{
		myStateName = "문제 풀기 State";
		backgroundUI = myModuleInfo.getBackgroundUI ();//추가적인 버튼 및 맞는지 틀렸는지 알려주기 위해 backgroundUI 필요함

		base.Init ();

		lockFPSScreen (true);

		setQuestions ();
		setAnswerSheet ();

	}

	public override void Process ()
	{
		base.Process ();


		getKeyInput ();
	}

	public override bool Goal ()
	{
		string[] questions = getProperty<string[]> ("Report_Question");
		if (correctNum == questions.Length) {
			return true;
		} else
			return false;


	}

	public override void Res ()
	{
		base.Res ();
		lockFPSScreen (false);
	}

	//문제 설정함수
	public void setQuestions()
	{
		string[] queArray = getProperty<string[]> ("Report_Question");

		myUIInfo.GetComponent<ReportForm> ().setQuestionTxt (queArray[currQuesIdx]);
	}
	//답안지 설정 함수
	public void setAnswerSheet()
	{
		
		string[][] ansArray = getProperty<string[][]> ("Report_Answer");
		myUIInfo.GetComponent<ReportForm> ().setAnsTxt (ansArray[currQuesIdx]);

		currAnsNum = ansArray [currQuesIdx].Length;


	}
	//키 입력 받는 함수 
	public void getKeyInput()
	{
		//x버튼: 답 결정

		myUIInfo.GetComponent<ReportForm> ().setSelectedTxt (currAnsIdx);

		if (isKeyDown (selectButton) == true) {
			int[] trueAns = getProperty<int[]> ("Report_TrueAns");

			if (trueAns [currQuesIdx] == currAnsIdx) {//정답 맞음
				correctNum = correctNum + 1;
				currQuesIdx = currQuesIdx + 1;
				currAnsIdx = 0;
				myUIInfo.GetComponent<ReportForm> ().setSelectedTxt (currAnsIdx);

				if (currQuesIdx < trueAns.Length) {

					setQuestions ();
					setAnswerSheet ();
				}

				backgroundUI.GetComponent<BackgroundForm> ().changeButtonInfo ("정답입니다");
				backgroundUI.GetComponent<BackgroundForm> ().toggleShownObject (BackgroundForm.BGPart.BG_BUTTONINFO, true);

			} else {//만일 답이 틀릴 경우
				backgroundUI.GetComponent<BackgroundForm>().changeButtonInfo("틀렸습니다.");
				backgroundUI.GetComponent<BackgroundForm> ().toggleShownObject (BackgroundForm.BGPart.BG_BUTTONINFO, true);


			}
			//addProperty ("Report_TrueAns", trueAnswer);
		}
		//y버튼: 선택 답 이동
		if (isKeyDown (moveButton) == true) {
			currAnsIdx = currAnsIdx + 1;
			if (currAnsIdx >= currAnsNum) {
				//다시 처음으로 reset
				currAnsIdx = 0;
			}
		}
	}



}
