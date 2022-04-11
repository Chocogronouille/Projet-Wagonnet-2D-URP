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
            //     //Physics2D.SetLayerCollisionMask();
            // }

            if (player.GetComponent<Rigidbody2D>().velocity.y >= 2) return;
            
            player.isAirborn = false;
            player.coyoteFloat = false;
            player.isFalling = false;
            player.GetComponent<Rigidbody2D>().drag = player.groundDrag;
                
            player.ResetSpinJump();
        }
    }
}
