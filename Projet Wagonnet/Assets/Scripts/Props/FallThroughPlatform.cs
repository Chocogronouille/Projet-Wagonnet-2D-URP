using System;
using Cinemachine;
using UnityEngine;

namespace Props
{
    public class FallThroughPlatform : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("collide");
            var thisCollider = GetComponent<Collider2D>();
            other.gameObject.GetComponent<PlayerInput>().StandOnPlatform(thisCollider);
        }

        private void OnTriggerExit2D(Collider2D other) //Si le joueur est descendu de la plateforme
        {
            GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
