using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public enum TouchGesture
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3,
    None = 99,
}
public class TouchManager : SingletonMonoBehaviour<TouchManager> {
    //Dictionary<int,TouchInfo> touchState = new Dictionary<int,TouchInfo>();
    /*public TouchInfo GetTouch(int i){
		if (touchState.ContainsKey (i)) {
			return touchState [i];
		}
		return TouchInfo.None;
	}*/
    
    public Vector2? touchPosBefore = null;
    public Vector2? touchPosAnchor = null;
    public Vector2? touchPosCurrent = null;

    // Use this for initialization
    void Start()
    {
        EventManager.OnTouchMove.AddListener(OnTouchMove);
        EventManager.OnTouchBegin.AddListener(OnTouchMove);
        EventManager.OnTouchEnd.AddListener(OnTouchEnd);
    }

    public Vector2 GetTouchAcc ()
    {
        if (touchPosCurrent != null)
        {
            return ((Vector2)touchPosCurrent - (Vector2)touchPosBefore);
        }

        return Vector2.zero;
    }
    public Vector2 GetTouchDistance()
    {
        if (touchPosAnchor != null && touchPosCurrent != null)
        {
            return ((Vector2)touchPosCurrent - (Vector2)touchPosAnchor);
        }

        return Vector2.zero;
    }
    void OnTouchMove(int num)
    {
        if (num == 0)
        {
            if (touchPosCurrent != null)
            {
                touchPosBefore = touchPosCurrent;
            }

            if (touchPosAnchor == null)
            {
                touchPosAnchor = TouchInput.GetTouchPosition(num);
            }
            else touchPosBefore = TouchInput.GetTouchPosition(num);
            touchPosCurrent = TouchInput.GetTouchPosition(num);
            TouchGesture gesture = PositionToGesture((Vector2)touchPosAnchor, (Vector2)touchPosCurrent);
            EventManager.Invoke(ref EventManager.OnTouchGestureMove, num, gesture);
        }
    }
    /// <summary>
    ///タッチ座標(始点、終点)をジェスチャーに
    /// </summary>
    /// <param name="pos1">アンカー(始点)</param>
    /// <param name="pos2">終点</param>
    /// <returns></returns>
    TouchGesture PositionToGesture(Vector2 pos1,Vector2 pos2)
    {
         Vector2 touchDist = pos2 - pos1;
        TouchGesture gesture = TouchGesture.None;
        if (touchDist.magnitude > 10)
        {
            float angle = MathUtil.GetAim(pos1, pos2);
            gesture = AngleToGesture(angle);
        }

        return gesture;
    }
    /// <summary>
    /// タッチ角度をジェスチャーに変換
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    TouchGesture AngleToGesture(float angle)
    {
        TouchGesture ret = TouchGesture.None;
        float sin = Mathf.Sin(angle * Mathf.PI / 180);
        float cos = Mathf.Cos(angle * Mathf.PI / 180);
        if (Mathf.Abs(sin) > Mathf.Abs(cos))
        {
            if (sin >= 0) ret = TouchGesture.Up;
            if (sin < 0) ret = TouchGesture.Down;
        }
        else
        {
            if (cos < 0) ret = TouchGesture.Left;
            if (cos >= 0) ret = TouchGesture.Right;
        }
        //Debug.Log("" + ret);
        return ret;
    }
    void OnTouchEnd(int num)
    {
        if (num == 0)
        {
            TouchGesture gesture = PositionToGesture((Vector2)touchPosAnchor, (Vector2)touchPosCurrent);
            EventManager.Invoke(ref EventManager.OnTouchGestureEnd,num,gesture);
            touchPosBefore = null;
            touchPosCurrent = null;
            touchPosAnchor = null;
        }
    }

    public Vector3 GetTouchWorldPos(int num){
		Camera cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		return TouchInput.GetTouchWorldPosition (cam, num);
	}
	void Update () {
		
		int touchNum = 1;
		#if (UNITY_EDITOR || UNITY_STANDALONE)
		#else

		touchNum = Input.touchCount;

		#endif
		for(int i = 0;i < touchNum; i++){
			if (TouchInput.GetTouch(i) == TouchInfo.Began)EventManager.Invoke  (ref EventManager.OnTouchBegin,TouchInput.GetID(i));
			if (TouchInput.GetTouch(i) == TouchInfo.Moved || TouchInput.GetTouch(i) == TouchInfo.Stationary)EventManager.Invoke (ref EventManager.OnTouchMove,TouchInput.GetID(i));
			if (TouchInput.GetTouch(i) == TouchInfo.Ended)EventManager.Invoke  (ref EventManager.OnTouchEnd,TouchInput.GetID(i));
		}/*
		for(int i = 0;i < touchNum; i++){
				if (touchState.ContainsKey (i)) {
				touchState [i] = TouchInput.GetTouch (i);
				//	Debug.Log (i);
				} else {
	
				touchState.Add (i, TouchInfo.Began);
				}
		}*/
	}
}
