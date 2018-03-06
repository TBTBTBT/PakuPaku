using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[RequireComponent(typeof(Camera))]
public class AsciiDisplay : MonoBehaviour {
    private GUIStyle style;
    private Camera camera;
    private bool isReset = false;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 16;
        style.fontStyle = FontStyle.Normal;
        style.normal.textColor = Color.white;
        camera = GetComponent<Camera>();
        // Debug.Log(LogText);
    }

  
	// Update is called once per frame
	void Update ()
	{
	    isReset = false;
	}
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
       // src.
        string outText = "";
        if (isReset == false)
        {

        }
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.Label(rect, outText, style);
        isReset = true;

    }
}
*/