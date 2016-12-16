using UnityEngine;
using System.Collections;
/*
 * 무전기 걸기 clientState
 * Init: 그냥 무전기 버튼만 표시해주기
 * Process: client로부터의 
 * Goal: 특정 버튼을 누를 때까지
 * Res: Raio animation 실행(혹은 radio가 화면 한 가운데에 나옴)
 * 
 * 필요한 정보
 * 무전기 동작 버튼
 * 
 * */

 
public class SenderTurnRadiosClientState : ClientStateModuleTemplate {

    private string radioButton;

    public SenderTurnRadiosClientState()
    {
        //이름 정의
        myClientState = "TurnRadiosClientState";

    }

    public void setRadioTurnButton(string _button)
    {
        radioButton = _button;
    }

    public override void Init()
    {
        /*
         * 무전기를 걸기 직전의 모습을 보여주기, 아마 ui 등으로 무전기 부분에 화살표 표시?
         * */
    }

    public override void Process()
    {
        base.Process();
    }

    public override bool Goal()
    {
        return PlayerTemplate.isKeyDown(radioButton);
    }

    public override void Res()
    {
        /*
         * 무전기를 받았다는 표시를 하자 무전기를 화면 정중앙에 띄우면서 종료 혹은 무전기 state라고 정의하자
         * */
    }

}
