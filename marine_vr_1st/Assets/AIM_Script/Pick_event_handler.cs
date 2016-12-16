using UnityEngine;
using System.Collections;

/* 
player에게 attach하는 script, player가 raycast를 이용해 앞쪽 일정 거리 이내에 있는 object를 인식할 수 있게한다. 
만약 tag가 Pickable 이라면 pick가능한 object로 간주하고, object를 pick up 한다. 
pickup 후에는 Hide_by_SetActive 스크립트의 change_activate_state()함수를 이용해서 object를 deactivate 시킨다.
 mouse를 클릭하는 경우에 raycast를 발사하도록 한다.([TODO]:조이스틱에선 다른 키로 mapping 해야함)
 */

public class Pick_event_handler : MonoBehaviour {

    GameObject[] pickableObj;
    float shout_range = 50.0f;
    private float shout_angle_dot = Mathf.Cos(30.0f * Mathf.PI / 180.0f);
    

    public GameObject closestInteractable()
    {
        float minDistance = 9999;
        int idx=-1;

        for (int i = 0; i < pickableObj.Length; i++)
        {
            float distance = (pickableObj[i].transform.position - gameObject.transform.position).magnitude;
            float current_dot = Vector3.Dot((pickableObj[i].transform.position - gameObject.transform.position).normalized, Camera.main.transform.forward.normalized);

            if (distance < shout_range && current_dot > shout_angle_dot)
            {

                if (distance < minDistance)
                {
                    minDistance = distance;
                    idx = i;
                }
            }
        }
        if (idx == -1)
            return null;
        else
            return pickableObj[idx];

        

    }

    // Use this for initialization
    void Start()
    {
        pickableObj = GameObject.FindGameObjectsWithTag("Pickable");
	}

    // Update is called once per frame
    void Update()
    {
        //later, change it to joystick~!~!

        
        bool isKeyDown = false;
        if (gameObject.GetComponent<CentralSystem>().isJoystick == true)
            isKeyDown = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.X));
        else if(gameObject.GetComponent<CentralSystem>().isLeapMotion == true)
            isKeyDown = gameObject.GetComponent<CentralSystem>().isLeapMotionClicked(LeapMotionType.CLICK);
        else
            isKeyDown = Input.GetKeyDown(KeyCode.X);

        if (isKeyDown)
        {
            GameObject closeObj = closestInteractable();

            if (closeObj != null)
            {
                
                FireNoticeManager fireNoticeManagerInstance = (gameObject.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireNoticeManager>();
                FireMethodManager fireMethodManagerInstance = (gameObject.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireMethodManager>();
                FireReportManager fireReportManagerInstance = (gameObject.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireReportManager>();
                FireFailManager fireFailManagerInstance = (gameObject.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireFailManager>();
                FireExtManager fireExtManagerInstance = (gameObject.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireExtManager>();

                if (closeObj.transform.name == "extinguisher" && fireMethodManagerInstance.isDoingTask == false && fireMethodManagerInstance.isDoneTask == false && fireReportManagerInstance.isDoneTask == true) // 소화기를 pickup한 경우
                {
                    Debug.Log("what?");

                    StartCoroutine(timeDelayToStartTask(fireMethodManagerInstance));

                    if (GameParameter.isSinglePlayer == true)
                        closeObj.transform.GetComponent<Hide_by_SetActive>().change_active_state();
                    else
                    {
                        closeObj.transform.GetComponent<PhotonView>().RPC("change_active_state", PhotonTargets.All);//for network serialize, use RPC function
                    }

                    GameObject.Find("interactable").transform.FindChild("extinguisher").GetComponent<Control_objectify>().setObjectifyValue(false);
                }
                else if (closeObj.transform.name == "fire_alarm_box" && fireReportManagerInstance.isDoingTask == true && fireReportManagerInstance.isDoneTask == false)
                {
                    fireReportManagerInstance.executeAlarm(closeObj.transform);//change the task boolean
                    closeObj.transform.GetComponent<PhotonView>().RPC("enableAudio", PhotonTargets.All);//run the audio
                    closeObj.transform.FindChild("Text").GetComponent<MeshRenderer>().enabled = false;
                    GameObject.Find("extinguisher").transform.GetComponent<PhotonView>().RPC("setTagInfo", PhotonTargets.All);

                    
                }
                else if (closeObj.transform.name == "ShipPhone")
                {
                    if (fireNoticeManagerInstance.isDoneTask == true &&
                        fireReportManagerInstance.isDoingTask == false && fireReportManagerInstance.isDoneTask == false)
                    {
                        StartCoroutine(timeDelayToStartTask(fireReportManagerInstance));//좀 있다가 시작함...
                    }
                    else if (fireFailManagerInstance.isDoingTask == false && fireFailManagerInstance.isDoneTask == false
                        && fireExtManagerInstance.isDoneTask == true)
                    {
                        StartCoroutine(timeDelayToStartTask(fireFailManagerInstance));//좀 있다가 시작함...
                        
                    }

                }
            }
            else
            {
                Debug.Log("No obj...");
            }

                        

        }
    }



    public IEnumerator timeDelayToStartTask(TaskManagerTemplate t)
    {
        
        yield return new WaitForSeconds(0.5f);
        t.isDoingTask = true;
        
    }
}


        

        
