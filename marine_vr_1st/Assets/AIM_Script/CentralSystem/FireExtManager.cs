using UnityEngine;
using System.Collections;

public class FireExtManager : TaskManagerTemplate {
    //UI massenger

    FireExtUIManager fireExtUIManagerInstance;
  //여긴 일단 냅두자 나중에 수정많이 할꺼라서...

    //static value
    private bool extinguisherOn = false;
    private int countLeft = 0;
    private int countRight = 0;
    private int countCen = 0;
    private float extinguisherAngle = 270.0f;
    private GameObject currFire;
    private int isFireCollide = 0;//if the particle collide to fire, then the value is true
    private bool isFireOn = false;


    

    // Use this for initialization
    private float trainingScore;//training score of task[FireSpread]
    //time INFO
    private float timeCount;//elapsed time during the task[FireSpread]
    private float startTime;


    private float timeLimitMin=5;//time limit is 5 minutes
    public string timeMessage;
    
    //spread INFO
    //count the limitation


    

    private int collideThreshold = 2;
    private int spreadThreshold = 1;

    //position information


    
    private float extinguisherLimitAngle = 270.0f;
    private float extinguisherInterval = 50.0f;
    private int barWidth;
    private int barPosWidth;
    private float offSet=0.12f;//offset for detail midification


    //warning 
    bool isWarning = false;
    private int warningThreshold=1;
    

    //sampling rate of meet limitation
    private float countSamplingRate = 3.0f;
    private int countLimitation=2;
    private float samplingTime;


    //Extinguishable Fire
    
    private int countFireExtinguished = 0;
    private int numOfExtinguishableFire = 3;









    //spread Determine Info
    private float timeInterval = 1.0f;//sampling the count of movement of horse within timeInterval seconds


    


    //static function called from other space


    private void initialize()
    {
        startTime = Time.time;
        samplingTime = startTime;
        timeLimitMin = 5.0F;

        
    }


        
    public void getCountofSpread(bool _isExtingiusherOn, bool _isMeetLimitLeft, bool _isMeetLimitRight, bool _isExtingiusherNone, float _extinguisherAngle)
    {
        extinguisherOn = _isExtingiusherOn;

        if (_isMeetLimitLeft == true)
            countLeft++;
        else if (_isMeetLimitRight == true)
            countRight++;
        else if (_isExtingiusherNone)//if no button is pressed
            countCen++;


        extinguisherAngle = _extinguisherAngle;

    }

    public void getCollideInfo(GameObject _collidedFire)
    {
        currFire = _collidedFire;
        isFireCollide++;
    }


    


    //get the count of spread key to estimate the training score and stop the time delay
    void calcTimeDelay()
    {
        timeCount = Time.time - startTime;

        float min = timeCount/60.0F;
        float sec = timeCount % 60.0F;
        float fraction = (timeCount * 100.0F) % 100.0F;

        if (fraction >= 50.0F)
            fraction = fraction - 50.0F;
        else
            fraction = fraction + 50.0F;

        fraction = (int)(fraction % 100.0F);

        timeMessage = string.Format("{00:00}:{1:00}:{2:00}", min, sec, fraction);
    }

    float calcDirBarPos()
    {
        float normalAngle = (extinguisherAngle - extinguisherLimitAngle)+extinguisherInterval;

        
        
        float calcPos = (barWidth + barPosWidth)/(2*extinguisherInterval) * normalAngle + barPosWidth/2 - barWidth/2 + barWidth * offSet;
        
        return calcPos;
        
    }

    void determineWarning()
    {
        float currTime = Time.time - samplingTime;
        

        if(currTime > countSamplingRate)
        {
            //count for spreading~!
            if (countLeft < warningThreshold || countRight < warningThreshold)
            {
                isWarning = true;
            }
            else
                isWarning = false;
            //count for colliding~!

            isFireCollide = 0;
            countLeft = 0;
            countRight = 0;
            samplingTime = Time.time;
        }
    }

    //일단 불 꺼지는 조건은 간단하게 합시다
    void determineExtinguishing()
    {
        if(countLeft >= spreadThreshold && countRight >= spreadThreshold && isFireCollide >= collideThreshold)
        {
            if (GameParameter.isSinglePlayer == true)
                currFire.GetComponent<Hide_by_SetActive>().change_active_state();
            else
                currFire.GetComponent<PhotonView>().RPC("change_active_state", PhotonTargets.All);

            countLeft = 0;
            countRight = 0;
            isFireCollide = 0;
            countFireExtinguished++;

        }
    }

    void determineTask()
    {

        if(countFireExtinguished == numOfExtinguishableFire)
        {
            getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(taskNumber);
        }
    }

    void showExtinguishableFire()
    {
        if (isDoingTask == true && isFireOn == false)
        {
            if(GameParameter.isSinglePlayer == true)
                GameObject.Find("ExtinguishableFire").GetComponent<TurnOffChild>().turn_on_child("ExtinguishableFireInstance");
            else
                GameObject.Find("ExtinguishableFire").GetComponent<PhotonView>().RPC("turn_on_child", PhotonTargets.All, "ExtinguishableFireInstance");
            
            isFireOn = true;
        }
    }


    //Callback for information from the spread side

    private void passSpreadInfo2UI()
    {
        fireExtUIManagerInstance.SetCurrFireInput(extinguisherOn, calcDirBarPos());
        if (extinguisherOn == true)
            fireExtUIManagerInstance.SetWarning(isWarning);
    }

    //pass the collide between the particle from the fire extinguisher and the fire itself


    //main processor of central controller




    public override void Destroy()
    {
        isDoingTask = false;
        GameObject.Find("interactable").transform.FindChild("ShipPhone").transform.FindChild("Phone").GetComponent<Control_objectify>().setObjectifyValue(true);



        getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber + 1];


    }

    //pass the info from central 2 UI
    public override void passInfo2UI()
    {
        passSpreadInfo2UI();
    }

    public override void processor()
    {
        showExtinguishableFire();
        calcTimeDelay();
        determineWarning();
        determineExtinguishing();
        determineTask();
    }

    public override void Init()
    {
        initialize();
    }



    public override void Process()
    {
        
        //if fire extinguisher is picked, then the extinguisher working is continued


        if (isDoneTask == true && isDoingTask == true)
        {
            //LHW input audio source
            //CentralSystem.playAudio("I_love_daddy");
            Destroy();
        }

        else if (isFirstCall == true && getOwnedUI().isMappingTask == true)
        {
            fireExtUIManagerInstance = getOwnedUI().getTaskUIManager<FireExtUIManager>();
            isFirstCall = false;
        }
        else if (isDoingTask == true && isFirstCall == false)
        {
            //isFireCollide = false;//at first time, FireCollide is false    
            processor();
            getInfofromUI();
            passInfo2UI();

            


            getUIInstance().GetComponent<TurnOffImageNText>().transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.subTaskInfo[taskNumber];
        }
        
    }
}
