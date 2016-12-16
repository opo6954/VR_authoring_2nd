using UnityEngine;
using System.Collections;

public class FireNoticeUIManager : TaskUITemplate {

    FireNoticeManager fireNoticeManagerInstnace;



    private GameObject UIInstance;
    

    public override void Init()
    {
        fireNoticeManagerInstnace = getOwnedSystem().getTaskManager<FireNoticeManager>();

        UIInstance = GameObject.Find("FireDiscover_UI");

        UIInstance.SetActive(false);       
    }

    public override void Process()
    { 
        if (fireNoticeManagerInstnace.isDoingTask == true)
            UIInstance.SetActive(true);
        else if(fireNoticeManagerInstnace.isDoneTask == true)
            UIInstance.SetActive(false);      
    }
}
