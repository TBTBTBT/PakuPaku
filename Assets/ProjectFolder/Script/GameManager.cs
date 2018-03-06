using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEngine;
/// <summary>
/// フィールド1マス単位に必要な情報
/// </summary>
public class FieldInfo
{
    public int floorNum　= 0;//床の状態を表す。 0:何もなし 10:カベ
    //↓※オブジェクトが各床に対して必ず1つしか存在しない場合のみ使用 複数ある場合はオブジェクトに場所情報を持たせる形で
    //public int objectNum;//存在するオブジェクト 
    

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
    private int _width = 20;
    private int _height = 20;
    //じゃぐ配列にするか検討
    FieldInfo[,] _field;
    ObjectBase _player = new ObjectBase();
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
            for (int j = 0; j < _height; j++)
            {
                bool isBlock = Random.Range(0, 10) == 0;
                _field[i, j].floorNum = isBlock ? 10 : 0;
                //if (i == _width / 2 && j == _height / 2)
                //{
                //    _field[i, j].floorNum = 20;
                //}
            }
        }
        _player.pos = new Vector2Int(_width / 2,_height/2);
    }
    void GetTouchGesture(int i,TouchGesture gesture)
    {
        if (i == 0)
        {
            PlayerMove(gesture);
        }
    }

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
    //trueなら大丈夫 falseならはみ出し
    bool FieldIndexCheck(Vector2Int pos, Vector2Int move)
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
    Vector2Int PlayerMove(Vector2Int pos, Vector2Int move)
    {
        if (move.sqrMagnitude != 0)
        {
            if (FieldIndexCheck(pos, move))
            {
                Vector2Int ind = pos + move;
                if (_field[ind.x, ind.y].floorNum != 10)
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
                    switch (_field[i, j].floorNum)
                    {
                        case 0:
                            s += "  ";
                            break;
                        case 10:
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
	    DebugField();

	}
}
