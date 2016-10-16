using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
class FireNotice
{
    public int limit_time = 450;
}
class FireReport
{
    public int limit_time = 450;
}
class FireAlarm
{
    public int limit_time = 450;
}
class FireMethod
{
    public int limit_time = 450;
}


public class Task_object : GUIDraggableObject {

    private string task_name;
    private string scenario;
    public bool remove = false;

    public State_object state_object;

    private FireNotice fire_notice;
    private FireReport fire_report;
    private FireAlarm fire_alarm;
    private FireMethod fire_method;

    public Task_object(string name, Vector2 position) : base(position)
    {
        string[] name_split = name.Split('/');
        scenario = name_split[0];
        task_name = name_split[1];

        state_object = new State_object(task_name);

        if (scenario == "Fire")
        {
            if (task_name == "FireNotice") fire_notice = new FireNotice();
            else if (task_name == "FireReport") fire_report = new FireReport();
            else if (task_name == "FireAlarm") fire_alarm = new FireAlarm();
            else if (task_name == "FireMethod") fire_method = new FireMethod();
            else Debug.LogError("wrong task name!");
        }

        else Debug.LogError("wrong scenario name!");

    }

    public Task_object(XmlNode task, string scenario ,Vector2 position) : base(position)
    {
        this.scenario = scenario;
        task_name = task.Attributes["name"].Value;

        state_object = new State_object(task_name);

        if (scenario == "Fire")
        {
            if (task_name == "FireNotice")
            {
                fire_notice = new FireNotice();
            }
            else if (task_name == "FireReport")
            {
                fire_report = new FireReport();
            }
            else if (task_name == "FireAlarm")
            {
                fire_alarm = new FireAlarm();
            }
            else if (task_name == "FireMethod")
            {
                fire_method = new FireMethod();
            }
            else Debug.LogError("wrong task_name!");
        }
        else Debug.LogError("wrong scenario name!");
    }
    
    public Dictionary<string, string> get_xml_dict()
    {
        Dictionary<string, string> xml_dict = new Dictionary<string, string>();
        if (scenario == "Fire")
        {
            if (task_name == "FireNotice")
            {
                xml_dict.Add("name", task_name);
                xml_dict.Add("type", task_name);
            }
            else if (task_name == "FireReport")
            {
                xml_dict.Add("name", task_name);
                xml_dict.Add("type", task_name);
            }
            else if (task_name == "FireAlarm")
            {
                xml_dict.Add("name", task_name);
                xml_dict.Add("type", task_name);
            }
            else if (task_name == "FireMethod")
            {
                xml_dict.Add("name", task_name);
                xml_dict.Add("type", task_name);
            }
            else Debug.LogError("wrong task name!");
        }
        else Debug.LogError("scenario name wrong!");

        return xml_dict;
    }
    

    public string get_scenario()
    {
        return scenario;
    }

    public string get_task()
    {
        return task_name;
    }

    // selected_scenario는 현재 보고 있는 tab을 알려주기 위한 역할
    public void OnGUI(string selected_scenario)
    {
        if (scenario == "Fire" && selected_scenario == "Fire")
        {
            if (task_name == "FireNotice")
            {
                Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
                /* DrawLine is from node editor concept */
                DrawLine.windows.Add(drawRect);

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("화재 인식 태스크", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);


                if (GUILayout.Button("삭제"))
                {
                    remove = true;
                    Debug.Log(task_name + "is removed.");
                }

                GUILayout.EndArea();

                Drag(dragRect);

            }


            else if (task_name == "FireReport")
            {
                Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
                /* DrawLine is from node editor concept */
                DrawLine.windows.Add(drawRect);

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("화재 보고 태스크", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);


                if (GUILayout.Button("삭제"))
                {
                    remove = true;
                    Debug.Log(task_name + "is removed.");
                }

                GUILayout.EndArea();

                Drag(dragRect);

            }


            else if (task_name == "FireAlarm")
            {
                Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
                /* DrawLine is from node editor concept */
                DrawLine.windows.Add(drawRect);

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("화재 경보 태스크", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);


                if (GUILayout.Button("삭제"))
                {
                    remove = true;
                    Debug.Log(task_name + "is removed.");
                }

                GUILayout.EndArea();

                Drag(dragRect);

            }


            else if (task_name == "FireMethod")
            {
                Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
                /* DrawLine is from node editor concept */
                DrawLine.windows.Add(drawRect);

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("화재 진화 태스크", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);


                if (GUILayout.Button("삭제"))
                {
                    remove = true;
                    Debug.Log(task_name + "is removed.");
                }

                GUILayout.EndArea();

                Drag(dragRect);

            }
            else Debug.LogError("wrong task name!");
        }
        else Debug.LogError("wrong scenario name!");

    }
}
