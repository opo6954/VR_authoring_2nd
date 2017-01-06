using UnityEngine;
using System.Collections;
/*
 * 소화기에서의 Part부분을 순서대로 맞추고 맞췄을 때의 동영상을 보여주는 clientState임
 * 필요한 파라미터:
 * Select_Button: "x"
 * VideoName: "video4, video2, video3, video1"
 * isVideo: True
 * PartCount: 4
 * 
 * 필요한 오브젝트:
 * Interaction_to_Object: "FireExtinguisher_Segment"
 * 
 * */
public class EquipmentOrderClientState : ClientStateModuleTemplate {

    //property들
    public string select_button = "";
    public string[] videoName;

    public bool isVideo;

    public int partCount; 


    //Order를 맞추는 object가 들어가 있음
    public string interact_object;

    //이 부분은 추후에 parent에서 virtual로 만들어놔야 할듯.......~!~!~!~! 꼭 반드시 해야함
    public void setParameters(string _button, string _videoName, bool _isVideo, int _partCount, string _object_name)
    {
        //다른 파라미터 저장해주기
        select_button = _button;

        

        string[] videoSet = _videoName.Split(',');

        videoName = new string[videoSet.Length];

        for (int i = 0; i < videoSet.Length; i++)
        {
            videoName[i] = videoSet[i];
        }

        isVideo = _isVideo;

        partCount = _partCount;

        //object를 이름으로 찾아서 저장해주기

        interact_object = _object_name;
        
    }




    public object[] getParameters()
    {
        object[] parameters = new object[6];

        parameters[0] = this.myClientState;

        parameters[1] = select_button;

        parameters[2] = string.Join(",",videoName);

        parameters[3] = isVideo;

        parameters[4] = partCount;

        parameters[5] = interact_object;

        return parameters;
    }

    public override void Init()
    {
        base.Init();
        //For the test between the server and client~!~!~!~!~!~!~!~!~!~!~!~!
        if(select_button != "")
        {
            Debug.Log("I got the parameters...");
            Debug.Log("select_button: " + select_button);
            Debug.Log("videoName: " + videoName[1]);
            Debug.Log("isVideo: " + isVideo);
            Debug.Log("partCount: " + partCount);
            Debug.Log("Interacted Object: " + interact_object);
        }
        else
        {
            Debug.Log("No set parameters");
        }

    }

    public override void Process()
    {

        Debug.Log("object name is " + interact_object + " 끝내려면 x 버튼을 누르세여");
    }

    public override bool Goal()
    {
        if (Input.GetKey("x") == true)
            return true;
        return false;
    }

    

    public override void Res()
    {
        Debug.Log("개굳");
        base.Res();
    }


}
