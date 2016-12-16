using UnityEngine;
using System.Collections;

public class FireFailUIManager : TaskUITemplate {

    FireFailManager fireFailManagerInstance;


    GameObject reportInstance;

    private bool isStichingStart = false;
    

    public override void Init()
    {
        reportInstance = GameObject.Find("Task5");

        fireFailManagerInstance = getOwnedSystem().getTaskManager<FireFailManager>();
        
    }

    public void showReport()
    {
        if (isStichingStart == false)
        {
            CentralSystem.initStitching();
            isStichingStart = true;
        }
        if (CentralSystem.stitchingEffect() == true)
        {
            CentralSystem.setActiveChild(reportInstance, "reportfailwindow", true);
        }
        else
        {
            CentralSystem.setActiveChild(reportInstance, "reportfailwindow", false);
        }
    }

    public override void Destroy()
    {
        CentralSystem.setActiveChild(reportInstance, "reportfailwindow", false);
    }

    public override void Process()
    {
        if(fireFailManagerInstance.isDoingTask == true && fireFailManagerInstance.isDoneTask == false)
        {
            showReport();
        }
        
        else if(fireFailManagerInstance.isDoneTask == true)
        {
            Destroy();
        }        
    }


}
