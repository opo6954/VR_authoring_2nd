using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(STK_WaypointController))]
public class GameManagerEditor : Editor {

	[MenuItem("Tools/TurnTheGameOn/Waypoint Controller")]
	static void Create(){
		GameObject instance = Instantiate(Resources.Load("Waypoint Controller", typeof(GameObject))) as GameObject;
		instance.name = "Waypoint Controller";
		instance = null;
	}

	public override void OnInspectorGUI(){

		STK_WaypointController waypointController = (STK_WaypointController)target;
		Texture waypointTexture = Resources.Load("Arrow Waypointer") as Texture;
		GUIStyle inspectorStyle = new GUIStyle(GUI.skin.label);
		inspectorStyle.fixedWidth = 256;
		inspectorStyle.fixedHeight = 64;
		inspectorStyle.margin = new RectOffset((Screen.width-256)/2, (Screen.width-256)/2, 0, 0);

		EditorGUILayout.Space();
		GUILayout.Label(waypointTexture,inspectorStyle);

		EditorGUILayout.HelpBox ("Current Waypoint:\n" + waypointController.currentWaypoint.ToString() + "\n\n*Assign the Player transform to use radius for waypoints.", MessageType.Info);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		DrawDefaultInspector ();


		if (GUILayout.Button ("Cleanup Old Wayponts")) {
			waypointController.CleanUpWaypoints ();
		}
	}
	
}