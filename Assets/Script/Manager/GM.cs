using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GM : MonoBehaviour
{
    void Awake()
    {
        GameData.Instance._gm = this;        
    }

    public CameraFollow _camera_follow;
    public int _stage;
    public int _score;
    public int _safety;
    public int _foul;
    public int _miss;

    public void OnStartSetting()
    {
        _stage = 1;
        _score = 0;
        _safety = 0;
        _foul = 0;
        _miss = 0;

        OnStage();

        OnHomrunCameraReset();
        GameData.Instance._ball.OnBallReset();
        GameData.Instance._ui.OnGuideReset();        
    }    

    public void OnPitching()
    {
        GameData.Instance._ball.OnPitching();
    }

    public void OnHitting()
    {
       GameData.Instance._ball.OnHitting();
    }

    public void OnHomrunCamera(Transform ball)
    {
        if(_camera_follow._target == null) _camera_follow._target = ball;
        GameData.Instance._ui.OnObject(2, false);
        _camera_follow.gameObject.SetActive(true);
    }

    public void OnHomrunCameraReset()
    {
        GameData.Instance._ui.OnObject(2, true);
        _camera_follow.gameObject.SetActive(false);
    }

    public void OnStage()
    {
        GameData.Instance._ui.OnText(0, string.Format("{0}/10 구" , _stage));
        GameData.Instance._ui.OnText(1, string.Format("홈런 개수\n{0}개" , _score));
    }

    public void OnNextStage()
    {
        GameData.Instance._ball.OnBallReset();
        GameData.Instance._ui.EndAnnounce();//아나운스 종료

        _stage++;

        if(_stage > 10)
        {
            _start_trigger = false;
            OnGameResult();
        }
        else  
        {
            OnStage();
            GameData.Instance._ui.OnTimer(GameData.Instance._ball.OnPitching, 0.5f);
        }       
    }   

    bool _start_trigger;

    public void OnGameStart()
    {
        if(_start_trigger) return;
        _start_trigger = true;

        OnStartSetting();

        GameData.Instance._ui.OnObject(0, false);
        

        GameData.Instance._ui.OnStartCount(GameData.Instance._ball.OnPitching);
    }

    public void OnGameResult()
    {        
        GameData.Instance._ui.OnText(3, string.Format("Homerun : {0}\nSafety : {1}\nFoul : {2}\nMiss : {3}", _score, _safety, _foul, _miss));
        GameData.Instance._ui.OnObject(1, true);
        GameData.Instance._ui.EndAnnounce();
    }

    public void OnRestart()
    {        
        GameData.Instance._ui.OnObject(1, false);
        OnGameStart();
    }

    public void OnTitle()
    {
        GameData.Instance._ui.OnObject(0, true);
        GameData.Instance._ui.OnObject(1, false);
    }
}
