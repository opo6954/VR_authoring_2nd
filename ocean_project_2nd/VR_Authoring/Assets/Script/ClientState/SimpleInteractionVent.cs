using UnityEngine;
using System.Collections;

public class SimpleInteractionVent : SimpleInteractionModuleTemplate {
	private Animator anim;
	private AudioSource audio;

	public override bool isEnded () {
		return false;
	}

	public override void stateOn() {
		if (curState)
			return;
		anim.enabled = true;;
		audio.enabled = true;
		curState = true;
	}
	public override void stateOff() {
		if (!curState)
			return;
		anim.enabled = false;
		anim.enabled = false;
		curState = false;
	}

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator> ();
		audio = gameObject.GetComponentInChildren<AudioSource> ();
		anim.enabled = false;
		audio.enabled = false;
	}

	// Update is called once per frame
	void Update () {
	}
}