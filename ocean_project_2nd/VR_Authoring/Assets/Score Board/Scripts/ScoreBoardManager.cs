using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreBoardManager : MonoBehaviour {
	private bool flag;
	public CanvasGroup cg;
	public Text content;
	private List<ObjectiveContent> oblist;

	// Use this for initialization
	void Start () {
		flag = false;
		oblist = new List<ObjectiveContent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Back")) {
//			Debugging purposes
//			if (!flag)
//				Debug.Log ("Fire1 button down!");
			flag = true;
			if (cg.alpha < 1.0f)
				cg.alpha += 0.1f;
		} else {
			cg.alpha = 0.0f;
			flag = false;
		}

//		 Debugging purposes
//		if (Input.GetKeyDown(KeyCode.Q)) {
////			gameObject.GetComponent<TaskTimer>().resetTimer();
////			gameObject.GetComponent<TaskTimer>().startTimer ();
//			addObjective ("Hello" + string.Format("{0:0.000}", Random.value));
//		}
	}

	public void addObjective(string s) {
		content.text = "";

		gameObject.GetComponent<TaskTimer> ().startTimer ();
		oblist.Add(new ObjectiveContent(s, 0));
		if (oblist.Count > 1) {
			oblist [oblist.Count - 2].Timestamp = gameObject.GetComponent<TaskTimer> ().getTime ();
		}

		foreach (ObjectiveContent ob in oblist) {
			if (ob.Timestamp == 0)
				content.text += "<color=white>";
			content.text += ob.Objective;
			if (ob.Timestamp != 0) {
				long elapsed = ob.Timestamp;
				double seconds = (elapsed % 60000) / 1000.0;
				int minutes = (int)(elapsed / 60000) % 60;
				int hours = (int)(elapsed / 3600000);
				string stamp = string.Format ("\t{0:00}:{1:00}:{2:00.00}\n", hours, minutes, seconds);
//				Debugging purposes
//				Debug.Log (stamp);
				content.text += stamp;
			}
			if (ob.Timestamp == 0)
				content.text += "</color>";
		}
	}

	public class ObjectiveContent {
		private string objective;
		private long timestamp;

		public ObjectiveContent(string s, long t) {
			this.objective = s;
			this.timestamp = t;
		}

		public string Objective {
			get { return this.objective; }
			set { this.objective = value; }
		}

		public long Timestamp {
			get { return this.timestamp; }
			set { this.timestamp = value; }
		}
	}
}
