using UnityEngine;
using System.Collections;

public class BackgroundForm : MonoBehaviour {

    GameObject timeInfo;
    GameObject totalTaskInfo;
    GameObject buttonInfo;

    bool isInit = false;

    public enum BGPart {BG_TIMEINFO, BG_TOTALINFO, BG_BUTTONINFO};

	
	void Start () {
        Init();
	}
    void Init()
    {
        timeInfo = gameObject.transform.FindChild("timeInfo").gameObject;
        totalTaskInfo = gameObject.transform.FindChild("totalTaskInfo").gameObject;
        buttonInfo = gameObject.transform.FindChild("buttonInfo").gameObject;

        isInit = true;
    }

    public void toggleShownObject(BGPart bg, bool value)
    {
        switch (bg)
        {
            case BGPart.BG_TIMEINFO:
                timeInfo.GetComponent<UIEffect>().isShown(value);
                break;
            case BGPart.BG_TOTALINFO:
                totalTaskInfo.GetComponent<UIEffect_child>().isShown(value);
                break;
            case BGPart.BG_BUTTONINFO:
                buttonInfo.GetComponent<TwinkleEffect>().isShown(value);
                break;
        }
    }

    public void changeButtonInfo(string value)
    {
        buttonInfo.GetComponent<UIEffect>().setText(value);
    }

    public void changeTimeInfo(string value)
    {
        timeInfo.GetComponent<UIEffect>().setText(value);
    }

    




}
