using UnityEngine;
using System.Collections;

public class CentralUISystem : MonoBehaviour{

    //for singleton
    
    [SerializeField]
    private string[] taskUIManagerNames;
    private TaskUITemplate[] taskUIList;


    private bool isFirstCall = true;

    public bool isMappingTask = false;

    private bool isShowMultiTask = false;
    
    

    private string timeValue;

    private float timeOffset = 100f;



    private GameObject taskWindow;
    private GameObject finishWindow;

    private GameObject timeWindow;

    CentralSystem ownedSystem;




    //color info
    public enum texFlag { BOUNDARY, FULL };


    //maybe for the optimization, use dictionary to find fast......
    public T getTaskUIManager<T>()
    {
        for (int i = 0; i < taskUIList.Length; i++)
        {
            if (taskUIList[i] is T)
            {
                return (T)(object)taskUIList[i];
            }
        }
        return default(T);
    }

    public TaskUITemplate getTaskUIManager(string typeName)
    {
        for (int i = 0; i < taskUIManagerNames.Length; i++)
        {
            if (taskUIManagerNames[i] == typeName)
                return taskUIList[i];
        }
        return null;
    }

    void mappingTaskUIManager()
    {
        System.Array.Resize(ref taskUIList, taskUIManagerNames.Length);
        
        for (int i=0; i< taskUIManagerNames.Length; i++)
        {
            System.Type taskType = (System.Type.GetType(taskUIManagerNames[i]));

            taskUIList[i] = System.Activator.CreateInstance(taskType) as TaskUITemplate;
                        
            taskUIList[i].setOwnedSystem(gameObject.GetComponent("CentralSystem") as CentralSystem);
            taskUIList[i].Init();

        }

        isMappingTask = true;
        
    }

    public static void LoadImageToTex2DByResources(ref Texture2D tex2D, string filename)
    {

        TextAsset tmpTA = Resources.Load(filename) as TextAsset;
        tex2D = new Texture2D(2,2);
        tex2D.LoadImage(tmpTA.bytes);
 
    }
    public static Texture2D MakeTex(int width, int height, Color col, texFlag tf)
    {
        Color[] pix = new Color[width * height];

        if (tf == texFlag.BOUNDARY)
        {

            for (int i = 0; i < pix.Length; ++i)
            {
                if (i < 2 * width || i % width == 0 || i % width == 1 || i % width == width - 1 || i % width == width - 2 || i > width * height - 2 * width)
                    pix[i] = new Color(1,1,1);
                else
                    pix[i] = col;
            }
        }
        else if (tf == texFlag.FULL)
        {
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public void setTime(string _timeValue)
    {
        timeValue = _timeValue;
    }


    //time delay system
    void ShowTimeDelay()
    {
        /*
        float timePosWidth = ownedSystem.screenWidth * ownedSystem.timePositionWidthRatio;
        float timePosHeight = ownedSystem.screenHeight * ownedSystem.timePositionHeightRatio;
        

        GUIStyle gs = new GUIStyle();
        
        
        gs.fontSize = 40;
        gs.fontStyle = FontStyle.Bold;
        
        
        gs.normal.textColor = new Color(1, 0.2f, 0.3f);

        GUI.Label(new Rect(timePosWidth - timeOffset, timePosHeight, 0, 0), timeValue, gs);//8
        */

        timeWindow.GetComponentInChildren<UnityEngine.UI.Text>().text = timeValue;
        

        
    }


    void Init()
    {
        mappingTaskUIManager();
        ownedSystem = gameObject.GetComponent<CentralSystem>();
    }

    void Run()
    {
        if(ownedSystem.isFinishAll == false)
            ShowTimeDelay();
        for(int i=0; i<taskUIList.Length; i++)
        {
            taskUIList[i].Process();
        }
    }

    void initMainUI()
    {
        taskWindow = GameObject.Find("wholeui");
        timeWindow = taskWindow.transform.FindChild("timeinfo_background").gameObject;
        taskWindow.transform.FindChild("playModeInfo").GetComponent<TurnOffImageNText>().turnOnOffDuration(1.0f, 2);
        
        
    }

    void handleTaskInfo()
    {
        if (GameObject.Find("NetworkScript").GetComponent<CentralNetworkSystem>().isMultiPlayerStart == true && isShowMultiTask == false)
        {
            taskWindow.transform.FindChild("playModeInfo").FindChild("Text").GetComponent<UnityEngine.UI.Text>().text = "그룹 훈련입니다. \n 다른 플레이어와 같이 task를 수행하세요";
            taskWindow.transform.FindChild("playModeInfo").GetComponent<TurnOffImageNText>().turnOnOffDuration(1.0f, 2);
            isShowMultiTask = true;
        }


        if (ownedSystem.isScreenChange == true)
        {
            float boxWidth = ownedSystem.screenWidth * ownedSystem.wholeTaskWidth;
            float boxHeight = ownedSystem.screenHeight * ownedSystem.wholeTaskHeight;

            //taskWindow.transform.FindChild("taskinfo").GetComponent<RectTransform>().sizeDelta = new Vector2(boxWidth, boxHeight);
            //taskWindow.transform.localPosition = new Vector3(ownedSystem.screenWidth * ownedSystem.wholeTaskPosX - boxWidth / 2 - ownedSystem.screenWidth * ownedSystem.wholeTaskInfoOffset / 2, ownedSystem.screenHeight * ownedSystem.wholeTaskPosY - boxHeight / 2 + ownedSystem.screenHeight * ownedSystem.wholeTaskInfoOffset, 0);


        }

        //for the first task~!
        CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task1").gameObject, "oncheck", true);
        CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task1").gameObject, "uncheck", false);

        for (int i = 0; i < taskUIList.Length; i++)
        {
            if (ownedSystem.taskManagerList[i].isDoneTask == true)
            {
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "check", true);
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "oncheck", false);
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "uncheck", false);








            }
            else if (i > 0 && ownedSystem.taskManagerList[i - 1].isDoneTask == true)
            {
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "check", false);
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "oncheck", true);
                CentralSystem.setActiveChild(taskWindow.transform.FindChild("taskinfo").transform.FindChild("task" + (i + 1).ToString()).gameObject, "uncheck", false);
            }
        }
    }

    void showFinish()
    {
        finishWindow = taskWindow.transform.FindChild("finishInfo").gameObject;
        

        finishWindow.gameObject.SetActive(true);

//taskWindow.transform.FindChild("taskinfo").GetComponent<RectTransform>().sizeDelta = new Vector2(boxWidth, boxHeight);
    }

    public void runMainUI()
    {
        handleTaskInfo();
        if (ownedSystem.isFinishAll == true)
            showFinish();

    }
    
    
    void Start()
    {
        initMainUI();
    }
    //main gui
    void OnGUI()
    {
        if (isFirstCall == true)
        {
            Init();
            isFirstCall = false;
        }

        Run();
        runMainUI();
    }
}
