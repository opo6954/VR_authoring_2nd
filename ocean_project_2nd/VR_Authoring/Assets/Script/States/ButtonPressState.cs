using UnityEngine;
using System.Collections;
/*
 * 멈춰있는 상태에서 버튼을 누를 경우 완료
 * 필요한 Property:
 * 1. TaskName: 화재를 발견했습니다.
 * 2. ButtonInfo: X버튼을 눌러 다음 task를 수행하세요
 * 
 * 필요한 Object:
 * 필요없음
 * 
 * */
public class ButtonPressState : StateModuleTemplate {

    GameObject backgroundUI;

	string button = "";


	public ButtonPressState(TaskModuleTemplate _myModule, GameObject _UI) : base(_myModule, _UI)
	{
		
	}
	 

	public override void setProperty (System.Collections.Generic.Dictionary<string, object> properties)
	{
		addProperty("Notice_Contents", properties["Notice_Contents"]);
		addProperty("Guide_Contents", properties["Guide_Contents"]);
		addProperty ("Select_Button_Info", properties ["Select_Button_Info"]);

		button = getProperty<string> ("Select_Button_Info");

	}

	public override void setObject (System.Collections.Generic.Dictionary<string, object> objects)
	{
		base.setObject (objects);
	}

    public override void Init()
    {
		myStateName = "화면이 멈춘 상태에서 특정 키 누르기";

        base.Init();

        

        if (isContainProperty("Notice_Contents") == false || isContainProperty("Guide_Contents") == false)
        {
            Debug.Log("Notice_Contents Property와 Guide_Contents Property가 설정되지 않았습니다.");
        }
        else
        {
            myUIInfo.GetComponent<DefaultForm>().changeCurrTaskInfo(getProperty<string>("Notice_Contents"));
            myUIInfo.GetComponent<DefaultForm>().toggleShownCurrTaskInfo(true);



            backgroundUI = myModuleInfo.getBackgroundUI();

            backgroundUI.GetComponent<BackgroundForm>().changeButtonInfo(getProperty<string>("Guide_Contents"));
            
            


            //lock the screen

            lockFPSScreen(true);


        }
    }

    public override void Process()
    {
        base.Process();
        backgroundUI.GetComponent<BackgroundForm>().toggleShownObject(BackgroundForm.BGPart.BG_BUTTONINFO, true);
    }

    public override bool Goal()
    {
        if (isKeyDown(button) == true)
        {
            return true;
        }

        return base.Goal();
    }

    public override void Res()
    {
        base.Res();
        lockFPSScreen(false);//unlock the screen
        backgroundUI.GetComponent<BackgroundForm>().toggleShownObject(BackgroundForm.BGPart.BG_BUTTONINFO, false);
        myUIInfo.GetComponent<DefaultForm>().toggleShownCurrTaskInfo(false);
        
    }

}
