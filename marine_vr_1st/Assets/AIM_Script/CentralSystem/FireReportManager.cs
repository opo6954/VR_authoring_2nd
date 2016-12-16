using UnityEngine;
using System.Collections;

public class FireReportManager : TaskManagerTemplate {


    private FireReportUIManager fireReportUIManagerInstance;

    public int stageIdx=0;
    public int correctNum = 0;//number of correct answer among total question
    public int currIdx=0;//current button clicked
    public bool islock = false;
    public bool isStartAlarm = false;

    //key를 눌렀을때 select한 part가 한 단계 밑으로 움직이는 부분임
    public void moveCurrIdx()
    {
        Debug.Log("move button is pressed, " + currIdx);
        currIdx = currIdx + 1;

        if (currIdx == GameParameter.numSubInfo)
            currIdx = 0;
        
    }

    public void getInput()
    {
        bool isMovingButtonPressed = false;
        bool isSelectButtonPressed = false;

        if (getOwnedSystem().isJoystick == true)
        {
            isSelectButtonPressed = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.X));
            isMovingButtonPressed = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.Y));
        }
        else if (getOwnedSystem().isLeapMotion == true)
        {
            
        }
        else
        {
            isSelectButtonPressed = Input.GetKeyDown("x");
            isMovingButtonPressed = Input.GetKeyDown("z");
        }
        if (isMovingButtonPressed == true)
            moveCurrIdx();
        else if (isSelectButtonPressed == true)
        {
            if (GameParameter.ansSeries[stageIdx] == currIdx)
            {
                fireReportUIManagerInstance.isCorrect = true;
                correctNum++;

                if (stageIdx == GameParameter.numTotInfo - 1)
                {
                    fireReportUIManagerInstance.isQuestionDone = true;

                    GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").GetComponent<Control_objectify>().setObjectifyValue(false);
                    GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").transform.FindChild("Text").GetComponent<Hide_by_renderer>().turnRenderer(false);
                    GameObject.Find("interactable").transform.FindChild("fire_alarm_box").GetComponent<Control_objectify>().setObjectifyValue(true);
                }
                else
                {
                    stageIdx++;
                }
            }
            else
            {
                fireReportUIManagerInstance.isFail = true;
            }
        }
    }


    /*
    public void OnClick(string _buttonName)
    {
        for (int i=0; i < GameParameter.numSubInfo; i++)
        {
            if(_buttonName == GameParameter.strButtonTag[i])
            {
                //if it is correct

                currIdx = i;

                if(GameParameter.ansSeries[stageIdx] == i)
                {
                    fireReportUIManagerInstance.isCorrect = true;

                    correctNum++;

                    if (stageIdx == GameParameter.numTotInfo - 1)
                    {
                        fireReportUIManagerInstance.isQuestionDone = true;
                        
                        GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").GetComponent<Control_objectify>().setObjectifyValue(false);
                        GameObject.Find("interactable").transform.FindChild("fire_alarm_box").GetComponent<Control_objectify>().setObjectifyValue(true);
                    }
                    else
                    {
                        stageIdx++;
                    }
                }
                //if it is not correct
                else
                {
                    fireReportUIManagerInstance.isFail = true;
                }
                break;
            }
        }
    }


    */
    
    public void executeAlarm(Transform alarmObject)
    {
        isStartAlarm = true;
        getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(taskNumber);
        GameObject.Find("interactable").transform.FindChild("fire_alarm_box").GetComponent<Control_objectify>().setObjectifyValue(false);

        CentralSystem.playAudio("fire_report");

    }



    public override void Destroy()
    {
        isDoingTask = false;


        GameObject.Find("interactable").transform.FindChild("extinguisher").GetComponent<Control_objectify>().setObjectifyValue(true);
        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber+1];
    }




    public override void Init()
    {
        isDoingTask = false;
        isDoneTask = false;
    }

    public override void processor()
    {
        getInput();
        if (islock == false)
        {
            getOwnedSystem().lockFPSScreen(true);
            islock = true;
        }
    }


    public override void Process()
    {
        if(isFirstCall == true && getOwnedUI().isMappingTask == true)
        {
            
            fireReportUIManagerInstance = getOwnedUI().getTaskUIManager<FireReportUIManager>();
            isFirstCall = false;
        }
        if (isDoingTask == true)
        {
            getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
            getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber + 1];

            if(isDoneTask == false && isStartAlarm == false && isFirstCall == false)
            {
                getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);
                processor();
            }
            if (isStartAlarm == true)
            {
                getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
                getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "화재 경보기를 작동하세요";

                if (islock == true)
                {
                    getOwnedSystem().lockFPSScreen(false);
                    islock = false;
                }
            }
            if (isDoneTask == true)
            {

                Destroy();
            }
        }
    }

}
