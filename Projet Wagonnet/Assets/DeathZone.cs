using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DeathZoneActivé");
        transform.position = new Vector3(0,3, 0);
    }
}