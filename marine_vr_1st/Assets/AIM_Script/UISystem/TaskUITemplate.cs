using UnityEngine;
using System.Collections;

public class TaskUITemplate{

    private CentralSystem ownedSystem;
    private CentralUISystem ownedUI;
    
    public void setOwnedSystem(CentralSystem _system)
    {   
        ownedSystem = _system;
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
        Debug.Log("Init in Template");
    }

    public virtual void  Process()
    {
        Debug.Log("Process in Template");
    }

    public virtual void Destroy()
    {

    }

}