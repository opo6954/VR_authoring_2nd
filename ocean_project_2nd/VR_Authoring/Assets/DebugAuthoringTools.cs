using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DebugAuthoringTools : MonoBehaviour {

	// Use this for initialization

    GameObject[] players;
    GameObject debugPlayer;



	PlayerTemplate pt =  null;

	/*
	 * dictionary 순회 가능, keys형식의 list로 주는
	 * */

 

	void Start () {

        //기본 설정
        players = GameObject.FindGameObjectsWithTag("Player");
        debugPlayer = players[0];
        pt =  debugPlayer.GetComponent<PlayerTemplate>();
        
		ScenarioModuleTemplate myScenario = new ScenarioModuleTemplate ();




        
		ScenarioController scenarioController = debugPlayer.AddComponent<ScenarioController>();
		//xml 저장 check

        /*
        FireExtinguishScenario smt = new FireExtinguishScenario();
		smt.MyDifficulty = 13;
		smt.MyTimeout =111.303;
		smt.MyScenarioName ="Fire";
        smt.setMyParent(scenarioController);//controller만 parent로 설정해주면 된다


        smt.insertTask(madeTask1());
        smt.insertTask(madeTask2());
        smt.insertTask(madeTask3());
 
        smt.insertTask(madeTask4());
        
		

		scenarioController.insertScenario (smt);
         * 
         * */

        XmlManager xm = new XmlManager();

        xm.setMyPlayer(pt);
        xm.setMyScenarioController(scenarioController);

        List<ScenarioModuleTemplate> myScList = xm.xmlScenarioGroupLoader("demo_small.xml");


        

        for (int i = 0; i < myScList.Count; i++)
        {
            scenarioController.insertScenario(myScList[i]);
        }
        

        
		scenarioController.triggerScenario ();
        
		 

		 
        //처음 task를 시작하자 이 부분은 나중에 재생버튼(만든 플랫폼 실행)과 연관될 듯
        


	}

    // Update is called once per frame
    void Update()
    {
	} 
    /*
	public TaskModuleTemplate madeTask1()
	{
		//fire Notice
		//task 실제로 적용한 부분

        TaskModuleTemplate task = new FireNotice() as TaskModuleTemplate;

		task.myTaskName = "FireNotice";


		//property 설정                                                                                                                                                                                                                                                                                                                                                                                                                                                          

		task.addProperty("Patrol_Contents", "선박 내부를 순찰하세요");
		task.addProperty("Notice_Contents", "화재를 발견했습니다.");
		task.addProperty("Guide_Contents", "X버튼: 다음 task 진행");
		task.addProperty ("Select_Button_Info", "x");
		//object 설정


		task.addObject("Approach_to_Object",GameObject.Find("Wake"));
		

		task.readyTask ();

		return task;
	}

	public TaskModuleTemplate madeTask2()
	{
		TaskModuleTemplate task2 = new FireReport() as TaskModuleTemplate;

		task2.myTaskName = "FireReport";

		string[] questions = new string[]{"화재 장소", "화재 종류", "화재 크기"};
		string[][] answer = new string[][] {
			new string[]{ "기관실", "선장실", "객실A", "객실B" },
			new string[] { "일반화재", "유류화재", "전기화재" },
			new string[] {
				"대형",
				"중형",
				"소형"
			}
		}; 

		int[] trueAns = new int[] {0,2,1};


		task2.addProperty("taskPeriod", 0.0);
		task2.addProperty ("isHighLight", true);

		task2.addProperty ("Patrol_Contents", "선박 전화기를 통해 화재 내용을 보고하세요");
		task2.addProperty ("Report_Count", 3);
		task2.addProperty ("Report_Question", questions);
		task2.addProperty("Report_Answer", answer);
		task2.addProperty ("Report_TrueAns", trueAns);

		task2.addProperty ("Select_Button_Info", "x");
		task2.addProperty ("Move_Button_Info", "z");

		task2.addObject ("Approach_to_Object", GameObject.Find ("ShipPhone"));


		
		

		task2.readyTask ();



		return task2;
	}

	public TaskModuleTemplate madeTask3()
	{
		TaskModuleTemplate task3 = new FireAlarm() as TaskModuleTemplate;

		task3.myTaskName = "FireAlarm";

		task3.addProperty ("taskPeriod", 0.0);
		task3.addProperty ("isHighLight", true);

		task3.addProperty ("SoundName", "Whistling");
		task3.addProperty ("Loop", true);
		task3.addProperty ("Patrol_Contents", "화재 경보기를 찾으세요");
		task3.addProperty ("Notice_Contents", "화재 경보기를 발견했습니다.");
		task3.addProperty ("Guide_Contents", "x버튼: 화재 경보기 동작");
		task3.addProperty ("Select_Button_Info", "x");

		task3.addObject ("Sound_from_Object", GameObject.Find ("FireAlarm"));
		task3.addObject ("Approach_to_Object", GameObject.Find ("FireAlarm"));

		
		

		task3.readyTask ();

		return task3;
	}
	public TaskModuleTemplate madeTask4()
	{
		string[] videos = new string[]{"4","2","3","1"};
		int[] partAns = new int[]{ 3, 1, 2, 0 };

		TaskModuleTemplate task4 = new FireMethod() as TaskModuleTemplate;

		task4.myTaskName = "FireMethod";



		task4.addProperty ("taskPeriod", 0.0);
		task4.addProperty ("isHighLight", true);

		task4.addProperty ("Approach_Distance", 3.0f);
		task4.addProperty ("Approach_Angle", 3.0f);


		task4.addProperty ("Patrol_Contents", "초기 진화를 위한 소화기를 찾으세요");
		task4.addProperty ("Notice_Contents", "소화기를 동작하세요");
		task4.addProperty ("Guide_Contents", "X버튼: 소화기 동작 미션 시작");
		task4.addProperty ("Select_Button_Info", "x");
		task4.addProperty ("Skip_Button_Info", "z");
		task4.addProperty ("PartCount", 4);
		task4.addProperty ("isVideo", true);
		task4.addProperty ("VideoName", videos); 
		task4.addProperty ("PartAnswer", partAns);

		task4.addObject ("Approach_to_Object", GameObject.Find ("FireExtinguisher_Segment"));
		task4.addObject ("Interaction_to_Object", GameObject.Find ("FireExtinguisher_Segment"));

		
		

		task4.readyTask ();

		return task4;
	}
    */

	public void SaveData()//scenario내의 task의 property 저장 및 obj 저장
	{
		
	}
	public void LoadData()//scenario내의 task의 property 불러오기 및 obj 불러오기
	{
	}

	public void SaveObj()//obj의 position, rotation만 저장해도 될듯
	{
	}

	public void LoadObj()//obj의 position, rotation을 불러와서 obj를 instantiate하자
	{
	}

}
