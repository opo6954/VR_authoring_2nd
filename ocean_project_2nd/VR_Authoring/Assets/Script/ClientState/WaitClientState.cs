using UnityEngine;
using System.Collections;
/*
 * 다른 player나 server의 응답을 기다리는 wait ClientState임
 * 걍 아무것도 안함
 * 걍 UI상으로 대기하라고 표시는 해줘야 할 듯
 * */
public class WaitClientState : ClientStateModuleTemplate {







    public override void Init()
    {
        base.Init();
    }

    public override void Process()
    {
        base.Process();
    }

    public override bool Goal()
    {
        return base.Goal();
    }

    public override void Res()
    {
        base.Res();
    }
}
