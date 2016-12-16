using UnityEngine;
using System.Collections;

public class FireReportUIManager : TaskUITemplate {

    FireReportManager fireReportManagerInstance;




    private GameObject UIInstance;

    private Color prevButtonColor;

    private Color highlightColor;

    public bool isCorrect = false;
    public bool isFail = false;
    private float buttonWidth;
    private float buttonHeight;

    private float buttonOffset;

    private Vector2[] buttonPos;

    private float currTime;
    public bool isShowingMark = false;
    public bool isQuestionDone = false;
    
    
    public void showMark()
    {
        Vector2 posInCanvas = CentralSystem.UIAbsPt2UIRef(new Vector2(buttonPos[fireReportManagerInstance.currIdx].x + buttonWidth / 2, buttonPos[fireReportManagerInstance.currIdx].y + buttonHeight / 2));


        if (isShowingMark == false)
        {
            currTime = Time.time;
        }
        if (isCorrect == true || isFail == true)
        {
            isShowingMark = true;

            if (isCorrect == true)
            {
                UIInstance.transform.FindChild("SuccessMark").transform.localPosition = new Vector3(posInCanvas.x + buttonOffset * buttonWidth, posInCanvas.y, 0);
                CentralSystem.setActiveChild(UIInstance, "SuccessMark", true);
                CentralSystem.setActiveChild(UIInstance, "FailMark", false);
                CentralSystem.setActiveChild(UIInstance, "CheckMark", false);
            }
            else if (isFail == true)
            {
                UIInstance.transform.FindChild("FailMark").transform.localPosition = new Vector3(posInCanvas.x + buttonOffset * buttonWidth, posInCanvas.y, 0);
                CentralSystem.setActiveChild(UIInstance, "SuccessMark", false);
                CentralSystem.setActiveChild(UIInstance, "FailMark", true);
                CentralSystem.setActiveChild(UIInstance, "CheckMark", false);
            }
        }
        else//일반적인 상황에서
        {
            UIInstance.transform.FindChild("CheckMark").transform.localPosition = new Vector3(posInCanvas.x + buttonOffset * buttonWidth, posInCanvas.y, 0);
            CentralSystem.setActiveChild(UIInstance, "CheckMark", true);
        }
        
    }

    public void stopMark()
    {
        if (isShowingMark == true)
        {
            if (Time.time - currTime > 1.0f)
            {
                CentralSystem.setActiveChild(UIInstance, "SuccessMark", false);
                CentralSystem.setActiveChild(UIInstance, "FailMark", false);

                isCorrect = false;
                isFail = false;

                isShowingMark = false;

                if (isQuestionDone == true)
                {
                    isQuestionDone = false;

                    fireReportManagerInstance.isStartAlarm = true;
                }
            }
        }
    }


    public void updateQuestion()
    {
        UIInstance.transform.FindChild("Firetype").transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.strMainInfo[fireReportManagerInstance.stageIdx];

        Event e = Event.current;





        for (int i = 0; i < GameParameter.numSubInfo; i++)
        {
            UnityEngine.UI.Button button = UIInstance.transform.FindChild(GameParameter.strButtonTag[i]).GetComponent<UnityEngine.UI.Button>();
            button.transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = GameParameter.strSubInfo[fireReportManagerInstance.stageIdx * ( GameParameter.numTotInfo) + i];



            if (getOwnedSystem().isScreenChange == true)
            {
                Rect buttonRelativePos = button.GetComponent<RectTransform>().rect;
                Vector2 buttonLocalPosRelative = CentralSystem.UIRefPt2UIAbsPt(new Vector2(button.transform.localPosition.x, button.transform.localPosition.y));
                Rect buttonCanvasPos = new Rect(buttonLocalPosRelative.x - buttonRelativePos.width / 2, buttonLocalPosRelative.y - buttonRelativePos.height / 2, buttonRelativePos.width, buttonRelativePos.height);
                buttonPos[i] = new Vector2(buttonCanvasPos.x, buttonCanvasPos.y);
            }

            if (isHovered(new Rect(buttonPos[i].x, buttonPos[i].y, button.transform.GetComponent<RectTransform>().rect.width, button.transform.GetComponent<RectTransform>().rect.height), e.mousePosition) == true)
            {
                button.GetComponent<UnityEngine.UI.Image>().color = highlightColor;

            }
            else
            {
                button.GetComponent<UnityEngine.UI.Image>().color = prevButtonColor;
            }
        }

    }


    public bool isHovered(Rect buttonArea, Vector2 mousePosition)
    {
        if (buttonArea.Contains(mousePosition))
            return true;
        else
            return false;

    }

    public override void Init()
    {
        fireReportManagerInstance = getOwnedSystem().getTaskManager<FireReportManager>();
        buttonOffset = 0.35f;

        UIInstance = GameObject.Find("FireReport_UI");

        buttonPos = new Vector2[ GameParameter.numSubInfo];
        for (int i = 0; i < GameParameter.numSubInfo; i++)
            buttonPos[i] = new Vector2();



        for (int i = 0; i < GameParameter.numSubInfo; i++)//+1 is for fire type
        {
            UnityEngine.UI.Button button;
            button = UIInstance.transform.FindChild(GameParameter.strButtonTag[i]).GetComponent<UnityEngine.UI.Button>();
            //button.onClick.AddListener(() => fireReportManagerInstance.OnClick(button.name));

            Rect buttonRelativePos = button.GetComponent<RectTransform>().rect;
            Vector2 buttonLocalPosRelative = CentralSystem.UIRefPt2UIAbsPt(new Vector2(button.transform.localPosition.x, button.transform.localPosition.y));
            Rect buttonCanvasPos = new Rect(buttonLocalPosRelative.x - buttonRelativePos.width / 2, buttonLocalPosRelative.y - buttonRelativePos.height / 2, buttonRelativePos.width, buttonRelativePos.height);


            buttonPos[i] = new Vector2(buttonCanvasPos.x, buttonCanvasPos.y);

            buttonWidth = buttonRelativePos.width;
            buttonHeight = buttonRelativePos.height;

            if (i == 0) prevButtonColor = button.transform.GetComponent<UnityEngine.UI.Image>().color;

        }
        UIInstance.SetActive(false);


        highlightColor = new Color(0.7f, 0.7f, 0.7f);

        CentralSystem.setActiveChild(UIInstance, "SuccessMark", false);
        CentralSystem.setActiveChild(UIInstance, "FailMark", false);
        CentralSystem.setActiveChild(UIInstance, "CheckMark", true);




    }

    public override void Process()
    {
        if (fireReportManagerInstance.isStartAlarm == true || fireReportManagerInstance.isDoneTask == true)
        {
            UIInstance.SetActive(false);
        }
        else if ((fireReportManagerInstance.isDoingTask == true || isShowingMark == true) && fireReportManagerInstance.isStartAlarm == false)
        {
            //mark is blind
            if (isShowingMark == false)
            {
                UIInstance.SetActive(true);
                if(isCorrect == false)
                    updateQuestion();
            }

            showMark();
            stopMark();

        }
        
    }
}
