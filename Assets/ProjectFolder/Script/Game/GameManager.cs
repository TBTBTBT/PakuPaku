using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// フィールド1マス単位に必要な情報
/// </summary>
public class FieldInfo
{
    public int floorNum = 0;//床の状態を表す。 0:何もなし 1~:エサ

    public int feedSpawnTime = 0;//エサが現れてからの経過時間

    //↓※オブジェクトが各床に対して必ず1つしか存在しない場合のみ使用 複数ある場合はオブジェクトに場所情報を持たせる形で
    //public int objectNum;//存在するオブジェクト 
    public bool isPassable = false; // 通れるかかどうか
    public bool isUnlock = false;
	public bool isDug = false;
}
/// <summary>
/// フィールドに存在するオブジェクトのベースクラス
/// </summary>
public class ObjectBase
{
    //座標
    public Vector2Int pos;
    public Vector2Int beforePos;
}


public class GameManager : SingletonMonoBehaviourCanDestroy<GameManager>
{
    [System.NonSerialized]
    public int _width = 15;
    [System.NonSerialized]
    public int _height = 15;
	

	//じゃぐ配列にするか検討
    FieldInfo[,] _field;
    //フィールドの見た目の大きさ倍率
    private float _extend = 1.6f;
    private int frameTimeMax = 300;//床が増えるまでの時間

    [System.NonSerialized]
    public UnityEvent OnChangeField = new UnityEvent();
	[System.NonSerialized]
	public UnityEvent OnPushDig= new UnityEvent();

    public GameObject _playerPrefab;

    private int _unlockRange = 2;
    //今アニメーション中か


    void Awake()
    {
        
        base.Awake();
        _field = new FieldInfo[_width, _height];
        FieldInit();
        PlayerInit();
    }
	public void PushDigButton(){
		OnPushDig.Invoke ();
		OnChangeField.Invoke();
	}
	public bool IsDug(Vector2 pos){
		return IsDug (PositionToIndex (pos));
	}
	public bool IsDug(Vector2Int pos){
		return _field [pos.x, pos.y].isDug;
	}
    /// <summary>
    /// フィールドの初期化
    /// </summary>
    void FieldInit()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _field[i, j] = new FieldInfo();
            }
        }
        for (int i = 0; i < _width; i++)
        {
           // int erase = Random.Range(0, 4);
           // int bnum = 0;
            for (int j = 0; j < _height; j++)
            {
                int x = i <= _width / 2 ? i : FieldData.data.GetLength(1)-1 - (i-_width/2);
                int y = j <= _height / 2 ? j : FieldData.data.GetLength(0) - 1 - (j - _height/2);
                //Debug.Log(y);
                //int rect = 2;
                //bool isUnlock = ((_width / 2) + rect > i && (_width / 2) - rect < i) &&
                //                ((_height / 2) + rect > j && (_height / 2) - rect < j);

               // _field[i, j].isUnlock = isUnlock;
                _field[i, j].isPassable = FieldData.data[y, x] == 0;
                //               if (i % 10 == j * 6 % 10) bnum++;
                // bool isBlock = i % 10 == ((j+1) * 7 + 1) % 10;// && bnum!=erase;
                //if ((i == _width / 2 && j == _height / 2 + 1))
                //DigoutBlock(new Vector2Int(i,j), new Vector2Int(Random.Range(0,_width), Random.Range(0, _height)), new Vector2Int(i, j), 5);
                //(i == _width / 2 && j == _height / 2) ;
                //_field[i, j].floorNum = isBlock ? 10 : 0;
                //_field[i, j].isUnlock = !isBlock;
                //if (i == _width / 2 && j == _height / 2)
                //{
                //    _field[i, j].floorNum = 20;
                //}
            }
        }
        FieldEnlarge(2);
        //_player.pos = new Vector2Int(_width / 2,_height/2 + 1);
    }
	public FieldInfo Field(int x,int y){
		return _field [x, y];
	}
    void PlayerInit()
    {
        GameObject.Instantiate(_playerPrefab, IndexToPosition(new Vector2Int(_width / 2,_height/2+1)), Quaternion.identity);
    }
    public bool CanMove(Vector2Int pos, Vector2Int move)
    {
        if (move.sqrMagnitude != 0)
        {
            Vector2Int ind = pos + move;
            if (IsFieldPassable(ind))
            {
                //ブロック以外
                // Debug.Log("" + ind.x + "," + ind.y);
                //return PlayerMove(ind, move);
                return true;
            }

            //{//通行不能ブロック
            //     return pos;
            //}
        }

        return false;
    }
    #region 座標変換

   
    public Vector2 IndexToPosition(Vector2Int pos)
    {
        float fx = (float)pos.x;
        float fy = (float)pos.y;
        Vector2 ret = new Vector2(fx - _width / 2,-(fy - _height / 2))*_extend;
        return ret;
    }
    public Vector2Int PositionToIndex(Vector2 pos)
    {
        int fx = (int)((_width / 2f + pos.x/_extend));
        int fy = (int)((_height / 2f - pos.y/_extend));
        fx = IndexModify(fx, 0, _width - 1);
        fy = IndexModify(fy, 0, _height - 1);
        Vector2Int ret = new Vector2Int(fx,fy);
        return ret;
    }
    //はみ出したindexを範囲内にする。
    int IndexModify(int ind,int min,int max)
    {
        if (ind < min) return min;
        if (ind > max) return max;
        return ind;
    }
    #endregion

    #region エサ関係

    void FeedSpawner()
    {
        int x = Random.Range(0, _width);
        int y = Random.Range(0, _height);
        if (_field[x, y].isUnlock && _field[x, y].isPassable)
        _field[x, y].floorNum = 1;
    }
    /*
    public void Feeding(Vector2Int pos)
    {
        if (_field[pos.x, pos.y].floorNum > 0)
        {
            _field[pos.x, pos.y].floorNum = 0;
            _field[pos.x, pos.y].feedSpawnTime = 0;
        }
    }
    */
#endregion


    //床の情報
    public int FieldState(int x,int y)
    {
        return _field[x, y].floorNum;
    }

#region 周辺通行可能チェック

    //上下左右が通行可能
    bool IsFieldPassableSurround(Vector2Int pos)
    {
        bool my = IsFieldPassable(pos);
        bool left = IsFieldPassable(pos + new Vector2Int(-1, 0));
        bool up = IsFieldPassable(pos + new Vector2Int(0, -1));
        bool right = IsFieldPassable(pos + new Vector2Int(1, 0));
        bool down = IsFieldPassable(pos + new Vector2Int(0, 1));
        if (left || up || down || right) return true;
        return false;
    }

#endregion

#region 通行可能チェック
    //通行可能か isUnlock isPassableがtrueなら通行可能
    public bool IsFieldPassable(Vector2Int pos)
    {
        return IsFieldPassable(pos.x, pos.y);
    }
    public bool IsFieldPassable(int x, int y)
    {
        
        if(FieldIndexCheck(x,y))
        return _field[x, y].isUnlock && _field[x, y].isPassable;
        return false;
    }

    
#endregion

#region このゲーム独特の挙動

    //フィールドを広げる
    void FieldEnlarge(int r)
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (new Vector2Int(_width / 2 - i, _height / 2 - j).magnitude < r)
                    _field[i, j].isUnlock = true;
                else _field[i, j].isUnlock = false;
            }
        }
    }


#endregion


#region フィールド配列チェック
    //trueなら大丈夫 falseならはみ出し
    public bool FieldIndexCheck(Vector2Int pos, Vector2Int move)
    {
        Vector2Int ind = pos + move;
        if (ind.x < 0 || ind.x >= _field.GetLength(0))
        {
            return false;
        }
        if (ind.y < 0 || ind.y >= _field.GetLength(1))
        {
            return false;
        }
        return true;
    }
    public bool FieldIndexCheck(int x,int y)
    {
        if (x < 0 || x >= _field.GetLength(0))
        {
            return false;
        }
        if (y < 0 || y >= _field.GetLength(1))
        {
            return false;
        }
        return true;
    }
#endregion
	public void Dig(Vector2 pos) {
		Dig (PositionToIndex (pos));
	}
	public void Dig(Vector2Int pos){
		if (FieldIndexCheck(pos.x,pos.y)) {
			if (IsFieldPassable (pos)) {
				_field [pos.x, pos.y].isDug = true;
			}
		}
	}
    // Use this for initialization
    void Start()
    {
       // EventManager.OnTouchGesture.AddListener(GetTouchGesture);
        
    }

	void Update ()
	{
	    if (Time.frameCount % 300 == 0)
	    {
	        _unlockRange++;
            FieldEnlarge(_unlockRange);
	        OnChangeField.Invoke();
        }
	    //if(Random.Range(0,100) == 0)FeedSpawner();

        // DebugField();
	    //_player.PlayerMove(ref _playerMoveDirection,ref _playerMoveBuffer);
	}

   
}



//以下未使用参考

#region 未使用
#region 旧プレイヤー関係
/*
public class Player : ObjectBase
{
    public int _playerMoveTime = 0;
    public int _playerMoveSpeed = 20;//0が最速
    public bool _isMoving = false;
    public float _animationTime = 0;
    public void PlayerMoveAnimation(ref TouchGesture gesture, ref TouchGesture buffer)
    {
        Vector2Int moveDir = GestureToDir(gesture);
        Vector2Int postMoveDir = GestureToDir(buffer);
        if (_isMoving)
        {
            _playerMoveTime++;
        }

       // if (_playerMoveTime % _playerMoveSpeed == 0)
        {
            if (CanMove(pos, postMoveDir))
            {
                moveDir = postMoveDir;
                gesture = buffer;
            };
            beforePos = pos;
            pos = PlayerMove(pos, moveDir);
        }
    }
    public void PlayerMove(ref TouchGesture gesture,ref TouchGesture buffer)
    {
        Vector2Int moveDir = GestureToDir(gesture);
        Vector2Int postMoveDir = GestureToDir(buffer);
        if (_isMoving)
        {
            _playerMoveTime++;
        }

        if (_playerMoveTime % _playerMoveSpeed == 0)
        {
            if (CanMove(pos, postMoveDir))
            {
                moveDir = postMoveDir;
                gesture = buffer;
            };
            pos = PlayerMove(pos, moveDir);
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
                moveDir.y = -1;
                break;
            case TouchGesture.Right:
                moveDir.x = 1;
                break;
            case TouchGesture.Down:
                moveDir.y = 1;
                break;
        }

        return moveDir;
    }
    bool CanMove(Vector2Int pos, Vector2Int move)
    {
        GameManager game = GameManager.Instance;
        if (move.sqrMagnitude != 0)
        {
                Vector2Int ind = pos + move;
                if (game.IsFieldPassable(ind))
                {
                    //ブロック以外
                    // Debug.Log("" + ind.x + "," + ind.y);
                    //return PlayerMove(ind, move);
                    return true;
                }

            //{//通行不能ブロック
            //     return pos;
            //}
        }

        return false;
    }
    Vector2Int PlayerMove(Vector2Int pos, Vector2Int move)
    {
        GameManager game = GameManager.Instance;
        if (move.sqrMagnitude != 0)
        {
            if (game.FieldIndexCheck(pos, move))
            {
                Vector2Int ind = pos + move;
                if (game.IsFieldPassable(ind))
                {
                    _isMoving = true;
                    //game.Feeding(ind);
                    //ブロック以外
                    // Debug.Log("" + ind.x + "," + ind.y);
                    //return PlayerMove(ind, move);
                    return ind;
                }
            }

            //{//通行不能ブロック
            //     return pos;
            //}
        }

        _isMoving = false;
        _playerMoveTime = 0;
        return pos;
    }
}
*/
#if false
    Vector2Int PlayerMove(Vector2Int pos, Vector2Int move)
    {
        if (move.sqrMagnitude != 0)
        {
            if (FieldIndexCheck(pos, move))
            {
                Vector2Int ind = pos + move;
                if (_field[ind.x, ind.y].isUnlock)
                {
                    //ブロック以外
                    Debug.Log("" + ind.x + "," + ind.y);
                    return PlayerMove(ind, move);

                }
            }

            //{//通行不能ブロック
            //     return pos;
            //}
        }

        return pos;
    }
    void PlayerMove(TouchGesture gesture)
    {
        Vector2Int moveDir = new Vector2Int(0, 0);
        switch (gesture)
        {
            case TouchGesture.Left:
                moveDir.x = -1;
                break;
            case TouchGesture.Up:
                moveDir.y = -1;
                break;
            case TouchGesture.Right:
                moveDir.x = 1;
                break;
            case TouchGesture.Down:
                moveDir.y = 1;
                break;
        }

        _player.pos = PlayerMove(_player.pos, moveDir);
    }
    void PlayerMoveAnimated(TouchGesture gesture)
    {

        Vector2Int moveDir = new Vector2Int(0, 0);
        switch (gesture)
        {
            case TouchGesture.Left:
                moveDir.x = -1;
                break;
            case TouchGesture.Up:
                moveDir.y = -1;
                break;
            case TouchGesture.Right:
                moveDir.x = 1;
                break;
            case TouchGesture.Down:
                moveDir.y = 1;
                break;
        }

        StartCoroutine(PlayerMoveAnimated(_player.pos, moveDir, 1));
    }
    void PlayerMoveCanCurve(TouchGesture gesture)
    {

        Vector2Int moveDir = new Vector2Int(0, 0);
        switch (gesture)
        {
            case TouchGesture.Left:
                moveDir.x = -1;
                break;
            case TouchGesture.Up:
                moveDir.y = -1;
                break;
            case TouchGesture.Right:
                moveDir.x = 1;
                break;
            case TouchGesture.Down:
                moveDir.y = 1;
                break;
        }


        PlayerMoveCanCurve(_player.pos, moveDir);
    }
    void PlayerMoveCanCurve(Vector2Int pos, Vector2Int move)
    {
        if (move.sqrMagnitude != 0)
        {
            Vector2Int ind = pos;
            if (FieldIndexCheck(ind, move))
            {//次にいけるか
                ind = ind + move;
                if (_field[ind.x, ind.y].isUnlock)
                {
                    //ブロック以外
                    //  Debug.Log("" + ind.x + "," + ind.y);
                    _player.pos = ind;
                    Feeding(ind);
                }
            }

        }

    }
    IEnumerator PlayerMoveAnimated(Vector2Int pos, Vector2Int move, int waitFrame)
    {
        if (move.sqrMagnitude != 0)
        {
            Vector2Int ind = pos;
            while (FieldIndexCheck(ind, move))
            {//次にいけるか
                ind = ind + move;
                if (_field[ind.x, ind.y].isUnlock)
                {
                    //ブロック以外
                    //  Debug.Log("" + ind.x + "," + ind.y);
                    _player.pos = ind;
                    Feeding(ind);
                    for (int i = 0; i < waitFrame; i++)
                    {
                        yield return null;
                    }
                }
                else
                {//通行不能ブロック
                    break;
                }
            }

        }

        AnimationEndRequest();
    }
#endif

#endregion
#region 未分類

//public void UpdateAnimationTime(float time)
//{
//_player._animationTime = time;
//}
//public void PlayerMoveWithAnimation()
//{
//_player.PlayerMoveAnimation(ref _playerMoveDirection, ref _playerMoveBuffer);
// }

#if false
/// <summary>
/// 初期化時に通行可能ブロックを作る
/// </summary>
    public void DigoutBlock(Vector2Int pos, Vector2Int aim,Vector2Int first,int num)
    {
        Vector2Int direction = new Vector2Int(0,0);
        if (aim.x != pos.x)
        {
            direction.x = (int) Mathf.Sign(aim.x - pos.x);
        }
        else if (aim.y!=pos.y)
        {
            direction.y = (int)Mathf.Sign(aim.y- pos.y);
        }
        
        if (direction.magnitude > 0 && FieldIndexCheck(pos, direction)&&!IsFieldPassableSurround(pos))
        {
            DigoutBlock(pos+direction, aim,first,num);
        }
        else
        {
            if (num > 0)
            {
                if(num!=1)DigoutBlock(pos + direction, new Vector2Int(Random.Range(0, _width), Random.Range(0, _height)),first, num-1);
                else DigoutBlock(pos + direction, first,first, num - 1);

            }
        }
        _field[pos.x, pos.y].isPassable = true;
        _field[pos.x, pos.y].isUnlock = true;
    }
#endif
/*  public Vector2Int PlayerPosition()
  {
      return _player.pos;
  }*/
/*
void GetTouchGesture(int i,TouchGesture gesture)
{
    if (i == 0)
    {
        //NonAnimated
        //PlayerMove(gesture);

        //Animated
        //if (AnimationStartRequest())
        //{
        //   PlayerMoveAnimated(gesture);
        //}

        //Normal
        if (gesture != TouchGesture.None) {
            if (_playerMoveDirection == TouchGesture.None)
            {//最初だけ
                _playerMoveDirection = gesture;
            }

            //if (_playerMoveBuffer == TouchGesture.None)
            {//次に行く方向
                _playerMoveBuffer = gesture;
            }
        }
//			PlayerMoveCanCurve(gesture);
    }
}*/
/*
void DebugField()
{
    for (int j = 0; j < _height; j++)
    {
        string s = "";
        for (int i = 0; i < _width; i++)
        {
            if (_player.pos == new Vector2Int(i, j))
            {
                s += "v";
            }
            else
            {
                switch (_field[i, j].isUnlock && _field[i, j].isPassable)
                {
                    case true:
                        s += "  ";
                        break;
                    case false:
                        s += "[]";
                        break;
                    //case 20:
                    //  s += "v";
                    //  break;
                }
            }

        }

        s += "/";
        DisplayLog.Log(s);
    }

}
*/
#endregion
#endregion