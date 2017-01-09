using UnityEngine;
using System.Collections;

public class SimpleInteractionFireAlarm : SimpleInteractionModuleTemplate {
	public override bool isEnded () {
		return false;
	}

	public override void stateOn() {
		if (curState)
			return;
		gameObject.GetComponent<AudioSource> ().mute = false;
		curState = true;
	}
	public override void stateOff() {
		if (!curState)
			return;
		gameObject.GetComponent<AudioSource> ().mute = true;
		curState = false;
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	}
}
