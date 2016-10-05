using UnityEngine;
using System.Collections;

/*
 * 일단 가장 중요한 것은 report에서 답안 보기 개수에 따라 답안이 정렬되어야 한다.
 * 예를 들어 question은 3개가 존재하고 answer sheet가 각각 3개, 4개, 5개일시
 * 그에 맞춰서 여백이 정렬되어야 한다 이를 어떻게 깔끔하게 짤 수 있을 까? 걍 수학적으로 계산해야하나?
 * 
 * */

public class ReportForm : MonoBehaviour {

	Transform questionPart=null;

	Transform ansPart=null;

	void Awake()
	{
		questionPart = transform.FindChild ("QuestionRegion");
		ansPart = transform.FindChild ("AnsRegion");
	}

	void Start()
	{
		





		//이 부분에 state를 넣읍시다 일단 임시로

	}





	public void setQuestionTxt(string contents)
	{
		questionPart.GetComponent<UIEffect> ().setText (contents);
	}

	public void setAnsTxt(string[] contents)
	{
		int number = contents.Length;
		
		UIEffect_AnswerSheet uiAnswer = ansPart.GetComponent<UIEffect_AnswerSheet> ();
		uiAnswer.addTxtChild (number);

		for (int i = 0; i < number; i++) {
			uiAnswer.setTxtChild (i, contents [i]);
		}


	}

	public void setSelectedTxt(int idx)
	{
		ansPart.GetComponent<UIEffect_AnswerSheet> ().setSelectedTxt(idx);

	}
}
