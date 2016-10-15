using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class string_pair
{
    public string key;
    public string value;

    public string_pair(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
}

public class State
{
    public string name;
    public Dictionary<string, string> Properties;
    public Dictionary<string, string> Objects;

    public State(string name, List<string_pair> prop_list, List<string_pair> obj_list)
    {
        this.name = name;
        Properties = new Dictionary<string, string>();
        Objects = new Dictionary<string, string>();
        foreach (string_pair data in prop_list)
        {
            Properties.Add(data.key, data.value);
        }
        foreach (string_pair data in obj_list)
        {
            Objects.Add(data.key, data.value);
        }
    }

    public void OnGUI()
    {
        GUILayout.Label(name);
        // Properties edit
        List<string> keys = new List<string> (Properties.Keys); // foreach dict iteration 도중엔 dict의 값을 바꿀 수 없다.
        foreach (string key in keys)
        {
            Properties[key] = EditorGUILayout.TextField(new GUIContent(key), Properties[key], GUILayout.ExpandWidth(true));
        }
        // Objects edit
        keys = new List<string>(Objects.Keys);
        foreach (string key in keys)
        {
            Objects[key] = EditorGUILayout.TextField(new GUIContent(key), Objects[key], GUILayout.ExpandWidth(true));
        }
    }
}

// state_object는 하나에 여러가지 state를 list로 자기 자신이 들고 있게 한다.
// task_name은 state_object가 가지고 각각 list의 elem이 state name, dict를 들고 있게 한다.
public class State_object : EditorWindow {

    public string task_name;
    public List<State> state_list = new List<State>();

    public State_object(string task_name)
    {
        this.task_name = task_name;
        string state_name;
        List<string> key_list, value_list;
        List<string_pair> prop_list, obj_list;

        if (task_name == "FireNotice")
        {
            // state name
            state_name = "ApproachObjState";
            // Property
            key_list = new List<string> { "Patrol_Contents" };
            value_list = new List<string> { "선박 내부를 순찰하세요" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Approach_to_Object" };
            value_list = new List<string> { "Wake" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "ButtonPressState";
            // Property
            key_list = new List<string> { "Notice_Contents", "Guide_Contents", "Select_Button_Info" };
            value_list = new List<string> { "화재를 발견했습니다.", "X버튼: 다음 task 진행", "x" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { };
            value_list = new List<string> { };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

        }
        else if (task_name == "FireReport")
        {
            // state name
            state_name = "ApproachObjState";
            // Property
            key_list = new List<string> { "Patrol_Contents" };
            value_list = new List<string> { "선박 전화기를 통해 화재 내용을 보고하세요" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Approach_to_Object" };
            value_list = new List<string> { "ShipPhone" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "QuestioningState";
            // Property
            key_list = new List<string> { "Report_Count", "Report_Question", "Report_Answer", "Report_TrueAns", "Select_Button_Info", "Move_Button_Info" };
            value_list = new List<string> { "3", "화재 장소,화재 종류,화재 크기", "기관실,선장실,객실A,객실B /,일반화재,유류화재,전기화재 /,대형,중형,소형 /", "0,2,1", "x", "z" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> {  };
            value_list = new List<string> {  };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));
        }
        else if (task_name == "FireAlarm")
        {
            // state name
            state_name = "ApproachObjState";
            // Property
            key_list = new List<string> { "Patrol_Contents" };
            value_list = new List<string> { "화재 경보기를 찾으세요" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Approach_to_Object" };
            value_list = new List<string> { "FireAlarm" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "ButtonPressState";
            // Property
            key_list = new List<string> { "Notice_Contents", "Guide_Contents", "Select_Button_Info" };
            value_list = new List<string> { "화재 경보기를 발견했습니다.", "x버튼: 화재 경보기 동작", "x" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> {  };
            value_list = new List<string> {  };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "PlaySoundsState";
            // Property
            key_list = new List<string> { "SoundName", "Loop" };
            value_list = new List<string> { "Whistling", "True" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Sound_from_Object" };
            value_list = new List<string> { "FireAlarm" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));
        }
        else if (task_name == "FireMethod")
        {
            // state name
            state_name = "ApproachObjState";
            // Property
            key_list = new List<string> { "Patrol_Contents", "Approach_Distance", "Approach_Angle" };
            value_list = new List<string> { "초기 진화를 위한 소화기를 찾으세요", "3", "3" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Approach_to_Object" };
            value_list = new List<string> { "FireExtinguisher_Segment" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "ButtonPressState";
            // Property
            key_list = new List<string> { "Notice_Contents", "Guide_Contents", "Select_Button_Info" };
            value_list = new List<string> { "소화기를 동작하세요", "X버튼: 소화기 동작 미션 시작", "x" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> {  };
            value_list = new List<string> {  };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));

            // state name
            state_name = "MethodLearnState";
            // Property
            key_list = new List<string> { "PartCount", "isVideo", "VideoName" ,"PartAnswer", "Select_Button_Info", "Skip_Button_Info" };
            value_list = new List<string> { "4", "True", "4,2,3,1", "3,1,2,0", "x", "z" };
            prop_list = make_string_pair_list(key_list, value_list);
            // Objects
            key_list = new List<string> { "Interaction_to_Object" };
            value_list = new List<string> { "FireExtinguisher_Segment" };
            obj_list = make_string_pair_list(key_list, value_list);
            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));
        }
        else Debug.LogError("task name error at state!");

    }

    public State_object(XmlNode task)
    {
        task_name = task.Attributes["name"].Value;
        string state_name;
        List<string> key_list = new List<string> { };
        List<string> value_list = new List<string> { };
        List<string_pair> prop_list = new List<string_pair> { };
        List<string_pair> obj_list = new List<string_pair> { };

        XmlNodeList states = task.ChildNodes;
       
        foreach (XmlNode state in states)
        {
            /*
            <State name="ApproachObjState">
                <Properties Patrol_Contents="초기 진화를 위한 소화기를 찾으세요" Approach_Distance="3" Approach_Angle="3" />
                <Objects Approach_to_Object="FireExtinguisher_Segment" />
            </State>

            위의 형식을 parsing해서 state 클래스로 만들어서 집어넣쟈앙.
            */
            state_name = state.Attributes["name"].Value;

            // properties나 objects가 존재하지 않는 경우도 존재함.
            foreach (XmlNode prop_or_obj in state.ChildNodes)
            {
                foreach (XmlAttribute attr in prop_or_obj.Attributes)
                {
                    key_list.Clear();
                    value_list.Clear();
                    key_list.Add(attr.LocalName.ToString());
                    value_list.Add(attr.Value.ToString());
                }
                if (prop_or_obj.Name.ToString() == "Properties")
                {
                    prop_list = make_string_pair_list(key_list, value_list);
                }
                else if (prop_or_obj.Name.ToString() == "Objects")
                {
                    obj_list = make_string_pair_list(key_list, value_list);
                }
                else Debug.LogError("attribute not exist(State)");
            }

            // add to state list
            state_list.Add(new State(state_name, prop_list, obj_list));
        }

    }

    private List<string_pair> make_string_pair_list(List<string> keys, List<string> values)
    {
        Debug.Assert(keys.Count == values.Count, "string pair length not match!");
        List<string_pair> ret_list = new List<string_pair>();
        for (int i=0; i < keys.Count; i++)
        {
            ret_list.Add(new string_pair(keys[i], values[i]));
        }
        return ret_list;
    }


    public void OnGUI()
    {
        Rect rect = new Rect(1, 160, Screen.width - 400, Screen.height - 100);
        GUILayout.BeginArea(rect, GUI.skin.GetStyle("Box"));
        GUILayout.Label("State 설정 창", GUI.skin.GetStyle("Box"), GUILayout.ExpandWidth(true));

        foreach (State state in state_list)
        {
            state.OnGUI();
        }

        GUILayout.EndArea();
    }

}
