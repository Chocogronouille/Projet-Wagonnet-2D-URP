using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Player;
using UnityEngine;

public class Ballon : MonoBehaviour
{
    [SerializeField] private float gravityScaleBallon;
    [SerializeField] private float duréeBallon;
    [SerializeField] private float duréeReapparition;
    [SerializeField] private float vitesseMaxY;
    [SerializeField] private Transform ballonFolder;
    [SerializeField] private float offsetX;
    private Rigidbody2D _rbPlayer;
    private PlayerInput _playerInput;
    private Vector3 _oldPos;
    private bool _oldPosInstance;
    private bool _asJumped;

    public Animator animator;
  //  animator.SetBool("isFlying",true);
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rbPlayer = other.GetComponentInParent<Rigidbody2D>();
        _playerInput = other.GetComponentInParent<PlayerInput>();
        _playerInput.ResetSpinJump();
        if (_oldPosInstance) return;
        _oldPosInstance = true;
        _oldPos = transform.position;

        transform.SetParent(other.transform);
        transform.localPosition = new Vector2(offsetX,transform.localPosition.y);

        StartCoroutine(UtilisationBallon());
    }

    private IEnumerator UtilisationBallon()
    {
        animator.SetBool("isFlying",true);
        PlayerInput.instance.animator.SetBool("isBallon",true);
        _playerInput.isAirborn = false;
        _playerInput.jumpState = PlayerInput.JumpState.Ballon;
        //GetComponent<SpriteRenderer>().DOColor(Color.red, duréeBallon);
        
        _rbPlayer.gravityScale = gravityScaleBallon;
        if (_rbPlayer.velocity.y < _playerInput.apexThreshold)
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, _playerInput.apexThreshold);
        }
        else
        {
            _rbPlayer.velocity = new Vector2(_rbPlayer.velocity.x, Mathf.Clamp(_rbPlayer.velocity.y,0f,vitesseMaxY));
        }
        
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(duréeBallon);

        if (_asJumped)
        {
            _asJumped = false;
        }
        else
        {
            _playerInput.isAirborn = true;
            _rbPlayer.gravityScale = _playerInput.defaultGravityScale;
            StartCoroutine(ReapparitionBallon());
        }
        StopCoroutine(UtilisationBallon());
    }

    public void JumpFromBallon() //Fonction appellée dans le PlayerInput
    {
        _asJumped = true;
        _playerInput.ResetSpinJump();
        StartCoroutine(ReapparitionBallon());
     //   animator.SetBool("isFlying",false);
        animator.SetBool("isExplode",true);
     
        PlayerInput.instance.animator.SetBool("isBallon",false);
    }

    private IEnumerator ReapparitionBallon()
    {
        transform.SetParent(ballonFolder);
        yield return new WaitForSeconds(0.16f);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.position = _oldPos;
        transform.rotation = Quaternion.Euler(0,0,0);
        _oldPosInstance = false;
        animator.SetBool("isFlying",false);
        PlayerInput.instance.animator.SetBool("isBallon",false);

        yield return new WaitForSeconds(0.01f);
        animator.SetBool("isExplode",false);

        yield return new WaitForSeconds(duréeReapparition);
        
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        
        StopCoroutine(ReapparitionBallon());
    }
}
