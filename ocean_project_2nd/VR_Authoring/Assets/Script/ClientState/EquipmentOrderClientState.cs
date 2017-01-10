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


    public override void setParameters()
    {

        //property 설정
        select_button = propertyGroup[propertyList[0]];
        videoName = propertyGroup[propertyList[1]].Split(',');
        isVideo = bool.Parse(propertyGroup[propertyList[2]]);
        partCount = int.Parse(propertyGroup[propertyList[3]]);

        //object 설정
        interact_object = objectGroup[objectList[0]];

    }
 

    public override void Init()
    {
        base.Init();

        propertyList.Add("Select_Button");
        propertyList.Add("VideoName");
        propertyList.Add("isVideo");
        propertyList.Add("PartCount");
        objectList.Add("FireExtinguisher_Segment");

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

    //clientstate의 결과물을 message protocol로 변환하는 작업 필요
    public override void convertRes2Msg()
    {
        //완료 메시지 보내는 거임
        MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.CLIENTSTATE, 3, new string[] {true.ToString(), propertyList[0],select_button});
        _cm.passClientStateInfo(mp);
    }

}
