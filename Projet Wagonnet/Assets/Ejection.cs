using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Cinemachine.PlayerInput;

public class Ejection : MonoBehaviour
{
    public float EjecteZ;
    public Vector3 direction;
    public Vector3 dir;
    public GameObject player;
   // [HideInInspector] public Tween currentTween;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
   /*     direction = player.transform.position - transform.position;
        dir = direction.normalized;
        EjecteZ = transform.parent.transform.localEulerAngles.z; */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //  currentTween?.Kill();
            Debug.Log("ok");
            collision.gameObject.GetComponent<Cinemachine.PlayerInput>().isSurfing = false;
            //      collision.GetComponent<Rigidbody2D>().AddForce(-dir * 20000);
            //    collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(100,0,0),ForceMode2D.Impulse);
            PlayerInput.instance.ApplyJumpForce();
        }
    }
}
