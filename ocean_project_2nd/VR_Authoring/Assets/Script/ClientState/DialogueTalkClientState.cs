using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 6. 대화 시작: 선원 가까이에서 대화 버튼을 누르면 대화 시작되면서 종료
 * 
 * 7. 대화 정보 교환: 상대로부터 대화 내용을 보여주는 상태에서 내가 보낼 정보를 선택하면 성공
*/

public class DialogueTalkClientState : ClientStateModuleTemplate {
	public string[] options;
	public int answer;

	private int num_items;
	private GameObject dialogue;
	public void setoptions(string[] slist) {
		options = slist;
	}

	//초기 clientState 시작 후 init (처음 한 번만 불림)
	public override void Init()
	{
		// Debugging Purposes -->
//		setoptions(new string[] {"이협우", "박광빈", "박철웅"});
		// <-- Debugging Purposes

		num_items = 0;
		dialogue = GameObject.Instantiate ((GameObject)Resources.Load ("UserInterface/DialogueUI/Dialogue GUI"));
		myClientState = "DialogueTalkClientState";
		base.Init();
	}
	//ClientState가 시작한 후 계속 불림
	public override void Process()
	{
		Debug.Log ("DialogueTalkClientState Processing...");
		if (options == null) {
			dialogue.active = false;
			return;
		} else {
			dialogue.active = true;
		}
			
		if (num_items > 0) {
			;
		} else {
			for (int i = 0; i < options.Length; i++) {
				Text text = GameObject.Find ("Button" + i.ToString() + "/Text").GetComponent<Text>();
				text.text = options [i];
				num_items++;
			}
			for (int i = num_items; i < 4; i++) {
				GameObject.Find ("Button" + i.ToString()).active = false;
			}
		}

		base.Process();
	}
	//ClientState의 종료조건, 계속 조건을 확인해서 Goal이 true를 리턴하면 종료
	public override bool Goal()
	{
		answer = dialogue.GetComponent<DialogueAnswerContainer> ().answer;
		if (answer >= 0) {
			//Debug.Log ("DialogueTalkClientState Ended...");
			return true;
		}
		return false;
	}
	public override void Res()
	{
		GameObject.Destroy (dialogue);
		base.Res();
	}
}
