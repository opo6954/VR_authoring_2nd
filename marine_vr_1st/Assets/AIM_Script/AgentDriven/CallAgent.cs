using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CallAgent : MonoBehaviour {
	public Transform target;
	private GameObject[] agents;
	private GameObject text_notify;
	private const float height_offset = 8.0f;
	private const float shout_range = 30.0f;
	private float shout_angle_dot = Mathf.Cos (50.0f * Mathf.PI/180.0f);
	private float current_distance;
    private Vector3 exitPt;

    PassengerEscapeManager passengerControllerInstance;

    private GameParameter.playerRolePlay myRole;

    private bool isFirstCall = true;
    private bool initAgentOutline = false;

	void Initialize () {
		agents = GameObject.FindGameObjectsWithTag ("Agent");
    }

	// Use this for initialization
	void Start () {

        exitPt = GameObject.Find("EscapePoint").transform.position;

        myRole = gameObject.transform.parent.GetComponent<NetworkSender>().getMyRole();

        

		text_notify = new GameObject ("3D_Text");
		text_notify.AddComponent<TextMesh> ();
		text_notify.GetComponent<TextMesh> ().text = "";
		text_notify.GetComponent<TextMesh> ().fontSize = 15;
		text_notify.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
		text_notify.transform.parent = gameObject.transform;
		text_notify.SetActive (false);

        target = gameObject.transform.parent;

		Initialize ();
	}

    //fire fail이랑 연결하자~!~!~!

    public bool determineFinish()
    {
        if (agents != null)
        {
            int count = 0;

            for (int i = 0; i < agents.Length; i++)
            {
                if ((agents[i].transform.position - exitPt).magnitude < shout_range)
                    count++;
            }
            if (count == agents.Length)
            {
                return true;
            }
        }

        return false;

    }


    // Update is called once per frame
    void Update()
    {
        //only work for finder...

        

        if (myRole == GameParameter.playerRolePlay.FINDER)
        {
            
            if (isFirstCall == true)
            {
                passengerControllerInstance = gameObject.GetComponent<CentralSystem>().getTaskManager<PassengerEscapeManager>();
                passengerControllerInstance.getAgentInfo(agents);

                //FOr Debug     passengerControllerInstance.isDoingTask = true;
                
                isFirstCall = false;
            }

            
            

            if (determineFinish() == true)
            {
                passengerControllerInstance.isMyTaskFinish = true;
            }

            else if (passengerControllerInstance.isDoingTask == true && passengerControllerInstance.isDoneTask == false && passengerControllerInstance.isMyTaskFinish == false)
            {
                if (initAgentOutline == false)
                {
                    for (int i = 0; i < agents.Length; i++)
                    {
                        agents[i].transform.FindChild("emphasizeSpot").gameObject.SetActive(true);
                    }
                    initAgentOutline = true;
                }
                int agentIdx=-1;
                GameObject highlighted = null;
                current_distance = shout_range;
                float min_distance = shout_range;
                Vector3 player_pos = Camera.main.transform.position - new Vector3(0.0f, height_offset, 0.0f);
                for (int i = 0; i < agents.Length; i++)
                {
                    current_distance = (agents[i].transform.position - player_pos).magnitude;
                    float current_dot = Vector3.Dot((agents[i].transform.position - player_pos).normalized, Camera.main.transform.forward.normalized);
                    if (current_distance < shout_range && current_dot > shout_angle_dot)
                    {
                        if (current_distance < min_distance)
                        {
                            highlighted = agents[i];
                            agentIdx = i;
                            min_distance = current_distance;
                        }
                    }
                }

                if (highlighted && highlighted.GetComponent<AgentFollower>().isTargetOn == false)
                {
                    text_notify.SetActive(true);
                    text_notify.GetComponent<TextMesh>().text = "<size=20><color=red>X</color></size>키를 눌러\n승객이 당신을\n따라오도록 하세요";
                    //Camera.main.(Camera.main.WorldToScreenPoint(highlighted.transform.position));
                    //text_notify.transform.localPosition = new Vector3 (5.0f, 5.0f, current_distance);
                    text_notify.transform.position = highlighted.transform.position + new Vector3(0.0f, height_offset, 0.0f);
                    text_notify.transform.LookAt(Camera.main.transform.position);
                    text_notify.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                }
                else
                {
                    text_notify.SetActive(false);
                }

                if (highlighted)
                {
                    bool isButtonPressed = false;
                    if (gameObject.GetComponent<CentralSystem>().isJoystick == true)
                        isButtonPressed = Input.GetKeyDown(CentralSystem.getJoystickMappingInfo(JoystickType.X));
                    else
                        isButtonPressed = Input.GetKey(KeyCode.X);

                    if (isButtonPressed)
                    {
                        highlighted.transform.FindChild("emphasizeSpot").gameObject.SetActive(false);


                        highlighted.GetComponent<PhotonView>().RPC("setMovementOn", PhotonTargets.All, true);
                        highlighted.GetComponent<PhotonView>().RPC("setAnimatorValue", PhotonTargets.All, 1);
                        highlighted.GetComponent<NetworkAnimationChanger>().playerPos = target;

                        highlighted.GetComponent<NetworkAnimationChanger>().isMoveFirst = true;

                        passengerControllerInstance.currInteractPassenger++;
                    }
                }
            }
        }
    }



}
