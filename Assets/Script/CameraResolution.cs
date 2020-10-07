using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Awake()
    {
        Camera _camera = GetComponent<Camera>();
        Rect _rect = _camera.rect;
        
        float _scale_height = ((float)Screen.width / Screen.height) / (16f/9f);
        float _scale_width = 1 / _scale_height;

        if(_scale_height < 1)
        {
            _rect.height = _scale_height;
            _rect.y = (1 - _scale_height) / 2f;
        }
        else
        {
            _rect.width = _scale_width;
            _rect.x = (1f - _scale_width) / 2f;
        }
        
        /*
        float _scale_height = ((float)Screen.width / Screen.height) / (16f/9f);
        float _scale_width = 1 / _scale_height;

        if(_scale_height < 1)
        {
            _rect.height = _scale_height;
            _rect.y = (1 - _scale_height) / 2f;
        }
        else
        {
            _rect.width = _scale_width;
            _rect.x = (1f - _scale_width) / 2f;
        }
        */
        _camera.rect = _rect;
    }
}
