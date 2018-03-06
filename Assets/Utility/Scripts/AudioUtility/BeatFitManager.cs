using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatFitManager : MonoBehaviour {
    public AudioSource audio;
    public float bpm = 175f;
    //x秒に一回というやつ
    //private float sec = 15;
    void Start () {
		
	}
    void Update()
    {

    }
    //再生時間
    public float GetAudioTime()
    {
        return audio.time;
    }
    //再生開始からの拍数
    //timing : 拍の間隔 標準15くらい
    public int GetBeatNum(float timing)
    {
        return (int)Mathf.Floor((audio.time) / (timing / bpm));
    }
    //拍からどのくらいずれているか
    //0~1 1に近いほどずれている。
    public float GetBeatGap(float timing)
    {
        return ((audio.time) % (timing / bpm)) / (timing / bpm);
    }
    
}
