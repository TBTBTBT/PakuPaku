using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/// <summary>
/// 未完成
/// </summary>
public class AnimationManagerBase : MonoBehaviour
{
    protected string[] _states;
    protected int _nowState = 0;
    public Animator _anim;
    public CharacterBase _animateChara;//キャラクターにアニメーションを付加する場合
    //void ChangeState(string state)
    //{
    //    _anim.SetTrigger(state);
    //}


    virtual protected void Start()
    {

		//キャラクターからデータをもらう
        if (_animateChara)
        {
            _animateChara.OnChangeState.AddListener(ChangeState);
            _states = _animateChara.GetStates();
            Debug.Log(_states);
        }
    }
    protected void ChangeState(int state)
    {
        _nowState = state;
        if (state >= 0 && _states.Length > state)
        {
            _anim.SetTrigger(_states[state]);
        }
    }
}
