using UnityEngine;
using System.Collections;
/*
User의 task 진행 상황을 모든 object에서 공유할 수 있게 해주는 global variable 저장소용 script.
unity에서 static은 global variable이라서 어디서든 접근 가능하다.(이 script가 아무 object에도 포함되어 있지 않아도!)
*/
public class UserTaskStatus : MonoBehaviour {
    
    public static bool FireRecognize = false; // 화재 인식 여부
    public static bool FireReported = false; // 화재 보고 여부
    //for debug by lhw
    public static bool GetFireExtinguisher = false; // 소화기 들었는지 여부
    public static bool GetReadyExtinguisher = true;//소화기를 든 이후 작동법대로 진행하고 나서 소화기로 소화하기 직전
    public static bool HumanRescued = false; // 사람들 구출 했는지 여부



	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	}
}
