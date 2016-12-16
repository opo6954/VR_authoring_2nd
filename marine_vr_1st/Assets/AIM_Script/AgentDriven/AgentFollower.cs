using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public class AgentFollower : MonoBehaviour {
	public Vector3 targetPos;
	private NavMeshAgent agent;
    public bool isTargetOn = false;
    private Vector3 rallyPt;
    private Vector3 EscapePt;

    public float shout_range = 40.0f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
        rallyPt = GameObject.Find("AssemblyPoint").transform.position;
        EscapePt = GameObject.Find("EscapePoint").transform.position;
	}
	
	// Update is called once per frame

    /*
     *  if ((agents[i].transform.position - exitPt.position).magnitude < shout_range)
    */
    void Update () {
        if ((transform.position - rallyPt).magnitude < shout_range)
        {
            agent.destination = EscapePt;
        }

        else if (isTargetOn == true)
        {
            agent.destination = targetPos;
        }
	}
}
