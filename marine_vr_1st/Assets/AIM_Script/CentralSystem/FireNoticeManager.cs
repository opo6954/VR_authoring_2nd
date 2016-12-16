using UnityEngine;
using System.Collections;

public class FireNoticeManager : TaskManagerTemplate {
    

    private bool isLock = false;

    private void initStatement()
    {
        GameParameter.totChoice = 4;
        GameParameter.answerIdx = 4;
        GameParameter.buttonName = new string[GameParameter.totChoice];

        for (int i = 0; i < GameParameter.totChoice; i++)
            GameParameter.buttonName[i] = "Button" + (i + 1);
    }
    public override void Destroy()
    {
        if (isLock == true)
        {
            getOwnedSystem().lockFPSScreen(false);
            isLock = false;
        }
        isDoingTask = false;
        GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").GetComponent<Control_objectify>().setObjectifyValue(true);

        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber+1];

    }

    public override void Init()
    {
        initStatement();

        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber];
        
    }
    public override void processor()
    {
        bool isXKeyDown = false;
        if (getOwnedSystem().isJoystick == true)
        {
            isXKeyDown = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.X));
        }
        else if (getOwnedSystem().isLeapMotion == true)
        {
            isXKeyDown = getOwnedSystem().isLeapMotionClicked(LeapMotionType.CLICK);
        }
        else
        {
            isXKeyDown = Input.GetKeyDown("x");
        }

        if (isXKeyDown == true)//종료 trigger
        {
            getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(taskNumber);
        }
            

    }

    public override void Process()
    {
        if (isDoneTask == true && isDoingTask == true)
            Destroy();
        else if (isDoingTask == true)
        {
            getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);
            
            if (isLock == false)
            {
                getOwnedSystem().lockFPSScreen(true);
                isLock = true;
            }
            processor();
        }
        
    }

}
