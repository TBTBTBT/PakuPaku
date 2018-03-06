using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FaderBehaviour : SingletonMonoBehaviour<FaderBehaviour> {
	float fadetime = 0;
	public Animator fade;
	string nextScene;
	bool isLoaded = false;
	// Use this for initialization
	void Start () {
		
	}

    public static void Fade(string next)
    {
        if (Instance)
        {
            Instance.FadeExec(next);
        }
    }

    public void FadeExec(string next){
		nextScene = next;
		if(fade.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
		{
			fade.SetTrigger ("Start");
			isLoaded = false;
		}

	}

	// Update is called once per frame
	void Update () {
		if(fade.GetCurrentAnimatorStateInfo(0).IsName("Stop"))
		{
			if (isLoaded == false) {
				isLoaded = true;
				SceneManager.LoadScene (nextScene);
				fade.SetTrigger ("Continue");
			}

		}
	}
}
