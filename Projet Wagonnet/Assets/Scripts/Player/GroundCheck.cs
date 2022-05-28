using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Cinemachine.PlayerInput;


namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;
        public ParticleSystem Effects;
        private void OnTriggerEnter2D(Collider2D other)
        {
            player.ResetMaxSpeed();
            Effects.Play();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y >= 2) return;
            
            player.isAirborn = false;
            player.coyoteFloat = false;
            player.isFalling = false;
            player.GetComponent<Rigidbody2D>().drag = player.groundDrag;
            Gamepad.current.SetMotorSpeeds(0f, 0f);
            
            player.ResetSpinJump();
        }
    }
}
