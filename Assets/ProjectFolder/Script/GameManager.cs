using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEditor;
using UnityEngine;
/// <summary>
/// フィールド1マス単位に必要な情報
/// </summary>
public class FieldInfo
{
    public int floorNum = 0;//床の状態を表す。 0:何もなし 1~:エサ

    public int feedSpawnTime = 0;//エサが現れてからの経過時間

    //↓※オブジェクトが各床に対して必ず1つしか存在しない場合のみ使用 複数ある場合はオブジェクトに場所情報を持たせる形で
    //public int objectNum;//存在するオブジェクト 
    public bool isUnlock = false; // 通れるかかどうか
}
/// <summary>
/// フィールドに存在するオブジェクトのベースクラス
/// </summary>
public class ObjectBase
{
    //座標
    public Vector2Int pos;
}
public class GameManager : SingletonMonoBehaviourCanDestroy<GameManager>
{
    [System.NonSerialized]
    public int _width = 15;
    [System.NonSerialized]
    public int _height = 15;
	
	//じゃぐ配列にするか検討
    FieldInfo[,] _field;
    ObjectBase _player = new ObjectBase();
    

    //今アニメーション中か
    private bool isAnimated = false;
    void Awake()
    {
        
        base.Awake();
        _field = new FieldInfo[_width, _height];
        FieldInit();
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
                Debug.Log(y);
                _field[i, j].isUnlock = FieldData.data[y,x] == 0;
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
        _player.pos = new Vector2Int(_width / 2,_height/2 + 1);
    }
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

        _field[pos.x, pos.y].isUnlock = true;
    }
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
    public Vector2Int PlayerPosition()
    {
        return _player.pos;
    }
    void GetTouchGesture(int i,TouchGesture gesture)
    {
        if (i == 0)
        {
            //NonAnimated
            //PlayerMove(gesture);
            
            //Animated
            if (AnimationStartRequest())
            {
                PlayerMoveAnimated(gesture);
            }
        }
    }

    #region エサ関係

    void FeedSpawner()
    {
        int x = Random.Range(0, _width);
        int y = Random.Range(0, _height);
        if (_field[x, y].isUnlock)
        _field[x, y].floorNum = 1;
    }

    void Feeding(Vector2Int pos)
    {
        if (_field[pos.x, pos.y].floorNum > 0)
        {
            _field[pos.x, pos.y].floorNum = 0;
            _field[pos.x, pos.y].feedSpawnTime = 0;
        }
    }

    #endregion
    void PlayerMove(TouchGesture gesture)
    {
        Vector2Int moveDir = new Vector2Int(0,0);
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

        _player.pos =  PlayerMove(_player.pos, moveDir);
    }

    public bool AnimationStartRequest()
    {
        if (!isAnimated)
        {
            isAnimated = true;
            return true;
        }

        return false;
    }
    
   public void AnimationEndRequest()
    {
        isAnimated = false;
    }
    public int FieldState(int x,int y)
    {
        return _field[x, y].floorNum;
    }

    public bool IsFieldPassable(Vector2Int pos)
    {
        return IsFieldPassable(pos.x, pos.y);
    }
    public bool IsFieldPassable(int x, int y)
    {
        if(FieldIndexCheck(x,y))
        return _field[x, y].isUnlock;
        return false;
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

        StartCoroutine(PlayerMoveAnimated(_player.pos, moveDir,1));
    }
    IEnumerator PlayerMoveAnimated(Vector2Int pos, Vector2Int move,int waitFrame)
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
	// Use this for initialization
    void Start()
    {
        EventManager.OnTouchGesture.AddListener(GetTouchGesture);
        
    }

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
                    switch (_field[i, j].isUnlock)
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
	// Update is called once per frame
	void Update ()
	{
	    if(Random.Range(0,100) == 0)FeedSpawner();

        DebugField();

	}
}
