using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Audio.Google;

/// <summary>
/// ゲームの見た目にかかわる処理
/// </summary>
public class GameViewManager : SingletonMonoBehaviourCanDestroy<GameViewManager> {
    public GameObject _blockParent;
    
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
        //PlayerViewInit(game);
        FieldViewInit(game);
        game.OnChangeField.AddListener(OnChangeField);
    }

   

    void FieldViewInit(GameManager game)
    {
        _fieldObj = new GameObject[game._width, game._height];
        for (int i = 0; i < game._width; i++)
        {
            for (int j = 0; j < game._height; j++)
            {
                _fieldObj[i, j] = GameObject.Instantiate(_blockPrefab, _blockParent.transform);
                _fieldObj[i, j].transform.position = game.IndexToPosition(new Vector2Int(i, j));
                _fieldObj[i, j].transform.position = new Vector3(_fieldObj[i, j].transform.position.x, _fieldObj[i, j].transform.position.y,2);
                bool my = game.IsFieldPassable(i,j);
                bool left  = game.IsFieldPassable(i - 1, j);
                bool up    = game.IsFieldPassable(i, j - 1);
                bool right = game.IsFieldPassable(i + 1, j);
                bool down  = game.IsFieldPassable(i, j + 1);
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
    /*
    public Vector2 IndexToPositionPlayerAnimated(Player p,float time)
    {
        Vector2 ret = new Vector2(0,0);
        ret = IndexToPosition(p.beforePos) + (IndexToPosition(p.pos) - IndexToPosition(p.beforePos)) * time;
        return ret;
    }
    */

    void OnChangeField()
    {
        GameManager game = GameManager.Instance;
        fieldViewUpdate(game);
    }
	// Update is called once per frame
    void fieldViewUpdate(GameManager game)
    {
        for (int i = 0; i < game._width; i++)
        {
            for (int j = 0; j < game._height; j++)
            {
                bool my = game.IsFieldPassable(i, j);
                bool left = game.IsFieldPassable(i - 1, j);
                bool up = game.IsFieldPassable(i, j - 1);
                bool right = game.IsFieldPassable(i + 1, j);
                bool down = game.IsFieldPassable(i, j + 1);
                _fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeBlockState(my, left, up, right, down);
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
    void Update()
    {
        GameManager game = GameManager.Instance;
        //PlayerViewUpdate(game);
        //for (int i = 0; i < game._width; i++)
        {
            for (int j = 0; j < game._height; j++)
            {
                //_fieldObj[i, j].GetComponent<FieldSpriteManager>().ChangeFeedSprite(game.FieldState(i,j));

            }
        }
    }
}

#region プレイヤー関係


/// <summary>
/// プレイヤーの見た目（GameObject）の初期化
/// </summary>
//void PlayerViewInit(GameManager game)
//{
//    _playerObj = GameObject.Instantiate(_playerPrefab, IndexToPosition(game.PlayerPosition()), Quaternion.identity);
//}

//void PlayerViewUpdate(GameManager game)
//{
//   _playerObj.transform.position = IndexToPositionPlayerAnimated(game._player);
//Vector3.Lerp(_playerObj.transform.position, IndexToPosition(game.PlayerPosition()), 0.3f);//IndexToPosition(game.PlayerPosition());
// }


#endregion