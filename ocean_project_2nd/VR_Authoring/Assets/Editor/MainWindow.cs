using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainWindow : EditorWindow {

    public static MainWindow window;


    //private static Texture2D tex;
    private static Dictionary<string, Texture2D> tex_dict;

    public int tab = 0;


    private bool doRepaint = false;
    private List<DataObject> DataObjects = new List<DataObject>();

    [MenuItem ("Tools/marine_editor")]

    /* 윈도우 열릴 때 실행 되는 부분, resource 로딩은 여기에 밀어 넣는다. */
    public static void OpenWindow()
    {
        window = (MainWindow)EditorWindow.GetWindowWithRect(typeof(MainWindow), new Rect(0, 0, 1400, 900)); // create window
        window.title = "marine_editor"; // set a window title

        // load image to texture
        tex_dict = new Dictionary<string, Texture2D>();
        string resource_path = "Assets/Resources/";

        img_load(resource_path + "box_img.jpg", 50, 50, "box_img");
        img_load(resource_path + "kaist_logo.jpg", 200, 50, "kaist_logo");
        img_load(resource_path + "kriso_logo.jpg", 200, 50, "kriso_logo");
        img_load(resource_path + "title.png", 800, 50, "title");
        img_load(resource_path + "scenario_explain_example.png", 400, 800, "explain");
    }

    private static void img_load(string img_path, int width, int height, string name)
    {
        Texture2D tex;
        tex = new Texture2D(width, height);
        tex.LoadImage(System.IO.File.ReadAllBytes(img_path));
        tex.Apply();
        tex_dict.Add(name, tex);
    }

    // Update is called once per frame
	void Update () {
	    if (doRepaint)
        {
            Repaint();
        }
	}

    /* 추가하기에서 선택했을 때, Callback */
    void Callback(object obj)
    {
        string[] result = obj.ToString().Split('/');
        Debug.Log("tab: " + result[0] + " name: " + result[1]);

        string tab = result[0]; // ex) Task
        string name = result[1]; // ex) Fire Recognition

        DataObjects.Add(new DataObject(tab, name, new Vector2(200.0f + Random.Range(-10.0f, 10.0f), 200.0f + Random.Range(-10.0f, 10.0f))));
    }

    void OnGUI()
    {


        if (window == null)
            OpenWindow();

        ShowMenu(tab);

        //[TODO] 나중에 설명 이미지로 대체하는 부분. 탭에 따라 다르게.
        /* Texture */
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width-200, 500, 50, 50), tex_dict["box_img"]);
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width-250, 35, 200, 50), tex_dict["kaist_logo"]);
        EditorGUI.DrawPreviewTexture(new Rect(50, 35, 200, 50), tex_dict["kriso_logo"]);
        EditorGUI.DrawPreviewTexture(new Rect(300, 35, 800, 50), tex_dict["title"]);
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width - 400, 100, 400, 800), tex_dict["explain"]);
        /* tab */
        tab = GUILayout.Toolbar(tab, new string[] { "Scenario", "Task", "State" });
        Dictionary<int, string> tab_dict = new Dictionary<int, string>();
        tab_dict.Add(0, "Scenario");
        tab_dict.Add(1, "Task");
        tab_dict.Add(2, "State");


        Handles.BeginGUI();
        Handles.color = Color.white;

        // [TODO]
        /* 아래 부분 하드코딩한거 고쳐야 함.*/
        Handles.DrawLine(new Vector3(0, 100), new Vector3(Screen.width, 100));
        Handles.DrawLine(new Vector3(Screen.width-400, 100), new Vector3(Screen.width - 400, Screen.height));
        Handles.DrawLine(new Vector3(1, 100), new Vector3(1, Screen.height));
        Handles.DrawLine(new Vector3(Screen.width-1, 100), new Vector3(Screen.width-1, Screen.height));
        Handles.DrawLine(new Vector3(0, Screen.height-23), new Vector3(Screen.width, Screen.height-23));
        Handles.EndGUI();


        bool PreviousState;
        Color color;

        foreach(DataObject data in DataObjects)
        {
            if (data.get_tab() == tab_dict[tab])
            {
                PreviousState = data.Dragging;
                color = GUI.color;

                data.OnGUI();
                GUI.color = color;
                if (data.Dragging)
                {
                    doRepaint = true;
                }
            }
        }
        //Debug.Log(DrawLine.windows.Count);
        /*DrawLine TESTING*/
        // 이거는 임시 코드일 뿐이고, 제대로 작동할 수 있는 버전으로 고쳐야 함....
        if (DrawLine.windows.Count == 2)
        {
            DrawLine.connections.Add(new Rect_Pair(DrawLine.windows[0], DrawLine.windows[1]));
        }
        else if (DrawLine.windows.Count == 3)
        {
            DrawLine.connections.Add(new Rect_Pair(DrawLine.windows[0], DrawLine.windows[1]));
            DrawLine.connections.Add(new Rect_Pair(DrawLine.windows[1], DrawLine.windows[2]));
        }

        foreach (Rect_Pair connection in DrawLine.connections)
        {
            //Debug.Log(connection.start);
            DrawLine.DrawNodeCurve(connection.start, connection.end);
            DrawLine.DrawArrowHead(connection.start, connection.end, new Vector2(0.25f, 1.0f), new Vector2(0.0f, 0.5f), 50f, 30f, 10f);
        }
        DrawLine.windows.Clear();
        DrawLine.connections.Clear();
        /**/
        
        DataObjects.RemoveAll(data => data.remove == true);
    }

    /* 추가하기 누르면 열리는 메뉴창 */
    void ShowMenu(int tab)
    {
        Event currentEvent = Event.current;

        Rect contextRect = new Rect(1, 100, 125, 30);
        EditorGUI.DrawRect(contextRect, Color.green);
        GUI.Label(contextRect, "추가하기");
        
        if (currentEvent.type == EventType.ContextClick)
        {
            Vector2 mousePos = currentEvent.mousePosition;
            string result = "";

            if (contextRect.Contains(mousePos))
            {
                switch (tab)
                {
                    case 0: // Scenario
                        result = "Scenario/";
                        GenericMenu menu_0 = new GenericMenu();
                        menu_0.AddItem(new GUIContent("화재발생훈련"), false, Callback, result + "Fire/");
                        menu_0.AddItem(new GUIContent("선박침수훈련"), false, Callback, result + "Water/");
                        menu_0.AddItem(new GUIContent("선박대피훈련"), false, Callback, result + "Escape/");
                        menu_0.ShowAsContext();
                        currentEvent.Use();
                        break;
                    case 1: // Task
                        result = "Task/";
                        GenericMenu menu_1 = new GenericMenu();
                        menu_1.AddItem(new GUIContent("Task_0"), false, Callback, result + "0/");
                        menu_1.AddItem(new GUIContent("Task_1"), false, Callback, result + "1/");
                        menu_1.AddItem(new GUIContent("Task_2"), false, Callback, result + "2/");
                        menu_1.ShowAsContext();
                        currentEvent.Use();
                        break;
                    case 2: // State
                        result = "State/";
                        GenericMenu menu_2 = new GenericMenu();
                        menu_2.AddItem(new GUIContent("State_0"), false, Callback, result + "0/");
                        menu_2.AddItem(new GUIContent("State_1"), false, Callback, result + "1/");
                        menu_2.AddItem(new GUIContent("State_2"), false, Callback, result + "2/");
                        menu_2.ShowAsContext();
                        currentEvent.Use();
                        break;
                    default:
                        Debug.LogError("tab selection error!");
                        break;
                }
            }
        }
    }
}
