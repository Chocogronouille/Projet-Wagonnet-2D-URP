using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectScript : MonoBehaviour
{
    private GameObject EmptyObject;
    public Vector3 PlayerPos;
    public GameObject Player;
    public GameObject PlayerCollider;
    public GameObject Track;
    public bool isSurfing = false;

    private int index;
    private int moveIndex;
    private int NewIndex;

    private float delayTime = 0.00001f;
    public bool isCoroutine = false;

    public static EmptyObjectScript instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de EmptyObjectScript dans la scène");
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.transform.localScale = new Vector3(1,1,0);
        EmptyObject = GameObject.Find("V2EmptyObject");
        Player = GameObject.Find("Player");
        Track = GameObject.Find("Track");
        PlayerCollider = GameObject.Find("PlayerCollider");
        index = transform.parent.GetSiblingIndex();
        Debug.Log(index);
   //     GetComponent<BoxCollider2D>().size = new Vector2(5f,1f);
   
    }

    // Update is called once per frame
    void Update()
    {
       if(index - 1 < 0)
        {
            moveIndex = 0;
        }
        else
        {
           moveIndex = 1;
        }

          if(isCoroutine)
    {
      IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("WaitAndPrint ");
        isCoroutine = false;
    }
    }

    /*    if(index <= NewIndex)
            {
                Destroy(transform.parent.gameObject);
            } */
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnCollisionEnter2D");
            PlayerPos = other.transform.position;
            other.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>().enabled = true;
            other.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>().PosDebut = PlayerPos;
            Debug.Log(other.transform.position.y);
            Instantiate(EmptyObject);
            EmptyObject.transform.parent = Track.transform;
            EmptyObject.transform.position = PlayerPos;
            EmptyObject.transform.SetSiblingIndex(index);
            EmptyObject.AddComponent<IndexScript>();
       //     NewIndex = index;
       //     DestroyRails.instance.PlayerPosPos = EmptyObject;
       /*     EmptyObject.AddComponent<Cinemachine.CinemachinePath>();
            EmptyObject.AddComponent<DirectionGizmo>(); */
            PlayerCollider.GetComponent<BoxCollider2D>().enabled = false;
            isCoroutine = true;
            other.gameObject.GetComponent<Cinemachine.Deactive>().isSurfing = true;
    //        TrackCreator.instance.GenerateTrack();
            StartCoroutine(Text(delayTime));
        }
    }

    IEnumerator Text(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   Debug.Log("ok");
   TrackCreator.instance.GenerateTrack();
}
 /*   void DestroyObjects()
    {
      if(index < NewIndex)
      {
          Destroy(transform.parent.gameObject);
      }
    } */

}
