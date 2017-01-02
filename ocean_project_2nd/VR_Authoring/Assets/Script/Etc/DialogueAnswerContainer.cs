using UnityEngine;
using System.Collections;

public class DialogueAnswerContainer : MonoBehaviour {
	public int answer;
	// Use this for initialization

	public void setAnswer(int i) {
		answer = i;
	}

	void Start () {
		answer = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
