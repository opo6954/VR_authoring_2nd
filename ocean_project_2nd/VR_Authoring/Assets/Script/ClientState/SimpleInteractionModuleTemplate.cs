using UnityEngine;
using System.Collections;

public class SimpleInteractionModuleTemplate : MonoBehaviour {
	public string siMessage;
	protected bool curState;

	public bool getState() {
		return curState;
	}
	public virtual bool isEnded () {
		return false;
	}
	public virtual void stateOn() {
	}
	public virtual void stateOff() {
	}

	// Use this for initialization
	void Start () {
		siMessage = "Empty String!";
		curState = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
