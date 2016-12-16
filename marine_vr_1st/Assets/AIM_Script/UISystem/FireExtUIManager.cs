using UnityEngine;
using System.Collections;

public class FireExtUIManager : TaskUITemplate{


    FireExtManager fireExtManagerInstance;

    private float currBarPos=0;//current position of concentration
    private bool isExtinguisherOn = false;


    
    //Warning info
    [SerializeField]
    private bool isWarning=false;
    

    //bar synchronize info
    public int concenBarWidth=0;
    public int concenBarPosWidth=0;

    GameObject UIInstnace;

	
	// Update is called once per frame
	void Update () {
        
}


    //GUI function

    //이 부분은 좀 더 qual를 높이고 canvas_UI상에서 만듭시다
    void ShowWarningText()
    {
        if (isWarning == true && CentralSystem.stitchingEffect() && isExtinguisherOn == true)
        {
            UIInstnace.transform.FindChild("warningMessage").GetComponent<TurnOffImageNText>().turnOnOff(true);
        }        
        else
            UIInstnace.transform.FindChild("warningMessage").GetComponent<TurnOffImageNText>().turnOnOff(false);
    }


    void ShowUI()
    {
        if (fireExtManagerInstance.isDoneTask == false)
        {
            if (fireExtManagerInstance.isDoingTask == true)
            {
                ShowWarningText();
            }
        }
        else
            UIInstnace.transform.FindChild("warningMessage").GetComponent<TurnOffImageNText>().turnOnOff(false);
    }

    //callback from Central to UI
    
    public void SetCurrFireInput(bool _isExtingiusherOn, float _currBarPos)
    {
        isExtinguisherOn = _isExtingiusherOn;
        currBarPos = _currBarPos;
    }
    public void SetWarning(bool isWarningNeeds)
    {
        isWarning = isWarningNeeds;
    }
    

    public void initialize()
    {
        UIInstnace = GameObject.Find("Task4").gameObject;
        UIInstnace.transform.FindChild("warningMessage").GetComponent<TurnOffImageNText>().turnOnOff(false);
    }




    public override void Init()
    {
        fireExtManagerInstance = getOwnedSystem().getTaskManager<FireExtManager>();

        initialize();
    }

    public override void Process()
    {
        if (fireExtManagerInstance.isDoingTask == true)
        {
            
             
        }
        ShowUI();
    }
}
