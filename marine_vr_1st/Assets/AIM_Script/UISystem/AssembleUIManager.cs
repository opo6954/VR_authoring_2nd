using UnityEngine;
using System.Collections;

public class AssembleUIManager : TaskUITemplate{

    AssembleManager assembleManagerInstance;

    GameObject assembleReachInstance;
    GameObject assembleWaitInstance;

    public bool isReashSpot = false;
    private bool isStichingStart = false;

    private float duration = 2.0f;
    private bool setInitTime = true;
    private float currTime;
    private bool turnOffUIAll = false;
    

    public override void Init()
    {
        assembleReachInstance = GameObject.Find("Task6").transform.FindChild("reachAssembleSpot").gameObject;
        assembleWaitInstance = GameObject.Find("Task6").transform.FindChild("waitOtherPlayers").gameObject;
        assembleManagerInstance = getOwnedSystem().getTaskManager<AssembleManager>();
        assembleReachInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
        assembleWaitInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
    }

    public override void Process()
    {
        if (assembleManagerInstance.isDoneTask == true && turnOffUIAll == true)
            Destroy();


        if (isReashSpot == true && turnOffUIAll == false)
        {
            if (assembleManagerInstance.isDoneTask == false)
            {
                assembleReachInstance.GetComponent<TurnOffImageNText>().turnOnOff(true);


                if (isStichingStart == false)
                {
                    CentralSystem.initStitching();
                    isStichingStart = true;
                }
                if (CentralSystem.stitchingEffect() == true)
                {
                    assembleWaitInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
                }
                else
                {
                    assembleWaitInstance.GetComponent<TurnOffImageNText>().turnOnOff(true);
                }
            }
            else
            {
                if (setInitTime == true)
                {
                    currTime = Time.time;
                    setInitTime = false;
                }
                if (Time.time - currTime > duration)
                {
                    assembleReachInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
                    turnOffUIAll = true;
                }
            }
        }
        
    }

    public override void Destroy()
    {
        assembleReachInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
        assembleWaitInstance.GetComponent<TurnOffImageNText>().turnOnOff(false);
    }
}
