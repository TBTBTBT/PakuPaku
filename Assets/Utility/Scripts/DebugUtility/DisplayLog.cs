using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DisplayLog : SingletonMonoBehaviour<DisplayLog>
{
    private static string LogText = "";

    private static bool isReset = false;
    private static bool isResetLine = false;
    // Use this for initialization
    private GUIStyle style;

    struct PairVec2
    {
        public  Vector2 start;
        public  Vector2 end;
    }
    private List<PairVec2> linePos = new List<PairVec2>();
    void Start () {
	    style = new GUIStyle();
	    style.fontSize = 12;
	    style.fontStyle = FontStyle.Normal;
	    style.normal.textColor = Color.white;
       // Debug.Log(LogText);
    }

    public static void Log(string str)
    {
        if (Instance)
        {
            if (isReset == true)
            {
                LogText = "";
                isReset = false;
            }

            LogText += str + System.Environment.NewLine;
        }
    }
    public static void Line(Vector2 s, Vector2 e)
    {
        if (Instance)
        {
            if (isResetLine == true)
            {
                Instance.linePos.Clear();
                isResetLine = false;
            }
            Instance.linePos.Add(new PairVec2() {start = s,end = e});
        }
    }
    // Update is called once per frame
    void Update () {
    }
#if UNITY_EDITOR
    void OnGUI()
    {
        string outText = "DisplayLog Enabled" + System.Environment.NewLine;
        outText += "FPS     : "+ 1f / Time.deltaTime + "" + System.Environment.NewLine
                  +"Scene : "+ SceneManager.GetActiveScene().name + System.Environment.NewLine;

        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.Label(rect, outText + LogText, style);
        isReset = true;

    }
    void OnRenderObject()
    {
        foreach (var line in linePos)
        {
            GL.PushMatrix();
            //GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);
            GL.Vertex3(line.start.x, line.start.y,0);
            GL.Vertex3(line.end.x, line.end.y, 0);
            GL.End();
            GL.PopMatrix();
        }

        isResetLine = true;
    }
#endif
}
