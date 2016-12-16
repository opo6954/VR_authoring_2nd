using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * 특정 obj와의 거리가 특정 값 미만일 경우 종료
 * 
 * 필요한 property:
 * 1. TaskName: 선박 내부를 순찰하세요
 * 필요한 object:
 * 1. Object: "WakeUpObject"
 
 * */

public class ApproachObjState : StateModuleTemplate {

	public ApproachObjState()
	{
		myStateName = "ApproachObjState";
	}
}
