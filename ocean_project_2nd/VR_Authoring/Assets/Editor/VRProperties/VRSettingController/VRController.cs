using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class VRController {
    static string currentSceneName = "";

    

    static VRController()
    {
        EditorApplication.hierarchyWindowChanged += VRModeChanged;
    }

    static void VRModeChanged()
    {
        currentSceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;

        //VR mode setting이 없는 개발자 버전
        if (currentSceneName.Contains("NO_VR") == true)
        {
            
            DeveloperProperty.disableVR();
        }
        else if (currentSceneName.Contains("VR") == true)//VR mode가 포함된 데모 버전
        {
            
            VRProperty.enableVR();
            SteamVR_Settings svrs = new SteamVR_Settings();
            
        }

        
    }    
}
