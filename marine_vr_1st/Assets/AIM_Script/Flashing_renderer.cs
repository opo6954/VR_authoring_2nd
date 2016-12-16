using UnityEngine;
using System.Collections;

public class Flashing_renderer : MonoBehaviour {

    public bool isBlinking = true;
    private float blink_frequency = 0.5f;
    Renderer myRenderer;

	
	void Start () {
        myRenderer = gameObject.GetComponent<Renderer>();
	    StartCoroutine(BlinkRenderer());
	}
	
	// Update is called once per frame

	void Update () {
	
	}

    
    public IEnumerator BlinkRenderer()
    {
        while(isBlinking == true)
        {
            myRenderer.enabled = true;
            yield return new WaitForSeconds(blink_frequency);
            myRenderer.enabled = false;
            yield return new WaitForSeconds(blink_frequency);
        }
    }
}
