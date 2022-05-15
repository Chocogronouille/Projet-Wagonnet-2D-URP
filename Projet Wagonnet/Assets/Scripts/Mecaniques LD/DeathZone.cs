using UnityEngine;
using System.Collections;
public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
    [SerializeField] private Animator fadeSystem;

    public bool instanceDeathzone;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (instanceDeathzone) return;
        instanceDeathzone = true;
        StartCoroutine(ReplacePlayer(collision));
    }

    private IEnumerator ReplacePlayer(Collider2D collision)
    {
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        if (collision.GetComponent<Rigidbody2D>())
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            collision.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        }
        instanceDeathzone = false;
        collision.transform.position = playerSpawn.position;
    }
}
