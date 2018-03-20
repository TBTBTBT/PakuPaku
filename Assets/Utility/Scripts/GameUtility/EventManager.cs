
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class UnityEventArg<Type> : UnityEvent<Type>{};
public class UnityEventArg<Type,Type2> : UnityEvent<Type,Type2> { };
public class EventManager : MonoBehaviour {
	
	#region Events

	static public UnityEvent<int> OnTouchBegin = new UnityEventArg<int>();
	static public UnityEvent<int> OnTouchMove = new UnityEventArg<int>();
    static public UnityEvent<int> OnTouchEnd = new UnityEventArg<int>();
    static public UnityEvent<int,TouchGesture> OnTouchGestureEnd = new UnityEventArg<int,TouchGesture>();
    static public UnityEvent<int, TouchGesture> OnTouchGestureMove = new UnityEventArg<int, TouchGesture>();
    #endregion

    #region Methods

    void SetEvent(ref UnityEvent u ){
		if (u == null) {
			u = new UnityEvent ();
		}
	}
	void SetEvent<Type>(ref UnityEvent<Type> u ){
		if (u == null) {
			u = new UnityEventArg<Type>();
		}
	}

    public static void Invoke(ref UnityEvent u ){
		if (u != null) {
			u.Invoke();
		}
	}
    public static void Invoke<Type>(ref UnityEvent<Type> u,Type arg)
	{
		if (u != null)
		{
			u.Invoke(arg);
		}
	}
    public static void Invoke<Type,Type2>(ref UnityEvent<Type, Type2> u, Type arg, Type2 arg2)
    {
        if (u != null)
        {
            u.Invoke(arg,arg2);
        }
    }

    #endregion
    //がんばった
}


