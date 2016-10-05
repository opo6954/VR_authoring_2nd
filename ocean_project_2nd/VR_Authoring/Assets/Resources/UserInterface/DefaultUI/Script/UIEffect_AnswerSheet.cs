using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEffect_AnswerSheet : UIEffect {

	GameObject myChildForm;

	float totAnsHeight;
	float singleAnsHeight;

	float offsetInit=-20.0f;
	float offsetEnd=0.0f;

	int num=0;

	List<GameObject> myChildObj = new List<GameObject> ();
	//현재 들어있는 answersheet 모두 저장



	public override void Init ()
	{
		
		base.Init ();

		myChildForm = transform.FindChild ("Ans").gameObject;

		totAnsHeight = transform.GetComponent<RectTransform> ().rect.height;
		singleAnsHeight = myChildForm.GetComponent<RectTransform> ().rect.height;

	}

	public override void UIProcessing ()
	{
		base.UIProcessing ();


	}
	public override void isShown (bool value)
	{
		base.isShown (value);
	}

	public override void setText (string value)
	{
		base.setText (value);
	}

	public void clearTxtChild()
	{
		myChildObj.Clear ();
		for (int i = 0; i < num; i++) {
			
			GameObject childInfo = transform.FindChild("Ans" + i.ToString()).gameObject;
			if (childInfo != null)
				Destroy (childInfo);
			else
				Debug.Log ("No GameObject to clear");
		}
	}

	public void addTxtChild(int _num)
	{
		clearTxtChild ();





		myChildForm.SetActive (true);
		num = _num;





		float diff = (totAnsHeight - ( offsetInit + offsetEnd + num * singleAnsHeight)) / (num - 1);





		for (int i = 0; i < num; i++) {
			GameObject newChild = GameObject.Instantiate (myChildForm);
			newChild.name = "Ans" + i.ToString ();

			newChild.transform.SetParent (gameObject.transform);



			newChild.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, offsetInit -i * diff);
			newChild.GetComponent<UIEffect> ().setText (i.ToString ());

			myChildObj.Add (newChild);

		}
		myChildForm.SetActive (false);
	}

	public void setTxtChild(int idx, string contents)
	{
		myChildObj [idx].GetComponent<UIEffect> ().setText (contents);
	}
	//답안지 선택시 색깔 바꾸기
	public void setSelectedTxt(int idx)
	{
		if(myChildObj.Count > 0)
		{
			for (int i = 0; i < myChildObj.Count; i++)
				myChildObj [i].GetComponent<UnityEngine.UI.Image> ().color = Color.white;
			
			myChildObj [idx].GetComponent<UnityEngine.UI.Image> ().color = Color.red;
		}
	}


}
