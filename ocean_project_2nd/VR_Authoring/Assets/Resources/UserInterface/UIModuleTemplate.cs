using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIModuleTemplate : MonoBehaviour {

     
    public string myUIFormName;

    protected Dictionary<string, GameObject> uiFormList = new Dictionary<string,GameObject>();
    
    protected GameObject canvas_ui;
    bool isInit = false;
	
	void Start () {
	}

    public UIModuleTemplate()
    {
        
    }
	

	void Update () {
	
	}

    public virtual void initUI()
    {
        if (isInit == false)
        {
            canvas_ui = GameObject.Find("Canvas_UI");
            isInit = true;
        }
    }

    public virtual void loadUIPrefab(string formName)//UI prefab을 불러오기
    {
        initUI();

		Debug.Log ("Count?: " + uiFormList.Count.ToString());

        string UImoduledirectory = "UserInterface/" + myUIFormName;


        if (System.IO.Directory.Exists("Assets/Resources/" + UImoduledirectory) == true)
        {
            GameObject newUI = GameObject.Instantiate(Resources.Load(UImoduledirectory + "/Prefab/" + formName, typeof(GameObject))) as GameObject;

            newUI.transform.SetParent(canvas_ui.transform, false);

            uiFormList.Add(formName, newUI);
        }

		setOrderofUI ();
    }

    public void deleteUIPrefab(string formName)
    {
        if (uiFormList.ContainsKey(formName) == true)
        {
            uiFormList.Remove(formName);

        }
    }
	//backgroundUI를 제외하고 모두 지우기
	public void deleteUIAll()
	{
		GameObject back = uiFormList ["BackgroundForm"];
		uiFormList.Clear ();
		uiFormList.Add ("BackgroundForm", back);
	}

    public GameObject getUIPrefab(string formName)
    {
        if (uiFormList.ContainsKey(formName) == true)
        {
            return uiFormList[formName];
        }
        else
            Debug.Log(formName + "의 이름을 가진 UI Prefab은 존재하지 않습니다.");

        return null;
    }

	public void setOrderofUI()
	{
		if (uiFormList.ContainsKey ("BackgroundForm") == true) {
			GameObject back = uiFormList ["BackgroundForm"];

			int idx = back.transform.GetSiblingIndex ();

			back.transform.SetSiblingIndex (idx + uiFormList.Count);
		}
	}

}
