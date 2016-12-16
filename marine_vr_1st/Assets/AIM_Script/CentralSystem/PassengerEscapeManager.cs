using UnityEngine;
using System.Collections;

public class PassengerEscapeManager : TaskManagerTemplate {
    


    public int totalPassenger;
    public int currInteractPassenger=0;
    public GameObject[] agents;
    private Transform exitPt;
    
    private bool isLock = false;
    public bool isMyTaskFinish = false;

    public void getAgentInfo(GameObject[] _agents)
    {
        
        System.Array.Resize<GameObject>(ref agents,_agents.Length);
        System.Array.Copy(_agents, agents, agents.Length);
    }

    public void determineFinish()
    {
        
        if (isMyTaskFinish == true)
        {
            getOwnedSystem().gameObject.transform.parent.GetComponent<NetworkSender>().changeLocalNetworkTaskDone(taskNumber);
        }
    }

    public override void Destroy()
    {
        isDoingTask = false;
        getOwnedSystem().isFinishAll = true;

        GameObject.Find("AssemblyPoint").transform.FindChild("emphasizeSpot").gameObject.SetActive(false);
        getUIInstance().GetComponent<TurnOffImageNText>().turnOnOff(false);

        if (isLock == false)
        {
            getOwnedSystem().lockFPSScreen(true);
            isLock = true;
        }
        if (agents != null)
        {
            for (int i = 0; i < agents.Length; i++)
                agents[i].gameObject.SetActive(false);
        }

    }

    public override void Init()
    {

        agents = null;


    }


    public override void Process()
    {
        if(isDoingTask == true)
        {
            determineFinish();
        }
        if (isDoneTask == true && isDoingTask == true)
            Destroy();
    }

}
