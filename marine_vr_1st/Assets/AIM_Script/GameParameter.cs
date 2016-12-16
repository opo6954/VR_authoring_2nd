using UnityEngine;
using System.Collections;







public class GameParameter
{
    public static int totTask = 7;


    //task 1, fire notice
    public static int totChoice = 4;
    public static int answerIdx = 4;
    public static string[] buttonName;
    
    public static bool isSinglePlayer = true;

    public static string[] subTaskInfo;

    public static string[] subMultiTaskInfo;





    //task 2, fire report
    public static int numTotInfo = 3;
    public static int numSubInfo = 3;

    public static string[] strMainInfo;//question title
    public static string[] strSubInfo;//choice contens

    public static string[] strButtonTag;//button tag(for finding gameobject with string

    public static int[] ansSeries;

    //task 3. fire Ext Method
    public static string[] filenames;
    public static string path;
    public static int totalVideoIndex;
    public static string[] hoverTotMessage;
    public static string[] spotName;
    public static System.Collections.Generic.Dictionary<string, Rect> fireExtinguisherSpots;

    //task 7, passenger escape
    public static string[] passengerMaterialInfo;
    public static string[] passengerObjectifyMaterialInfo;
    public static int NofPassenger;

    public enum playerRolePlay { COUNTER, FINDER};

    public static void initStatement()
    {
        //task 1
        buttonName = new string[totChoice];

        for (int i = 0; i < totChoice; i++)
            buttonName[i] = "Button" + (i + 1);




        //task 2
        ansSeries = new int[numTotInfo];

        ansSeries[0] = 2;
        ansSeries[1] = 1;
        ansSeries[2] = 2;


        strMainInfo = new string[numTotInfo];
        strSubInfo = new string[numTotInfo * numSubInfo];

        strMainInfo[0] = "화재 장소";
        strMainInfo[1] = "화재 종류";
        strMainInfo[2] = "화재 크기";

        strSubInfo[0] = "복도";
        strSubInfo[1] = "여객실";
        strSubInfo[2] = "기관실";

        strSubInfo[3] = "금속 화재";
        strSubInfo[4] = "일반 화재";
        strSubInfo[5] = "유류 화재";

        strSubInfo[6] = "소형 화재";
        strSubInfo[7] = "중형 화재";
        strSubInfo[8] = "대형 화재";

        strButtonTag = new string[numSubInfo];

        strButtonTag[0] = "Ans1";
        strButtonTag[1] = "Ans2";
        strButtonTag[2] = "Ans3";

        //task 3
        totalVideoIndex = 4;

        System.Array.Resize(ref filenames, totalVideoIndex);

        for (int i = 0; i < totalVideoIndex; i++)
            filenames[i] = (i + 1).ToString();
        path = "fireMethodTask/videos/";

        hoverTotMessage = new string[totalVideoIndex];


        hoverTotMessage[0] = "안전핀";
        hoverTotMessage[1] = "노즐";
        hoverTotMessage[2] = "손잡이";
        hoverTotMessage[3] = "몸통";


        spotName = new string[totalVideoIndex];

        spotName[0] = "safepin";
        spotName[1] = "nozzle";
        spotName[2] = "handle";
        spotName[3] = "body";

        //set extinguishable fire spot
        fireExtinguisherSpots = new System.Collections.Generic.Dictionary<string, Rect>();

        fireExtinguisherSpots.Add(spotName[0], new Rect(0.49f, 0.16f, 0.1f, 0.1f));
        fireExtinguisherSpots.Add(spotName[1], new Rect(0.35f, 0.4f, 0.2f, 0.2f));
        fireExtinguisherSpots.Add(spotName[2], new Rect(0.51f, 0.06f, 0.1f, 0.1f));
        fireExtinguisherSpots.Add(spotName[3], new Rect(0.42f, 0.6f, 0.2f, 0.2f));


        //task 7
        NofPassenger = 3;

        passengerMaterialInfo = new string[NofPassenger];

        passengerMaterialInfo[0] = "Materials/passenger/female_v1_01";
        passengerMaterialInfo[1] = "Materials/passenger/female_v1_07";
        passengerMaterialInfo[2] = "Materials/passenger/female_v1_20";

        passengerObjectifyMaterialInfo = new string[NofPassenger];

        
        passengerObjectifyMaterialInfo[0] = "Materials/customizedObjectify/Outline_female_fatm";
        passengerObjectifyMaterialInfo[1] = "Materials/customizedObjectify/Outline_female_slim";
        passengerObjectifyMaterialInfo[2] = "Materials/customizedObjectify/Outline_female_fat_LOW";



        subTaskInfo = new string[totTask];
        subTaskInfo[0] = "선박 안을 순찰하세요";
        subTaskInfo[1] = "화재 사실을 선상실에 보고하세요";
        subTaskInfo[2] = "소화기를 작동시키세요";
        subTaskInfo[3] = "표시된 화재의 초기 진화를 하세요";
        subTaskInfo[4] = "초기 진화 실패를 보고하세요";
        subTaskInfo[5] = "집결 장소로 모이세요";
        subTaskInfo[6] = "";

        subMultiTaskInfo = new string[2];

        subMultiTaskInfo[0] = "대피 장소의 승객을 확인하세요";
        subMultiTaskInfo[1] = "객실에서 승객을 찾아 데려오세요";


    }

    




}
