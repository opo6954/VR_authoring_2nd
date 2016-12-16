using UnityEngine;
using System.Collections;

public class FireMethodUIManager : TaskUITemplate {

    FireMethodManager fireMethodManagerInstance;
    FireExtManager fireExtManagerInstance;
  
    //for video canvas
    public Transform checkSign;
    public Transform uncheckSign;
    public Transform okayButton;
    Transform videoPlayOnButton;


    //for size of video

    private float widthRatio = 0.6f;
    private float heightRatio = 0.6f;

    //instance of video
    private MovieTexture videoInfo;
    private AudioSource audioInfo;

    private Texture2D extinguisherTex2D;
    private Texture2D emphasizerTex2D;


    
    private string hoverMessage = "";


    public bool isPlayingNow = false;
    private bool isStartTask = true;


    GameObject UIInstance;
    GameObject videoObject;







    public void playVideo(bool isNeedVideo)
    {
        if (isNeedVideo == true)
        {
            if (fireMethodManagerInstance.isStartVideo == true)
            {
                if (isPlayingNow == false)
                {
                    CentralSystem.setActiveChild(UIInstance, "Video", true);
                    
                    UnityEngine.UI.RawImage rim = videoObject.GetComponent<UnityEngine.UI.RawImage>();
                    videoInfo = Resources.Load(GameParameter.path + GameParameter.filenames[fireMethodManagerInstance.currVideo], typeof(MovieTexture)) as MovieTexture;

                    audioInfo = videoObject.GetComponent<AudioSource>();

                    rim.texture = videoInfo as MovieTexture;
                    audioInfo.clip = videoInfo.audioClip;

                    videoInfo.Stop();
                    audioInfo.Stop();

                    videoInfo.Play();
                    audioInfo.Play();

                    isPlayingNow = true;

                    fireMethodManagerInstance.isStartVideo = false;
                }
            }

            else if (isPlayingNow == true && videoInfo.isPlaying == false)
            {
                videoObject.SetActive(false);
                isPlayingNow = false;

                if (fireMethodManagerInstance.currVideo == GameParameter.totalVideoIndex - 1)
                {
                    getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(fireMethodManagerInstance.taskNumber);
                    //걸린 시간 표시해주기
                }
            }
        }
        else
        {
            if (fireMethodManagerInstance.currVideo == GameParameter.totalVideoIndex - 1)
            {
                getOwnedSystem().transform.parent.GetComponent<NetworkSender>().changeGlobalTaskDone(fireMethodManagerInstance.taskNumber);
                //걸린 시간 표시해주기
            }
        }
    }


    public void stopVideo()
    {
        if (videoInfo.isPlaying)
        {
            videoInfo.Stop();
            audioInfo.Stop();
        }

    }




    
    void videoInit()
    {
        UIInstance = GameObject.Find("Task3");

        videoObject = UIInstance.transform.FindChild("Video").gameObject;   
    }
    void videoOptionInit()
    {
        Transform videoOption = UIInstance.transform.FindChild("VideoOptions");

        checkSign = videoOption.FindChild("check");
        uncheckSign = videoOption.FindChild("uncheck");
        videoPlayOnButton = videoOption.FindChild("VideoPlayOn");
        okayButton = videoOption.FindChild("Okay");

        UnityEngine.UI.Button button = videoPlayOnButton.GetComponent<UnityEngine.UI.Button>();

        
        
    }

    //소화기 사진 보여주고 순서대로 누르는 부분임
    public void showSelectPart()
    {
        int picIdx = fireMethodManagerInstance.idxMapping[fireMethodManagerInstance.currIdx];
        Transform originMark = UIInstance.transform.FindChild("Extinguisher2D").FindChild("EmphasizeMark" + picIdx.ToString());
        Transform selectMark = UIInstance.transform.FindChild("Extinguisher2D").FindChild("SelectMark");
        
        
        for (int i = 0; i < GameParameter.totalVideoIndex; i++)
        {
            UIInstance.transform.FindChild("Extinguisher2D").FindChild("EmphasizeMark" + (i + 1).ToString()).GetComponent<UnityEngine.UI.RawImage>().enabled = true;

            Transform tmp = UIInstance.transform.FindChild("Extinguisher2D").FindChild("Mark" + (i+1).ToString());

            if (fireMethodManagerInstance.currAns[i] == true)
            {
                tmp.GetComponent<UnityEngine.UI.Text>().enabled = true;
            }

            else
            {
                tmp.GetComponent<UnityEngine.UI.Text>().enabled = false;
            }
            
        }

        originMark.GetComponent<UnityEngine.UI.RawImage>().enabled = false;

        
        selectMark.transform.position = originMark.transform.position;




    }

    public void showInteractBox()
    {


    }

    

    public void setHoveringText(string _message)
    {
        hoverMessage = _message;
    }

    void initialize()
    {
        fireMethodManagerInstance = getOwnedSystem().getTaskManager<FireMethodManager>();
        fireExtManagerInstance = getOwnedSystem().getTaskManager<FireExtManager>();
        videoInit();
        videoOptionInit();

        CentralSystem.setActiveChild(UIInstance,"Extinguisher2D",false);
        CentralSystem.setActiveChild(UIInstance, "Video", false);

    }

    public void showVideoOptionButton()
    {
        uncheckSign.GetComponent<UnityEngine.UI.RawImage>().enabled = true;
        videoPlayOnButton.GetComponent<UnityEngine.UI.Image>().enabled = true;
        videoPlayOnButton.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = true;

        okayButton.GetComponent<UnityEngine.UI.Image>().enabled = true;
        okayButton.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = true;
    }

    void ShowUI()
    {
        if (isPlayingNow == false)
        {
            showSelectPart();
            CentralSystem.setActiveChild(UIInstance, "Extinguisher2D", true);

            if (fireMethodManagerInstance.isWrongMessage == true)
            {

                Transform failMessage = UIInstance.transform.FindChild("Extinguisher2D").FindChild("failMessage");

                failMessage.GetComponent<TurnOffImageNText>().turnOnOffDuration(1.0f);
                
                
                fireMethodManagerInstance.isWrongMessage = false;
            }

        }
        else
        {
            CentralSystem.setActiveChild(UIInstance, "Extinguisher2D", false);
        }
        playVideo(fireMethodManagerInstance.isVideoOption);
    }

    public override void Init()
    {
        initialize();
    }

    


    public override void Process()
    {
        if (fireMethodManagerInstance.isDoingTask == true)
        {
            if (isStartTask == true)
            {
                showVideoOptionButton();
                isStartTask = false;
            }

            if (fireMethodManagerInstance.isVideoOptionDone == true)
            {
                checkSign.gameObject.SetActive(false);
                uncheckSign.gameObject.SetActive(false);
                videoPlayOnButton.gameObject.SetActive(false);
                okayButton.gameObject.SetActive(false);
                ShowUI();
            }
        }
        else if (fireMethodManagerInstance.isDoneTask == true)
        {
            CentralSystem.setActiveChild(UIInstance, "Extinguisher2D", false);
        }
    }

}
