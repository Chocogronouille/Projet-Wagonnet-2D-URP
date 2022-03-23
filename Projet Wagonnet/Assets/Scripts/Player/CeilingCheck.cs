using UnityEngine;

namespace Player
{
    public class CeilingCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (player.isAirborn)
            {
                player.Fall();
            }
        }
    
    }
}
