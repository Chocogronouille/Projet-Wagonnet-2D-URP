using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AnimationRail : MonoBehaviour
{
    [SerializeField] private GameObject lightRail;
    [SerializeField] private Vector2 offsetLight;
    [SerializeField] private float animSpeedSecond;

    private Transform _thisTransform;
    private int _tileAmount;
    private float _tileScale;
    private float _startPositionX;
    private Vector3 _clonePosition;
    private SpriteRenderer[] _lights;
    private int _centerClone;
    private bool _stop;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;
        _tileAmount = Mathf.RoundToInt(GetComponent<SpriteRenderer>().size.x);
        _lights = new SpriteRenderer[_tileAmount];
        _tileScale = transform.localScale.x;
        _startPositionX = -_tileAmount / 2f + _tileAmount*_tileScale/2f;

        for (int i = 0; i < _tileAmount; i++)
        {
            _clonePosition = new Vector3(_startPositionX + offsetLight.x + i, transform.localPosition.y + offsetLight.y,0f);
            GameObject clone = Instantiate(lightRail, _thisTransform);
            clone.transform.localPosition = _clonePosition;
            _lights[i] = clone.GetComponent<SpriteRenderer>();
        }

        foreach (var vGameObject in _lights)
        {
            vGameObject.enabled = false;
        }

        StartCoroutine(Animation());
    }

    private void StopAnimation()
    {
        _stop = true;
    }

    private IEnumerator Animation()
    {
        
        while (!_stop)
        {
            if (_centerClone < 3)
            {
                StartAnim();
                _centerClone++;
            }
            else if (_centerClone == _tileAmount-1)
            {
                EndAnim();
                _centerClone = 0;
            }
            else
            {
                Anim();
                _centerClone++;
            }
            yield return new WaitForSeconds(animSpeedSecond);
        }
    }

    private void StartAnim()
    {
        switch (_centerClone)
        {
            case 0:
                _lights[_tileAmount-2].enabled = false;
                _lights[_tileAmount-1].enabled = true;
                _lights[_centerClone].enabled = true;
                _lights[_centerClone+1].enabled = true;
                break;
            
            case 1:
                _lights[_tileAmount-1].enabled = false;
                _lights[_centerClone+1].enabled = true;
                break;
            
            case 2:
                _lights[_centerClone-2].enabled = false;
                _lights[_centerClone+1].enabled = true;
                break;
        }
    }
    
    private void Anim()
    {
        _lights[_centerClone-2].enabled = false;
        _lights[_centerClone + 1].enabled = true;
    }
    
    private void EndAnim()
    {
        _lights[_centerClone-2].enabled = false;
        _lights[0].enabled = true;
    }
}
