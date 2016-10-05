using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

/****
from
http://forum.unity3d.com/threads/simple-node-editor.189230/
****/

public class Rect_Pair
{
    public Rect start, end;
    public Rect_Pair(Rect start, Rect end)
    {
        this.start = start;
        this.end = end;
    }
}

public class DrawLine : EditorWindow {

    public static List<Rect> windows = new List<Rect>();
    public static List<Rect_Pair> connections = new List<Rect_Pair>();


    public static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 1);
    }

    public static void DrawArrowHead(Rect start, Rect end, Vector2 vStartPercentage, Vector2 vEndPercentage, float fHandleDistance, float fLength, float fWidth)
    {
        float fHandleDistanceDouble = fHandleDistance * 2;

        Vector3 startPos = new Vector3(start.x + start.width * vStartPercentage.x, start.y + start.height * vStartPercentage.y, 0);
        Vector3 startTan = startPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vStartPercentage.y);

        Vector3 endPos = new Vector3(end.x + end.width * vEndPercentage.x, end.y + end.height * vEndPercentage.y, 0);
        Vector3 endTan = endPos + Vector3.right * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.x) + Vector3.up * (-fHandleDistance + fHandleDistanceDouble * vEndPercentage.y);

        float dy = endTan.y - endPos.y;
        float dx = endTan.x - endPos.x;

        Vector3 vDelta = endTan - endPos;
        Vector3 vNormal = new Vector3(-dy, dx, 0f).normalized;

        Vector3 vArrowHeadEnd1 = endPos + vDelta.normalized * fLength + vNormal.normalized * fWidth;
        Vector3 vArrowHeadEnd2 = endPos + vDelta.normalized * fLength + vNormal.normalized * -fWidth;

        Vector3 vHalfwayPoint = endPos + vDelta.normalized * fLength * 0.5f;

        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(endPos, vArrowHeadEnd1, endPos, vHalfwayPoint, Color.green, null, 2);

        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(endPos, vArrowHeadEnd2, endPos, vHalfwayPoint, Color.green, null, 2);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
