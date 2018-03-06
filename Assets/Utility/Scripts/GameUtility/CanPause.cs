using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ポーズ用Monobehaviour
/// 一時停止ができるようになる。
/// rigidbodyにも対応(2D)
/// </summary>
public class CanPause : MonoBehaviour
{
    private bool pause = false;

    private Rigidbody2D rigidbody2d;
    //private Rigidbody rigidbody;
	void Awake()
	{
	    if (GetComponent<Rigidbody2D>()) rigidbody2d = GetComponent<Rigidbody2D>();

        AwakeCanPause();
	}

    protected virtual void AwakeCanPause()
    {

    }
    // Update is called once per frame
    protected virtual void UpdateCanPause () {
		
	}
    void Update()
    {
        if(!pause)UpdateCanPause();
    }
}
