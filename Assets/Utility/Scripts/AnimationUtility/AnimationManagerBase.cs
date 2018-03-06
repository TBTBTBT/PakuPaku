using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/// <summary>
/// 未完成
/// </summary>
public class AnimationManagerBase : MonoBehaviour {
    public enum State
    {
        Wait = 0,
        Light = 1,
        Win = 2,
        Lose = 3,
    }

    public State state;
    public Animator anim;
    public ParticleSystem particle;
    public void ChangeState(State s)
    {
        if (state != s)
        {
            state = s;
            ChangeAnimation();
            ChangeParticleState();
        }
    }

    void ChangeAnimation()
    {
        if (anim != null)
        {
            anim.SetInteger("state",(int)state);
            anim.SetTrigger("Change");
        }
    }

    void ChangeParticleState()
    {
        if (state == State.Light)
        {
                particle.Play();
        }
        else
        {
            //if(particle.isPlaying)
                particle.Stop();
        }
    }
	// Use this for initialization
	void Start ()
	{
	    ChangeState(State.Wait);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
