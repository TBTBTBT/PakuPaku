using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    TouchGesture _moveDirection = TouchGesture.None;
    TouchGesture _moveBuffer = TouchGesture.None;
    float speed = 0.035f;

    public Animator anim;

    private int _animState = 0;
    //float speed = 0.
	// Use this for initialization
	void Start () {
	    EventManager.OnTouchGesture.AddListener(GetTouchGesture);
    }

    #region 入力

    void GetTouchGesture(int i, TouchGesture gesture)
    {
        if (i == 0)
        {
            //
            //_moveDirection = gesture;


            //Normal
            if (gesture != TouchGesture.None)
            {
                if (_moveDirection == TouchGesture.None)
                {//最初だけ
                    ChangeDirection(gesture);
                    //_moveDirection = gesture;
                }

                //if (_moveBuffer == TouchGesture.None)
                {//次に行く方向
                    _moveBuffer = gesture;
                }
            }
            else
            {
                ChangeDirection(gesture);
                _moveBuffer = gesture;
            }
        }
    }


    #endregion

    #region 移動

    void Move()
    {
        GameManager game = GameManager.Instance;
        
        Vector2Int dir = GestureToDir(_moveDirection);
        Vector2Int buf = GestureToDir(_moveBuffer);
        
        Vector2Int pos = game.PositionToIndex(transform.position);
        bool left = game.IsFieldPassable(pos + new Vector2Int(-1, 0));
        bool up = game.IsFieldPassable(pos + new Vector2Int(0, -1));
        bool right = game.IsFieldPassable(pos + new Vector2Int(1, 0));
        bool down = game.IsFieldPassable(pos + new Vector2Int(0, 1));
        if (CanMove(left,up,right,down, buf))
        {
            dir = buf;
            ChangeDirection(_moveBuffer);
        }
        transform.position += (Vector3)(Vector2)dir * speed;
//        Debug.Log(pos);
        
        
        Gravitation(left,up,right,down,pos,0.15f);   
    }

    void ChangeDirection(TouchGesture dir)
    {
        _moveDirection = dir;
        int state = 0;
        switch (dir)
        {
            case TouchGesture.Left: state = 1;break;
            case TouchGesture.Up: state = 2; break;
            case TouchGesture.Right: state = 3; break;
            case TouchGesture.Down: state = 4; break;
            case TouchGesture.None: state = 0; break;
        }

        if (_animState != state)
        {
            _animState = state;
            SetNextState(_animState);
        }
    }
    bool CanMove(bool left, bool up, bool right, bool down, Vector2Int dir)
    {
        if (!left && dir.x < 0) return false;
        if (!up && dir.y > 0) return false; //upは内部的には↑だが見た目的には↓かも Todo:要確認
        if (!right && dir.x > 0) return false;
        if (!down && dir.y < 0) return false;
        return true;
    }
    /// <summary>
    /// 実質壁とのあたり判定 周囲が壁だったら今いるマスの中心に引き寄せられる
    /// </summary>
    void Gravitation(bool left, bool up, bool right, bool down, Vector2Int ind,float buffer = 0)
    {
        GameManager game = GameManager.Instance;
        Vector2 pos = game.IndexToPosition(ind);
        float lerpTime = 0.3f;
        
        if (!left && transform.position.x < pos.x - buffer)
        {
            
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x - buffer, lerpTime), transform.position.y, transform.position.z);
        }

        if (!up && transform.position.y > pos.y + buffer)
        {
            //Debug.Log(pos.y);
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, pos.y + buffer, lerpTime), transform.position.z);
        }

        if (!right && transform.position.x > pos.x + buffer)
        {
            
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, pos.x + buffer, lerpTime), transform.position.y, transform.position.z);
        }

        if (!down && transform.position.y < pos.y - buffer)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, pos.y - buffer, lerpTime), transform.position.z);
        }
           

    }
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

    #endregion

    // Update is called once per frame
    void Update () {
        #region アニメーション
        AnimatorStateInfo Info = anim.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(Info.normalizedTime+"."+Info.length);
        //GameManager game = GameManager.Instance;
        //GameViewManager view = GameViewManager.Instance;
        //game.UpdateAnimationTime(Info.normalizedTime);
        //Debug.Log(Info.normalizedTime);

        if (Info.normalizedTime >= 1)
        {

            SetNextState(_animState);
            //game.PlayerMoveWithAnimation();
            //transform.position = view.IndexToPositionPlayerAnimated(game._player, 0);
        }
        else
        {
            //transform.position = view.IndexToPositionPlayerAnimated(game._player, Info.normalizedTime);
        }


        #endregion

        Move();

    }
  
    void SetNextState(int state)
    {
        //nim.SetInteger("State",state);
        switch (state) {
        
            case 0:
                anim.SetTrigger("Wait");
                break;
            case 1:
                anim.SetTrigger("WalkLeft");
                break;
            case 3:
                anim.SetTrigger("WalkRight");
                break;
            case 4:
                anim.SetTrigger("WalkDown");
                break;
        }
        
    }
}
