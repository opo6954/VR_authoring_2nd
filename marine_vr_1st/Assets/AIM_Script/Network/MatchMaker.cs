using UnityEngine;
using System.Collections;
using Photon;
using UnityStandardAssets.Characters.FirstPerson;


public class MatchMaker : Photon.PunBehaviour {
    //for matching games...

    private Color startColor;
    private float alpha=0.0f;
    private float startTime;
    public bool isJoinRoom = false;
    public bool isJoinFail = false;
    private int user_num = 0;
    


	// Use this for initialization
	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");
        startColor = new Color(0, 0, 0);
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (isJoinRoom == true && startColor.a > 0)
        {
            startColor = new Color(startColor.r, startColor.g, startColor.b, fadeoutColor());
        }
    } 

    void OnGUI()
    { 
        if (startColor.a > 0)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, startColor);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
        }
        if(isJoinFail == true)
            GUILayout.Label("Fail to join room due to maximum players...");
        else
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        
    }
    float fadeoutColor()
    {
        alpha = alpha + Time.deltaTime / 3.0f;
        if (alpha > 1.0f)
            alpha = 1.0f;
        return 1-alpha;
    }



}
