using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpriteManager : MonoBehaviour
{
    public GameObject[] _walls;
    public Animator _road;
	// Use this for initialization
	void Start () {
		
	}


    public void ChangeRoadState(int state)
    {
        //_road .SetActive(true);

    }
    public void ChangeBlockState(bool my,bool left,bool up,bool right,bool down)
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
