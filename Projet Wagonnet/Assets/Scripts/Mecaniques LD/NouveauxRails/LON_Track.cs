using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class LON_Track : MonoBehaviour
{
    // Rotation
    private float Rotz;

    // Aide au jump
    //public Transform HelpJump;
//    public GameObject Ejecte;

    public Transform origin;
    public GameObject next;

    public ParticleSystem SurfEffects;

    [Range(0,50)]
    public float speed;

    float trackLength;
    LON_Track track;
    [HideInInspector] public LON_Track last; //Not used, just in case, if needed.
    
    // Start
    void Start()
    {
        if(gameObject.tag == "Right")
        {
            SurfEffects = GameObject.Find("VFX_Surf_Rails_Right").GetComponent<ParticleSystem>();
        }
         else if(gameObject.tag == "Left")
        {
            SurfEffects = GameObject.Find("VFX_Surf_Rails_Left").GetComponent<ParticleSystem>();
        }
     //   SurfEffects = GameObject.Find("VFX_Surf_Rails").GetComponent<ParticleSystem>();
        Rotz= gameObject.transform.localEulerAngles.z;

        if (next is null) return;
        
        track = next.GetComponent<LON_Track>();
        if (track is null) return;
        track.last = this;
    }

    PlayerInput player => PlayerInput.instance;
    Rigidbody2D playerRigidBody => PlayerInput.instance.rbCharacter;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.isSurfing = true;
        SurfEffects.Play();
        MoveNext();
        collision.transform.localEulerAngles = new Vector3(0,0,Rotz);
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        SurfEffects.Stop();
    }

    public void MoveNext()
    {
        if (next != null)
        {
            SetLengthFromPoint(playerRigidBody.transform.position);
            player.currentTween?.Kill();
            player.currentTween = playerRigidBody.DOMove(track != null 
                ? track.origin.position : next.transform.position, trackLength / speed).SetEase(Ease.Linear).OnComplete(() => track?.MoveNext());

            var direction = next.transform.position - player.transform.position;
            player.railDirection = direction.normalized;
        }
    }
    
    public void SetLengthFromPoint(Vector3 from)
    {
        trackLength = Vector3.Distance(from, track != null ? track.origin.position : next.transform.position);
    }
}
