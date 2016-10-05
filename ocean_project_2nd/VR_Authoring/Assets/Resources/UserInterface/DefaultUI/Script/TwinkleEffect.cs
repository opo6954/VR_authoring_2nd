using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TwinkleEffect : UIEffect {

    
    float period = 0.5f;

    bool isStitchingOn = true;



    public IEnumerator turnOnOffDuration()
    {
        isStitchingOn = false;

        gameObject.GetComponent<Image>().enabled = true;
        transform.GetChild(0).GetComponent<Text>().enabled = true;

        
        

        yield return new WaitForSeconds(period);

        gameObject.GetComponent<Image>().enabled = false;
        transform.GetChild(0).GetComponent<Text>().enabled = false;

        yield return new WaitForSeconds(period);

        isStitchingOn = true;

        
        
    }

    public override void Init()
    {
        base.Init();

        gameObject.GetComponent<Image>().enabled = false;
        transform.GetChild(0).GetComponent<Text>().enabled = false;

    }

    public override void UIProcessing()
    {
   
        if (isStitchingOn == true)
        {
            
            
            StartCoroutine(turnOnOffDuration());
        }
    }

}
