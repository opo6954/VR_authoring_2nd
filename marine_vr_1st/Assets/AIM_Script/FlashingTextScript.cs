using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* text 를 깜빡깜빡 거리게 하는 script */
/* [TODO] : 시작할 때, deactivate 되어있는 object에게는 적용이 안되는 듯 하다. 이 문제를 해결해야 한다. */
public class FlashingTextScript : MonoBehaviour {

    Text flashingText;
    public bool isBlinking = true; // flag to determine if you want blinking to happen
    public float blink_frequency = .5f;
    public string textToFlash = "화재를 보고하세요!!";
    public string blankText = "";
    public string staticText = "I'M FLASHING NO MORE";

	// Use this for initialization
	void Start () {
        // get the text component
        flashingText = GetComponent<Text>();
        StartCoroutine(BlinkText());
        StartCoroutine(StopBlinking());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // function to blink the text
    
    public IEnumerator BlinkText()
    {
        while (isBlinking)
        {
            flashingText.text = blankText;
            yield return new WaitForSeconds(blink_frequency);
            flashingText.text = textToFlash;
            yield return new WaitForSeconds(blink_frequency);
        }
    }

    IEnumerator StopBlinking()
    {
        yield return new WaitForSeconds(5f);
        isBlinking = false;
        flashingText.text = staticText;
    }
}
