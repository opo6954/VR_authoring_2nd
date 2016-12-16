using UnityEngine;
using System.Collections;

/* 
UI(Canvas) Object 는 Renderer 를 쓰지 않고 , CanvasRenderer를 쓰기 때문에 enalbed기능이 없다. 
그래서 setActive를 써야 한다. 다른 gameobject에도 사용 할 순 있지만 renderer는 실제로 존재는 하는데 보이지만 않게 해주는 것인데,
SetActive는 존재 자체를 없애서, 투명일 뿐만 아니라 충돌도 안하게 된다.
deactivated 된 gameobject는 sendmessage를 받지 못한다.
 */
public class Hide_by_SetActive : Photon.MonoBehaviour{

    public bool isActive = true;

    


	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	}
    [PunRPC]
    public void change_active_state()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }
    [PunRPC]
    public void setTagInfo()
    {
        gameObject.tag = "Pickable";//tag 바꿔주기
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


}
