using UnityEngine;
using System.Collections;

public class TurnOffImageNText : MonoBehaviour {
    private bool isOn = true;

    public void turnOnOffDuration(float t)
    {
        StartCoroutine(turnOnOffDurationInside(t));
    }

    public void turnOnOffDuration(float t, int times)
    {
        StartCoroutine(turnOnOffDurationInsideMultiple(t, times));
    }

    public IEnumerator turnOnOffDurationInside(float t)
    {
        gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
        transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = true;

        yield return new WaitForSeconds(t);

        gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
        transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = false;
    }

    public IEnumerator turnOnOffDurationInsideMultiple(float t, int times)
    {
        for (int i = 0; i < times; i++)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = true;

            yield return new WaitForSeconds(t);

            gameObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = false;

            yield return new WaitForSeconds(t);
        }
    }


    public void turnOnOff(bool value)
    {
        if (isOn != value)
        {
            isOn = value;

            gameObject.GetComponent<UnityEngine.UI.Image>().enabled = isOn;
            transform.FindChild("Text").GetComponent<UnityEngine.UI.Text>().enabled = isOn;
        }
    }
}
