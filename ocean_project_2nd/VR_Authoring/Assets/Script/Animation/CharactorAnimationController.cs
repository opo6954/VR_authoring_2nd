using UnityEngine;
using System.Collections;

public enum CharacterAniState
{
    IDLE, WALK, TALKING, BUTTON, PHONE

}

public class CharactorAnimationController : MonoBehaviour {
   

    CharacterAniState myState;
    public Animator myAnimator;

    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame

    public void resetAnimation()
    {
        if (myState != CharacterAniState.IDLE)
        {
            myState = CharacterAniState.IDLE;
            myAnimator.SetTrigger("Idle");
            myAnimator.ResetTrigger("Talking");
            myAnimator.ResetTrigger("Button");
            myAnimator.ResetTrigger("Phone");
        }
    }

    public void checkActionAnimation(CharacterAniState action)
    {
        switch (action)
        {
            case CharacterAniState.TALKING:
                if (myState != CharacterAniState.TALKING)
                {
                    myState = CharacterAniState.TALKING;
                    myAnimator.SetTrigger("Talking");
                }
                
                break;
            case CharacterAniState.BUTTON:
                if (myState != CharacterAniState.BUTTON)
                {
                    myState = CharacterAniState.BUTTON;
                    myAnimator.SetTrigger("Button");
                }

                break;
            case CharacterAniState.PHONE:
                if (myState != CharacterAniState.PHONE)
                {
                    myState = CharacterAniState.PHONE;
                    myAnimator.SetTrigger("Phone");
                }
                break;
        }
    }

    void checkMovementAnimation()
    {
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            if (myState != CharacterAniState.WALK)
            {
                myState = CharacterAniState.WALK;
                myAnimator.SetTrigger("Walk");

            }
        }
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp("s"))
        {
            if (myState != CharacterAniState.IDLE)
            {
                myState = CharacterAniState.IDLE;
                myAnimator.SetTrigger("Idle");
            }
        }
    }
     

	void Update () {
        if (Input.GetKeyDown("r"))
        {
            checkActionAnimation(CharacterAniState.BUTTON);
        }
        else if (Input.GetKeyDown("t"))
        {
            resetAnimation();
        }
        checkMovementAnimation();        
	}
}
