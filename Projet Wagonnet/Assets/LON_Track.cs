using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class LON_Track : MonoBehaviour
{
    public Transform origin;
    public GameObject next;

    [Range(0,50)]
    public float speed;

    float trackLength;
    LON_Track track;
    [HideInInspector] public LON_Track last; //Not used, just in case, if needed.
    
    // Start
    void Start()
    {
        try
        {
            track = next.GetComponent<LON_Track>();
            track.last = this;
        }
        catch
        {

        }      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerCol"))
        {
            PlayerInput player = collision.transform.parent.GetComponent<PlayerInput>();
            MoveNext(player);
        }
    }

    public void MoveNext(PlayerInput player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (next != null)
        {
            SetLengthFromPoint(rb.transform.position);
            player.currentTween?.Kill();
            player.currentTween = rb.DOMove(track != null ? track.origin.position : next.transform.position, trackLength / speed).SetEase(Ease.Linear).OnComplete(() => track?.MoveNext(player));
        }
    }
    
    public void SetLengthFromPoint(Vector3 from)
    {
        trackLength = Vector3.Distance(from, track != null ? track.origin.position : next.transform.position);
    }
}
