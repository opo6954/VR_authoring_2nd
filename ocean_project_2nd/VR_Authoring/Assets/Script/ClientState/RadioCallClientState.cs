using UnityEngine;
using System.Collections;

public class RadioCallClientState : ClientStateModuleTemplate
{
    private string radio_button = "c";
    private bool is_calling = false; // 현재 무전기 사용중인지 아닌지 상태.
    private bool is_end = false;
    private int ans = 0;

    public string[] calling_list = { "선장", "소화반장", "일항사", "지원반장" };
    public string calling_list_txt = "1.선장\n2.소화반장\n3.일항사\n4.지원반장\n"; //TEST EXAMPLE

    public RadioCallClientState()
    {
        // 이름 정의
        myClientState = "RadioCallClientState";
    }

    public void setRadioTrunButton(string _button)
    {
        radio_button = _button;
    }

    private void CallEnd() // 무전기 끝날떄 호출되는 함수
    {
        Debug.Log("call end");
        UnityEngine.UI.Text calling_list_txt = GameObject.Find("Canvas/calling_list").GetComponent<UnityEngine.UI.Text>();
        calling_list_txt.text = "";
        is_calling = false;
        PlayerTemplate.getRadio().SetActive(false);
    }

    private int SelectAnswer() // 사용자가 어떤 answer를 선택했는지 체킹하는 함수.(일단 임시로 해놓음. 향후 시선처리 등 return값으로 대체)
    {
        if (PlayerTemplate.isKeyDown("1")) return 1;
        else if (PlayerTemplate.isKeyDown("2")) return 2;
        else if (PlayerTemplate.isKeyDown("3")) return 3;
        else if (PlayerTemplate.isKeyDown("4")) return 4;
        else return 0;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Process()
    {
        if (PlayerTemplate.isKeyDown(radio_button) && !is_calling) // 무전기 사용하기
        {
            Debug.Log("call start");
            is_calling = true;
            PlayerTemplate.getRadio().SetActive(true);

            PlayerTemplate.myCanvas.transform.FindChild("Background").gameObject.SetActive(true);
            PlayerTemplate.myCanvas.transform.FindChild("question").gameObject.SetActive(true);
            

        }
        else if(PlayerTemplate.isKeyDown(radio_button) && is_calling) // 무전기 끄기
        {
            CallEnd();
        }
        
        if (is_calling) // 무전기 사용중일땐, 누구에게 연락할지 보여준다.
        {
            //Debug.Log("calling");
            UnityEngine.UI.Text calling_list_txt = GameObject.Find("Canvas/calling_list").GetComponent<UnityEngine.UI.Text>();
            calling_list_txt.text = this.calling_list_txt;

            ans = SelectAnswer();
            if (ans > 0)
            {
                CallEnd();
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
        base.Res();

        PlayerTemplate.myCanvas.transform.FindChild("question").GetComponent<UnityEngine.UI.Text>().text = "";


        Debug.Log(calling_list[ans]);
    }
}
