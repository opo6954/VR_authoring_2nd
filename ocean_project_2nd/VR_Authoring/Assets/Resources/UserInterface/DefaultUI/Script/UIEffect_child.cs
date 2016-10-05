using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEffect_child : UIEffect {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public override void UIProcessing()
    {
        gameObject.GetComponent<Image>().enabled = isShownOn;
        gameObject.transform.GetChild(0).gameObject.SetActive(isShownOn);
    }

    public override void setText(string value)
    {
        Debug.Log("Child 형식의 경우 text 바꾸는 거 추후에 구현하자");
    }
}
