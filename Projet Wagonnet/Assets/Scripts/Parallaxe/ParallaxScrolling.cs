using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    private float _length, _startPosition, _distance, _ecartParallax;
    private Vector3 _newPosition;
    
    public float parallaxFactor;
    public GameObject camPlayer;

    void Start()
    {
        _startPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    
    void Update()
    {
        var positionCamPlayer = camPlayer.transform.position;
        
        _ecartParallax = positionCamPlayer.x * (1 - parallaxFactor);
        _distance = positionCamPlayer.x * parallaxFactor;
        _newPosition = new Vector3(_startPosition + _distance, transform.position.y, transform.position.z);
        transform.position = _newPosition;

        if (_ecartParallax>_startPosition + (_length/2))
        {
            _startPosition += _length;
        }
        else if(_ecartParallax < _startPosition - (_length/2))
        {
            _startPosition -= _length;
        }
    }
}
