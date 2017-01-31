using UnityEngine;
using System.Collections;

public class PatrolClientState : ClientStateModuleTemplate {
	//초기 client State시작후 1번만 불림
	public override void Init()
	{
		myClientState = "PatrolClientState";
		base.Init();
	}
	//client state가 살아있는 동안 계속 불림
	public override void Process()
	{
		Debug.Log ("PatrolClientState Processing...");
		base.Process();
	}
	//Client state의 종료 조건임, 계속 조건 확인함
	public override bool Goal()
	{
		return false;
	}
	//Client state가 종료된 후에 하는 일
	public override void Res()
	{
		Debug.Log(myClientState + " client state 종료");
	}
}
