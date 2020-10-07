using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    void Awake()
    {
        GameData.Instance._ui = this;
    }

    void Update()
    {
        if(_guide_trigger)
        {
            OnHittingGuide();
        }
    }

    public GameObject _guide;
    public Image _target_guide;

    bool _guide_trigger;
    public void OnGuideReset()
    {
        _guide_trigger = false;
        _guide.SetActive(false);

        _target_guide.transform.localScale = new Vector3(3, 3, 1);

        OnObject(3, false);
    }

    public void OnGuideStart()
    {
        _guide.SetActive(true);
        _guide_trigger = true;

        OnObject(3, true);
    }

    float _guide_size;
    public List<Text> _txt = new List<Text>();
    public void OnHittingGuide()
    {   
        _guide_size = 1f + (2f * GameData.Instance._ball._ball_range * GameData.Instance._ball._ball_range_max);

        _target_guide.transform.localScale = new Vector3(_guide_size, _guide_size, 1);
    }

    public List<GameObject> _anounce_result = new List<GameObject>();
    GameObject _anounce;

    public void EndAnnounce()
    {
        if(_anounce != null) _anounce.SetActive(false);
    }
    public void OnResult(int code)
    {
        _anounce = _anounce_result[code];
        _anounce.SetActive(true);
    }

    public void OnText(int code, string text)
    {
        _txt[code].text = text;
    }

    public delegate void Call();

    public void OnStartCount(Call call)
    {
        _txt[2].transform.localScale = new Vector3(1.5f, 1.5f, 1);
        OnText(2, "3");
        _txt[2].transform.DOScale(Vector3.one, 1).SetEase(Ease.Linear).OnComplete(()=>{
            _txt[2].transform.localScale = new Vector3(1.5f, 1.5f, 1);
            OnText(2, "2");
            _txt[2].transform.DOScale(Vector3.one, 1).SetEase(Ease.Linear).OnComplete(()=>{
                _txt[2].transform.localScale = new Vector3(1.5f, 1.5f, 1);
                OnText(2, "1");
                _txt[2].transform.DOScale(Vector3.one, 1).SetEase(Ease.Linear).OnComplete(()=>{
                    OnText(2, "");
                    call();
                }); 
            }); 
        }); 
    }

    public void OnTimer(Call call, float time)
    {
        transform.DOScale(Vector3.one, time).OnComplete(()=>{                    
            call(); 
        });
    }

    public List<GameObject> _obj = new List<GameObject>();

    public void OnObject(int code, bool show)
    {
        _obj[code].SetActive(show);
    }
}
