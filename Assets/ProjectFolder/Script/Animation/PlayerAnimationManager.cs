using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : AnimationManagerBase
{
    
	void Start ()
	{

        //プレイヤーアニメーションの状態一覧
	    _states = new[]
	    {
	        "Wait",
	        "WalkLeft",
	        "WalkUp",
	        "WalkRight",
	        "WalkDown"
	    };


	}

	void Update () {
	   
    }
}
#region アニメーション独自ループ
// AnimatorStateInfo Info = _anim.GetCurrentAnimatorStateInfo(0);
//if (Info.normalizedTime >= 1)
//{

//ChangeState(_nowState);
//}



#endregion