using UnityEngine;
using System.Collections;
public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
   [SerializeField]
    private Animator fadeSystem;

    private bool _instance;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_instance) return;
        _instance = true;
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
        _instance = false;
        collision.transform.position = playerSpawn.position;

    }
}
