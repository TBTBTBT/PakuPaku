using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSpriteManager : MonoBehaviour
{
    public SpriteRenderer _feedRenderer;
    public GameObject[] _walls;
    public Sprite[] _feedSprites;
	// Use this for initialization
	void Start () {
		
	}

    public void ChangeFeedSprite(int i)
    {
        if (i >= 0 && _feedSprites.Length > i)
        {
            _feedRenderer.sprite = _feedSprites[i];
        }
    }

    public void ChangeBlockState(bool my,bool left,bool up,bool right,bool down)
    {
        if (my && _walls.Length>3)
        {
            _walls[0].SetActive(!left);
            _walls[1].SetActive(!up);
            _walls[2].SetActive(!right);
            _walls[3].SetActive(!down);
        }
    }

}
