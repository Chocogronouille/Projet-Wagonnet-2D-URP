using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Props
{
    public class FallThroughPlatform : MonoBehaviour
    {
        private Collider2D[] _thisColliders;
        private void OnCollisionEnter2D(Collision2D other)
        {
            _thisColliders = other.gameObject.GetComponent<PlayerInput>().currentPlatform;
            if (_thisColliders != null)
            {
                foreach (var iCollider2D in _thisColliders)
                {
                    iCollider2D.enabled = true;
                }
            }       //On fait réapparaitre les dernières plateformes touchées avant de retenir les nouvelles
            
            _thisColliders = GetComponents<Collider2D>();
            other.gameObject.GetComponent<PlayerInput>().StandOnPlatform(_thisColliders);
            
        }
    }
}
