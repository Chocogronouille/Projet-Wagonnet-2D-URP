using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;

        private void OnTriggerStay2D(Collider2D other)
        {
            player.isAirborn = false;
            player.coyoteFloat = false;
            player.canSpinJump = false;
            player.isFalling = false;
            player.GetComponent<Rigidbody2D>().drag = 10;
        }
    
    }
}
