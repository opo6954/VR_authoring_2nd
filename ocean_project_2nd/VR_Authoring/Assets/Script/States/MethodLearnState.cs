using UnityEngine;
using System.Collections;

/*
 * method state, 3D 물체의 각 부분을 인식하고 관련 영상 재생하기
 * 3D object를 확인하고 선택하는 효과는 일단 바꿀 수 없게 하는데 바꾸는 게 저작도구에는 넣어야 하지 않을까?
 * */
//method 각 부분 시야로 인식하는 거 좀 향상이 필요할듯 그리고 정답 판정을 넓혀야 할듯


public class MethodLearnState : StateModuleTemplate {
    
    public MethodLearnState()
	{
		myStateName = "MethodLearnState";
	}
}
