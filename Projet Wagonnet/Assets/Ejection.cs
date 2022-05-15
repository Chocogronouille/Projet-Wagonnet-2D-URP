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
   
    void Start()
    {
        player = GameObject.Find("Player");
   /*     direction = player.transform.position - transform.position;
        dir = direction.normalized;
        EjecteZ = transform.parent.transform.localEulerAngles.z; */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            Debug.Log("ok");
            collision.gameObject.GetComponent<Cinemachine.PlayerInput>().isSurfing = false;
            //player.GetComponent<Cinemachine.PlayerInput>().isEject = true;
            //PlayerInput.instance.ApplyJumpForce();
            
            StartCoroutine(ChangeSpeed());
        }
    }
     private IEnumerator ChangeSpeed()
        {
            player.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.5f);
            // player.GetComponent<Cinemachine.PlayerInput>()._maxSpeed = player.GetComponent<Cinemachine.PlayerInput>().walkSpeed;
            // player.GetComponent<Cinemachine.PlayerInput>().isSurfing = false;
            player.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(0.0001f);
            //player.GetComponent<Cinemachine.PlayerInput>().isEject = false;
        }
}
