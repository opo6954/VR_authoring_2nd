using UnityEngine;
using System.Collections;

/*
* 2. 장비 동작a: 특정 장비에 다가가서 특정 버튼을 누르면 장비 동작(혹은 비동작)하면서 성공
* 포함되는 task: 형상물 켜기(형상물 내부에 light 켜기), 환풍기 끄기(환풍기 버튼 색깔 바꾸기), 비상대응알림 켜기(소리 재생) 등
*/

public class SimpleInteractionClientState : ClientStateModuleTemplate {
	public GameObject playerObject;
	public GameObject targetObject;
	public bool desiredState;
	private SimpleInteractionModuleTemplate siModule;
	private GameObject tooltip;
	private bool isInitialized;
	private bool finished;

	public void setPlayer(GameObject ob) {
		playerObject = ob;
	}
	public void setPlayer(string ob) {
		playerObject = GameObject.Find (ob);
	}
	public void setTarget(GameObject ob) {
		targetObject = ob;
		siModule = targetObject.GetComponent<SimpleInteractionController> ().siModule;
	}
	public void setTarget(string ob) {
		targetObject = GameObject.Find (ob);
		siModule = targetObject.GetComponent<SimpleInteractionController> ().siModule;
	}

	//초기 clientState 시작 후 init (처음 한 번만 불림)
	public override void Init()
	{
		isInitialized = false;
		tooltip = GameObject.Instantiate ((GameObject)Resources.Load ("UserInterface/DefaultUI/Prefab/3DTextForm"));
		tooltip.transform.name = "SimpleInteraction Notifier";
		tooltip.active = false;

		myClientState = "SimpleInteractionClientState";
		base.Init();
	}

	//ClientState가 시작한 후 계속 불림
	public override void Process()
	{
		if (!isInitialized) {
			if (siModule != null && playerObject != null) {
				tooltip.GetComponent<TextMesh> ().text = siModule.siMessage;
				isInitialized = true;
			}
			return;
		}

		Camera playerCamera = playerObject.GetComponentInChildren<Camera> ();
		Vector3 screenPoint = playerCamera.WorldToViewportPoint (targetObject.transform.position);
		Vector3 direction = (targetObject.transform.position - playerCamera.transform.position).normalized;

		// Debugging Purposes -->
		//		Vector3 temp = target.transform.position;
		//		Debug.Log (string.Format("x :{0}, y :{1}, z :{2}", temp.x, temp.y, temp.z));
		// <-- Debugging Purposes

		if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1 && screenPoint.z < 5) {
			tooltip.active = true;
			tooltip.transform.position = targetObject.transform.position - (direction);
			tooltip.transform.rotation = playerCamera.transform.rotation;
			// If press X key, switch on/off the simple interaction object
			if (Input.GetKeyDown(KeyCode.X)) {
				tooltip.active = false;
				if (desiredState)
					siModule.stateOn ();
				else
					siModule.stateOff ();
			}
		} else {
			tooltip.active = false;
		}
		Debug.Log ("SimpleInteractionClientState Processing...");
		base.Process();
	}

	//ClientState의 종료조건, 계속 조건을 확인해서 Goal이 true를 리턴하면 종료
	public override bool Goal()
	{
		if (desiredState == siModule.getState())
			return true;
		//Debug.Log ("SimpleInteractionClientState Ended...");
		return false;
	}
	public override void Res()
	{
		GameObject.Destroy (tooltip);
		base.Res();
	}
}
