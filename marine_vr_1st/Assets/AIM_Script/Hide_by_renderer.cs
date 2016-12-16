using UnityEngine;
using System.Collections;

/* 
gameobject 를 hide 시켰다가 보이게 했다가 하는데 쓰이는 스크립트 
상태 전환은 다른 스크립트에서 event를 보내서 처리 한다.
*/

public class Hide_by_renderer : MonoBehaviour {
    public bool visible = false;
    public Renderer rend;



    public void turnRenderer(bool value)
    {
        rend.enabled = value;
    }


	void Start () {
        
        rend = GetComponent<Renderer>();
        rend.enabled = visible;
	}
    
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            visible = !visible;
            rend.enabled = visible;
        }
         * */
	}
}
