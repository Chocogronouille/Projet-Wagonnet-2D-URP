using System;
using Cinemachine;
using UnityEngine;

namespace Props
{
    public class FallThroughPlatform : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            var thisCollider = GetComponent<Collider2D>();
            other.gameObject.GetComponent<PlayerInput>().StandOnPlatform(thisCollider);
        }
    }
}
