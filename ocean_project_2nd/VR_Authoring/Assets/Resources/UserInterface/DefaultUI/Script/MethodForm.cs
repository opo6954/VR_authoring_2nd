using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MethodForm : MonoBehaviour {

	Transform videoRegion=null;
	Transform stateRegion=null;

	public MovieTexture videoInfo;
	public AudioSource audioInfo;

	string[] fileNames;



	public bool isPlayMode = false;//현재 video 재생 모드인지(stop이든 상관없음, 재생이 모두 완료될때까지 계속 true임)
	bool isPlayingVideo = false;//현재 video 재생중인지(stop일시 false)
	bool isStopVideo = false;//현재 video 정지되어 있는지




	// Use this for initialization
	void Start () {

		videoRegion = transform.GetChild (0);
		stateRegion = transform.GetChild(1);

		toggleShownVideoInfo (false);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (videoInfo != null) {
			if (videoInfo.isPlaying == false && isStopVideo == false) {
				isPlayMode = false;
				toggleShownVideoInfo (false);
			}
		}

	}


	public void toggleShownVideoInfo(bool value)
	{
		//video part 끄기

		videoRegion.GetComponent<UnityEngine.UI.RawImage> ().enabled = value;
		videoRegion.GetComponent<AudioSource> ().enabled = value;


		//state part 끄기

		stateRegion.GetChild(0).GetComponent<UIEffect> ().isShown (value);
		stateRegion.GetChild (1).GetComponent<UIEffect> ().isShown (value);

	}

	public void setVideoName(string[] _fileNames)
	{
		fileNames = new string[_fileNames.Length];
		_fileNames.CopyTo (fileNames, 0);

	}

	public void playVideo(int currIdx)
	{
		RawImage rim = videoRegion.GetComponent<RawImage> ();
		videoInfo = Resources.Load ("Video/" + fileNames [currIdx], typeof(MovieTexture)) as MovieTexture;

		audioInfo = videoRegion.GetComponent<AudioSource> ();

		rim.texture = videoInfo as MovieTexture;
		audioInfo.clip = videoInfo.audioClip;

		videoInfo.Stop ();
		audioInfo.Stop ();

		videoInfo.Play ();
		audioInfo.Play ();

		isPlayingVideo = true;
		isPlayMode = true;
	}

	public void stopVideo()
	{
		if (isPlayingVideo == true && isStopVideo == false) {

			videoInfo.Pause ();
			audioInfo.Pause ();

			isPlayingVideo = false;
			isStopVideo = true;

			stateRegion.GetChild (0).GetComponent<UIEffect> ().setText ("Play(x)");


		} else if (isPlayingVideo == false && isStopVideo == true) {
			videoInfo.Play ();
			audioInfo.Play ();

			isStopVideo = false;
			isPlayingVideo = true;

			stateRegion.GetChild (0).GetComponent<UIEffect> ().setText ("Stop(x)");
		}
	}

	public void skipVideo()
	{
		toggleShownVideoInfo (false);
		isPlayingVideo = false;
		isStopVideo = false;

		isPlayMode = false;

	}


}
