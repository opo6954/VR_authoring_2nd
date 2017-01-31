using UnityEngine;
using UnityEditor;
using System.Collections;

public class LoadShipModel : MonoBehaviour {
	// Add a menu item named "Do Something" to MyMenu in the menu bar.
	[MenuItem ("Ships/Load ShipModel")]
	static void LoadShip () {
		GameObject ship;

		// Displays an OpenFilePanel so the user can select the ship model to load
		string [] filters = {"Autodesk .fbx format", "fbx", "Wavefront .obj format", "obj"};
		string path = EditorUtility.OpenFilePanelWithFilters ("Selecting ship model", "Assets", filters);
		if (path == string.Empty) {
			Debug.Log ("No such file or directory");
			return;
		}

		// Evaluate relative path from Assets/ folder to the ship model to load
		int len = Application.dataPath.Length;
		path = path.Substring (len - ("Assets".Length));

		// Start Loading...
		Debug.Log ("Loading Ships...");
		GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath (path, typeof(GameObject));
		if (obj == null) {
			Debug.Log ("Load failed.");
			return;
		}
		else {
			LoadShipOptionForm form = new LoadShipOptionForm ();
			System.Windows.Forms.DialogResult result = form.ShowDialog ();
			if (result == System.Windows.Forms.DialogResult.OK) {
				ship = (GameObject)GameObject.Instantiate (obj, Vector3.zero, Quaternion.identity);
				if (form.flipXY) {
					ship.transform.Rotate (new Vector3 (-90.0f, 0.0f, 0.0f));
				}
				ship.name = "ShipModel";
				Debug.Log ("Successfully Loaded.");
			} else {
				Debug.Log ("Load Cancelled.");
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
