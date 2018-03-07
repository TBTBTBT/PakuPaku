using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Audio.Google;

/// <summary>
/// ゲームの見た目にかかわる処理
/// </summary>
public class GameViewManager : MonoBehaviour {
    public GameObject _blockParent;
    public GameObject _playerPrefab;
    public GameObject _blockPrefab;
    GameObject _playerObj;
    GameObject[,] _fieldObj;

    private float _width;

    private float _height;
    // Use this for initialization
    void Start ()
    {
        GameManager game = GameManager.Instance;
        _width = (float)game._width;
        _height = (float)game._height;
        PlayerViewInit(game);
        FieldViewInit(game);
    }

    #region プレイヤー関係


    /// <summary>
    /// プレイヤーの見た目（GameObject）の初期化
    /// </summary>
    void PlayerViewInit(GameManager game)
    {
        _playerObj = GameObject.Instantiate(_playerPrefab, IndexToPosition(game.PlayerPosition()), Quaternion.identity);
    }

    void PlayerViewUpdate(GameManager game)
    {
        _playerObj.transform.position = IndexToPosition(game.PlayerPosition());
    }
    

    #endregion

    void FieldViewInit(GameManager game)
    {
        _fieldObj = new GameObject[game._width, game._height];
        for (int i = 0; i < game._width; i++)
        {
            for (int j = 0; j < game._height; j++)
            {
                _fieldObj[i, j] = GameObject.Instantiate(_blockPrefab, _blockParent.transform);
                _fieldObj[i, j].transform.position = IndexToPosition(new Vector2Int(i, j));
                _fieldObj[i, j].transform.position = new Vector3(_fieldObj[i, j].transform.position.x, _fieldObj[i, j].transform.position.y,2);
                bool my = !game.IsFieldPassable(i,j);
                bool left  = !game.IsFieldPassable(i - 1, j);
                bool up    = !game.IsFieldPassable(i, j - 1);
                bool right = !game.IsFieldPassable(i + 1, j);
                bool down  = !game.IsFieldPassable(i, j + 1);
                _fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeBlockState(my,left,up,right,down);
                /*switch(game.FieldState(i, j))
                {
                    case 10:
                        _fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeBlockState(1);
                        break;
                    default:
                        _fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeBlockSprite(0);
                        break;
                }*/
            }
        }

    
    }

    Vector2 IndexToPosition(Vector2Int pos)
    {
        float fx = (float) pos.x;
        float fy = (float) pos.y;
        Vector2 ret  = new Vector2(fx - _width/2, -(fy - _height / 2));
        return ret;
    }
	// Update is called once per frame
    void Update()
    {
        GameManager game = GameManager.Instance;
        PlayerViewUpdate(game);
        for (int i = 0; i < game._width; i++)
        {
            for (int j = 0; j < game._height; j++)
            {
                _fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeFeedSprite(game.FieldState(i,j));

            }
        }
    }
}
