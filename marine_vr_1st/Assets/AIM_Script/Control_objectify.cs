using UnityEngine;
using System.Collections;

public class Control_objectify : MonoBehaviour {

    private bool isObjectify = false;
    private bool isOjbectifyDone = false;
    private Renderer r;
    private float baseWidth;
	
	// Update is called once per frame
    void Start()
    {
        r = gameObject.GetComponent<Renderer>();
        baseWidth = r.material.GetFloat("_Outline_Width");
    }

	void Update () {
        if (isObjectify == true)
        {
            r.material.SetFloat("_Outline_Width", baseWidth);
        }
        else if (isObjectify == false)
        {
            r.material.SetFloat("_Outline_Width", 0);
        }
	}


    public void setObjectifyValue(bool value)
    {
        isObjectify = value;
    }


}
