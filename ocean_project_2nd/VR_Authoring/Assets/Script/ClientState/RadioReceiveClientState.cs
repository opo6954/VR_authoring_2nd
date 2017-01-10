using UnityEngine;
using System.Collections;

public class RadioReceiveClientState : ClientStateModuleTemplate {
    private string radio_button = "r";
    private bool is_ringing = false;
    private bool is_end = false;

    public RadioReceiveClientState()
    {
        //이름 정의
        myClientState = "RadioReceiveClientState";
    }

    public void ringRadio() // 무전기 울리게 하기
    {
        is_ringing = true;
        // 무전기 울리는 알림
        UnityEngine.UI.Text alarm_txt = GameObject.Find("Canvas/Alarm_txt").GetComponent<UnityEngine.UI.Text>();
        alarm_txt.text = "무전기가 울립니다!!!!!";
        PlayerTemplate.getAlarmImg().enabled = true;
    }

    public void pickRadio() // 무전기 받기
    {
        PlayerTemplate.getRadio().SetActive(true);
        UnityEngine.UI.Text alarm_txt = GameObject.Find("Canvas/Alarm_txt").GetComponent<UnityEngine.UI.Text>();
        alarm_txt.text = "";
        PlayerTemplate.getAlarmImg().enabled = false;
        is_ringing = false;
        is_end = true;
    }

    public void setRadioTurnButton(string _button)
    {
        radio_button = _button;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Process()
    {
        if (PlayerTemplate.isKeyDown(radio_button) && is_ringing) // 무전기 울려서 받은 상황 
        {
            pickRadio();
        }
        else if (PlayerTemplate.isKeyDown(radio_button) && !is_ringing) // 무전기 울리게 하기
        {
            ringRadio();
        }
    }

    public override bool Goal()
    {
        return is_end;
    }

    public override void Res()
    {
        base.Res();
    }

}
