using UnityEngine;
using System.Collections;

public class SimpleInteractionBalloon : SimpleInteractionModuleTemplate {
	public Material matOn;
	public Material matOff;
	public Light lightSource;
	public float targetHeight;
	private bool flag;

	private IEnumerator raiseBalloon() {
		float curY = transform.position.y;
		while (transform.position.y - curY < targetHeight) {
//			Debug.Log ("Origianl Y:" + curY + "  Current Y:" + transform.position.y);
			transform.position += new Vector3 (0.0f, 0.01f, 0.0f);
			yield return new WaitForSeconds (0.01f);
		}
	}

	public override bool isEnded () {
		return false;
	}

	public override void stateOn() {
		if (curState)
			return;
		gameObject.GetComponent<MeshRenderer> ().material = matOn;
		lightSource.intensity = 1.0f;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		curState = true;
		flag = true;
	}
	public override void stateOff() {
		if (!curState)
			return;
		gameObject.GetComponent<MeshRenderer> ().material = matOff;
		lightSource.intensity = 0.0f;
		gameObject.GetComponent<Rigidbody> ().isKinematic = false;
		curState = false;
	}

	// Use this for initialization
	void Start () {
		flag = false;
        curState = false;
	}

	// Update is called once per frame
	void Update () {
		if (flag) {
			flag = false;
			StartCoroutine ("raiseBalloon");
		}
	}
}