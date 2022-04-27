using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class NewEjection : MonoBehaviour
{
    PlayerInput Player => PlayerInput.instance;
    
    [SerializeField] private float ejectionForce;
    [SerializeField] private float ejectionDuration;
    
    [SerializeField] private Transform origin;
    [SerializeField] private Transform end;

    private Vector2 _ejectionDirection;
    private bool _instance;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_instance) return;
        _instance = true;
        StartCoroutine(EjectionTime());
    }

    private IEnumerator EjectionTime()
    {
        DOTween.KillAll();
        _ejectionDirection = end.position - origin.position;
        _ejectionDirection.Normalize();
        Player.transform.localEulerAngles = new Vector3(0,0,0);
        Player.isEject = true;
        Player.GetComponent<Rigidbody2D>().AddForce(_ejectionDirection*ejectionForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(ejectionDuration);
        Player.isEject = false;
        Player.isSurfing = false;
        _instance = false;
        StopCoroutine(EjectionTime());
    }
}