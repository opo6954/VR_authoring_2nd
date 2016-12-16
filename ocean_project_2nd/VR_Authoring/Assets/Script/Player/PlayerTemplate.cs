using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
 * client의 player 고유 설정 정보
 * 입력 장치 설정 등이 포함됨
 * */
public class PlayerTemplate : MonoBehaviour
{

    //가상 현실 장비 관련 옵션
    public static bool isJoystick = false;
    public static bool isLeapMotion = false;

    public float myWalkSpeed = 0.0f;

    //player 관련 util 함수

    //버튼 입력 관련 함수
    public static bool isKeyDown(string keyName)
    {
        bool isKeyPressed = false;

        if (isJoystick == true)
        {
            isKeyPressed = Input.GetKeyDown(InputDeviceSettings.Instance().joystickMappingTable[keyName]);
        }
        else
        {
            isKeyPressed = Input.GetKeyDown(keyName);
        }

        return isKeyPressed;
    }

    
}