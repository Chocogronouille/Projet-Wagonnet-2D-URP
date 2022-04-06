using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace Cinemachine
{
    /// <summary>
    /// This is a very simple behaviour that constrains its transform to a CinemachinePath.
    /// It can be used to animate any objects along a path, or as a Follow target for 
    /// Cinemachine Virtual Cameras.
    /// </summary>
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [DisallowMultipleComponent]
    //   [HelpURL(Documentation.BaseURL + "manual/CinemachineDollyCart.html")]
    public class PlayerInput : MonoBehaviour
    {
        private InputActions farmerInputActions;
        private float _jumpBuffer;
        private float _horizontalSpeed;
        private float _jumpDuration;
        private bool _wantToEndJump;
        private float _maxSpeed;
        private int _canSpinJump;

        public bool isFalling;
        public static PlayerInput instance; // singleton
        public InputAction movement;
        public bool isAirborn;
        public bool coyoteFloat;
        public int numberOfSpinJump;
        public Vector2 direction;
        public float apexThreshold;
        public float defaultGravityScale;
        public float stopDrag;
        public float groundDrag;
        public GameObject groundCheck;

        public Animator animator;
        private float delaySpinJump = 0.35f;

        //private string currentState;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer screenRenderer;
        public Rigidbody2D rbCharacter;

        [HideInInspector] public Tween currentTween;


        [SerializeField] private float walkSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float minJumpDuration;
        [SerializeField] private float maxJumpDuration;
        [SerializeField] private float spinJumpDuration;

        // Variable des Rails
        public bool isSurfing;
        private float waitTime = 0.0001f;
        private GameObject TheChild;

        void Awake()
        {
            TheChild = GameObject.Find("PlayerCollider");
            farmerInputActions = new InputActions();

            #region singleton

            if (instance != null)
            {
                Debug.LogError("Il y a plusieurs instance de PlayerInput");
                return;
            }

            instance = this;

            #endregion

            animator = GetComponent<Animator>();
            
        }

        private void OnEnable()
        {
            movement = farmerInputActions.Player.Movement;
            movement.Enable();

            farmerInputActions.Player.Jump.performed += DoJump;
            farmerInputActions.Player.Jump.canceled += EndJump;
            farmerInputActions.Player.Jump.Enable();

            farmerInputActions.Player.SpinMove.performed += DoSpin;
            farmerInputActions.Player.SpinMove.Enable();
        }

        #region InputAction

        private void DoJump(InputAction.CallbackContext obj) //Quand le bouton de saut est enfoncé
        {
            currentTween?.Kill();

            _jumpBuffer = jumpBufferTime; //On attribue à la variable _jumpBuffer le temps prédéfini du Jump Buffer
            if(isSurfing)
            {
            gameObject.GetComponent<CinemachineDollyCart>().enabled=false;
            rbCharacter.AddForce(new Vector2(0,10),ForceMode2D.Impulse);
            StartCoroutine(LeJump(waitTime));
            }
        }

        // Couroutine pour les rails
        IEnumerator LeJump(float waitTime)
    {
     //   gameObject.transform.rotation = new Quaternion(0.0f,90,0.0f,90);
        yield return new WaitForSeconds(waitTime);
        isSurfing = false;
        yield return new WaitForSeconds(waitTime);
        Debug.Log("couroutine");
        TheChild.GetComponent<BoxCollider2D>().enabled = true;
    }

        private void DoSpin(InputAction.CallbackContext obj) //Quand la touche de Spin Jump est enfoncée
        {
            if (isAirborn)
            {
                if (_canSpinJump!=0) //On regarde si le joueur peut Spin Jump
                {
                    SpinJump(); //Si oui, il l'effectue
                }
            }
        }

        private void EndJump(InputAction.CallbackContext obj) //Quand le bouton de saut est relaché
        {
            groundCheck.SetActive(true);
            
            if (!isFalling) //Si le joueur n'est pas en train de tomber
            {
                if (isAirborn) //On regarde si le joueur est dans les airs
                {
                    _wantToEndJump =
                        true; //Si c'est le cas, on retient le fait que le joueur veuille arrêter son saut
                }
            }
            else //Sinon, le décompte du Jump Buffer passe à 0
            {
                _jumpBuffer = 0;
            }
        }

        #endregion

        private void FixedUpdate()
        {
   //         gameObject.transform.rotation = new Quaternion(0,0,0,0);
            if(isSurfing)
            {
                animator.SetBool("isSurfing",true);
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
            }
            else
            {
              // gameObject.transform.rotation = new Quaternion(0,0,0,0);
              // animator.SetBool("isSurfing",false);
              // gameObject.GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
              // gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
              // gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        //    gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x,90,gameObject.transform.rotation.z,0);
            direction = movement
                .ReadValue<Vector2>(); //La variable direction prend la valeur de position du Joystick gauche
            float characterVelocity = Mathf.Abs(rbCharacter.velocity.x); //N'EST PAS UNE DE MES FONCTIONS
            animator.SetFloat("Speed", characterVelocity); //N'EST PAS UNE DE MES FONCTIONS


            // Jump Buffer
            if (
                _jumpBuffer >
                0) //Si la variable _jumpBuffer est supérieure à 0 (que la touche de saut a été enfoncée) 
            {
                _jumpBuffer -= 1 * Time.deltaTime; //On réduit la variable selon le temps passé entre deux frames

                if (isAirborn == false) //Si le joueur n'est pas en l'air (au sol / sur un rail / sur un ballon)
                {
                    Jump(); //Le joueur saute
                }
            }


            // Gestion de la vitesse de chute et du nuancier de saut

            if (!isFalling) //Si le joueur n'est pas en train de tomber
            {
                if (isAirborn) //Si le joueur est dans les airs
                {
                    _jumpDuration += 1 * Time.deltaTime; //On compte la durée de son saut

                    if (_jumpDuration >
                        maxJumpDuration) //Si la durée de son saut est supérieur à la durée max d'un saut
                    {
                        Fall(); //Le joueur commence à retomber
                    }

                    if (_wantToEndJump) //Si le joueur a relaché la touche de saut
                    {
                        if (rbCharacter.velocity.y >
                            apexThreshold) //Si le joueur a une vitesse en y supérieur au seuil de vitesse ascensionel
                        {
                            //(on vérifie pour que le joueur ne puisse pas augmenter la taille son saut en l'arrêtant juste avant son apogée)

                            if (_jumpDuration >
                                minJumpDuration) //Si la durée du saut est supérieure à la durée minimale d'un saut
                            {
                                //(permet d'avoir un saut par défaut lorsque le bouton de saut est relaché directement)

                                Fall(); //Le joueur commence à retomber
                            }
                        }
                    }

                }
            }


            // Coyote Time

            if (!isAirborn) //Si le joueur n'est pas encore en train de tomber
            {
                if (
                    rbCharacter.velocity.y <
                    -1) //Si sa vitesse verticale est négative (<-1 pour éviter que le Composite Collider des tilemaps ne le déclenche alors que le personnage est encore au sol)
                {
                    if (!coyoteFloat) //S'il n'est pas déjà en Coyote Time
                    {
                        coyoteFloat = true; //Il entre en Coyote Time
                        StartCoroutine(CoyoteTime()); //Et on lance la coroutine associée
                    }
                }
            }


            // Fast Fall

            if (direction.y < -0.9f) //Si le joystick gauche est orienté vers le bas
            {
                FastFall(); //On lance la FastFall
            }


            //Marche

            if ((direction.x < -0.1f) ||
                (0.1f < direction.x)) //Si le joystick gauche est orienté vers la gauche ou la droite
            {
                _maxSpeed = walkSpeed; //Sa vitesse max devient sa vitesse de marche
                Move(); //On lance la fonction Move pour le déplacement


                //ChangeAnimationState(PLAYER_RUN);// Tentative animator                //N'EST PAS UNE DE MES FONCTIONS
            }
            else //Si le joystick gauche n'est ni à gauche, ni à droite
            {
                if (isFalling) //Si le joueur est en l'air
                {

                    rbCharacter.drag = stopDrag;


                    //ChangeAnimationState(PLAYER_RUN);// Tentative animator            //N'EST PAS UNE DE MES FONCTIONS
                }

                //Test Animator                                                         //N'EST PAS UNE DE MES FONCTIONS
                // if (direction.x == 0)                                                //N'EST PAS UNE DE MES FONCTIONS
                // {                                                                    //N'EST PAS UNE DE MES FONCTIONS
                //     ChangeAnimationState(PLAYER_IDLE);                               //N'EST PAS UNE DE MES FONCTIONS
                // }                                                                    //N'EST PAS UNE DE MES FONCTIONS
            }
        }

        #region FonctionsDéplacements

        // void ChangeAnimationState(string newState)                                    //N'EST PAS UNE DE MES FONCTIONS
        //  {                                                                            
        //      if (currentState == newState) return;                                    //N'EST PAS UNE DE MES FONCTIONS
        //      animator.Play(newState);                                                 //N'EST PAS UNE DE MES FONCTIONS
        //      currentState = newState;                                                 //N'EST PAS UNE DE MES FONCTIONS
        //  }

        private void Move() //Fonction Move appelée lorsqu'on veut déplacer le personnage
        {
            rbCharacter.drag = 0; //La friction du personnage passe à 0 (pas de résistance en l'air)
            rbCharacter.AddForce(new Vector2(_maxSpeed * direction.x * 10,
                0f)); //On applique une force au personnage pour l'accélerer dans la direction du joystick gauche
            _horizontalSpeed =
                Mathf.Clamp(rbCharacter.velocity.x, -_maxSpeed,
                    _maxSpeed); //Si sa vitesse horizontale est suppérieur à sa vitesse max, on fixe _horizontalSpeed à la vitesse max
            rbCharacter.velocity =
                new Vector2(_horizontalSpeed,
                    rbCharacter.velocity
                        .y); //On applique au joueur une vitesse égale à _horizontalSpeed en X et sa vitesse en Y

            Flip(rbCharacter.velocity
                .x); //Flip le joueur en fonction de sa vitesse  //N'EST PAS UNE DE MES FONCTIONS
        }

        private void Jump() //Fonction jump appelée lorsqu'on veut faire sauter le personnage
        {
            rbCharacter.gravityScale =
                defaultGravityScale; //On remet la Gravity Scale du personnage à la Gravity Scale par défaut
            rbCharacter.drag = 0; //On remet la friction du personnage à 0
            isFalling = false; //Le joueur n'est pas en train de tomber
            isAirborn = true; //Le joueur est en l'air
            _jumpBuffer = 0; //Le compteur du Jump Buffer est remis à 0
            _jumpDuration = 0; //On commence à compter la durée du saut

            rbCharacter.AddForce(new Vector2(0, jumpForce),
                ForceMode2D.Impulse); //On applique une force vers le haut au personnage égale à jumpForce
            animator.SetBool("isJumping", true); //N'EST PAS UNE DE MES FONCTIONS
            
            groundCheck.SetActive(false);
        }

        private void SpinJump() //Fonction appelée lorsqu'on veut faire Spin Jump le personnage
        {
            rbCharacter.gravityScale =
                defaultGravityScale; //On remet la Gravity Scale à la Gravity Scale par défaut
            isFalling = false; //Le joueur n'est pas en train de tomber
            isAirborn = true; //Le joueur est en l'air
            coyoteFloat = false; //Le joueur n'est pas en Coyote Time
            _jumpDuration = maxJumpDuration - spinJumpDuration;

            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, 0); //On arrête la chute du personnage
            rbCharacter.AddForce(new Vector2(0, spinJumpForce*_canSpinJump),
                ForceMode2D.Impulse); //On applique une force vers le haut au personnage égale à sa spinjumpForce*son nombre de spin jump (Des sauts dégressifs)

            animator.SetBool("IsSpinJumping", true); //N'EST PAS UNE DE MES FONCTIONS
            StartCoroutine(TimerSpinJump(delaySpinJump));
            
            _canSpinJump -= 1; //On retire 1 au nombre de SpinJumps restant
        }

         IEnumerator TimerSpinJump(float delaySpinJump)
{
   yield return new WaitForSeconds(delaySpinJump);
   animator.SetBool("IsSpinJumping", false);
}

        public void Fall() //Fonction appelée lorsqu'on veut faire chuter le personnage
        {
            isFalling = true; //Le joueur est en train de tomber
            _wantToEndJump = false; //Le joueur n'est plus considéré comme voulant arrêter son saut
            rbCharacter.gravityScale =
                defaultGravityScale; //On remet la Gravity Scale à la Gravity Scale par défaut

            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, 0f); //On arrête la montée du personnage
            rbCharacter.AddForce(new Vector2(0, apexEndJump),
                ForceMode2D
                    .Impulse); //On lui applique une force vers le haut égale à apexEndJump (pour rendre la descente plus douce)

            animator.SetBool("IsFalling", true); //N'EST PAS UNE DE MES FONCTIONS
            animator.SetBool("isJumping", false); //N'EST PAS UNE DE MES FONCTIONS
        }

        private void FastFall() //Fonction appelée lorsqu'on veut lancer la Fast Fall
        {
            if (isAirborn) //Si le personnage est en l'air
            {
                rbCharacter.velocity =
                    new Vector2(rbCharacter.velocity.x,
                        -fastFallSpeed); //On le fait aller vers le bas à la vitesse fastFallSpeed
            }
        }

        public void ResetSpinJump()
        {
            _canSpinJump = numberOfSpinJump;
        }

        #endregion

        #region FonctionsAnimator

        void Flip(float velocity) //Fonction utilisée pour changer le sens du sprite du personnage
        {
            if (velocity > 0.1f) //Si le joueur va vers la droite
            {
                spriteRenderer.flipX = false; //On garde le sprite dans son orientation de base (vers la droite)
                screenRenderer.flipX = false;
            }
            else if (velocity < -0.1f) //Si le joueur va vers la gauche
            {
                spriteRenderer.flipX = true; //On oriente le sprite vers la gauche
                screenRenderer.flipX = true;
            }
        }

        #endregion

        #region Coroutine

        private IEnumerator CoyoteTime() //Coroutine du coyote time
        {
            rbCharacter.drag = 0; //La friction du personnage passe à 0

            yield return new WaitForSeconds(coyoteTime); //On attend pendant coyoteTime secondes

            isAirborn = true; //Le joueur est en l'air
            coyoteFloat = false; //Le joueur n'est plus en Coyote Time
            isFalling = true; //Le joueur est en train de tomber
            rbCharacter.gravityScale =
                defaultGravityScale; //On remet la Gravity Scale du personnage à la Gravity Scale par défaut

            StopCoroutine(CoyoteTime()); //On arrête la coroutine Coyote Time
        }

        #endregion
      
    }
}