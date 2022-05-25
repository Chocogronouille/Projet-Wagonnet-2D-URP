using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Props
{
    public class FallThroughPlatform : MonoBehaviour
    {
        private Collider2D _thisCollider;
        private void OnCollisionEnter2D(Collision2D other)
        {
            _thisCollider = other.gameObject.GetComponent<PlayerInput>().currentPlatform;
            if (_thisCollider != null)
            {
                _thisCollider.enabled = true;
            }       //On fait réapparaitre les dernières plateformes touchées avant de retenir les nouvelles
            
            _thisCollider = GetComponent<Collider2D>();
            other.gameObject.GetComponent<PlayerInput>().StandOnPlatform(_thisCollider);
            
        }
    }
}
