using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
	
public class OvershootStablizer : MonoBehaviour {
		
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Dot (transform.forward.normalized, (gameObject.GetComponent<NavMeshAgent> ().steeringTarget - transform.position).normalized) < Mathf.Cos (30.0f * Mathf.PI / 180.0f)) {
			//Debug.Log("Agent forward: " + transform.forward.normalized);
			//Debug.Log("Target toward: " + (gameObject.GetComponent<NavMeshAgent> ().steeringTarget - transform.position).normalized);

			gameObject.GetComponent<NavMeshAgent> ().velocity = Vector3.zero;
		}
	}
}
