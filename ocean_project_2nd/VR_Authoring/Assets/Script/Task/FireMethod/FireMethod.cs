using UnityEngine;
using System.Collections;
/*
 * FireMethod: 소화기를 찾아서 준비하는 task
 * 필요한 state: approach state, buttonPressed state, method state(Object select and Video Education)까지 필요
 * method state는 이전에 만들었던 소화기 부분 차례대로 선택하는 류의 동작 필요할 듯
 * 
 * d
 * 
 * 
 * 필요한 Property 및 Object
 * Property:
 * partCount: int, 전체 활성화 부분 개수
 * isVideo: video 재생 여부를 걍 초기 파라미터로 조정하자
 * VideoName: string[], 준비된 영상 교육 영상
 * 
 * 
 * Object:
 * 
 * TargetObject
 * 
  object가 필요한데 objectify할 부분별 object 모두 필요 혹은 set로 가지고 있기 아니면 object 설정 창에서 attribute를 설정할 수 있도록 할까???
 * 이전까지 mesh renderer를 걍 꺼놓으면 normal한 소화기로 유지된다
 * 
 * 필요한 state:
 * Approach
 * ButtonPress(소화기 동작)
 * Method
 * 
 * */
public class FireMethod : TaskModuleTemplate {

	GameObject defaultForm = null;

	GameObject methodForm = null;
	public FireMethod()
	{
		myTaskType = "FireMethod";
	}

	public override void TaskInit ()
	{
		base.TaskInit ();

		propertiesList = new string[] {"Approach_Distance", "Approach_Angle", "Patrol_Contents", "Notice_Contents", "Guide_Contents", "Select_Button_Info", "Skip_Button_Info", "PartCount", "isVideo", "VideoName", "PartAnswer"};
		objectsList = new string[] { "Approach_to_Object", "Interaction_to_Object" };

	}

	public override void TaskProcess ()
	{
		base.TaskProcess ();
	}

	public override void readyTask ()
	{
		
		ApproachObjState a = new ApproachObjState (this);
		a.setProperty (getProperties ());
		a.setObject (getObjects ());

		ButtonPressState b = new ButtonPressState (this);
		b.setProperty (getProperties ());
		b.setObject (getObjects ());


		MethodLearnState c = new MethodLearnState (this);
		c.setProperty (getProperties ());
		c.setObject (getObjects ());

		myStateList.Add (a); 
		myStateList.Add (b);
		myStateList.Add (c);


	}

	public override void TaskStart ()
	{
		

		myUIInfo.loadUIPrefab ("DefaultForm");
		myUIInfo.loadUIPrefab ("MethodForm");

		defaultForm = myUIInfo.getUIPrefab ("DefaultForm");
		methodForm = myUIInfo.getUIPrefab ("MethodForm");

		//firemethod task만의 고유한 부분이니까 이렇게 hard coding해도 됨... 근데 state로 task를 저작하는 단계로 진입하면 힘들듯...
		myStateList[0].setUI(defaultForm);
		myStateList [1].setUI (defaultForm);
		myStateList [2].setUI (methodForm);


        base.TaskStart();
	}


	//Delete the UI instance please...
	public override void TaskFinish ()
	{
		base.TaskFinish ();

		GameObject.Destroy (defaultForm);
		GameObject.Destroy (methodForm);

		myUIInfo.deleteUIPrefab ("DefaultForm");
		myUIInfo.deleteUIPrefab ("MethodForm");
	}
}
