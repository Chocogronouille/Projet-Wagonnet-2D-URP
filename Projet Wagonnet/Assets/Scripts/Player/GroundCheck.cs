using Cinemachine;
using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;

        private void OnTriggerStay2D(Collider2D other)
        {
            // if (other.gameObject.CompareTag("Plateforme"))
            // {
            //     Physics2D.IgnoreLayerCollision(other.gameObject.layer,player.gameObject.layer);
            // }

            if (player.GetComponent<Rigidbody2D>().velocity.y >= 2) return;
            
            player.jumpState = PlayerInput.JumpState.ground;
            player.isAirborn = false;
            player.coyoteFloat = false;
            player.isFalling = false;
            player.GetComponent<Rigidbody2D>().drag = player.groundDrag;
                
            player.ResetSpinJump();
        }
    }
}
