using System;
using UnityEngine;

public class triggerFondFin : MonoBehaviour
{
    [SerializeField] private GameObject parallaxe;

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var variableParallaxScrolling in parallaxe.GetComponentsInChildren<ParallaxScrolling>())
        {
            variableParallaxScrolling.parallaxFactor = 0;
        }
    }
}
