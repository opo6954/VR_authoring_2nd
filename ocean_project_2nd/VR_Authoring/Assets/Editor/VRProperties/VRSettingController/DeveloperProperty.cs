using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

//HMD 안쓰는 경우에 대한 환경 설정
//VIrtual reality supported 값을 false로 해주는 게 대표적

public class DeveloperProperty{

    public static void disableVR()
    {
        //VR support를 일단 false로 해주기
        if (PlayerSettings.virtualRealitySupported == true)
        {
            Debug.Log("No VR settings exist...");
            PlayerSettings.virtualRealitySupported = false;
        }
    }
}
