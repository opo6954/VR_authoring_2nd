using UnityEngine;
using System.Collections;
/*
 * 특정 sound 파일을 재생한다
 * 필요한 Property:
 * 1. SoundName Sound file Name: 재생할 소리 파일
 * 2. Loop: 반복 재생 여부
 * 
 * 필요한 Object:(소리 파일을 붙일 용도)
 * 1. Sound_Object
 * 
 * 
 * */
public class PlaySoundsState : StateModuleTemplate {
	
    public PlaySoundsState()
	{
		MyStateName = "PlaySoundsState";
	}

}
