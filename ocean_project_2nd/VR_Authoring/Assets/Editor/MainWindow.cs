using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class MainWindow : EditorWindow
{

    public static MainWindow window;


    //private static Texture2D tex;
    private static Dictionary<string, Texture2D> tex_dict;

    private int tab = 0, tab2 = 0, tab3_1 = 0, tab3_2 = 0;
    private int selected_scene_in_task_tab = 0;
    public string xml_path = "test.xml";

    private bool doRepaint = false;
    private List<Scenario_object> scenario_list = new List<Scenario_object>();
    //private List<Task_object> task_list = new List<Task_object>();

    // [TODO] state는 그냥 task에 정보를 포함 시켜 버리는 쪽으로 생각 해보자.
    private List<State_object> state_list = new List<State_object>();

    [MenuItem("Tools/marine_editor")]

    /* 윈도우 열릴 때 실행 되는 부분, 모든 resource 로딩은 여기에 밀어 넣는다. */
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
    void Update()
    {
        if (doRepaint)
        {
            Repaint();
        }
    }

    /* 추가하기에서 선택했을 때, Callback */
    void Callback(object obj)
    {
        // result tab/task(scenario)(state)/information(name)(ex. Fire/Task1/State2)
        string[] result = obj.ToString().Split('#');
        Debug.Log("tab: " + result[0] + " name: " + result[1]);

        string tab = result[0]; // ex) Task
        string name = result[1]; // ex) Fire Recognition

        if (tab == "Scenario")
        {
            scenario_list.Add(new Scenario_object(name, new Vector2(200.0f + Random.Range(-10.0f, 10.0f), 200.0f + Random.Range(-10.0f, 10.0f))));
        }
        else if (tab == "Task")
        {
            scenario_list[tab2].task_list.Add(new Task_object(name, new Vector2(200.0f + Random.Range(-10.0f, 10.0f), 200.0f + Random.Range(-10.0f, 10.0f))));
        }
        else if (tab == "State")
        {
            Debug.Log("State");
        }
        else
        {
            Debug.Log("wrong tab");
        }
    }

    void OnGUI()
    {


        if (window == null)
            OpenWindow();


        ShowMenu(tab);


        /* 기본 배경 */
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width - 200, 500, 50, 50), tex_dict["box_img"]);
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width - 250, 35, 200, 50), tex_dict["kaist_logo"]);
        EditorGUI.DrawPreviewTexture(new Rect(50, 35, 200, 50), tex_dict["kriso_logo"]);
        EditorGUI.DrawPreviewTexture(new Rect(300, 35, 800, 50), tex_dict["title"]);
        /**/

        /* 오른쪽 설명 이미지 */
        //[TODO] 나중에 설명 이미지로 대체하는 부분. "탭에 따라 다르게."
        EditorGUI.DrawPreviewTexture(new Rect(Screen.width - 400, 100, 400, 800), tex_dict["explain"]);
        /**/

        /* tab */
        tab = GUILayout.Toolbar(tab, new string[] { "Scenario", "Task", "State" });
        Dictionary<int, string> tab_dict = new Dictionary<int, string>();
        tab_dict.Add(0, "Scenario");
        tab_dict.Add(1, "Task");
        tab_dict.Add(2, "State");

        /***********************************/
        // 하얀색 구분선
        // [TODO]
        /* 아래 부분 하드코딩한거 고쳐야 함. 윈도우 크기를 키우고 줄여도 잘 되게 해야함.*/
        Handles.BeginGUI();
        Handles.color = Color.white;
        Handles.DrawLine(new Vector3(0, 100), new Vector3(Screen.width, 100));
        Handles.DrawLine(new Vector3(Screen.width - 400, 100), new Vector3(Screen.width - 400, Screen.height));
        Handles.DrawLine(new Vector3(1, 100), new Vector3(1, Screen.height));
        Handles.DrawLine(new Vector3(Screen.width - 1, 100), new Vector3(Screen.width - 1, Screen.height));
        Handles.DrawLine(new Vector3(0, Screen.height - 23), new Vector3(Screen.width, Screen.height - 23));
        Handles.EndGUI();
        /*************************************/

        /* xml로 저장하기 버튼 */
        if (GUI.Button(new Rect(Screen.width - 500, Screen.height - 75, 100, 50), new GUIContent("저장하기", "이거슨 저장을 위한 버튼입니다.")))
        {
            string save_path = EditorUtility.SaveFilePanel("Save xml", "", "settings" + ".xml", "xml");
            save_xml(xml_path = save_path);
        }
        /* xml 불러오기 버튼 */
        if (GUI.Button(new Rect(Screen.width - 600, Screen.height - 75, 100, 50), new GUIContent("불러오기", "이거슨 불러오기를 위한 버튼입니다.")))
        {
            load_xml();
        }

    }

    /* 탭에 따라 다른 메뉴를 보여 준다. */
    void ShowMenu(int tab)
    {
        bool PreviousState;
        Color color;

        switch (tab)
        {

            case 0: // Scenario
                Event currentEvent = Event.current;
                Rect contextRect = new Rect(1, 100, 125, 30);
                EditorGUI.DrawRect(contextRect, Color.green);
                GUI.Label(contextRect, "시나리오 추가하기");
                /* 추가하기에 마우스 올리고 클릭 하면 */
                if (currentEvent.type == EventType.ContextClick)
                {
                    Vector2 mousePos = currentEvent.mousePosition;
                    string result = "";
                    if (contextRect.Contains(mousePos))
                    {
                        result = "Scenario#";
                        GenericMenu menu_0 = new GenericMenu();
                        menu_0.AddItem(new GUIContent("화재발생훈련"), false, Callback, result + "Fire");
                        menu_0.AddItem(new GUIContent("선박침수훈련"), false, Callback, result + "Water");
                        menu_0.AddItem(new GUIContent("선박대피훈련"), false, Callback, result + "Escape");
                        menu_0.ShowAsContext();
                        currentEvent.Use();
                    }
                }

                /* 시나리오 리스트 GUI에 보여주고 드래깅 시키기 */
                /* 시나리오 리스트 순서가 화살표 순서랑 같음. */
                foreach (Scenario_object data in scenario_list)
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
                DrawLine.Draw();
                /* 삭제하기 눌러진 애들은 리스트에서 삭제 */
                scenario_list.RemoveAll(data => data.remove == true);
                break;


            case 1: // Task
                /* 앞에서 정해진 시나리오 중 한 개를 선택 하는 메뉴 */

                if (scenario_list.Count <= 0) break;

                List<string> scenario_name_list = new List<string>();
                foreach (Scenario_object data in scenario_list)
                {
                    scenario_name_list.Add(data.get_name());
                }
                
                tab2 = GUI.Toolbar(new Rect(1, 100, Screen.width-400, 30), tab2, scenario_name_list.ToArray());
                if (tab2 >= scenario_list.Count) tab2 = 0;

                List<Task_object> task_list = scenario_list[tab2].task_list;

                Event currentEvent_task = Event.current;
                Rect contextRect_task = new Rect(1, 130, 125, 30);
                EditorGUI.DrawRect(contextRect_task, Color.green);
                GUI.Label(contextRect_task, "태스크 추가하기");
                /* 추가하기에 마우스 올리고 클릭 하면 */
                if (currentEvent_task.type == EventType.ContextClick)
                {
                    Vector2 mousePos = currentEvent_task.mousePosition;
                    string result = "Task#" + scenario_name_list[tab2] + "/";
                    if (contextRect_task.Contains(mousePos))
                    {
                        GenericMenu menu_1 = new GenericMenu();
                        /* 현재 선택중인 시나리오에 따라 다른 태스크 보여줌 */
                        if (scenario_name_list[tab2] == "Fire")
                        {
                            menu_1.AddItem(new GUIContent("화재 인식 태스크"), false, Callback, result + "FireNotice");
                            menu_1.AddItem(new GUIContent("화재 보고 태스크"), false, Callback, result + "FireReport");
                            menu_1.AddItem(new GUIContent("화재 경보 태스크"), false, Callback, result + "FireAlarm");
                            menu_1.AddItem(new GUIContent("화재 진화 태스크"), false, Callback, result + "FireMethod");
                            menu_1.ShowAsContext();
                        }
                        else
                        {
                            menu_1.AddItem(new GUIContent("TEST"), false, Callback, result + "TEST/");
                            menu_1.ShowAsContext();
                        }

                        currentEvent_task.Use();
                    }
                }

                /* 시나리오에 해당되는 태스크 리스트 GUI에 보여주고 드래깅 시키기 */
                /* 태스크 리스트 순서가 화살표 순서랑 같음. */
                /* 현재 selec중인 시나리오의 태스크만 gui에 보여줌 */
                foreach (Task_object data in task_list)
                {
                    PreviousState = data.Dragging;
                    color = GUI.color;

                    data.OnGUI(scenario_name_list[tab2]);
                    GUI.color = color;
                    if (data.Dragging)
                    {
                        doRepaint = true;
                    }
                }
                DrawLine.Draw();
                /* 삭제하기 눌러진 애들은 리스트에서 삭제 */
                task_list.RemoveAll(data => data.remove == true);
                break;




            case 2: // State
                /* 앞에서 정해진 시나리오+태스크 중 한 개를 선택 하는 메뉴 */
                if (scenario_list.Count <= 0) break;

                List<string> scenario_name_list_ = new List<string>();
                foreach (Scenario_object data in scenario_list)
                {
                    scenario_name_list_.Add(data.get_name());
                }

                tab3_1 = GUI.Toolbar(new Rect(1, 100, Screen.width-400, 30), tab3_1, scenario_name_list_.ToArray());
                if (tab3_1 >= scenario_list.Count) tab3_1 = 0;

                List<Task_object> task_list_ = scenario_list[tab3_1].task_list;
                if (task_list_.Count <= 0) break;
                List<string> task_name_list_ = new List<string>();
                foreach (Task_object data in task_list_)
                {
                    task_name_list_.Add(data.get_task());
                }

                tab3_2 = GUI.Toolbar(new Rect(1, 130, Screen.width - 400, 30), tab3_2, task_name_list_.ToArray());

                task_list_[tab3_2].state_object.OnGUI();


                break;



            default:
                Debug.LogError("tab selection error!");
                break;
        }
    }

    void save_xml(string xml_path="./test.xml")
    {
        Debug.Log("save xml");
        XmlDocument doc = new XmlDocument(); // Document 객체 인스턴스

        XmlElement root = doc.CreateElement("Root"); // 루트 설정
        doc.AppendChild(root);
        // [TODO] 시나리오와 태스크 스테이트 등의 변수명들이 다 다르고 경우에 따라서 달라지니까, 그냥 필요한 애들 달라고 하면 dict에 넣어서 주는 함수를 만들어야 겠다.
        // 시나리오
        foreach (Scenario_object scenario in scenario_list)
        {
            Dictionary<string, string> xml_dict = scenario.get_xml_dict();
            XmlElement scene_xml = doc.CreateElement("Scenario");

            foreach (KeyValuePair<string, string> items in xml_dict)
            {
                XmlAttribute sceneAttr = doc.CreateAttribute(items.Key);
                sceneAttr.Value = items.Value;
                scene_xml.Attributes.Append(sceneAttr);
            }

            root.AppendChild(scene_xml);

            // 태스크
            foreach (Task_object task in scenario.task_list)
            {
                xml_dict = task.get_xml_dict();

                XmlElement task_xml = doc.CreateElement("Task");

                foreach (KeyValuePair<string, string> items in xml_dict)
                {
                    XmlAttribute taskAttr = doc.CreateAttribute(items.Key);
                    taskAttr.Value = items.Value;
                    task_xml.Attributes.Append(taskAttr);
                }

                scene_xml.AppendChild(task_xml);

                // 스테이트
                foreach (State state in task.state_object.state_list)
                {
                    // State name 넣기
                    XmlElement state_xml = doc.CreateElement("State");
                    XmlAttribute stateAttr = doc.CreateAttribute("name");
                    stateAttr.Value = state.name;
                    state_xml.Attributes.Append(stateAttr);
                    task_xml.AppendChild(state_xml);

                    // property 넣기
                    XmlElement Properties = doc.CreateElement("Properties");
                    foreach (KeyValuePair<string, string> items in state.Properties)
                    {
                        XmlAttribute PropertiesAttr = doc.CreateAttribute(items.Key);
                        PropertiesAttr.Value = items.Value;
                        Properties.Attributes.Append(PropertiesAttr);
                        state_xml.AppendChild(Properties);
                    }
                    // object 넣기
                    XmlElement Objects = doc.CreateElement("Objects");
                    foreach (KeyValuePair<string, string> items in state.Objects)
                    {
                        XmlAttribute ObjectsAttr = doc.CreateAttribute(items.Key);
                        ObjectsAttr.Value = items.Value;
                        Objects.Attributes.Append(ObjectsAttr);
                        state_xml.AppendChild(Objects);
                    }
                }
            }
        }

        using (XmlTextWriter writer = new XmlTextWriter(xml_path, Encoding.UTF8))
        {
            writer.Formatting = Formatting.Indented;
            doc.Save(writer);
        }
    }

    void load_xml()
    {
        initialize();
        // 파일 불러오기 창 띄우기
        xml_path = EditorUtility.OpenFilePanel("load xml file", Application.streamingAssetsPath, "xml");
        Debug.Log(xml_path);
        // xml 파싱

        XmlDocument doc = new XmlDocument();
        doc.Load(xml_path);
        XmlElement root = doc.DocumentElement;

        // 노드 요소들
        XmlNodeList scenarios = root.ChildNodes; // 시나리오 리스트

        // 시나리오 추가
        foreach(XmlNode scenario in scenarios)
        {
            Scenario_object scenario_object = new Scenario_object(scenario, new Vector2(200.0f + Random.Range(-10.0f, 10.0f), 200.0f + Random.Range(-10.0f, 10.0f)));
            scenario_list.Add(scenario_object);
            string scenario_name = scenario.Attributes["name"].Value;
            // 태스크 추가
            XmlNodeList tasks = scenario.ChildNodes;
            foreach(XmlNode task in tasks)
            {
                Task_object task_object = new Task_object(task, scenario_name, new Vector2(200.0f + Random.Range(-10.0f, 10.0f), 200.0f + Random.Range(-10.0f, 10.0f)));
                scenario_object.task_list.Add(task_object);
                // 스테이트 추가
                State_object state_object = new State_object(task);
                task_object.state_object = state_object;
            }
        }

    }
    
    // 시나리오, 태스크, 스테이트 등의 정보를 초기화 시키는 함수.
    // 로드 xml 하기 전에 사용한다.
    void initialize()
    {
        scenario_list.Clear();
    }
}
