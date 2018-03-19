using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpriteManager : AnimationManagerBase
{
    public GameObject[] _walls;
    //public Animator _road;

	/// <summary>
	/// ステート一覧の初期化
	/// </summary>
	void SetStates()
	{
		_states = new []
		{
			"Fill",
			"Dug"
		};
	}

	void Start(){
		base.Start ();
		SetStates ();
	}

	public void ChangeBlockDigState(bool isDug){
		if (isDug) {
			ChangeState (1);
		}
		if (!isDug) {
			ChangeState (0);
		}
	}
    public void ChangeBlockPassableState(bool my,bool left,bool up,bool right,bool down)
    {
        if (my && _walls.Length>4)
        {
            _walls[4].SetActive(false);
            _walls[0].SetActive(!left);
            _walls[1].SetActive(!up);
            _walls[2].SetActive(!right);
            _walls[3].SetActive(!down);
        }

        if (!my)
        {
            for (int i = 0; i < _walls.Length; i++)
            {
                _walls[i].SetActive(false);

                if(i == 4) _walls[i].SetActive(true);
            }
            //_road.gameObject.SetActive(false);
        }
    }

}
