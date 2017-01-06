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
            
            int numOfProperty = propertyList.Count;
            int numOfObject = objectList.Count;
            int numOfNetworkPlayer = playerRoleList.Count;

            List<string> parameters  = new List<string>();

            parameters.Add(getProperty<string>("Select_Button"));
            parameters.Add(getProperty<string>("VideoName"));
            parameters.Add(getProperty<bool>("isVideo").ToString());
            parameters.Add(getProperty<int>("PartCount").ToString()); 
            parameters.Add(getObject<string>("FireExtinguisher_Segment"));

            
            for(int j=0; j<playerList.Length; j++)
            {
                parameters.Add(playerList[j]);
            }

            MessageProtocol mp = new MessageProtocol(MessageProtocol.MESSAGETYPE.CLIENTSTATE, parameters.Count, parameters.ToArray());

            _server.addAssignTable(playerName, mp);
        }
    }

}
