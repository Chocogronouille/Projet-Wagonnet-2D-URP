using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DébutDesCrédits : MonoBehaviour
{

    public GameObject credits;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        credits.SetActive(true);
    }
}
