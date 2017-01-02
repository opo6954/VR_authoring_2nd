using UnityEngine;
using System.Collections;

/* 6. 대화 시작: 선원 가까이에서 대화 버튼을 누르면 대화 시작되면서 종료
 * 
 * 7. 대화 정보 교환: 상대로부터 대화 내용을 보여주는 상태에서 내가 보낼 정보를 선택하면 성공
*/


public class DialogueStartClientState : ClientStateModuleTemplate {
	public GameObject target;	// 대화를 걸 상대 플레이어의 이름
	public Camera playerCamera;	// 플레이어 자신에게 달린 메인카메라

	private GameObject tooltip;	// 대화를 걸 대상의 옆에 띄울 말풍선 아이콘
	private bool finished;

	// 상대 플레이어의 이름을 매개로 GameObject를 찾아 넣는다
	public void settarget(string targetName)
	{
		target = GameObject.Find (targetName);
	}
	// 플레이어 자신의 이름을 매로 메인카메라를 찾아 넣는다
	public void setplayerCamera(string selfName)
	{
		playerCamera = GameObject.Find (selfName).GetComponentInChildren<Camera> ();
	}


	//초기 clientState 시작 후 init (처음 한 번만 불림)
	public override void Init()
	{
		// Debugging Purposes -->
//		settarget ("DummyPlayer");
//		setplayerCamera ("FirstPersonCharacter");
//		Debug.Log ("My Camera: " + playerCamera.transform.name);
		// <-- Debugging Purposes

		tooltip = GameObject.Instantiate((GameObject)Resources.Load ("UserInterface/DialogueUI/DialogueNotifier"));
		tooltip.transform.name = "DialogueNotifier";
		tooltip.active = false;

		finished = false;
		myClientState = "DialogueStartClientState";
		base.Init();
	}
	//ClientState가 시작한 후 계속 불림
	public override void Process()
	{
		Debug.Log ("DialogueStartClientState Processing...");
		if (target == null || playerCamera == null)
			return;
		
		Vector3 screenPoint = playerCamera.WorldToViewportPoint (target.transform.position);

		// Debugging Purposes -->
//		Vector3 temp = target.transform.position;
//		Debug.Log (string.Format("x :{0}, y :{1}, z :{2}", temp.x, temp.y, temp.z));
		// <-- Debugging Purposes

		if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z < 5) {
			tooltip.active = true;
			tooltip.transform.position = target.transform.position;
			tooltip.transform.rotation = playerCamera.transform.rotation;

			if (Input.GetKeyDown(KeyCode.X)) {
				tooltip.active = false;
				finished = true;
			}
		} else {
			tooltip.active = false;
		}

		base.Process();
	}
	//ClientState의 종료조건, 계속 조건을 확인해서 Goal이 true를 리턴하면 종료
	public override bool Goal()
	{
		if (finished) {
			Debug.Log ("DialogueStartClientState Ended...");
			GameObject.Destroy (tooltip);
			//Destroy (this);
			return true;
		}
		return false;
	}
	//마지막 ClientState가 종료된 후 1번 불림
	public override void Res()
	{
		base.Res();
	}




}
