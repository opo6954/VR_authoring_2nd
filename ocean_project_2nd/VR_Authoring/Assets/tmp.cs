using UnityEngine;
using System.Collections;

public class tmp : MonoBehaviour {

	// Use this for initialization

    /*
     * FOR Create instance of class passing through name of class with string type
    ClientStateController csc = null;
    EquipmentOrderClientState csmt = null;

    

    void Awake()
    {
        csc = gameObject.AddComponent<ClientStateController>();
    }

    void Start()
    {
        //이렇게 하면 될듯
        csmt = System.Activator.CreateInstance(System.Type.GetType("EquipmentOrderClientState")) as EquipmentOrderClientState;

        csmt.setParameters("power", "j,k,l", true, 3, "Fire");

        csc.setCurrClientState(csmt);
    }

    void Update()
    {
        if (csc.isCurrClientStateExist() == false)
        {
            csc.setCurrClientState(csmt);
        }
    }
     * */

    public string roleName = "";

    public bool isStart = false;
    

    void Awake()
    {
    }

    void Start()
    {
        Debug.Log("my Role Name is " + roleName);
    }

    void Update()
    {
        
        if (Input.GetKey("r") == true && isStart == false)
        {
            isStart = true;
            StartCoroutine("getNewValue", new string[3] {"A","B","C"});
        }
    }

    public void What()
    {
        Debug.Log("What>?");
    }

    IEnumerator getNewValue(string[] roleList)
    {
        Debug.Log("In the coroutine...");

        while (true)
        {
            What();
            if (Input.GetKey("d") == true)
            {
                Debug.Log("Press d button, so role is determine...");
                roleName = roleList[1];

                Debug.Log(roleName);

                yield break;
            }
            yield return null;
        }
    }
}
