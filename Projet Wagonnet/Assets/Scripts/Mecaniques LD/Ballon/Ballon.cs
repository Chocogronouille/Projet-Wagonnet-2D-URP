using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Ballon : MonoBehaviour
{
    [SerializeField] private float gravityScaleBallon;
    [SerializeField] private float duréeBallon;
    private Rigidbody2D _rbPlayer;
    private PlayerInput _playerInput;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rbPlayer = other.GetComponentInParent<Rigidbody2D>();
        _playerInput = other.GetComponentInParent<PlayerInput>();
        
        _playerInput.isAirborn = false;
        
        if (_rbPlayer.velocity.y < _playerInput.apexThreshold)
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, _playerInput.apexThreshold*1.1f);
        }
        _rbPlayer.gravityScale = gravityScaleBallon;
        
        StartCoroutine(UtilisationBallon());
    }

    private IEnumerator UtilisationBallon()
    {
        yield return new WaitForSecondsRealtime(duréeBallon);
        if (!_playerInput.isAirborn)
        {
            _playerInput.isAirborn = true;
            _playerInput.canSpinJump = true;
            _rbPlayer.gravityScale = 1;
        }
        StopCoroutine(UtilisationBallon());
    }
}
