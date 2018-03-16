using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{

    //ステート オーバーライドして使用
    protected string[] _states;
    //現在のステート‘
    protected int _nowState = 0;
    //ステートが変更されたときに発動するイベント
    [NonSerialized]
    public UnityEventArg<int> OnChangeState = new UnityEventArg<int>();
    //ステート定義(_statesの定義)をする関数 オーバーライドして使用
    protected virtual void SetStates(){}

    protected int ChangeState(int state)
    {
        _nowState = state;
        OnChangeState.Invoke(state);
        return state;
    }
    void Awake()
    {
        SetStates();
    }
    public string[] GetStates()
    {
        return _states;
    }


}
