using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


public class XmlManager {

    XmlDocument document = new XmlDocument();


    private PlayerTemplate pt=null;
    private ScenarioController sc = null;


    public void setMyPlayer(PlayerTemplate _pt)
    {
        pt = _pt;
    }

    public void setMyScenarioController(ScenarioController _sc)
    {
        sc = _sc;
    }




    public List<ScenarioModuleTemplate> xmlScenarioGroupLoader(string fileName)
    {
        //이거는 일단 넘기자


        string s = System.IO.File.ReadAllText(fileName);

        //string s System.IO.StringReader
        

        document.LoadXml(s);

        List<ScenarioModuleTemplate> scenarioList = new List<ScenarioModuleTemplate>();

        XmlNodeList scNodes = document.SelectNodes("Root/Scenario");

        for (int i = 0; i < scNodes.Count; i++)
        {
            scenarioList.Add(xmlScenarioLoader(scNodes[i]));
        }

        return scenarioList;
    }
     
    //scenario 로드
    public ScenarioModuleTemplate xmlScenarioLoader(XmlNode ScenarioGroupRoot)
	{
        XmlAttributeCollection xac = ScenarioGroupRoot.Attributes;


        //TaskModuleTemplate taskIns = System.Activator.CreateInstance(System.Type.GetType(myTypeStr)) as TaskModuleTemplate;

        ScenarioModuleTemplate smt = System.Activator.CreateInstance(System.Type.GetType(xac["type"].InnerText)) as ScenarioModuleTemplate;

        smt.MyScenarioName = xac["name"].InnerText;
        smt.MyDifficulty = int.Parse(xac["difficulty"].InnerText);
        smt.MyTimeout = double.Parse(xac["Time"].InnerText);

        smt.setMyParent(sc);

        XmlNodeList taskNodes = ScenarioGroupRoot.SelectNodes("Task");

        

        for (int i = 0; i < taskNodes.Count; i++)
        {
            smt.insertTask(xmlTaskLoader(taskNodes[i]));
        }

         



		return smt;
	}

	//task 로드
	public TaskModuleTemplate xmlTaskLoader(XmlNode ScenarioRoot)
	{
        XmlAttributeCollection xac = ScenarioRoot.Attributes;

        string myTypeStr = xac["type"].InnerText;

        TaskModuleTemplate taskIns = System.Activator.CreateInstance(System.Type.GetType(myTypeStr)) as TaskModuleTemplate;



        taskIns.myTaskName = xac["name"].InnerText;

        XmlNodeList stateNodes = ScenarioRoot.SelectNodes("State");

        for (int i = 0; i < stateNodes.Count; i++)
        {


            //property  설정
            XmlNode propInfo = stateNodes[i].SelectSingleNode("Properties");
            XmlAttributeCollection propAttr = propInfo.Attributes;

            
            for (int j = 0; j < propAttr.Count; j++)
            {
                //"/" 포함될 경우 2차원 이상의 배열임

                string s = propAttr[j].InnerText;

                if (s.Contains("/") == true)
                {
                    string[] strSplit = splitArray(s, '/');
                    int length = strSplit.Length - 1;

                    string[][] dataSet = new string[length][];

                    

                    for (int k = 0; k < length; k++)
                    {
                        if (k > 0)
                        {
                            strSplit[k] = strSplit[k].Substring(1);
                        }

                        string[] strSplitInner = splitArray(strSplit[k], ',');

                        
                        dataSet[k] = strSplitInner;                        
                    }

                    taskIns.addProperty(propAttr[j].Name, dataSet);
                }
                //"/"를 포함하지 않는 오직 ","만 포함할 경우...
                else if (s.Contains(",") == true)
                {
                    string[] dataSet = s.Split(',');

                    if (char.IsDigit(s, 0) == true)
                    {
                        int[] dataSetInt = new int[dataSet.Length];

                        for (int k = 0; k < dataSet.Length; k++)
                        {
                            dataSetInt[k] = int.Parse(dataSet[k]);
                            taskIns.addProperty(propAttr[j].Name, dataSetInt);
                        }
                    }
                    else
                        taskIns.addProperty(propAttr[j].Name, dataSet);
                }
                else
                {
                    if (s.Contains(".") == true && char.IsDigit(char.Parse(s.Substring(s.IndexOf(".") - 1, 1))) == true)
                        taskIns.addProperty(propAttr[j].Name, float.Parse(s));
                    else if (char.IsDigit(char.Parse(s.Substring(0, 1))) == true)
                        taskIns.addProperty(propAttr[j].Name, int.Parse(s));
                    else
                        taskIns.addProperty(propAttr[j].Name, s);
                }
                //"," 포함될 경우 1차원 배열임
                //Debug.Log(propAttr[j].Name + " AND " + propAttr[j].InnerText);
            }

            //object 설정

            XmlNode objInfo = stateNodes[i].SelectSingleNode("Objects");
            XmlAttributeCollection objAttr = objInfo.Attributes;

            for (int j = 0; j < objAttr.Count; j++)
            {
                string s = objAttr[j].InnerText;

                GameObject specificObj = GameObject.Find(s);

                if (specificObj != null)
                {
                    taskIns.addObject(objAttr[j].Name, specificObj);
                }

            } 
        }


        

        taskIns.setMyUI(pt.myUIInfo);
        taskIns.setMyPlayer(pt);
        taskIns.readyTask();

		return taskIns;
	}

    private string[] splitArray(string src, char chr)
    {
        string[] str = src.Split(chr);

        return str;

    }

	
}
