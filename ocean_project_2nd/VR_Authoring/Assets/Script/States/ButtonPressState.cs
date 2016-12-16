using UnityEngine;
using System.Collections;
/*
 * 멈춰있는 상태에서 버튼을 누를 경우 완료
 * 필요한 Property:
 * 1. TaskName: 화재를 발견했습니다.
 * 2. ButtonInfo: X버튼을 눌러 다음 task를 수행하세요
 * 
 * 필요한 Object:
 * 필요없음
 * 
 * */
public class ButtonPressState : StateModuleTemplate {

    public ButtonPressState()
	{
		myStateName = "ButtonPressState";
	}
}
