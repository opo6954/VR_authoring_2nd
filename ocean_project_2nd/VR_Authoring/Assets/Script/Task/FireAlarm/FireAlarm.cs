using UnityEngine;
using System.Collections;
/*
 * Fire alarm에 필요한 property 및 object
 * 
 * Property:
 * SoundName: string, 재생할 파일 이름
 * isLoop: bool, 연속 재생 여부
 * Patrol_Contents: 알람기 찾는 목표 부여
 * Alarm_Contents : 알람기 동작 목표 부여
 * Guide_Contents: 알람기 동작 키 알려주기
 * 
 * Object:
 * SoundObj: sound를 붙일 object
 * 
 * */

public class FireAlarm : TaskModuleTemplate
{
    /*
	GameObject defaultForm = null;

	public FireAlarm()
	{
		myTaskType = "FireAlarm";
	}

	public override void TaskInit ()
	{
		base.TaskInit ();


		propertiesList = new string[] {"Approach_Distance", "Approach_Angle","SoundName", "Loop", "Patrol_Contents", "Notice_Contents", "Guide_Contents", "Select_Button_Info" };
		objectsList = new string[] {"Sound_from_Object", "Approach_to_Object" };

	}

	public override void readyTask ()
	{
		ApproachObjState a = new ApproachObjState (this);

		a.setProperty (getProperties ());
		a.setObject (getObjects ());

		ButtonPressState b = new ButtonPressState (this);
		b.setProperty (getProperties ());
		b.setObject (getObjects ());

		PlaySoundsState c = new PlaySoundsState (this);
		c.setProperty (getProperties ());
		c.setObject (getObjects ());

		myStateList.Add (a);
		myStateList.Add (b);
		myStateList.Add (c);
	}



	public override void TaskStart ()
	{
		
		myUIInfo.loadUIPrefab ("DefaultForm");
		defaultForm = myUIInfo.getUIPrefab ("DefaultForm");

		//myStateList [0].setUI (defaultForm);
		//myStateList [1].setUI (defaultForm);
		//myStateList [2].setUI (defaultForm);


        base.TaskStart();
	}

	public override void TaskProcess ()
	{
		base.TaskProcess ();
	}
	public override void TaskFinish ()
	{
		base.TaskFinish ();

        GameObject.Destroy(defaultForm);

		
	}
     * */

}
