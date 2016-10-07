using UnityEngine;
using System.Collections;
using System.Xml;


public class XmlReader {

	public ScenarioModuleTemplate loadScenariofromXml(string scenarioName)
	{
		ScenarioModuleTemplate scenarioProduced = new ScenarioModuleTemplate ();

		Debug.Log ("load xml: ");
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.Load (scenarioName + ".xml");

		//scenario단의 특성 설정
		XmlElement itemListElement = xmldoc ["Scenario"];

		scenarioProduced.setMyPeriodTime(double.Parse(itemListElement.GetAttribute ("Time")));
		scenarioProduced.setMyDifficulty(int.Parse(itemListElement.GetAttribute ("difficulty")));
		scenarioProduced.setMyScenarioName(itemListElement.GetAttribute ("name"));

		//task단의 특성 설정

		XmlNodeList taskNodeList = xmldoc.GetElementsByTagName ("Task");

		foreach (XmlNode tasknode in taskNodeList) {
			XmlAttributeCollection attributeTaskList = tasknode.Attributes;

			foreach (XmlAttribute attributeTask in attributeTaskList) {
				
				
			}

		}


		//state단의 특성 설정은 걍 넘기자, task에 융합되도록 하자














	}


}
