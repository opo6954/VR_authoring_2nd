using UnityEngine;
using System.Collections;

public class CountAgent : MonoBehaviour {
	public Transform rally_point;//passenger의 최종 도착장소?
	public float rally_range = 30.0f;
	public float shout_range = 50.0f;
	private int counter = 0;
	private int max_count = 0;
	private GameObject[] agents;
	private bool[] is_counted;
    

	private const float height_offset = 8.0f;
	private float shout_angle_dot = Mathf.Cos (30.0f * Mathf.PI/180.0f);

    private bool isFirstCall = true;
    PassengerEscapeManager passengerControllerInstance;
    private GameParameter.playerRolePlay myRole;

    GameObject text_notify;

    private string buttonTaskInfo = "<size=20><color=red>X</color></size>키를 눌러\n승객을 확인하세요";

    void Initialize() {

        myRole = gameObject.transform.parent.GetComponent<NetworkSender>().getMyRole();

        
        rally_point = GameObject.Find("AssemblyPoint").transform;
        //FOr Debug //rally_point = gameObject.transform ;

		agents = GameObject.FindGameObjectsWithTag ("Agent");
		is_counted = new bool[agents.Length];
		max_count = is_counted.Length;
		for (int i = 0; i < max_count; i++)
			is_counted[i] = false;
        

        text_notify = new GameObject("3D_Text_Count");
        text_notify.AddComponent<TextMesh>();

        text_notify.GetComponent<TextMesh>().text = buttonTaskInfo;
        text_notify.GetComponent<TextMesh>().fontSize = 15;
        text_notify.GetComponent<TextMesh>().alignment = TextAlignment.Center;
        text_notify.transform.parent = gameObject.transform;

        text_notify.SetActive(false);

        if (myRole == GameParameter.playerRolePlay.COUNTER)
        {
            for (int i = 0; i < agents.Length; i++)
            {
                
            }
        }
    }
    // Use this for initialization
    void Start()
    {
		counter = 0;
		agents = null;
		Initialize ();
	}


    public bool determineFinish()
    {
        for (int i = 0; i < is_counted.Length; i++)
        {
            if (is_counted[i] == false)
                return false;
        }

        Destroy(text_notify);
        

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (myRole == GameParameter.playerRolePlay.COUNTER)
        {
            if (isFirstCall == true)
            {
                passengerControllerInstance = gameObject.GetComponent<CentralSystem>().getTaskManager<PassengerEscapeManager>();

                //for DEBUG                //passengerControllerInstance.isDoingTask = true;
                isFirstCall = false;
            }

            if (passengerControllerInstance.isDoingTask == true && passengerControllerInstance.isDoneTask == false && passengerControllerInstance.isMyTaskFinish == false)
            {
                if (determineFinish() == true)
                {
                    passengerControllerInstance.isMyTaskFinish = true;
                }


                Vector3 player_pos = Camera.main.transform.position - new Vector3(0.0f, height_offset, 0.0f);
                GameObject closestAgent=null;
                int agentIdx=-1;

                for (int i = 0; i < max_count; i++)
                {
                    if (!is_counted[i])
                    {
                        float current_distance = (agents[i].transform.position - player_pos).magnitude;
                        float current_dot = Vector3.Dot((agents[i].transform.position - player_pos).normalized, Camera.main.transform.forward.normalized);

                        
                        float minDistance = shout_range;

                        if (current_distance < shout_range && current_dot > shout_angle_dot && (rally_point.position - agents[i].transform.position).magnitude < rally_range)
                        {
                            agents[i].transform.FindChild("emphasizeSpot").gameObject.SetActive(true);

                            if (current_distance < minDistance)
                            {
                                minDistance = current_distance;
                                closestAgent = agents[i];
                                agentIdx = i;
                            }
                        }
                        else
                        {
                            
                        }
                    }
                }

                if (closestAgent != null)
                {
                    text_notify.transform.position = closestAgent.transform.position + new Vector3(0.0f, height_offset, 0.0f);
                    text_notify.transform.LookAt(Camera.main.transform.position);
                    text_notify.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
                    text_notify.SetActive(true);

                    bool isButtonPressed = false;
                    if (gameObject.GetComponent<CentralSystem>().isJoystick == true)
                    {
                        isButtonPressed = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.X));
                    }
                    else
                        isButtonPressed = Input.GetKey(KeyCode.X);


                    if (isButtonPressed == true)
                    {
                        //closestAgent.transform.FindChild("female").GetComponent<Control_objectify>().setObjectifyValue(false);



                        closestAgent.transform.FindChild("emphasizeSpot").gameObject.SetActive(true);



                        GameObject checkText = new GameObject("checkText");
                        checkText.AddComponent<TextMesh>();
                        checkText.GetComponent<TextMesh>().fontSize = 15;
                        checkText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                        checkText.transform.parent = gameObject.transform;


                        checkText.GetComponent<TextMesh>().text = (counter+1).ToString() + "번째 승객 확인";

                        checkText.transform.position = closestAgent.transform.position + new Vector3(0.0f, height_offset, 0.0f);
                        checkText.transform.LookAt(Camera.main.transform.position);
                        checkText.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));


                        checkText.SetActive(true);

                        
                        is_counted[agentIdx] = true;
                        counter++;

                        Destroy(checkText, 2.0f);
                    }

                    var text_obj = agents[agentIdx].GetComponentInChildren<TextMesh>();
                    if (text_obj)
                    {
                        text_obj.gameObject.transform.LookAt(Camera.main.transform.position);
                        text_obj.gameObject.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
                    }

                }
                else
                {
                    text_notify.SetActive(false);
                }
                    
                
            }
        }
	}
}
