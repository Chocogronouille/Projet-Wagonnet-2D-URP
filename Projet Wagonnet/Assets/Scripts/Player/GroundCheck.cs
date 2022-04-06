using Cinemachine;
using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;        //On récupère l'accès au script PlayerInput du prefab Player

        private void OnTriggerStay2D(Collider2D other)      //Tant que le GroundCheck collide avec quelque-chose
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y<2)
            {
                player.isAirborn = false;                       //Le joueur n'est plus en l'air
                player.coyoteFloat = false;                     //Le joueur n'est pas en Coyote Time
                player.isFalling = false;                       //Le joueur n'est pas en train de tomber
                player.GetComponent<Rigidbody2D>().drag = player.groundDrag;   //La friction du joueur passe à 10 pour l'arrêter lorsqu'il ne maintient plus son joystick dans une direction
                
                player.ResetSpinJump();                         //Le joueur récupère tout ses SpinJumps
            }
        }
    }
}
