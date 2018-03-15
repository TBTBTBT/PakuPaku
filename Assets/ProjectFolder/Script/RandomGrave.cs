using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGrave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive(Random.Range(0,5) == 0);
        Debug.Log("ra");
	}

}
