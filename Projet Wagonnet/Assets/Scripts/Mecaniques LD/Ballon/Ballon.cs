using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;

public class Ballon : MonoBehaviour
{
    [SerializeField] private float gravityScaleBallon;
    [SerializeField] private float duréeBallon;
    [SerializeField] private float vitesseMaxY;
    private Rigidbody2D _rbPlayer;
    private PlayerInput _playerInput;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rbPlayer = other.GetComponentInParent<Rigidbody2D>();
        _playerInput = other.GetComponentInParent<PlayerInput>();
        
        _playerInput.isAirborn = false;
        
        _rbPlayer.gravityScale = gravityScaleBallon;
        if (_rbPlayer.velocity.y < _playerInput.apexThreshold)
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, _playerInput.apexThreshold*1.1f);
        }
        else
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, Mathf.Clamp(_rbPlayer.velocity.y,0f,vitesseMaxY));
        }
        
        
        StartCoroutine(UtilisationBallon());
    }

    private IEnumerator UtilisationBallon()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(duréeBallon);
        GetComponent<BoxCollider2D>().enabled = true;
        if (!_playerInput.isAirborn)
        {
            _playerInput.isAirborn = true;
            _playerInput.canSpinJump = true;
            _rbPlayer.gravityScale = _playerInput.defaultGravityScale;
        }
        StopCoroutine(UtilisationBallon());
    }
}
