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


	public PlaySoundsState(TaskModuleTemplate _myModule, GameObject _UI) : base(_myModule, _UI)
	{

	}


	public override void setProperty (System.Collections.Generic.Dictionary<string, object> properties)
	{
		addProperty ("SoundName", properties["SoundName"]);
		addProperty ("Loop", properties["Loop"]);
	}

	public override void setObject (System.Collections.Generic.Dictionary<string, object> objects)
	{
		addObject ("Sound_from_Object", objects["Sound_from_Object"]);
	}

	public override void Init ()
	{
		base.Init ();

		myStateName = "소리 파일 재생";




		if (isContainObject ("Sound_from_Object") == false || isContainProperty ("SoundName") == false) {
			Debug.Log ("SoundName과 Sound_from_Object가 설정되지 않았습니다.");
		} else {
			AudioSource audioSource = getObject<GameObject> ("Sound_from_Object").AddComponent<AudioSource>();

			audioSource.clip = Resources.Load ("Sound/" + getProperty<string> ("SoundName")) as AudioClip;


		

			audioSource.loop = getProperty<bool> ("Loop");

			audioSource.Play ();

		}


		//음악 재생하자
	}

	public override void Process ()
	{
		base.Process ();
	}

	public override void Res ()
	{
		base.Res ();
	}

	public override bool Goal ()
	{
		return true;
	}
	
}
