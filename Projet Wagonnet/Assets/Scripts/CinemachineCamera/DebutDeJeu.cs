using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebutDeJeu : MonoBehaviour
{

    private InputActions farmerInputActions;
    public Animator fadeSystem;
    private bool _isActivated;
    [SerializeField] private GameObject player;


    public void Awake()
    {
        StartCoroutine(startScene());
    }

    public IEnumerator startScene()
    {
        fadeSystem.SetTrigger("DebutDeJeu");
        yield return new WaitForSeconds(1f);
    }
}

