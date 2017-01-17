using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadioTalkClientState : ClientStateModuleTemplate
{
    private string temp_initiate_button = "t";
    private int ans = 0;
    private bool is_end = false;
    private bool is_talking = false;

    // 질문-답변 저장용
    public Dictionary<string, string> question = new Dictionary<string, string>();
    public Dictionary<string, string[]> answers = new Dictionary<string, string[]>();


    public RadioTalkClientState()
    {
        //이름 정의
        myClientState = "RadioTalkClientState";

        // FOR TEST(AT REAL READ FROM XML)!!
        /*
        question.Add("qna_test", "선장님, 선원 A입니다. 현재 화재가 발생했습니다.");
        answers.Add("qna_test", new string[]{ "이협우\n박철웅\n박광빈\n정진기", "박철웅", "박광빈", "정진기"});
         * */
    }
    public void setInfo(string overMsg, string _question, string[] _answer)
    {

        question.Add("qna_test", _question);
        question.Add("You_msg", overMsg);
        answers.Add("qna_test", _answer);

    }


    private int SelectAnswer() // 사용자가 어떤 answer를 선택했는지 체킹하는 함수.(일단 임시로 해놓음. 향후 시선처리 등 return값으로 대체)
    {
        if (PlayerTemplate.isKeyDown("1")) return 1;
        else if (PlayerTemplate.isKeyDown("2")) return 2;
        else if (PlayerTemplate.isKeyDown("3")) return 3;
        else if (PlayerTemplate.isKeyDown("4")) return 4;
        else return 0;
    }

    public void startTalk(string key) // 보여줄 질문과 답을 key로 골라서 보여줌.
    {

        UnityEngine.UI.Text question_txt = GameObject.Find("Canvas/question").GetComponent<UnityEngine.UI.Text>();



        if (question["You_msg"] != "")
        {
            question_txt.text = question["You_msg"] + "\n";
            question_txt.text = question_txt.text + question[key];
        }
        else
            question_txt.text = question[key];
        
        







        UnityEngine.UI.Text answers_txt = GameObject.Find("Canvas/calling_list").GetComponent<UnityEngine.UI.Text>();
        answers_txt.text = answers[key][0];
        PlayerTemplate.getRadio().SetActive(true);
        is_talking = true;
    }

    public void showMessage(string msg) // 상대방이 보낸 message를 보여줌.
    {
        UnityEngine.UI.Text question_txt = GameObject.Find("Canvas/question").GetComponent<UnityEngine.UI.Text>();
        question_txt.text = msg;
        PlayerTemplate.getRadio().SetActive(true);
        is_talking = true;

    }


    public void endTalk()
    {
        UnityEngine.UI.Text question_txt = GameObject.Find("Canvas/question").GetComponent<UnityEngine.UI.Text>();
        question_txt.text = "";
        UnityEngine.UI.Text answers_txt = GameObject.Find("Canvas/calling_list").GetComponent<UnityEngine.UI.Text>();
        answers_txt.text = "";
        PlayerTemplate.getRadio().SetActive(false);
        is_talking = false;
    }

    public override void Init()
    {
        base.Init();
        PlayerTemplate.myCanvas.transform.FindChild("Background").gameObject.SetActive(true);
    }

    public override void Process()
    {
        startTalk("qna_test");
       
        if (is_talking)
        {
            ans = SelectAnswer();
            if (ans > 0)
            {
                endTalk();
                is_end = true;
            }
        }
    }

    public override bool Goal()
    {
        return is_end;
    }

    public override void Res()
    {
        PlayerTemplate.myCanvas.transform.FindChild("Background").gameObject.SetActive(false);
        PlayerTemplate.myCanvas.transform.FindChild("question").GetComponent<UnityEngine.UI.Text>().text = "";

        base.Res();
    }
}
