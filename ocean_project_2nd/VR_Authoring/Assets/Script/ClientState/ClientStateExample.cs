using UnityEngine;
using System.Collections;

/*
 * 모든 Client 관련된 일은 ClientManager에서 담당
 * 
 * Client가 할 일은 ClientState를 바탕으로 ClientStateController에서 담당
 * 
 * ClientState를 통해서 client의 동작 담당
 * 
 * ClientManager 파일의 Start에서 작성된 clientState의 동작을 확인할 수 있음
 * 
 * 
 * ClientState 예시
 * 
 * 크게 4가지로 구성
 * Init: 처음 clientState가 불릴 때 초기화 1번 불림
 * Process: clientState가 실행되는 동안 계속 불림
 * Goal: clientState의 종료 조건, true를 반환하면 종료
 * Res: clientState가 종료될 시 1번 불림
 * 
 * ClientState내의 변수는 다양하게 쓰되 저작도구의 state 내용을 반영할 수 있도록 open시키기
 * Player외의 다른 object와의 상호작용이 존재할 경우 그 object에 추가 script를 넣고 그 script를 통해서 상호작용 하는걸로 하기
 * (ex 형상물 켜기나 환풍기 켜기의 경우 clientState의 역할은 기기 동작으로 통일 가능하고 특정 장소에 가서 버튼을 눌러서 진행하는 것으로 일반화 가능
 * --> 버튼 누르면 Res()에서 형상물 object의 script를 가져와서 turn하는 형태, 
 * --> 버튼 누르면 Res()에서 환풍기 object의 script를 가져와서 turn하는 형태
 * 
 *  
 * 
 * 구현할 clientState:
 * 1. 순찰: 특정 물체 혹은 장소와의 거리가 가까워지면 성공
 * 포함되는 task: 화재 발견, 초기 진압 소화기나 고정식 소화 설비를 찾기
 * 
 * 2. 장비 동작a: 특정 장비에 다가가서 특정 버튼을 누르면 장비 동작(혹은 비동작)하면서 성공
 * 포함되는 task: 형상물 켜기(형상물 내부에 light 켜기), 환풍기 끄기(환풍기 버튼 색깔 바꾸기), 비상대응알림 켜기(소리 재생) 등
 * 
 * 3. 무전기 걸기: 자리에서 무전기 버튼을 누르고 누구에게 연락을 할지 선택하면 성공 --> 무전기 asset으로 그럴듯하게 가운데에 배치하는 게 좋을듯 -->일단 디자인은 추후에 생각하고 내용만 넣자
 * 
 * 4. 무전기 받기: 알람이 오고(화면 가운데에 알람 그림 표시) 이후 무전기 버튼을 누르면 성공
 * 
 * 5. 무전기 정보 교환: 다른 사람으로부터 무전기 연락이 온 내용을 보여주는 상태에서 내가 보낼 정보를 선택하면 성공
 * 
 * 6. 대화 시작: 선원 가까이에서 대화 버튼을 누르면 대화 시작되면서 종료
 * 
 * 7. 대화 정보 교환: 상대로부터 대화 내용을 보여주는 상태에서 내가 보낼 정보를 선택하면 성공
 * 
 * 8. 소화기 순서대로 동작: 특정 오브젝트의 부분별로 순서대로 동작시키는 훈련을 위한 state, 소화기 동작 순서(소화핀 등)을 훈련할 때 사용
 * 포함되는 task: 초기 진화용 소화기, 고정식 소화설비(이 부분은 결정이 안되었으니 대충 그럴듯하게만 만들기)
 * 
 * 9. 소화기 분사: 소화기를 이용하여 화재에 분사하기
 * 기존처럼 좌우로 소화기를 움직이면서 흩뿌리면서 화재에 소화 
 * 
 * 
 * 
 * 
 
 * 
*/


public class ClientStateExample : ClientStateModuleTemplate {

    public string parameter1;
    public int parameter2;

    public void setParameter1(string a)
    {
        parameter1 = a;
    }

    public void setParameter2(int b)
    {
        parameter2 = b;
    }


    //초기 clientState 시작 후 init (처음 한 번만 불림)
    public override void Init()
    {
    

        base.Init();
    }
    //ClientState가 시작한 후 계속 불림
    public override void Process()
    {
        Debug.Log(parameter2);
        Debug.Log("종료하려면 " + parameter1 + " key를 누르세여\n");

        base.Process();
    }
    //ClientState의 종료조건, 계속 조건을 확인해서 Goal이 true를 리턴하면 종료
    public override bool Goal()
    {
        if (Input.GetKey(parameter1))
            return true;
        return false;
    }
    //마지막 ClientState가 종료된 후 1번 불림
    public override void Res()
    {
        base.Res();
    }

    

	
}
