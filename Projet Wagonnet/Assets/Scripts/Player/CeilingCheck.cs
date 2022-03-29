using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CeilingCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;    //On récupère l'accès au script PlayerInput du prefab Player

        private void OnTriggerEnter2D(Collider2D other) //Quand le CeilingCheck collide avec quelque-chose
        {
            if (player.isAirborn)                       //Si le joueur est en l'air
            {
                player.Fall();                          //Le saut du joueur s'arrête et il commence à chuter
            }
        }
    }
}
