using UnityEngine;
using System.Collections;

public class FireFailManager : TaskManagerTemplate {


    bool isLock = false;
    public float startTime;


    bool isStartTime = false;

    private float duration = 2.0f;


    public override void Init()
    {

    }
    public override void Destroy()
    {
        if(isLock == true)
        {
            getOwnedSystem().lockFPSScreen(false);
            isLock = false; 
        }

        isDoingTask = false;

        GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").GetComponent<Control_objectify>().setObjectifyValue(false);
        GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").transform.FindChild("Text").GetComponent<Hide_by_renderer>().turnRenderer(false);

        getOwnedSystem().getNextTask(this.taskNumber).isDoingTask = true;

        
        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber+1];
        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);

    }

    public override void Process()
    {
        if (isDoingTask == true)
        {
            getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);

            if (isStartTime == false)
            {
                CentralSystem.playAudio("fire_fail_report");

                startTime = Time.time;
                isStartTime = true;

            }
            if (isLock == false)
            {
                getOwnedSystem().lockFPSScreen(true);
                
                isLock = true;
            }

            if(Time.time - startTime > duration)
            {
                //isDoneTask = true;
                getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(taskNumber);
            }
        }

        if (isDoneTask == true && isDoingTask == true)
            Destroy();
    }


}
