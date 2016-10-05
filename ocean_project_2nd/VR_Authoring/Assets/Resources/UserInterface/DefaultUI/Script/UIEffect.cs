using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEffect : MonoBehaviour {

    public bool isShownOn = true;
    private bool isChange = false;

	void Awake()
	{
		Init ();
	}
    public virtual void isShown(bool value)
    {
        isShownOn = value;
        isChange = true;
    }


    void OnGUI()
    {
        if (isChange == true)
        {
            UIProcessing();
            isChange = false;
        }
    }

    public virtual void Init()
    {
    }
    public virtual void UIProcessing()
    {  
        gameObject.GetComponent<Image>().enabled = isShownOn;
        transform.GetChild(0).GetComponent<Text>().enabled = isShownOn;
    }

    public virtual void setText(string value)
    {
        transform.GetChild(0).GetComponent<Text>().text = value;
    }

}
