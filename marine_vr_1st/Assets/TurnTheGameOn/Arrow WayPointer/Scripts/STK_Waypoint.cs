using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class STK_Waypoint : MonoBehaviour {

	[HideInInspector] public int waypointNumber;
	public Transform player;
	public int radius;
	private GameObject controller;

	// Use this for initialization
	void Start () {
		controller = this.transform.parent.gameObject;
	}

	void Update(){
		if (player) {
			if(Vector3.Distance(transform.position, player.position) < radius){
				controller.SendMessage("ChangeTarget", SendMessageOptions.RequireReceiver);
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		if(col.gameObject.tag == "Player"){
			controller.SendMessage("ChangeTarget", SendMessageOptions.RequireReceiver);
		}
	}
	
	void OnDrawGizmosSelected(){
		controller = this.transform.parent.gameObject;
		controller.SendMessage("OnDrawGizmosSelected", radius, SendMessageOptions.DontRequireReceiver);
	}


}