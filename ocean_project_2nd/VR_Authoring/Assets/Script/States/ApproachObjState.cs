using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * 
 * 순찰하는 Network State임
 * 화재 발견 task에서 주로 쓰임
 * 
 * 특정 obj와의 거리가 특정 값 미만일 경우 종료
 * 
 * 필요한 property:
 * 1. Patrol_Contents: mission UI에 보일 문구
 * 2. Approach_Angle: 접근 각도(optional)
 * 3. Approach_Distance: 접근 거리(optional)
 * 필요한 object:
 * 1. Approach_to_Object: 접근할 object의 이름임
 
 * */

public class ApproachObjState : StateModuleTemplate {

	public ApproachObjState()
	{
		MyStateName = "ApproachObjState";

        addPropertyList("Patrol_Contents");
        addObjectList("Approach_to_Object");
        addPropertyList("Approach_Angle");
        addPropertyList("Approach_Distance");
        addNetworkPlayerList("First_Extinguisher");
	}

    //state 초기화부분의 override
    public override void initState()
    {
        base.initState();
    }

    public override void buildClientStateTable()
    {
        /*
        List<string> playersRole = getNetworkPlayerList();//필요한 role이 모두 들어 있음

        for (int i = 0; i < playersRole.Count; i++)
        {
            //이 부분에서 list에 넣을 때 순찰에 대응하는 순찰ClientState버전을 넣어야 함 일단 임시로 ClientStateExample을 넣자
            ClientStateExample example = new ClientStateExample();
            example.setParameter1("x");
            example.setParameter2(142);

            _server.addAssignTable(playerName,example);
            
            //assignTable을 만들었음
        }
         * */
    }

    

    
    
}
