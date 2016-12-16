using UnityEngine;
using System.Collections;

public class AssembleManager : TaskManagerTemplate {

    GameObject assemblySpot;

    AssembleUIManager assembleUIManagerInstance;

    private const float threshold = 30.0f;
    public bool isAllReady = false;
    private bool isReportLocalTaskDone = false;
    private bool isStartTask = true;


    public override void Destroy()
    {
        isDoingTask = false;
        getOwnedSystem().getNextTask(taskNumber).isDoingTask = true;

        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
        if (getOwnedSystem().transform.parent.gameObject.GetComponent<NetworkSender>().getMyRole() == GameParameter.playerRolePlay.COUNTER)
        {
            getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subMultiTaskInfo[0];
        }
        else
        {
            getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subMultiTaskInfo[1];
        }
    }

    //완전 처음에만 실행되는 함수
    public override void Init()
    {
        assemblySpot = GameObject.Find("AssemblyPoint");

        
        
        
    }

    public bool checkAssemblySpot()
    {
        Vector3 player_pos = Camera.main.transform.position;

        float current_distance = (assemblySpot.transform.position - player_pos).magnitude;

        if (current_distance < threshold)
        {
            assembleUIManagerInstance.isReashSpot = true;

            return true;
        }

        return false;
    }

    public void determineFinish()//모든 mulitplayer가 준비될 경우 true task is done이 됩네당
    {
        if (checkAssemblySpot() == true)
        {
            if (isReportLocalTaskDone == false)
            {
                getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeLocalNetworkTaskDone(taskNumber);
                isReportLocalTaskDone = true;
            }

        }
        
    }
    


    //계속 실행되는 함수, isDoingTask 값에 따라 process 변경해야함 --> 대격변때 isDoingTask 변할때만 Process()가 실행되도록 해야하는게 좋을듯, 물론 isDoneTask 가 false일때도 역시 마찬가지로...
    public override void Process()
    {
        if (isFirstCall == true && getOwnedUI().isMappingTask == true)
        {
            assembleUIManagerInstance = getOwnedUI().getTaskUIManager<AssembleUIManager>();
            
            

            isFirstCall = false;

        }


        if (isDoingTask == true && isDoneTask == false)
        {
            if (isStartTask == true)
            {
                assemblySpot.transform.FindChild("emphasizeSpot").gameObject.SetActive(true);
                isStartTask = false;

                getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);
            }
            
            determineFinish();
        }
        else if (isDoneTask == true && isDoingTask == true)
            Destroy();
    }
}