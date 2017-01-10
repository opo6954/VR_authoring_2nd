using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using debug = UnityEngine.Debug;

public class TaskTimer : MonoBehaviour {
	public GameObject timer;
	private Stopwatch sw = new Stopwatch();
	private	Text text;
	// Use this for initialization
	void Start () {
		sw.Reset ();
		text = timer.GetComponent<Text>();
	}
	 
	// Update is called once per frame
	void Update () {
		long elapsed = sw.ElapsedMilliseconds;
		double seconds = (elapsed % 60000) / 1000.0;
		int minutes = (int)(elapsed / 60000) % 60;
		int hours = (int)(elapsed / 3600000);
		text.text = string.Format ("{0:00}:{1:00}:{2:00.00}", hours, minutes, seconds);
	}

	public long getTime() {
		return sw.ElapsedMilliseconds;
	}

	public void resetTimer() {
		sw.Reset ();
	}

	public void startTimer() {
		sw.Start ();
	}
}
