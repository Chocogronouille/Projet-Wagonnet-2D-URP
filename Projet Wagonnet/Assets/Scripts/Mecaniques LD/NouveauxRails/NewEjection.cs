using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class NewEjection : MonoBehaviour
{
    public ParticleSystem Effects;
    PlayerInput Player => PlayerInput.instance;
    
    [SerializeField] private float ejectionForce;
    [SerializeField] private float ejectionDuration;
    
    [SerializeField] private Transform origin;
    [SerializeField] private Transform end;

    private Vector2 _ejectionDirection;
    private bool _instance;

    public ParticleSystem EjectEffects;

    void Start()
    {
         if(gameObject.tag == "Right")
        {
            EjectEffects = GameObject.Find("VFX_Sparks_End_Right").GetComponent<ParticleSystem>();
        }
         else if(gameObject.tag == "Left")
        {
            EjectEffects = GameObject.Find("VFX_Sparks_End_Left").GetComponent<ParticleSystem>();
        }
    //    EjectEffects = GameObject.Find("VFX_Sparks_End").GetComponent<ParticleSystem>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_instance) return;
        _instance = true;
        EjectEffects.Play();
        StartCoroutine(EjectionTime());
    }

    private IEnumerator EjectionTime()
    {
        DOTween.KillAll();
        
        _ejectionDirection = end.position - origin.position;
        Player.isEject = true;
        Player.transform.localEulerAngles = new Vector3(0,0,0);
        Player.rbCharacter.velocity = Vector2.zero;
        Player.rbCharacter.gravityScale = 0;
        Player.GetComponent<Rigidbody2D>().drag = 0;
        var ejectionSpeed = _ejectionDirection.normalized * ejectionForce;
        Player.rbCharacter.AddForce(ejectionSpeed,ForceMode2D.Impulse);
        Player.SetAirSpeedAfterRail(ejectionSpeed.x);
        yield return new WaitForSeconds(ejectionDuration);
        Player.ResetSpinJump();
        Player.rbCharacter.gravityScale = Player.defaultGravityScale;
        Player.isEject = false;
        Player.isSurfing = false;
        Player.isFalling = true;
        Player.groundCheck.SetActive(true);
        _instance = false;
        Debug.Log("Eject");
        yield return new WaitForSeconds(0.001f);
        Player.transform.localEulerAngles = new Vector3(0,0,0);
        StopCoroutine(EjectionTime());
    }
}