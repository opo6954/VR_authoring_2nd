using UnityEngine;
using System.Collections;

public class TaskManagerTemplate{

    //task is start
    public bool isDoingTask=false;
    //if task is done, it is true
    public bool isDoneTask=false;
    public bool isFirstCall = true;
    public int taskNumber = -1;

    CentralSystem ownedSystem;
    CentralUISystem ownedUI;
    GameObject UIInstanceTot;


    public void setUIInstance()
    {
        UIInstanceTot = GameObject.Find("subTaskInfo");
    }

    public GameObject getUIInstance()
    {
        return UIInstanceTot;
    }
    
    public virtual void processor()
    {

    }
    public virtual void getInfofromUI()
    {

    }
    public virtual void passInfo2UI()
    {

    }
    public virtual void Destroy()
    {

    }

    public void setOwnedSystem(CentralSystem _player)
    {
        ownedSystem = _player;
    }

    public CentralSystem getOwnedSystem()
    {
        return ownedSystem;
    }

    public void setOwnedUI(CentralUISystem _playerUI)
    {
        ownedUI = _playerUI;
    }
    public CentralUISystem getOwnedUI()
    {
        return ownedUI;
    }


    public virtual void Init()
    {
        Debug.Log("Init in template");
    }
    public virtual void Process()
    {
        Debug.Log("Process in template");
    }

}
