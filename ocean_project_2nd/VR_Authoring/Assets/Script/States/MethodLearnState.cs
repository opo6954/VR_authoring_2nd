using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * method state, 3D 물체의 각 부분을 인식하고 관련 영상 재생하기
 * 3D object를 확인하고 선택하는 효과는 일단 바꿀 수 없게 하는데 바꾸는 게 저작도구에는 넣어야 하지 않을까?
 * 
 * 필요한 property
 * Select_Button: "x"
 * VideoName: "video4, video2, video3, video1"
 * isVideo: True
 * PartCount: 4
 * 
 * 필요한 object
 * Interaction_to_Object: "FireExtinguisher_Segment"
 * 
 * 
 * */
//method 각 부분 시야로 인식하는 거 좀 향상이 필요할듯 그리고 정답 판정을 넓혀야 할듯


public class MethodLearnState : StateModuleTemplate {
    
    public MethodLearnState()
	{
		MyStateName = "MethodLearnState";  

        addPropertyList("Select_Button");
        addPropertyList("VideoName");
        addPropertyList("isVideo");
        addPropertyList("PartCount");

        addObjectList("FireExtinguisher_Segment");
        addNetworkPlayerList("First_Extinguisher");

        //현 state에 필요한 clientState의 이름을 list형태로 저장하기
        clientStateList.Add("EuipmentOrderClientState");

        //network에서의 role도 역시 파라미터로 넘겨주는 형태로 만들자
        
	}

    public override void initState()
    {
        _server.setCurrState(this);
        base.initState();
    }

    public override void updateClientStateTable()
    {
        //_server.client

        //이런 형식으로 mp에서의 파라미터를 setParameterValue로 바꿔주면 됩니다
        //_server.clientStateAssignTable[getNetworkPlayerList()[0]][_server.currTableIdx].setParameterValue(0, _server.clientTrainInfo[getNetworkPlayerList()[0]]["myAns"].ToString());

        base.updateClientStateTable();
    }

    public override void buildClientStateTable()
    {
        List<string> playersRole = getNetworkPlayerList();

        for (int i = 0; i < playersRole.Count; i++)
        {
            string playerName = playersRole[i];

            Debug.Log("In building Client StateTable, role name is " + playerName);

            string[] playerList = getNetworkPlayerList().ToArray();
            

            MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.CLIENTSTATE, clientStateList[0], propertyList, propertyGroup, objectList, objectGroup);

            _server.addAssignTable(playerName, mp);
        }
    }

}
