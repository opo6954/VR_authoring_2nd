using UnityEngine;
using UnityEditor;
using System.Collections;

//general한 VR setting해주기ex) Virtual Reality Supported값

public class VRProperty : MonoBehaviour {
    public static void enableVR()
    {
        //VR support를 일단 false로 해주기
        if (PlayerSettings.virtualRealitySupported == false)
        {
            Debug.Log("Steam VR settings...");
            PlayerSettings.virtualRealitySupported = true;
        }
    }

}
