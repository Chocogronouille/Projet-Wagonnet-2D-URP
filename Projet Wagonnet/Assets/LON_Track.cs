using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class LON_Track : MonoBehaviour
{
    public Transform origin;
    public GameObject next;

    Tween currentTween;

    [Range(0,50)]
    public float speed;

    float trackLength;
    LON_Track track;



    // Start is called before the first frame update
    void Start()
    {
        try
        {
            track = next.GetComponent<LON_Track>();
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

            player.currentTween = MoveNext(player.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public Tween MoveNext(Transform t)
    {
        if (next != null)
        {
            SetLengthFromPoint(t.transform.position);
            currentTween = t.DOMove(track != null ? track.origin.position : next.transform.position, trackLength / speed).SetEase(Ease.Linear).OnComplete(() => track?.MoveNext(t));

            return currentTween;
        }

        return null;
    }
    

    public void SetLengthFromPoint(Vector3 from)
    {
        trackLength = Vector3.Distance(from, track != null ? track.origin.position : next.transform.position);
    }
}
