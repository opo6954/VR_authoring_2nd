using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTemplate : MonoBehaviour {

    //task가 순서대로 실행됩니다
    private Dictionary<string, TaskModuleTemplate> taskList;
    GameObject canvas_ui;
    public UIModuleTemplate myUIInfo;

    public bool isJoystick = false;
    public bool isLeapMotion = false;

    private string startTaskName = "";

	public float myWalkSpeed=0.0f;


    //prevTaskName 뒤에 붙이기, prevTaskName이 ""일 경우 제일 처음에 붙인다
    public bool insertTask(string prevTaskName, TaskModuleTemplate task)
    {

        task.setMyUI(myUIInfo);
        task.setMyPlayer(this);

        //아직 아무 task도 없을 때
        if (startTaskName == "")
        {
            startTaskName = task.myTaskName;
        }


        else//이미 task 1개라도 존재할 시
        {
            if (prevTaskName == "")//만일 처음에 task를 붙일 경우
            {
                task.prevTaskName = "";
                task.nextTaskName = startTaskName;

                getTask(startTaskName).prevTaskName = task.myTaskName;

                startTaskName = task.myTaskName;
            }
            else if (isTaskContains(prevTaskName) == true)
            {
                TaskModuleTemplate prevTask = getTask(prevTaskName);

                string nextTaskName = prevTask.nextTaskName;
                string beforeTaskName = prevTask.myTaskName;

                task.prevTaskName = beforeTaskName;
                task.nextTaskName = nextTaskName;

                prevTask.nextTaskName = task.myTaskName;

                if (nextTaskName != "")
                {
                    getTask(nextTaskName).prevTaskName = task.myTaskName;
                }

            }
            else
            {
                return false;
            }
        }
        taskList.Add(task.myTaskName, task);
        //prevTask있음
        return true;

    }



    public bool removeTask(string taskName)
    {
        if (isTaskContains(taskName) == true)
        {
            string prevTask = getTask(taskName).prevTaskName;
            string nextTask = getTask(taskName).nextTaskName;

            taskList.Remove(taskName);

            if (prevTask == "")//제일 처음 task일 경우
            {
                getTask(nextTask).prevTaskName = "";
                startTaskName = nextTask;
            }
            else if (nextTask == "")//제일 마지막 task일 경우
            {
                
                getTask(prevTask).nextTaskName = "";
            }


            if (prevTask != "" && nextTask != "")//중간에 존재하는 task일 경우
            {
                getTask(prevTask).nextTaskName = nextTask;
                getTask(nextTask).prevTaskName = prevTask;
            }
            return true;
        }
        
        return false;
    }


    public void beginFirstTask()
    {
        if (isTaskContains(startTaskName) == true)
        {
            getTask(startTaskName).setStartTrigger();
        }
    }


    public bool isTaskContains(string taskName)
    {
        return taskList.ContainsKey(taskName);
    }

    public TaskModuleTemplate getTask(string taskname)
    {
        if(isTaskContains(taskname))
            return taskList[taskname];
        return null;
    }





    public void setMyUI(UIModuleTemplate uiModule)
    {
        myUIInfo = uiModule;
    }

	//util function

	public Camera getCamera()
	{
		return transform.GetChild (0).GetChild (0).GetComponent<Camera> ();
	}

	// Use this for initialization
	void Start () {

		myWalkSpeed = transform.GetChild (0).transform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().m_WalkSpeed;

        taskList = new Dictionary<string, TaskModuleTemplate>();
        canvas_ui = GameObject.Find("Canvas_UI");
        
	}

	// Update is called once per frame
	void Update () {
	
	}
}
