using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    void Awake()
    {
        GameData.Instance._ball = this;        
    }
    void Start()
    {
        _ball_range_max = 1 / Vector3.Distance(_target_point.localPosition, _ball_start_point.localPosition);
    }
    void Update()
    {
        if(_pitchiing)
        {
           _ball_range = Vector3.Distance(_target_point.localPosition, _ball.transform.localPosition);  
        }

    }

    public GameObject _ball;
    public Transform _ball_start_point;
    public Transform _target_point;

    public float _ball_range;
    public float _ball_range_max;
    public bool _pitchiing;

    bool _pitch_trigger;
    bool _hit_trigger;

    public List<Transform> _landing = new List<Transform>();

    public void OnBallReset()
    {
        _pitchiing = false;   
        _pitch_trigger = false;

        _ball.SetActive(false);
        _ball.transform.localPosition = _ball_start_point.localPosition;
        _ball_range = Vector3.Distance(_target_point.localPosition, _ball_start_point.localPosition);
    }

    public void OnPitching()
    {
        if(_pitchiing) return;

        if(_pitch_trigger) return;
        _pitch_trigger = true;

        _ball.SetActive(true);

        _pitchiing = true;
        GameData.Instance._ui.OnGuideStart();

        _hit_trigger = false;
        
        _ball.transform.DOLocalMoveZ(_target_point.localPosition.z - 2, 1f).SetId("pitching").SetEase(Ease.Linear).OnComplete(()=>{                     
            GameData.Instance._ui.OnGuideReset();

            GameData.Instance._gm._miss++;
            GameData.Instance._ui.OnResult(4);
            OnBallReset();

            GameData.Instance._ui.OnTimer(GameData.Instance._gm.OnNextStage, 0.5f);   
        });        
    }

    public void OnHitting()
    {
        if(!_pitchiing) return;

        if(_hit_trigger) return;
        _hit_trigger = true;
        
        DOTween.Pause("pitching");

        GameData.Instance._sound.Play_EffectSound(9);

        bool _right_vector = false;
        if(_target_point.localPosition.z > _ball.transform.localPosition.z) _right_vector = true;

        GameData.Instance._ui.OnGuideReset();

        float _hitting_range = Vector3.Distance(_target_point.localPosition, _ball.transform.localPosition);    
         
        if(_hitting_range < 0.5f) //퍼펙트 홈런
        {
            Debug.Log("퍼펙트 홈런 : _hitting_range : " + _hitting_range);

            GameData.Instance._gm._score++;
            GameData.Instance._gm.OnHomrunCamera(_ball.transform);    
            GameData.Instance._ui.OnResult(0);

            if(_right_vector)
            {
                _ball.transform.DOLocalJump(_landing[1].localPosition, 10, 1, 1f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{                    
                    GameData.Instance._gm.OnHomrunCameraReset();
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            else
            {
                _ball.transform.DOLocalJump(_landing[0].localPosition, 10, 1, 1f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnHomrunCameraReset();
                    GameData.Instance._gm.OnNextStage();                  
                }); 
            }
            return;
        }
        if(_hitting_range < 1f) //노멀 홈런
        {
            Debug.Log("노멀 홈런 : _hitting_range : " + _hitting_range);

            GameData.Instance._gm._score++;
            GameData.Instance._gm.OnHomrunCamera(_ball.transform); 
            GameData.Instance._ui.OnResult(1);

            if(_right_vector)
            {
                _ball.transform.DOLocalJump(_landing[3].localPosition, 5, 1, 1f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{                    
                    GameData.Instance._gm.OnHomrunCameraReset();
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            else
            {
                _ball.transform.DOLocalJump(_landing[2].localPosition, 5, 1, 1f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnHomrunCameraReset();
                    GameData.Instance._gm.OnNextStage();                  
                }); 
            }
            
            return;
        }
        if(_hitting_range < 1.5f) // 안타
        {
            Debug.Log("안타 : _hitting_range : " + _hitting_range);

            GameData.Instance._gm._safety++;
            GameData.Instance._ui.OnResult(2);

            if(_right_vector)
            {
                _ball.transform.DOLocalJump(_landing[5].localPosition, 3, 1, 0.75f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            else
            {
                _ball.transform.DOLocalJump(_landing[4].localPosition, 3, 1, 0.75f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            return;
        }

        if(_hitting_range < 3.5f)//파울
        {
            Debug.Log("파울 : _hitting_range : " + _hitting_range);

            GameData.Instance._gm._foul++;
            GameData.Instance._ui.OnResult(3);

            if(_right_vector)
            {
                _ball.transform.DOLocalJump(_landing[7].localPosition, 1, 1, 0.25f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            else
            {
                _ball.transform.DOLocalJump(_landing[6].localPosition, 1, 1, 0.25f).SetId("hit").SetEase(Ease.Linear).OnComplete(()=>{
                    GameData.Instance._gm.OnNextStage();
                }); 
            }
            return;
        }

        GameData.Instance._gm._miss++;
        GameData.Instance._ui.OnResult(4);
        OnBallReset();

        GameData.Instance._ui.OnTimer(GameData.Instance._gm.OnNextStage, 0.5f);
    }
}
