using UnityEngine;
using UnityEditor;
using System.Collections;

// This class just has the capability of being draggaed in GUI - it could be any type of generic data class
public class DataObject : GUIDraggableObject {
    private string tab; // Task
    private string name; // FireRecognition
    public bool remove = false; // if true remove this

    public DataObject (string tab_, string name_, Vector2 position) : base(position)
    {
        tab = tab_;
        name = name_;
    }

    public string get_tab()
    {
        return tab;
    }

    public string get_name()
    {
        return name;
    }

    public void OnGUI()
    {
        /* tab과 name에 따라서 다른 정보를 표시해야 한다. */
        /* EditorGUILayout 클래스, GUILayout클래스를 참조 할 것.(다양한 함수들이 있음.) */

        if (tab == "Scenario")
        {
            if (name == "Fire")
            {
                Rect drawRect = new Rect(m_Position.x, m_Position.y, 250.0f, 300.0f), dragRect;

                /* TESTING */
                DrawLine.windows.Add(drawRect);
                /**/    
                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("화재 발생 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

                GUILayout.Label("화재발생훈련 편집창 입니다.");
                EditorGUILayout.IntField("제한시간(초)", 300, GUILayout.ExpandWidth(true));
                int[] option_values = { 1, 2, 3 };
                string[] option_names = { "상", "중", "하" };
                EditorGUILayout.IntPopup("난이도", 2, option_names, option_values, GUILayout.ExpandWidth(true));
                int[] option_values2 = { 1, 2, 3 };
                string[] option_names2 = { "대형", "중형", "소형" };
                EditorGUILayout.IntPopup("화재 발생 정도", 2, option_names2, option_values2, GUILayout.ExpandWidth(true));
                int[] option_values3 = { 1, 2, 3 };
                string[] option_names3 = { "조이스틱", "립모션", "vive" };
                EditorGUILayout.IntPopup("상호작용기기 선택", 2, option_names3, option_values3, GUILayout.ExpandWidth(true));
                GUILayout.Label("Task 포함시키기");
                EditorGUILayout.Toggle("화재 발견", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("화재 보고", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("소화기 동작", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("집합", false, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("승객 대피", false, GUILayout.ExpandWidth(true));

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

                /*TESTING*/
                DrawLine.windows.Add(drawRect);
                /**/

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("선박 침수 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

                GUILayout.Label("선박침수훈련 편집창 입니다.");
                EditorGUILayout.IntField("제한시간(초)", 300, GUILayout.ExpandWidth(true));
                int[] option_values = { 1, 2, 3 };
                string[] option_names = { "상", "중", "하" };
                EditorGUILayout.IntPopup("난이도", 2, option_names, option_values, GUILayout.ExpandWidth(true));
                int[] option_values2 = { 1, 2, 3 };
                string[] option_names2 = { "대형", "중형", "소형" };
                EditorGUILayout.IntPopup("침수 발생 정도", 2, option_names2, option_values2, GUILayout.ExpandWidth(true));
                int[] option_values3 = { 1, 2, 3 };
                string[] option_names3 = { "조이스틱", "립모션", "vive" };
                EditorGUILayout.IntPopup("상호작용기기 선택", 2, option_names3, option_values3, GUILayout.ExpandWidth(true));
                GUILayout.Label("Task 포함시키기");
                EditorGUILayout.Toggle("침수 발견", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("침수 보고", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("침수 응급 처치", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("집합", false, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("승객 대피", false, GUILayout.ExpandWidth(true));

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

                /*TESTING*/
                DrawLine.windows.Add(drawRect);
                /**/

                GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

                GUILayout.Label("선박 대피 훈련", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

                dragRect = GUILayoutUtility.GetLastRect();
                dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

                GUILayout.Label("선박대피훈련 편집창 입니다.");
                EditorGUILayout.IntField("제한시간(초)", 300, GUILayout.ExpandWidth(true));
                int[] option_values = { 1, 2, 3 };
                string[] option_names = { "상", "중", "하" };
                EditorGUILayout.IntPopup("난이도", 2, option_names, option_values, GUILayout.ExpandWidth(true));
                int[] option_values2 = { 1, 2, 3 };
                string[] option_names2 = { "대형", "중형", "소형" };
                EditorGUILayout.IntPopup("선박 크기 설정", 2, option_names2, option_values2, GUILayout.ExpandWidth(true));
                int[] option_values3 = { 1, 2, 3 };
                string[] option_names3 = { "조이스틱", "립모션", "vive" };
                EditorGUILayout.IntPopup("상호작용기기 선택", 2, option_names3, option_values3, GUILayout.ExpandWidth(true));
                GUILayout.Label("Task 포함시키기");
                EditorGUILayout.Toggle("대피 상황 인식", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("대피 보고", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("대피 경로 인식", true, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("집합", false, GUILayout.ExpandWidth(true));
                EditorGUILayout.Toggle("승객 대피", false, GUILayout.ExpandWidth(true));

                if (GUILayout.Button("삭제"))
                {
                    remove = true;
                    Debug.Log(name + "is removed.");
                }

                GUILayout.EndArea();

                Drag(dragRect);
            }
        }

        else if (tab == "Task")
        {
            Rect drawRect = new Rect(m_Position.x, m_Position.y, 150.0f, 200.0f), dragRect;

            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

            GUILayout.Label(tab + name, GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

            dragRect = GUILayoutUtility.GetLastRect();
            dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

            GUILayout.Label("태스크다아아");

            if (GUILayout.Button("삭제"))
            {
                remove = true;
                Debug.Log(name + "is removed.");
            }

            GUILayout.EndArea();

            Drag(dragRect);
        }

        else if (tab == "State")
        {
            Rect drawRect = new Rect(m_Position.x, m_Position.y, 150.0f, 200.0f), dragRect;

            GUILayout.BeginArea(drawRect, GUI.skin.GetStyle("Box"));

            GUILayout.Label(tab + name, GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

            dragRect = GUILayoutUtility.GetLastRect();
            dragRect = new Rect(dragRect.x + m_Position.x, dragRect.y + m_Position.y, dragRect.width, dragRect.height);

            GUILayout.Label("스테이트다아아");

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
            Debug.LogError("wrong tab format!");
        }
    }

}
