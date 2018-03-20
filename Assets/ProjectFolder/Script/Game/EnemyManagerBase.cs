using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerBase : CharacterOnField
{
    private MoveDirection _moveDirection;
    private MoveDirection _moveBuffer;
    private float speed = 0.05f;

    //さっき向き変更した
    private bool _changeDirNow = false;

    /// <summary>
    /// ステート一覧の初期化
    /// </summary>
    protected override void SetStates()
    {
        _states = new[]
        {
            "Wait",
            "WalkLeft",
            "WalkUp",
            "WalkRight",
            "WalkDown"
        };
    }

    #region 移動



    void Move()
    {
        GameManager game = GameManager.Instance;

        Vector2Int dir = DirectionToVector2Int(_moveDirection);
        Vector2Int buf = DirectionToVector2Int(_moveBuffer);

        Vector2Int pos = game.PositionToIndex(transform.position);
        bool left = game.IsFieldPassable(pos + new Vector2Int(-1, 0));
        bool up = game.IsFieldPassable(pos + new Vector2Int(0, -1));
        bool right = game.IsFieldPassable(pos + new Vector2Int(1, 0));
        bool down = game.IsFieldPassable(pos + new Vector2Int(0, 1));
        if (CanMove(left, up, right, down, buf))
        {
            dir = buf;
            _moveDirection = _moveBuffer;
            ChangeDirection(_moveBuffer);
        }
        transform.position += (Vector3)(Vector2)dir * speed;
        Gravitation(left, up, right, down, pos, 0.1f);
    }


    #endregion

    #region AI
    void AIInput(MoveDirection dir)
    {
        if (dir != MoveDirection.None)
        {
            if (_moveDirection == MoveDirection.None)
            {
                ChangeDirection(dir);
                _moveDirection = dir;

            }

            {
                _moveBuffer = dir;

            }
        }
        else
        {
            _moveDirection = dir;
            _moveBuffer = dir;
            ChangeDirection(dir);
        }
    }
    void AI()
    {
        GameManager game = GameManager.Instance;

        if (_moveDirection == MoveDirection.None)
        {
            AIInput(MoveDirection.Left);
        }
        Vector2Int pos = game.PositionToIndex(transform.position);
        Vector2Int dir = DirectionToVector2Int(_moveDirection);
        bool[] d = new bool[4] {false,false,false,false};
         d[0] = game.IsFieldPassable(pos + new Vector2Int(-1, 0));
         d[1] = game.IsFieldPassable(pos + new Vector2Int(0, -1));
         d[2] = game.IsFieldPassable(pos + new Vector2Int(1, 0));
        d[3] = game.IsFieldPassable(pos + new Vector2Int(0, 1));

        Vector2 cpos = game.IndexToPosition(pos);
        Vector2 tpos = transform.position;
        if ((cpos - tpos).magnitude < 0.2f)
        {
            if (!CanMove(d[0], d[1], d[2], d[3], dir))
            {
                AIInput((MoveDirection)Random.Range(0, 4));
            }
            int passCount = 0;
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i]) passCount++;
            }

            //交差点なら
            if (passCount > 2 && !_changeDirNow)
            {
                AIInput((MoveDirection) Random.Range(0, 4));
                _changeDirNow = true;
            }

            if (passCount <= 2)
            {
                _changeDirNow = false;
            }
        }
    }

    #endregion
    // Use this for initialization
    void Start () {
		
	}

    bool DigCheck()
    {
        GameManager game = GameManager.Instance;
        return game.IsDug(transform.position);
    }
    // Update is called once per frame
    void Update ()
	{
	    AI();
        Move();
	    if (DigCheck())
	    {
	        GameManager game = GameManager.Instance;
            game.GraveStamp(transform.position);
            Destroy(gameObject);
	    }

	}
}
