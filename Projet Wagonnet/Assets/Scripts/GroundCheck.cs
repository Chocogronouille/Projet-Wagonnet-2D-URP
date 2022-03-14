using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerInput player;

    private void OnTriggerStay2D(Collider2D other)
    {
        player.isAirborn = false;
        player.coyoteFloat = false;
    }
    
}
