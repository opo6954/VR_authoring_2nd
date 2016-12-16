using UnityEngine;
using System.Collections;

/* 
attach된 object에 유저가 collide하는 event가 발생 했을 때 해야 할 일 들을 처리하는 script 
화재 인식, 화재 보고 등 유저의 task 진행 상황에 대한 flag는 유저가 가지고 있는 script에서 static변수로 관리한다. 
모든 충돌 이벤트는 1회만 발생한다. 
FireRecognizePlane -> FireDiscover UI 띄우기 
FireReportCube -> FireReport UI 띄우기
 */

public class Collide_event_handler : MonoBehaviour {

    public GameObject[] LinkedObjects;

    

    
    
    void OnTriggerEnter(Collider other) // 충돌체간의 충돌을 감지해서 이벤트를 발생시키는 함수.
    {
        if (other.tag == "Player") { // 플레이어가 충돌 한 경우만 이벤트 발생

            /* FireRecognizePlane(transparent) 에 충돌 했을 경우 FireDiscoverUI를 띄움 */
            
            CentralSystem playerSystem = other.transform.FindChild("FirstPersonCharacter").GetComponent("CentralSystem") as CentralSystem;
            


            FireNoticeManager fireNoticeManagerInstance = playerSystem.getTaskManager<FireNoticeManager>();


            FireReportManager fireReportManagerInstance = playerSystem.getTaskManager<FireReportManager>();
            FireFailManager fireFailManagerInstance = playerSystem.getTaskManager<FireFailManager>();

            if ( fireNoticeManagerInstance.isDoingTask == false && fireNoticeManagerInstance.isDoneTask == false && name == "FireRecognizePlane(transparent)")
            {
                fireNoticeManagerInstance.isDoingTask = true;//initiate the whole task with collide
            }

            /* FireReportCube(transparent)에 충돌하고, FireRecognize가 이미 된 상태일 경우, FireReportUI를 띄우고 FireReportAlarmUI는 deactivate시킴 */
            
            else if ( fireReportManagerInstance.isDoingTask == false && fireNoticeManagerInstance.isDoneTask == true && fireReportManagerInstance.isDoneTask == false && this.name == "Phone")
            {
                gameObject.transform.FindChild("Text").GetComponent<Hide_by_renderer>().turnRenderer(true);
            }

             else if (fireReportManagerInstance.isDoingTask == false && fireNoticeManagerInstance.isDoneTask == true && fireReportManagerInstance.isDoneTask == false && this.name == "fire_alarm_box")		
             {		
                 gameObject.transform.FindChild("Text").GetComponent<Hide_by_renderer>().turnRenderer(true);		
             }

            //for the fire fail task
            else if (fireFailManagerInstance.isDoingTask == false && fireFailManagerInstance.isDoneTask == false && playerSystem.getTaskManager<FireExtManager>().isDoneTask == true && this.name == "Phone")
            {
                gameObject.transform.FindChild("Text").GetComponent<Hide_by_renderer>().turnRenderer(true);
            }

        }

        else if(other.tag == "ExtinguishableReal")//if the particle is collide to fire, then the fire is extinguished
        {
            (gameObject.transform.parent.parent.GetComponent("CentralSystem") as CentralSystem).getTaskManager<FireExtManager>().getCollideInfo(other.gameObject);
        }
        
        return;
    }
  
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	}
}
