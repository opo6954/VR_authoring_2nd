using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputDeviceSettings {

    private static InputDeviceSettings myInstance = null;

    public Dictionary<string, string> joystickMappingTable = new Dictionary<string, string>();

    public static InputDeviceSettings Instance()
    {
        if (myInstance == null)
        {
            myInstance = new InputDeviceSettings();
        }

        return myInstance;
    }

    public InputDeviceSettings()
    {
    }

    public void mappingJoystickButton()
    {
        joystickMappingTable.Add("a", "joystick button 0");
        joystickMappingTable.Add("z", "joystick button 1");
        joystickMappingTable.Add("x", "joystick button 2");
        joystickMappingTable.Add("b", "joystick button 3");
        joystickMappingTable.Add("lb", "joystick button 4");
        joystickMappingTable.Add("rb", "joystick button 5");
        joystickMappingTable.Add("lt", "joystick button 6");
        joystickMappingTable.Add("rt", "joystick button 7");
    }

}
