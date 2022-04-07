using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameLoad : MonoBehaviour
{
    public Animator debutDeJeu;
    // Start is called before the first frame update
    private void Awake()
    {
        debutDeJeu.SetTrigger("DebutDeJeu");
    }
}
