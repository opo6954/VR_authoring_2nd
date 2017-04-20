using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerLogger{

    public static ServerLogger myInstance = null;

    public static GameObject canvas=null;
    public static Text contents=null;


    public static ServerLogger Instance()
    {
        if(myInstance == null)
        {
            myInstance = new ServerLogger();
            
            canvas = GameObject.FindGameObjectWithTag("Server_Canvas");
            contents = canvas.transform.GetChild(0).transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>();
        }

        return myInstance;
    }

    public ServerLogger()
    {
    }


    public void addText(string text)
    {
        Debug.Log(text);
        contents.text = contents.text + "\n" + text;

        

        

    }








	
}
