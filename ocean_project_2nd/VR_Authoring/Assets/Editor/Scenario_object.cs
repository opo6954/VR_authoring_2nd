using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class Fire_scene
{
    public int limit_time = 450; // 제한 시간
    public int level = 2; // 난이도 1, 2, 3
    public int fire_size = 2; // 화재 크기 1, 2, 3
    public int device = 2; // 상호작용기기 1, 2, 3
    public bool FireRecognition = true; // 화재 발견 태스크
    public bool FireReport = true; // 화재 보고 태스크
    public bool FireExtinguisher = true; // 소화기 태스크
    public bool gathering = true; // 집합 테스크
    public bool evacuation = false; // 승객 대피 테스크
}

class Water_scene
{
    public int limit_time = 450; // 제한 시간
    public int level = 2; // 난이도 1, 2, 3
    public int water_size = 2; // 침수 크기 1, 2, 3
    public int device = 2; // 상호작용기기 1, 2, 3
    public bool WaterRecognition = true; // 침수 발견 태스크
    public bool WaterReport = true; // 침수 보고 태스크
    public bool WaterEmergency = true; // 침수 응급 처치 태스크
    public bool gathering = true; // 집합 테스크
    public bool evacuation = false; // 승객 대피 테스크
}

class Escape_scene
{
    public int limit_time = 450; // 제한 시간
    public int level = 2; // 난이도 1, 2, 3
    public int ship_size = 2; // 선박 크기 1, 2, 3
    public int device = 2; // 상호작용기기 1, 2, 3
    public bool EscapeRecognition = true; // 대피상황 인식 태스크
    public bool EscapeReport = true; // 대피 보고 태스크
    public bool EscapeRouteRecognition = true; // 대피 경로 인식 태스크
    public bool gathering = true; // 집합 테스크
    public bool evacuation = false; // 승객 대피 테스크
}

public class Scenario_object : GUIDraggableObject {
    private string name; // Scenario name, ex. Fire
    public bool remove = false; // if true remove this

    public List<Task_object> task_list = new List<Task_object>();

    private Fire_scene fire_status;
    private Water_scene water_status;
    private Escape_scene escape_status;

    public Scenario_object(string name, Vector2 position) : base(position)
    {
        this.name = name;
        if (name == "Fire") fire_status = new Fire_scene();
        else if (name == "Water") water_status = new Water_scene();
        else if (name == "Escape") escape_status = new Escape_scene();
        else Debug.LogError("wrong scenario name!");
    }

    public string get_name()
    {
        return name;
    }


    /* xml 저장 할 때, 좀 더 편하라고 필요한 정보를 dict로 정리해서 건네주는 함수. */
    public Dictionary<string, string> get_xml_dict()
    {
        Dictionary<string, string> xml_dict = new Dictionary<string, string>();
        if (name == "Fire")
        {
            xml_dict.Add("difficulty", fire_status.level.ToString());
            xml_dict.Add("Time", fire_status.limit_time.ToString());
        }
        else if (name == "Water")
        {
            xml_dict.Add("notadded", "notadded");
        }
        else if (name == "Escape")
        {
            xml_dict.Add("notadded", "notadded");
        }
        else Debug.LogError("wrong scenario name error!");

        return xml_dict;
    }

    public void OnGUI()
    {
        /* EditorGUILayout, GUILayout 클래스를 참조.. */
        /* 시나리오 종류에 따라서 다른 정보를 표시. */

        /* 화재 시나리오 */
        if (name == "Fire")
        {
            Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
            /* DrawLine is from node editor concept */
            DrawLine.windows.Add(drawRect);

            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

            GUILayout.Label("화재 발생 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

            dragRect = GUILayoutUtility.GetLastRect();
            dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

            GUILayout.Label("화재발생훈련 편집창 입니다.");
            fire_status.limit_time = EditorGUILayout.IntField("제한시간(초)", fire_status.limit_time, GUILayout.ExpandWidth(true));
            fire_status.level = EditorGUILayout.IntPopup("난이도", fire_status.level, new string[]{ "상", "중", "하" }, new int[]{ 1, 2, 3}, GUILayout.ExpandWidth(true));
            fire_status.fire_size = EditorGUILayout.IntPopup("화재 발생 정도", fire_status.fire_size, new string[]{"대형", "중형", "소형"}, new int[]{ 1, 2, 3}, GUILayout.ExpandWidth(true));
            fire_status.device = EditorGUILayout.IntPopup("상호작용기기 선택", fire_status.device, new string[]{"조이스틱", "립모션", "vive" }, new int[]{ 1, 2, 3}, GUILayout.ExpandWidth(true));
            GUILayout.Label("Task 포함시키기");
            fire_status.FireRecognition = EditorGUILayout.Toggle("화재 발견", fire_status.FireRecognition, GUILayout.ExpandWidth(true));
            fire_status.FireReport = EditorGUILayout.Toggle("화재 보고", fire_status.FireReport, GUILayout.ExpandWidth(true));
            fire_status.FireExtinguisher = EditorGUILayout.Toggle("소화기 동작", fire_status.FireExtinguisher, GUILayout.ExpandWidth(true));
            fire_status.gathering = EditorGUILayout.Toggle("집합", fire_status.gathering, GUILayout.ExpandWidth(true));
            fire_status.evacuation = EditorGUILayout.Toggle("승객 대피", fire_status.evacuation, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("삭제"))
            {
                remove = true;
                Debug.Log(name + "is removed.");
            }

            GUILayout.EndArea();

            Drag(dragRect);
        }


        else if (name == "Water")
        {
            Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;
            /* DrawLine is from node editor concept */
            DrawLine.windows.Add(drawRect);

            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

            GUILayout.Label("선박 침수 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

            dragRect = GUILayoutUtility.GetLastRect();
            dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

            GUILayout.Label("선박침수훈련 편집창 입니다.");
            water_status.limit_time = EditorGUILayout.IntField("제한시간(초)", water_status.limit_time, GUILayout.ExpandWidth(true));
            water_status.level = EditorGUILayout.IntPopup("난이도", water_status.level, new string[] { "상", "중", "하" }, new int[] { 1, 2, 3 }, GUILayout.ExpandWidth(true));
            water_status.water_size = EditorGUILayout.IntPopup("화재 발생 정도", water_status.water_size, new string[] { "대형", "중형", "소형" }, new int[] { 1, 2, 3 }, GUILayout.ExpandWidth(true));
            water_status.device = EditorGUILayout.IntPopup("상호작용기기 선택", water_status.device, new string[] { "조이스틱", "립모션", "vive" }, new int[] { 1, 2, 3 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("Task 포함시키기");
            water_status.WaterRecognition = EditorGUILayout.Toggle("선박 침수 발견", water_status.WaterRecognition, GUILayout.ExpandWidth(true));
            water_status.WaterReport = EditorGUILayout.Toggle("침수 보고", water_status.WaterReport, GUILayout.ExpandWidth(true));
            water_status.WaterEmergency = EditorGUILayout.Toggle("소화기 동작", water_status.WaterEmergency, GUILayout.ExpandWidth(true));
            water_status.gathering = EditorGUILayout.Toggle("집합", water_status.gathering, GUILayout.ExpandWidth(true));
            water_status.evacuation = EditorGUILayout.Toggle("승객 대피", water_status.evacuation, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("삭제"))
            {
                remove = true;
                Debug.Log(name + "is removed.");
            }

            GUILayout.EndArea();

            Drag(dragRect);
        }


        else if (name == "Escape")
        {
            Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;   
            DrawLine.windows.Add(drawRect);

            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

            GUILayout.Label("선박 대피 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

            dragRect = GUILayoutUtility.GetLastRect();
            dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

            GUILayout.Label("선박대피훈련 편집창 입니다.");
            escape_status.limit_time = EditorGUILayout.IntField("제한시간(초)", escape_status.limit_time, GUILayout.ExpandWidth(true));
            escape_status.level = EditorGUILayout.IntPopup("난이도", escape_status.level, new string[] {"상", "중", "하" }, new int[] {1, 2, 3}, GUILayout.ExpandWidth(true));
            escape_status.ship_size = EditorGUILayout.IntPopup("선박 크기 설정", escape_status.ship_size, new string[] { "소형", "중형", "대형" }, new int[] { 1, 2, 3 }, GUILayout.ExpandWidth(true));
            escape_status.device = EditorGUILayout.IntPopup("상호작용기기 선택", escape_status.device, new string[] { "조이스틱", "립모션", "vive" }, new int[] { 1, 2, 3 }, GUILayout.ExpandWidth(true));
            GUILayout.Label("Task 포함시키기");
            escape_status.EscapeRecognition = EditorGUILayout.Toggle("대피 상황 인식", escape_status.EscapeRecognition, GUILayout.ExpandWidth(true));
            escape_status.EscapeReport = EditorGUILayout.Toggle("대피 보고", escape_status.EscapeReport, GUILayout.ExpandWidth(true));
            escape_status.EscapeRouteRecognition = EditorGUILayout.Toggle("대피 경로 인식", escape_status.EscapeRouteRecognition, GUILayout.ExpandWidth(true));
            escape_status.gathering = EditorGUILayout.Toggle("집합", escape_status.gathering, GUILayout.ExpandWidth(true));
            escape_status.evacuation = EditorGUILayout.Toggle("승객 대피", escape_status.evacuation, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("삭제"))
            {
                remove = true;
                Debug.Log(name + "is removed.");
            }

            GUILayout.EndArea();

            Drag(dragRect);
        }


        else
        {
            Debug.LogError("wrong tab format at scenario object.");
        }
    }
}
