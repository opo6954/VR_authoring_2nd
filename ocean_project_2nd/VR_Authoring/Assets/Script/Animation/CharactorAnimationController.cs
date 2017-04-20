using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterAnimationState
{
    IDLE, WALK, TALKING, BUTTON, PHONE
}

public static class CharacterAniState
{


    public static Dictionary<CharacterAnimationState, string> stateMapping;

    static CharacterAniState()
    {
        stateMapping = new Dictionary<CharacterAnimationState, string>() { {CharacterAnimationState.IDLE, "Idle"},{CharacterAnimationState.WALK, "Walking"},{CharacterAnimationState.TALKING, "Talking"},{CharacterAnimationState.BUTTON, "Button"},{CharacterAnimationState.PHONE, "BeginPhone"} };
    }
}

public class CharactorAnimationController : MonoBehaviour {
   

    CharacterAnimationState myState;
    public Animator myAnimator;
    public GameObject ownRadio;
    public RPCController rpcController;
    public ClientManager clientManager;
    

	// Use this for initialization
	void Start () {
        
        if (ownRadio != null)
        {
            ownRadio.SetActive(false);
        }
	}
	
	// Update is called once per frame

    public void resetAnimation()
    {
        if (myState != CharacterAnimationState.IDLE)
        {
            myState = CharacterAnimationState.IDLE;
            myAnimator.SetTrigger(CharacterAniState.stateMapping[CharacterAnimationState.IDLE]);
            myAnimator.ResetTrigger(CharacterAniState.stateMapping[CharacterAnimationState.TALKING]);
            myAnimator.ResetTrigger(CharacterAniState.stateMapping[CharacterAnimationState.BUTTON]);
            myAnimator.ResetTrigger(CharacterAniState.stateMapping[CharacterAnimationState.PHONE]);
            ownRadio.SetActive(false);
        }
    }

    public void triggerAnimation(CharacterAnimationState action)
    {
        if (myState == CharacterAnimationState.PHONE)
        {
            ownRadio.SetActive(true);
        }
        if (myState != action)
        {
            myState = action;
            myAnimator.SetTrigger(CharacterAniState.stateMapping[action]);
        }        
    }
    //RPC로 Network상에서 animation을 바꿔주자
    void checkMovementAnimation()
    {
        
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            clientManager.actionAnimationSync(CharacterAnimationState.WALK);
        }
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp("s"))
        {
            clientManager.actionAnimationSync(CharacterAnimationState.IDLE);
        }
    }
     

	void Update () {
        //for Debugging
        /*
        if (Input.GetKeyDown("r"))
        {
            checkActionAnimation(AnimationState.PHONE);
        }
        else if (Input.GetKeyDown("t"))
        {
            resetAnimation();
        }
        */
        //Network

        //본인일 경우 animation setting을 하기        
        if(clientManager.isMine == true)
            checkMovementAnimation();        



        //For local

        /*
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            triggerAnimation(CharacterAnimationState.WALK);
        }
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp("s"))
        {
            triggerAnimation(CharacterAnimationState.IDLE);
        }
         * */
	}
}
