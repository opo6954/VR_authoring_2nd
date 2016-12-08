using UnityEngine;
using System.Collections;

/*
 * method state, 3D 물체의 각 부분을 인식하고 관련 영상 재생하기
 * 3D object를 확인하고 선택하는 효과는 일단 바꿀 수 없게 하는데 바꾸는 게 저작도구에는 넣어야 하지 않을까?
 * */
//method 각 부분 시야로 인식하는 거 좀 향상이 필요할듯 그리고 정답 판정을 넓혀야 할듯


public class MethodLearnState : StateModuleTemplate {

	//아마 추가적인 information이 들어가겠지?


	float originHeight;

	float objOffsetRot=30.0f;

	Transform[] objParts;

	GameObject backgroundUI;//background UI instance
	GameObject cloneObj;//clone obj

	Material[] partObjectify;
	Material partSelectObjectify;

    Ray firstRay;

	string selectButton="";
	string skipButton="";

	int selectIdx=-1;
	int currIdxOrder=0;

	int[] trueAns;

	bool isPlayingVideo = false;



	 




	public MethodLearnState(TaskModuleTemplate _myModule) : base(_myModule)
	{
		myStateName = "MethodLearnState";
	}

	public override void setProperty (System.Collections.Generic.Dictionary<string, object> properties)
	{
		
		addProperty ("PartCount", properties ["PartCount"]);//활성화된 부분 개수
		addProperty("isVideo", properties["isVideo"]);//Video 여부
		addProperty ("VideoName", properties ["VideoName"]);//video 이름
		addProperty("PartAnswer", properties["PartAnswer"]);//part 선택 순서


		addProperty ("Select_Button_Info", properties ["Select_Button_Info"]);
		//raycasting으로 highligh된 부분 선택하는 키 && 영상 재생시 일시정지 및 다시 재생 버튼 및 다시 재생 버튼(재생 다 끝날시)
		addProperty ("Skip_Button_Info", properties ["Skip_Button_Info"]);//영상 skip 버튼



		selectButton = getProperty<string> ("Select_Button_Info");
		skipButton = getProperty<string> ("Skip_Button_Info");
		trueAns = getProperty<int[]> ("PartAnswer");

		 


	}

	public override void setObject (System.Collections.Generic.Dictionary<string, object> objects)
	{
		addObject ("Interaction_to_Object", objects ["Interaction_to_Object"]);
	}








	///////////////////////////////INITIALIZE...
	/// 
	public void setInitScaleObj(GameObject cloneObj)
	{
		//scale 조절
		Vector3 myScale = cloneObj.transform.localScale;
		myScale = myScale * 1.5f;
		cloneObj.transform.localScale = myScale;
	}

	public void setInitPosObj(GameObject cloneObj)
	{
		//위치 조절
		firstRay = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2,0));
		

		Vector3 objPosition = new Vector3 ();


        objPosition = firstRay.origin + firstRay.direction * 3;


        objPosition.y = objPosition.y - cloneObj.GetComponent<MeshFilter>().mesh.bounds.max.z / 100.0f - 0.5f;


        /*
		objPosition = ray.origin -  ray.direction * 5;
        objPosition.x = ray.origin.x;

        
		objPosition.y = objPosition.y - cloneObj.GetComponent<MeshFilter> ().mesh.bounds.max.z / 100.0f - 0.5f;
        */

        cloneObj.transform.localPosition = objPosition;
	}

	public void setInitRotObj(GameObject cloneObj)
	{ 
		//각도 조절
		Quaternion q = cloneObj.transform.localRotation;
		Vector3 myEuler = myPosition.rotation.eulerAngles;
		Vector3 objEuler = new Vector3 ();
		objEuler.x = cloneObj.transform.rotation.eulerAngles.x;
		objEuler.y = myEuler.y + objOffsetRot;
		objEuler.z = myEuler.z;

		cloneObj.transform.localRotation = Quaternion.Euler(objEuler);
	}



	public override void Init ()
	{
		
		base.Init ();


		backgroundUI = myModuleInfo.getBackgroundUI ();//backgroundUI 가져오기


		lockFPSmoveScreen (true);



		cloneObj = GameObject.Instantiate (getObject<GameObject> ("Interaction_to_Object"));

		getObject<GameObject> ("Interaction_to_Object").SetActive (false);//원래의 obj는 false로 숨기자

		cloneObj.transform.SetParent(myPlayerInfo.transform);


		objParts = new Transform[getProperty<int> ("PartCount")];
		partObjectify = new Material[getProperty<int> ("PartCount")];

		partSelectObjectify = Resources.Load ("Shader/Edge_White", typeof(Material)) as Material;

		for (int i = 0; i < getProperty<int> ("PartCount"); i++) {
			objParts [i] = cloneObj.transform.GetChild (i);
			objParts [i].GetComponent<MeshRenderer> ().enabled = true;

			partObjectify [i] = objParts [i].GetComponent<MeshRenderer> ().materials [0];

			}

        



		setInitScaleObj (cloneObj);
		setInitPosObj (cloneObj);
		setInitRotObj (cloneObj);

		myUIInfo.GetComponent<MethodForm> ().setVideoName (getProperty<string[]> ("VideoName"));

	}


	/////////////////////////////////////////PROCESS
	public void playVideo()
	{
		myUIInfo.GetComponent<MethodForm> ().toggleShownVideoInfo (true);
		myUIInfo.GetComponent<MethodForm> ().playVideo (selectIdx);
		isPlayingVideo = true;
	}

	public void hitTest()
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hitObj;

		var layerMask = 1 << 8;

		if (Physics.Raycast (ray, out hitObj, 10.0f,layerMask)) {

			for (int i = 0; i < objParts.Length; i++) {
				if (hitObj.transform.name == objParts [i].name) {
					//objParts[i].GetComponent<MeshRenderer> ().materials [0] = partSelectObjectify;
					hitObj.transform.GetComponent<MeshRenderer> ().material = partSelectObjectify;

					selectIdx = i;

				} else {
					objParts [i].GetComponent<MeshRenderer> ().material = partObjectify [i];
				}
			}			
		}
	}

	public void videoOptionTest()
	{
		isPlayingVideo = myUIInfo.GetComponent<MethodForm> ().isPlayMode;
		if(isPlayingVideo == true)
		{
			if(isKeyDown(selectButton))
			{
				myUIInfo.GetComponent<MethodForm>().stopVideo();
			}
			else if(isKeyDown(skipButton))
			{
				
				myUIInfo.GetComponent<MethodForm> ().skipVideo ();




			}
		}

	}

	public void selectTest()
	{
		if(isKeyDown(selectButton) && isPlayingVideo == false)
		{
			//정답일시, video 재생
			if (selectIdx == trueAns [currIdxOrder]) {
				currIdxOrder++;

				objParts [selectIdx].GetComponent<MeshRenderer> ().enabled = false;

				playVideo ();

				backgroundUI.GetComponent<BackgroundForm> ().changeButtonInfo ("맞았습니다");
				backgroundUI.GetComponent<BackgroundForm> ().toggleShownObject (BackgroundForm.BGPart.BG_BUTTONINFO, true);


			} else {//오답일시 오답 표시하기 background에 ??
				backgroundUI.GetComponent<BackgroundForm>().changeButtonInfo("틀렸습니다");
				backgroundUI.GetComponent<BackgroundForm> ().toggleShownObject (BackgroundForm.BGPart.BG_BUTTONINFO, true);
			}
		}
	}




	public override void Process ()
	{
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        Debug.DrawRay (firstRay.origin, firstRay.direction * 1000, Color.red);


        myPlayerInfo.transform.GetChild(0).transform.GetChild(3).transform.position = ray.origin + ray.direction * 5;


        



		videoOptionTest ();

		hitTest ();
		selectTest ();





		base.Process ();
	}

	public override bool Goal ()
	{
		if (currIdxOrder == getProperty<int> ("PartCount") && isPlayingVideo == false)
			return true;
		return false;
	}

	public override void Res ()
	{
		cloneObj.SetActive (false);
		getObject<GameObject> ("Interaction_to_Object").SetActive (true);

		lockFPSmoveScreen (false);


		base.Res ();
	}
	
}
