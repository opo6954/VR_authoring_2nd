using UnityEngine;
using System.Collections;

/*
    Player 의 Animation을 공유 하기 위한 Script
*/

public class Network_Player_MotionRecognize : MonoBehaviour {

    public enum CharacterState
    {
        idle,
        walking,
        jump,
        run
    }

    public CharacterState _state;
    public Animator animator;
    public bool isMe = false;
     
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (isMe)
        {
            checkKey();
        }
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player : send the others our data
            stream.SendNext(animator.GetBool("isWalking"));
            stream.SendNext(animator.GetBool("isRun"));
            stream.SendNext(animator.GetBool("isJump"));
        }
        else
        {
            // Network Player : receive data
            animator.SetBool("isWalking", (bool)stream.ReceiveNext());
            animator.SetBool("isRun", (bool)stream.ReceiveNext());
            animator.SetBool("isJump", (bool)stream.ReceiveNext());
        }
    }

    void checkKey()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _state = CharacterState.walking;
            animator.SetBool("isWalking", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _state = CharacterState.run;
                animator.SetBool("isRun", true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _state = CharacterState.jump;
            animator.SetBool("isJump", true);
        }
        else
        {
            _state = CharacterState.idle;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRun", false);
            // isJump는 Jump animation을 exit할 때에 callback되는 jump_exit 스크립트에서 false설정 해줌. (안 그러면 sync가 맞지 않기 때문에)
        }
    }

}
