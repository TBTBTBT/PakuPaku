using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterOnField
{
    #region 変数

    TouchGesture _moveDirection = TouchGesture.None;
    TouchGesture _moveBuffer = TouchGesture.None;

    #region パラメーター

    float speed = 0.055f;//いどうそくど

    #endregion

    #endregion

    /// <summary>
    /// ステート一覧の初期化
    /// </summary>
    protected override void SetStates()
    {
        _states = new []
        {
            "Wait",
            "WalkLeft",
            "WalkUp",
            "WalkRight",
            "WalkDown"
        };
    }

    void Start () {
	    EventManager.OnTouchGestureMove.AddListener(GetTouchGesture);
		GameManager game = GameManager.Instance;
		game.OnPushDig.AddListener (Dig);
    }

    #region 入力

    void GetTouchGesture(int i, TouchGesture gesture)
    {
        if (i == 0)
        {
            //
            //_moveDirection = gesture;

            ChangeDirection((MoveDirection)gesture);
            /*
            //Normal
            if (gesture != TouchGesture.None)
            {
                if (_moveDirection == TouchGesture.None)
                {//最初だけ
                    ChangeDirection((MoveDirection)gesture);
                    _moveDirection = gesture;
                }

                //if (_moveBuffer == TouchGesture.None)
                {//次に行く方向
                    _moveBuffer = gesture;
                }
            }
            else
            {
                ChangeDirection((MoveDirection)gesture);
                _moveDirection = gesture;
                _moveBuffer = gesture;
            }*/
        }
    }


    #endregion
    #region 移動

    Vector2Int GestureToDir(TouchGesture gesture)
    {
        Vector2Int moveDir = new Vector2Int(0, 0);
        switch (gesture)
        {
            case TouchGesture.Left:
                moveDir.x = -1;
                break;
            case TouchGesture.Up:
                moveDir.y = 1;
                break;
            case TouchGesture.Right:
                moveDir.x = 1;
                break;
            case TouchGesture.Down:
                moveDir.y = -1;
                break;
        }

        return moveDir;
    }
	bool DigCheck(){
		GameManager game = GameManager.Instance;
		return game.IsDug (transform.position);
	}
    void Move()
    {
        GameManager game = GameManager.Instance;
        Vector2 dir = TouchManager.Instance.GetTouchDistance();

        /*
        Vector2Int dir = GestureToDir(_moveDirection);
        Vector2Int buf = GestureToDir(_moveBuffer);
        */
        Vector2Int pos = game.PositionToIndex(transform.position);
        bool left = game.IsFieldPassable(pos + new Vector2Int(-1, 0));
        bool up = game.IsFieldPassable(pos + new Vector2Int(0, -1));
        bool right = game.IsFieldPassable(pos + new Vector2Int(1, 0));
        bool down = game.IsFieldPassable(pos + new Vector2Int(0, 1));
        /*
        if (CanMove(left, up, right, down, buf))
        {
            dir = buf;
            _moveDirection = _moveBuffer;
            ChangeDirection((MoveDirection)_moveBuffer);
        }*/
        transform.position += (Vector3)(Vector2)dir.normalized * speed;
        //        Debug.Log(pos);


        Gravitation(left, up, right, down, pos, 0.15f);
    }
#endregion
	public void Dig(){
		GameManager game = GameManager.Instance;
		game.Dig (transform.position);
        //game.GraveStamp(transform.position);

	}
    void Update () {
        

        Move();
		if (DigCheck ()) {
			//Destroy (gameObject);
			isArrive = false;
		}

    }
 
}
