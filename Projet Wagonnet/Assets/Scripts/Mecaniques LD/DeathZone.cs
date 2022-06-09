using UnityEngine;
using System.Collections;
using Cinemachine;

public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
    [SerializeField] private Animator fadeSystem;
    private GameObject player;
    public AudioClip sound;


    public bool instanceDeathzone;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (instanceDeathzone) return;
        instanceDeathzone = true;
        player.GetComponent<Cinemachine.PlayerInput>().animator.SetBool("isDead",true);
        StartCoroutine(ReplacePlayer(collision));
    }

    private IEnumerator ReplacePlayer(Collider2D collision)
    { 
        fadeSystem.SetTrigger("FadeIn");
        collision.GetComponent<PlayerInput>().isSurfing = false;
        AudioManager.instance.PlayClipAt(sound, transform.position);
        var rb = collision.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Cinemachine.PlayerInput>().animator.SetBool("isDead",false);
        yield return new WaitForSeconds(0.5f);
        if (rb)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            collision.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        }
        instanceDeathzone = false;
        collision.transform.position = playerSpawn.position;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
