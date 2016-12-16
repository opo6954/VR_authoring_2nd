using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * 무전기 관리하는 state로써 server에서 읽음...
 * 모든 player의 대사가 순차적으로 포함되어야 할 듯
 * 
 * 일단 무전기 동작 버튼 누르고 
 * 
 * 
 * 
 * 필요한 NetworkPlayer:
 * 차례대로 Sender, Receiver순서대로 하자
 * Sender, Receiver
 * 
 * ##Choice# type에 대한 설명
 * Choice#: #만큼의 걸친 선택 후 단어를 합쳐서 문장으로 만든다.
 * Contents의 문법: #1에서 #3 #2발생했습니다./화재 발생 장소/선장실,객실A, 객실B/화재 종류/일반화재, 유류화재, 전기화재/화재크기/소형,중형,대형
 * #에 차례대로 선택한 문항이 합쳐진다. 예를 들어 (((객실A에서 중형 일반화재 발생했습니다.)))
 * 
 * 
 * 필요한 Property:
 * 1. Select_Button_Info: string, <"X">, 무전기 동작 버튼
 * 2. Select_Button_Guide_Contents: string, <"X버튼을 눌러 무전기를 동작하세요">, 무전기 동작 관련 설명
 * 3. Radio_Communication_Number: int, <"3">, 무전기 통신 보고 횟수
 * 4. Radio_Communication_Sender_Type: string[], <"Normal","Choice3", "Normal">, Message의 type 관련(Normal: 일방적인 보고, Choice#: 객관식으로 #번에 걸친 선택 후 합치기)
 * 5. Radio_Communication_Receiver_Type: string[], <"Normal","Normal","Normal">, Message의 type 관련(Normal: 일방적인 보고, Choice: 객관식으로 선택 보고)
 * 6. Radio_Communication_Sender_Contents: string[], <"감도 있습니까", "#에서 # #발생했습니다./화재 발생 장소/선장실,객실A, 객실B/화재 종류/일반화재, 유류화재, 전기화재/화재크기/소형,중형,대형", "초기 진화 시도하겠습니다."
 * 7. Radio_Communication_Receiver_Contents: string[], <"감도 있습니다. 응답하세요","주변 휴대식 소화기로 초기진화 하시오","화재 진압 후 연락바랍니다.">
 * 
 * 
 * 필요한 Object:
 * 없음
 * 
 * 필요한 NetworkPlayer:
 * 1. Radio_Sender: string, "Extinguisher", 무전기 거는 사람의 player 이름
 * 2. Radio_Receiver: string, "Manager", 무전기 받는 사람의 player 이름
 * */
public class RadiosP2PState : StateModuleTemplate {

	public RadiosP2PState()
	{
		myStateName = "RadiosP2PState";
	}
    

}

