using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Cinemachine.PlayerInput;


namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        private GameObject Player;
        [SerializeField] private PlayerInput player;
        public ParticleSystem Effects;
  /*      private ParticleSystem WalkEffectsRight;
        private ParticleSystem WalkEffectsLeft; */
        private GameObject WalkEffectsRight;
        private GameObject WalkEffectsLeft;

        void Start()
        {
            Player = GameObject.Find("Player");
     /*       WalkEffectsRight = GameObject.Find("VFX_Dust_Right").GetComponent<ParticleSystem>();
            WalkEffectsLeft = GameObject.Find("VFX_Dust_Left").GetComponent<ParticleSystem>(); */
            WalkEffectsRight = GameObject.Find("VFX_Dust_Right");
            WalkEffectsLeft = GameObject.Find("VFX_Dust_Left");
        }
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
            if(player.GetComponent<SpriteRenderer>().flipX)
            {
           //     WalkEffectsLeft.Play();
                  WalkEffectsRight.SetActive(true);
                  WalkEffectsLeft.SetActive(false);
            }
            else
            {
          //      WalkEffectsRight.Play();
                  WalkEffectsRight.SetActive(false);
                  WalkEffectsLeft.SetActive(true);
            }
            
            player.ResetSpinJump();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            WalkEffectsRight.SetActive(false);
            WalkEffectsLeft.SetActive(false);
        }
    }
}
