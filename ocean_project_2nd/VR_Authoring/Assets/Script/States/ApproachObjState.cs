using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * 특정 obj와의 거리가 특정 값 미만일 경우 종료
 * 
 * 필요한 property:
 * 1. TaskName: 선박 내부를 순찰하세요
 * 필요한 object:
 * 1. Object: "WakeUpObject"
 
 * */

public class ApproachObjState : StateModuleTemplate {

	public ApproachObjState(TaskModuleTemplate _myModule, GameObject _UI) : base(_myModule, _UI)
	{
		myStateName = "특정 Obj에 다가가기";
	}


	public override void setProperty (Dictionary<string, object> properties)
	{
		addProperty ("Patrol_Contents", properties ["Patrol_Contents"]);

		if(properties.ContainsKey("Approach_Distance"))
			addProperty ("Approach_Distance", properties ["Approach_Distance"]);

		if (properties.ContainsKey ("Approach_Angle"))
			addProperty ("Approach_Angle", properties ["Approach_Angle"]);
	}

	public override void setObject (Dictionary<string, object> objects)
	{
		addObject ("Approach_to_Object", objects ["Approach_to_Object"]);
	}


    public override void Init()
    {
		

        base.Init();

        
        
        if (isContainProperty("Patrol_Contents") == false || isContainObject("Approach_to_Object") == false)
        {
            Debug.Log("Patrol_Contents Property와 Approach_to_Object Object가 설정되지 않았습니다.");
        }
        else
        {
            myUIInfo.GetComponent<DefaultForm>().changeCurrTaskInfo(getProperty<string>("Patrol_Contents"));
            
        }

    }

    public override void Process()
    {
        base.Process();
    }

    public override bool Goal()
    {
        if (isContainObject("Approach_to_Object") == true)
        {
			if (amISeeObject(getObject<GameObject>("Approach_to_Object"), getProperty<float>("Approach_Angle"), getProperty<float>("Approach_Distance")) == true)
                return true;
        }
        else
        {
            Debug.Log("그런거 없시유");
        }
         
        return base.Goal();
    }

    public override void Res()
    {

        base.Res();
    }


}
