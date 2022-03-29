using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectScript : MonoBehaviour
{
    public GameObject EmptyObject;
    public Vector3 PlayerPos;
    public GameObject PlayerCollider;
    public GameObject Track;
    public bool isSurfing = false;

    public int index;
    private int moveIndex;
    private int NewIndex;

    private float delayTime = 0.00001f;

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
        Track = gameObject.transform.parent.parent.gameObject;
        PlayerCollider = GameObject.Find("PlayerCollider");  
    }

    // Update is called once per frame
    void Update()
    {
        EmptyObject = GameObject.Find("V2EmptyObject(Clone)");
        index = transform.parent.GetSiblingIndex();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPos = other.transform.position;
            other.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>().enabled = true;
            other.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>().PosDebut = PlayerPos;
            EmptyObject.transform.parent = Track.transform;
            EmptyObject.transform.position = new Vector3(PlayerPos.x + 5f,PlayerPos.y,PlayerPos.z);
            EmptyObject.transform.SetSiblingIndex(index);
            EmptyObject.AddComponent<IndexScript>();
            PlayerCollider.GetComponent<BoxCollider2D>().enabled = false;
            other.gameObject.GetComponent<Cinemachine.Deactive>().isSurfing = true;
            StartCoroutine(Text(delayTime));
        }
    }

    IEnumerator Text(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   TrackCreator.instance.GenerateTrack();
}

}
