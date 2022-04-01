using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class RespawnObject : MonoBehaviour
{
    public GameObject Line;
    public List<Vector3> VectorList = new List<Vector3>();
    public List<Quaternion> RotationList = new List<Quaternion>();
    public List<Vector3> ScaleList = new List<Vector3>();
    public GameObject parent;
    private float delayTime = 0.2f;

    private GameObject Player;
    public CinemachinePath LeTrack;

    private GameObject GameManager;

    private GameObject GrCheck;
    private GameObject CeCheck;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject;
        Player = GameObject.Find("Player");
        GameManager = GameObject.Find("GameManager");
        GrCheck = GameObject.Find("GroundCheck");
        CeCheck = GameObject.Find("CeilingCheck");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerCol"))
        {
       TrackCreator.instance.track = LeTrack;
       Player.GetComponent<Cinemachine.CinemachineDollyCart>().m_Path = LeTrack;
        }

     /*  GrCheck.SetActive(false);
       CeCheck.SetActive(false); */
   //    Player.GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
         if (other.gameObject.CompareTag("PlayerCol"))
        {
           /*    GrCheck.SetActive(true);
        CeCheck.SetActive(true); */
   //   Player.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("sortie");
        foreach (Transform child in gameObject.transform) 
        {
   //      Instantiate(child,new Vector3(0, 0, 0), new Quaternion(0,0,0,0), GameManager.transform);
         GameObject.Destroy(child.gameObject, 0.2f);
        }
        foreach (Vector3 pos in VectorList) 
        {
         Instantiate(Line,new Vector3(pos.x, pos.y, pos.z), new Quaternion(0,0,0,0), parent.transform);
         Line.GetComponent<makeparent>().parent = parent;
         StartCoroutine(Timer(delayTime));
        }
        }
    }

    IEnumerator Timer(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   VectorList.Clear();
   RotationList.Clear();
   ScaleList.Clear();
}
}
