using UnityEngine;
using System.Collections;

//global definition
public enum JoystickType { X, Y, A, B, LB, RB, LT, RT };
public enum LeapMotionType {CLICK, HANG};


public class CentralSystem : Photon.MonoBehaviour{

   

    //-1 is for total process
    private int debugSceneIdx = -1;
    public int globalTaskDone;
    public bool isGlobalTaskUpdate = false;
    /*
    0: fire notice
    1: fire report
    2: fire method
    3: fire extinguishing
    4: fail report
    5: Assemble spot
    6: passenger escape
    */



    public void setDebugScene()
    {
        
        if (debugSceneIdx >= 0)
        {
            for (int i = 0; i < debugSceneIdx; i++)
            {
                taskManagerList[i].isDoingTask = false;
                taskManagerList[i].isDoneTask = true;
            }

            taskManagerList[debugSceneIdx].isDoingTask = true;
            taskManagerList[debugSceneIdx].isDoneTask = false;
        }
    }

    public void setGlobalTaskDone()
    {

        if (globalTaskDone >= 0 && isGlobalTaskUpdate == true)
        {
            for(int i=0; i<globalTaskDone; i++)
            {
                taskManagerList[i].isDoneTask = true;
                taskManagerList[i].isDoingTask = false;
            }

            
            taskManagerList[globalTaskDone].isDoneTask = true;


            //global task indx의 의미는 지금까지 완성된 순서를 나타낸다
            //isdoingtask의 경우 각 manager 내부적으로 값을 바꾸니까 밖에서 바꾸면 destory 함수가 제대로 호출이 안되서 오류날 수 있어여 like lock fps screen임돠

            isGlobalTaskUpdate = false;
        }
    }



    public T getTaskManager<T>()
    {
        for (int i = 0; i < taskManagerList.Length; i++)
        {
            if (taskManagerList[i] is T)
            {
                return (T)(object)taskManagerList[i];
            }
        }
        return default(T);
    }

    public TaskManagerTemplate getTaskManager(string typeName)
    {
        for (int i = 0; i < taskManagerNames.Length; i++)
        {
            if (taskManagerNames[i] == typeName)
                return taskManagerList[i];
        }
        return null;
    }

    public TaskManagerTemplate getNextTask(int idx)
    {
        if (idx < taskManagerList.Length)
            return taskManagerList[idx + 1];
        else
            return null;
    }


    //for debug interface
    public float posX = 0.1f;
    public float posY = 0.1f;
    public float width = 0.8f;
    public float height = 0.8f;
    //for debug... continue

    public float timePositionHeightRatio = 0.015f;
    public float timePositionWidthRatio = 0.5f;

    public float taskPositionHeightRatio = 0.01f;
    public float taskPositionWidthRatio = 0.005f;
    public float taskWidthRatio = 0.3f;
    public float taskHeightRatio = 0.3f;

    public float wholeTaskInfoOffset = 0.05f;
    public float wholeTaskPosX = 0.5f;
    public float wholeTaskPosY = 0.5f;
    public float wholeTaskWidth = 0.2f;
    public float wholeTaskHeight = 0.3f;




    //for joystick mapping

    private static System.Collections.Generic.Dictionary<JoystickType, string> joystickMappingTable;
    




    
    
    public string[] taskManagerNames;

    
    public bool isJoystick = false;
    public bool isLeapMotion = false;


    public TaskManagerTemplate[] taskManagerList;
    

    //the training score of user
    private int score = 0;
    private float startTime;
    private string timeMessage;

    public bool isScreenChange = false;
    public float screenWidth;
    public float screenHeight;



    private static float samplingTime;
    private static float samplingRate = 0.5f;
    private static bool isStitchingOn = true;

    public bool isFinishAll = false;

    private string myName;

    void Awake()
    {
        Init();
        GameObject.Find("Canvas_UI").GetComponent<Canvas>().worldCamera = gameObject.transform.GetComponent<Camera>();
        globalTaskDone = -1;
    }



    //global utility
    public static void setActiveChild(GameObject myObject, string childObjectName, bool value)
    {
        myObject.transform.FindChild(childObjectName).gameObject.SetActive(value);
    }

    public static void setActiveChild(string parentObjectName, string childObjectName, bool value)
    {
        GameObject.Find(parentObjectName).transform.FindChild(childObjectName).gameObject.SetActive(value);
    }

    public static void setActive(string gameObjectName, bool value)
    {
        Debug.Log("You should implementation");
        //gameObject.transform.FindChild("Fire_Extinguisher_FPS").gameObject.SetActive(false);
    }

    public static void setActiveGameComponent(string gameObjectName, string gameComponentName, bool value)
    {
        ((MonoBehaviour)(GameObject.Find(gameObjectName).transform.GetComponent(gameComponentName))).enabled = value;
    }

    public static string getJoystickMappingInfo(JoystickType joys)
    {
        return joystickMappingTable[joys];
    }



    public IEnumerator tmp(float t)
    {
        yield return new WaitForSeconds(t);
    }

    public void getDuration(float t)
    {
        StartCoroutine(tmp(t));
    }


    //plz locate the audio source in the resource/sound folder
    public static void playAudio(string name)
    {
        AudioClip ac = Resources.Load("Sound/" + name, typeof(AudioClip)) as AudioClip;
        GameObject.Find("AdditionalSound").GetComponent<AudioSource>().clip = ac;
        GameObject.Find("AdditionalSound").GetComponent<AudioSource>().Play();
    }
    


    public void lockFPSScreen(bool wantLock)
    {
        if (wantLock == true)
        {
            setActiveGameComponent(gameObject.transform.parent.name, "FirstPersonController", false);
        }
        else
        {
            setActiveGameComponent(gameObject.transform.parent.name, "FirstPersonController", true);
        }
    }
    //FOR LEAP MOTION SUPPORTED
    public bool isLeapMotionClicked(LeapMotionType type)
    {
        //ADD FUNCTION OF LEAP MOTION~!~!~!
        return true;
    }



    public static Vector2 UIRefPt2UIAbsPt(Vector2 target)
    {
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;
        
        return new Vector2(target.x + (float)(screenWidth / 2.0f), screenHeight - (target.y + (float)(screenHeight / 2.0f)));
    }

    public static Vector2 UIAbsPt2UIRef(Vector2 target)
    {
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;

        return new Vector2(target.x - screenWidth/2, screenHeight/2 -  target.y);
    }

    public static void initStitching()
    {
        samplingTime = Time.time;
    }

    public static bool stitchingEffect()
    {
        float currTime = Time.time;
        float deltaTime = currTime - samplingTime;

        if (deltaTime > samplingRate)
        {
            samplingTime = currTime;

            if (isStitchingOn == false)
                isStitchingOn = true;
            else
                isStitchingOn = false;
        }

        return isStitchingOn;
    }


    //init function
    void mappingTaskManager()
    {
        if (taskManagerNames.Length > 0)
        {
            //resize the task manager list
            System.Array.Resize(ref taskManagerList, taskManagerNames.Length);

            for (int i = 0; i < taskManagerNames.Length; i++)
            {
                System.Type taskType = (System.Type.GetType(taskManagerNames[i]));

                taskManagerList[i] = System.Activator.CreateInstance(taskType) as TaskManagerTemplate;
                taskManagerList[i].setOwnedSystem(this);
                taskManagerList[i].setOwnedUI(gameObject.GetComponent("CentralUISystem") as CentralUISystem);
                taskManagerList[i].setUIInstance();
                taskManagerList[i].taskNumber = i;
                taskManagerList[i].Init();
            }
        }
        else
            Debug.Log("No task found");
    }
    private void makeJoystickMap()
    {
        joystickMappingTable = new System.Collections.Generic.Dictionary<JoystickType, string>();

        joystickMappingTable.Add(JoystickType.A, "joystick button 0");
        joystickMappingTable.Add(JoystickType.B, "joystick button 1");
        joystickMappingTable.Add(JoystickType.X, "joystick button 2");
        joystickMappingTable.Add(JoystickType.Y, "joystick button 3");
        joystickMappingTable.Add(JoystickType.LB, "joystick button 4");
        joystickMappingTable.Add(JoystickType.RB, "joystick button 5");
        joystickMappingTable.Add(JoystickType.LT, "joystick button 6");
        joystickMappingTable.Add(JoystickType.RT, "joystick button 7");
    }
    

    void Init()
    {
        makeJoystickMap();

        startTime = Time.time;
        

        GameParameter.initStatement();
    }

    void Start()
    {
        mappingTaskManager();
        setDebugScene();


        

    }


    void calcTimeDelay()
    {
        float timeCount = Time.time - startTime;

        int minTmp = (int)(timeCount / 60);
        float min = (float)minTmp;
        float sec = (int)(timeCount) % 60;
        float fraction = (timeCount * 100.0F) % 100.0F;

        if (fraction >= 50.0F)
            fraction = fraction - 50.0F;
        else
            fraction = fraction + 50.0F;

        fraction = (int)(fraction % 100.0F);

        timeMessage = string.Format("{00:00} : {1:00} : {2:00}", min, sec, fraction);

        gameObject.GetComponent<CentralUISystem>().setTime(timeMessage);
    }
    
    public void shutDown()
    {
        
    }


    void Update()
    {
        setGlobalTaskDone();//serialize the global task done info...

        

        if ((screenHeight != Screen.height || screenWidth != Screen.width))
        {        
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            isScreenChange = true;   
        }

        if (isFinishAll == false)
            calcTimeDelay();
        else
            shutDown();    
        
        for (int i = 0; i < taskManagerList.Length; i++)
        {
            taskManagerList[i].Process();
        }
    }
}
