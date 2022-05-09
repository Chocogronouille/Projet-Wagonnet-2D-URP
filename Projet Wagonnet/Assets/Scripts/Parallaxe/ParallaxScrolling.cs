using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    private float _length, _startPositionX, _startPositionY, _distanceX, _distanceY, _ecartParallax;
    private Vector3 _newPosition;
    
    public float parallaxFactor;
    public GameObject camPlayer;

    void Start()
    {
        _startPositionX = transform.position.x;
        _startPositionY = transform.position.y;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
        var positionCamPlayer = camPlayer.transform.position;
        
        _ecartParallax = positionCamPlayer.x * (1 - parallaxFactor);
        _distanceX = positionCamPlayer.x * parallaxFactor;
        _distanceY = positionCamPlayer.y * parallaxFactor;
        _newPosition = new Vector3(_startPositionX + _distanceX, _startPositionY + _distanceY, transform.position.z);
        transform.position = _newPosition;
 
        if (_ecartParallax>_startPositionX + (_length/2))
        {
            _startPositionX += _length;
        }
        else if(_ecartParallax < _startPositionX - (_length/2))
        {
            _startPositionX -= _length;
        }
    }
}
