using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class STK_WaypointController : MonoBehaviour {

	public enum Switch { Off, On }

	//Transform used to reference the Waypoint Arrow
	[HideInInspector]
	public Transform WaypointArrow;
	//Boolean used to automatically find the Waypont Arrow
	[HideInInspector]
	public bool AutoFindArrow = true;
	//Transform used to identify the Waypoint Arrow's target
	[HideInInspector]
	public Transform currentWaypoint;
	public Transform player;
	//[Range(0,5)]public float arrowRotationSpeed;
	public Switch configureMode;
	//Float used to determine how fast the arrow should smoothly target the next waypoint
	[Range(0.0001f,20)]public float arrowTargetSmooth;
	//Int used to determine how many Waypoints should be used
	[Range(1,100)]public int TotalWaypoints;
	//Array of Transforms, the list in order of where the Waypoint Arrow will point to next
	public Transform[] Waypoints;
	//A list of the components so we can modify them if needed
	public STK_Waypoint[] waypointComponents;
	private GameObject newWaypoint;
	private string newWaypointName;
	private int nextWP;
	private Transform arrowTarget;

	// Use this for initialization
	void Start () {
		if(Application.isPlaying){
			GameObject newObject = new GameObject();
			newObject.name = "Arrow Target";
			newObject.transform.parent = gameObject.transform;
			arrowTarget = newObject.transform;
			newObject = null;
		}else{
			CalculateWaypoints ();
		}
		ChangeTarget ();
		//Check if Arrow should be automatically found
		if(AutoFindArrow && WaypointArrow == null){
			//If true, create the Waypoint Arrow, it's automatically found and a reference is established
			CreateArrow();
			WaypointArrow = GameObject.Find("Waypoint Arrow").transform;
			WaypointArrow.gameObject.SetActive(true);
		}
		nextWP = 0;

	}


	//Draws a Gizmo in the scene view window to show the Waypoints
	void OnDrawGizmosSelected(int radius) {
		for(var i = 0; i < Waypoints.Length; i++){
			Gizmos.DrawIcon(Waypoints[i].position, "Waypoint", true);
			Gizmos.DrawWireSphere(Waypoints[i].position, waypointComponents[i].radius);
		}
	}

	void ChangeTarget(){
		float check = nextWP;
		if(check < TotalWaypoints){
			if(currentWaypoint == null)		currentWaypoint = Waypoints[0];
			currentWaypoint.gameObject.SetActive(false);
			currentWaypoint = Waypoints[nextWP];
			currentWaypoint.gameObject.SetActive(true);
			nextWP += 1;
		}
		if (check == TotalWaypoints){
			Destroy(WaypointArrow.gameObject);
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (configureMode == Switch.Off) {
			TotalWaypoints = Waypoints.Length;
		}
		//Check if the script is being executed in the Unity Editor
		#if UNITY_EDITOR
		if (configureMode == Switch.On) {
			CalculateWaypoints ();
		}
		if(player != null){
			for (var i = 0; i < TotalWaypoints; i++) {
				waypointComponents[i].player = player;
			}
		}
		#endif
		//Keep the Waypoint Arrow pointed at the Current Waypoint
		if (arrowTarget != null) {
			arrowTarget.localPosition = Vector3.Lerp (arrowTarget.localPosition, currentWaypoint.localPosition, arrowTargetSmooth * Time.deltaTime);
			arrowTarget.localRotation = Quaternion.Lerp (arrowTarget.localRotation, currentWaypoint.localRotation, arrowTargetSmooth * Time.deltaTime);
		} else {
			arrowTarget = currentWaypoint;
		}
		WaypointArrow.LookAt(arrowTarget);
	}

	public void CreateArrow(){
		GameObject instance = Instantiate(Resources.Load("Waypoint Arrow", typeof(GameObject))) as GameObject;
		instance.name = "Waypoint Arrow";
		instance = null;
	}

	public void CalculateWaypoints(){
		if (configureMode == Switch.On) {
			System.Array.Resize (ref Waypoints, TotalWaypoints);
			System.Array.Resize (ref waypointComponents, TotalWaypoints);
			for (var i = 0; i < TotalWaypoints; i++) {
				newWaypointName = "Waypoint " + (i + 1);
				if (Waypoints [i] == null) {
					foreach (Transform child in transform) {
						if (child.name == newWaypointName) {		Waypoints [i] = child;			}
					}
					if (Waypoints [i] == null) {

						newWaypoint = Instantiate (Resources.Load ("Waypoint")) as GameObject;
						newWaypoint.name = newWaypointName;
						newWaypoint.GetComponent<STK_Waypoint> ().waypointNumber = i + 1;
						if(player){
							newWaypoint.GetComponent<STK_Waypoint> ().player = player;
						}
						newWaypoint.transform.parent = gameObject.transform;
						Waypoints [i] = newWaypoint.transform;
						waypointComponents [i] = newWaypoint.GetComponent<STK_Waypoint> ();
						Debug.Log ("Waypoint Controller created a new Waypoint: " + newWaypointName);
					}
				}
			}
			CleanUpWaypoints ();
		}
	}
	
	public void CleanUpWaypoints(){
		if (configureMode == Switch.On) {
			if(Application.isPlaying){
				Debug.LogWarning ("ARROW WAYPOINTER: Turn Off 'Configure Mode' on the Waypoint Controller");
			}
			if (transform.childCount > Waypoints.Length) {
				foreach (Transform oldChild in transform) {
					if (oldChild.GetComponent<STK_Waypoint> ().waypointNumber  > Waypoints.Length) {
						DestroyImmediate (oldChild.gameObject);
					}
				}
			}
		}
	}

}