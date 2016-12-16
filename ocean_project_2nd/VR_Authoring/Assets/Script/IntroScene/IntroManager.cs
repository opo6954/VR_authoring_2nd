using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour {

    public GameObject canvas;

    InputField playerNameInfo;

    public string playerName;

    //canvas 입력해주세요

    void Start()
    {
        //일단 연결만 해놓음
        playerNameInfo = canvas.transform.FindChild("InputName").transform.FindChild("InputField").GetComponent<InputField>();

    }

    public void sendInfo() 
    {
        playerName = playerNameInfo.text;
        Debug.Log(playerName);

        DontDestroyOnLoad(this);
        UnityEngine.SceneManagement.SceneManager.LoadScene("NO_VR_Network_AuthroingTool_Network");
    }


}
