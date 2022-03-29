using System;
using Cinemachine;
using Player;
using UnityEngine;

public class PunchingBall : MonoBehaviour
{
    public float score;
    public float scoreMultiplier;

    private void Update()
    {
        Debug.Log(Mathf.RoundToInt(Math.Abs((score))));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        score += PlayerInput.instance.rbCharacter.velocity.x * scoreMultiplier;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
