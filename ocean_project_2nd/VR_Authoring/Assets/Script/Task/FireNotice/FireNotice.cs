using UnityEngine;
using System.Collections;

/*
 * 필요한 object 및 property
 * property: 
 * taskPeriod: 훈련시간
 * 
 * isHighLight: object에 강조표시 여부
 * 
 * Patrol_Contents: 순찰중일때 표시될 내용
 * Notice_Contents: 이상 상황을 발견했을 때 내용
 * 
 * object;
 * WakeUpObject: 화재 발견 Object
 * 
 */

public class FireNotice : TaskModuleTemplate  {

/*
    bool isNoticeFire = false;

	GameObject defaultForm=null;
    
	public FireNotice()
	{
		myTaskType = "FireNotice";
	}



    public override void TaskInit()
    {
        base.TaskInit();
        //일단 이 부분에서 UI 내용을 설정하자, 원래는 저작도구 상에서 설정해야함 나중에 timeline으로 dragNdrop할 때 표시될 내용 역시 설정을 하겠지??? 물론 그래도 기본값은 넣어야 할듯

		//필요한 properties, obj 알림
		propertiesList = new string[] {"Approach_Distance", "Approach_Angle","Patrol_Contents", "Notice_Contents", "Guide_Contents", "Select_Button_Info" };
		objectsList = new string[] {"Approach_to_Object"};
        



        //일단 첫 task니까 설정
    }


    
	public override	void readyTask()
	{
		

		myUIInfo.loadUIPrefab("DefaultForm");


		defaultForm = myUIInfo.getUIPrefab("DefaultForm");



		//defaultForm.GetComponent<DefaultForm>().changeCurrTaskInfo(getProperty<string>("Patrol_Contents"));

		//아마 추후에 이 부분도 저작할 수 있을 듯 합니다
		ApproachObjState a = new ApproachObjState(this);
		a.setProperty (getProperties ());
		a.setObject (getObjects ());

		// ,getProperty<string>("Patrol_Contents"), getObject<GameObject>("WakeUpObject"));
		ButtonPressState b = new ButtonPressState(this);
		b.setProperty (getProperties ());
		b.setObject (getObjects ());



		//myStateList에 차례대로 넣습니다.
		StateList.Add(a);
		StateList.Add(b);

        
	}
    */
   



}

