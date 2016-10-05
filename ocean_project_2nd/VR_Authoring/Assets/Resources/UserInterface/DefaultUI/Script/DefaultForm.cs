using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DefaultForm : MonoBehaviour {

    Text t = null;
    GameObject child = null;

    bool isInit = false;


    void Start()
    {
        
    }

    void Init()
    {
        child = gameObject.transform.GetChild(0).gameObject;
        
        t = child.transform.GetChild(0).GetComponent<Text>();
    }

    public void changeCurrTaskInfo(string value)
    {
        if (isInit == false)
        {
            Init();
            isInit = true;
        }


        t.text = value;
    }

    public void toggleShownCurrTaskInfo(bool value)
    {
        if (isInit == false)
        {
            Init();
            isInit = true;
        }

        child.GetComponent<UIEffect>().isShown(value);
    }

}
