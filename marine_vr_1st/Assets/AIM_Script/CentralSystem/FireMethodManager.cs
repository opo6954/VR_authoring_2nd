using UnityEngine;
using System.Collections;

public class FireMethodManager : TaskManagerTemplate {

    private FireMethodUIManager fireMethodUIManagerInstance;




    public int currVideo = 0;
    
    public bool isWrongMessage = false;
    public bool isStartVideo = false;
    public bool isVideoOption = false;
    public bool isVideoOptionDone = false;

    public int currIdx = 0;
    public int prevIdx = -1;


    public int[] idxMapping;
    public bool[] currAns;

    

    


    private float timeCount;
    private float startTime;
    private string timeMessage;
    private string hoverMessage;

    private bool isLock = false;

    private bool isHover;
    private bool isClick;

    void calcTimeDelay()
    {
        timeCount = Time.time - startTime;

        float min = timeCount / 60.0F;
        float sec = timeCount % 60.0F;
        float fraction = (timeCount * 100.0F) % 100.0F;

        if (fraction >= 50.0F)
            fraction = fraction - 50.0F;
        else
            fraction = fraction + 50.0F;

        fraction = (int)(fraction % 100.0F);

        timeMessage = string.Format("{00:00}:{1:00}:{2:00}", min, sec, fraction);
    }

    /*
    //click trigger for clicking part of extinguisher
    public void OnClickExtinguisher(int index)
    {
        if(index - currClick != 1)
        {
            //이 부분에 틀린지 맞는지 알려주기
            Debug.Log("wrong part is clicked");
            isWrongMessage = true;
        }
        else
        {
            Debug.Log("right part is clicked");

            currClick = index;
            currVideo = currClick;
            isStartVideo = true;//start the video according to the currVideoIndex
        }
    }
     * */
    /*
     * //button.onClick.AddListener(() => fireMethodManagerInstance.OnClickVideoOn());
        //okayButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => fireMethodManagerInstance.OnClickOkay());
     * */
    public void OnClickOkay()
    {
        isVideoOptionDone = true;
    }
    public void OnClickVideoOn()
    {
        if (isVideoOptionDone == false)
        {
            if (isVideoOption == true)
            {
                isVideoOption = false;
                fireMethodUIManagerInstance.checkSign.GetComponent<UnityEngine.UI.RawImage>().enabled = false;
                fireMethodUIManagerInstance.uncheckSign.GetComponent<UnityEngine.UI.RawImage>().enabled = true;

            }
            else
            {
                isVideoOption = true;
                fireMethodUIManagerInstance.checkSign.GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                fireMethodUIManagerInstance.uncheckSign.GetComponent<UnityEngine.UI.RawImage>().enabled = false;
            }
        }
    }

    public Rect getSpotPt(string spotName)
    {
        return GameParameter.fireExtinguisherSpots[spotName];
    }

    private void lockScreen()
    {
        if (isLock == false)
        {
            getOwnedSystem().lockFPSScreen(true);
            isLock = true;
        }
    }
    private void desctoryScreen()
    {
        getOwnedSystem().lockFPSScreen(false);
    }


    public void moveCurrIdx()
    {
        currIdx = currIdx + 1;

        if (currIdx == GameParameter.totalVideoIndex)
            currIdx = 0;
    }

    public void inputInteraction()
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
        {
            if (isVideoOptionDone == false)
            {
                OnClickVideoOn();
            }
            else
            {
                if (fireMethodUIManagerInstance.isPlayingNow == false)
                {
                    moveCurrIdx();
                    Debug.Log(currIdx);
                }
            }
        }
        else if (isSelectButtonPressed == true)
        {
            if (isVideoOptionDone == false)
            {
                OnClickOkay();
            }
            else
            {
                if (fireMethodUIManagerInstance.isPlayingNow == false)
                {
                    int realIdx = idxMapping[currIdx]-1;
                    if (realIdx - prevIdx != 1)
                    {
                        Debug.Log("wrong part is clicked");

                        for (int i = 0; i < GameParameter.totalVideoIndex; i++)
                            currAns[i] = false;
                         
                        isWrongMessage = true;
                        prevIdx = -1;
                    }
                    else
                    {
                        Debug.Log("right part is clicked");

                        currAns[realIdx] = true;

                        prevIdx = realIdx;
                        currVideo = realIdx;
                        isStartVideo = true;
                    }
                }
            }
        }



        /*
         *     public void OnClickExtinguisher(int index)
    {
        if(index - currClick != 1)
        {
            //이 부분에 틀린지 맞는지 알려주기
            Debug.Log("wrong part is clicked");
            isWrongMessage = true;
        }
        else
        {
            Debug.Log("right part is clicked");

            currClick = index;
            currVideo = currClick;
            isStartVideo = true;//start the video according to the currVideoIndex
        }
    }
         * */


         
    }


    public override void processor()
    {
        lockScreen();
        calcTimeDelay();
        inputInteraction();
    }
    public override void getInfofromUI()
    {

    }
    public override void passInfo2UI()
    {

    }

    //call after task is finished
    public override void Destroy()
    {
        if (isLock == true)
        {
            desctoryScreen();
            isLock = false;
        }
        isDoingTask = false;

        //initiate the fire spread

        //trigger the next task
        getOwnedSystem().getNextTask(taskNumber).isDoingTask = true;


        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(true);
        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber + 1];
        


    }

    public override void Init()
    {

        idxMapping = new int[GameParameter.totalVideoIndex];
        idxMapping[0]=3;
        idxMapping[1]=1;
        idxMapping[2]=2;
        idxMapping[3]=4;

        currAns = new bool[GameParameter.totalVideoIndex];
        for(int i=0; i<GameParameter.totalVideoIndex; i++)
            currAns[i] = false;


    }


    public override void Process()
    {
        if (isDoingTask == true && isDoneTask == true)
        {
            Destroy();
            CentralSystem.playAudio("information_broadcast");
        }

        if (isFirstCall == true && getOwnedUI().isMappingTask == true)
        {
            
            fireMethodUIManagerInstance = getOwnedUI().getTaskUIManager<FireMethodUIManager>();
            isFirstCall = false;
        }
        if(isDoingTask == true && isDoneTask == false)
        {
            getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);

            processor();
            getInfofromUI();
            passInfo2UI();
            //if the total finish signal is false and it already start the task, --> should be destroyed
        }
    }
}
