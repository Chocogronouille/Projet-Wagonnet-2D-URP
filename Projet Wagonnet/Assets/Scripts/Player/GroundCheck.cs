using UnityEngine;

namespace Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField] private PlayerInput player;        //On récupère l'accès au script PlayerInput du prefab Player

        private void OnTriggerStay2D(Collider2D other)      //Tant que le GroundCheck collide avec quelque-chose
        {
            player.isAirborn = false;                       //Le joueur n'est plus en l'air
            player.coyoteFloat = false;                     //Le joueur n'est pas en Coyote Time
            player.canSpinJump = false;                     //Le joueur ne peut pas Spin Jump
            player.isFalling = false;                       //Le joueur n'est pas en train de tomber
            player.GetComponent<Rigidbody2D>().drag = 10;   //La friction du joueur passe à 10 pour l'arrêter lorsqu'il ne maintient plus son joystick dans une direction
        }
    }
}
