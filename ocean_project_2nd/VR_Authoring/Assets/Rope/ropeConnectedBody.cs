using UnityEngine;
using System.Collections;

public class ropeConnectedBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ConfigurableJoint> ().connectedBody = gameObject.transform.parent.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
