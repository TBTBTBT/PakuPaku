using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera cam;

    [NonSerialized] private GameObject _chaseObj;
	// Use this for initialization
	void Start ()
	{
	    //_chaseObj = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	    if (!_chaseObj)
	    {
	        _chaseObj = GameObject.FindWithTag("Player");
        }
	    else
	    {
	        transform.position = Vector3.Lerp(transform.position,
	            new Vector3(_chaseObj.transform.position.x,_chaseObj.transform.position.y,-10),
	            0.2f);
	    }
	}
}
