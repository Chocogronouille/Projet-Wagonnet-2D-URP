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
    [SerializeField] private float duréeReapparition;
    [SerializeField] private float vitesseMaxY;
    [SerializeField] private Transform ballonFolder;
    [SerializeField] private Vector3 offset;
    private Rigidbody2D _rbPlayer;
    private PlayerInput _playerInput;
    private Vector3 _oldPos;
    private bool _oldPosInstance;
    private bool _asJumped;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rbPlayer = other.GetComponentInParent<Rigidbody2D>();
        _playerInput = other.GetComponentInParent<PlayerInput>();
        if (_oldPosInstance) return;
        _oldPosInstance = true;
        _oldPos = transform.position;

        transform.SetParent(other.transform);
        transform.localPosition = offset;

        Debug.Log("StartCoroutine");
        StartCoroutine(UtilisationBallon());
    }

    private IEnumerator UtilisationBallon()
    {
        _playerInput.isAirborn = false;
        //_playerInput.isFalling = true;
        //_playerInput.ResetJumpDurationBallon();
        
        _rbPlayer.gravityScale = gravityScaleBallon;
        if (_rbPlayer.velocity.y < _playerInput.apexThreshold)
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, _playerInput.apexThreshold*1.1f);
        }
        else
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, Mathf.Clamp(_rbPlayer.velocity.y,0f,vitesseMaxY));
        }
        
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(duréeBallon);

        if (_asJumped)
        {
            _asJumped = false;
        }
        else
        {
            _playerInput.isAirborn = true;
            _rbPlayer.gravityScale = _playerInput.defaultGravityScale;
            Debug.Log("GravityScale dans Ballon");
            StartCoroutine(ReapparitionBallon());
        }
        StopCoroutine(UtilisationBallon());
    }

    public void JumpFromBallon()
    {
        _asJumped = true;
        StartCoroutine(ReapparitionBallon());
    }
    
    private IEnumerator ReapparitionBallon()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        transform.SetParent(ballonFolder);
        transform.position = _oldPos;
        _oldPosInstance = false;

        yield return new WaitForSecondsRealtime(duréeReapparition);
        
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        
        StopCoroutine(ReapparitionBallon());
    }
}
