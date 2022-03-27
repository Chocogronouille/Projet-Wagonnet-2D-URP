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
    public GameObject parent;
    public GameObject unobject;
    private float delayTime = 0.2f;

    private GameObject TrackGenerate;
    private GameObject Player;
    public CinemachinePath LeTrack;

   [SerializeField]  public struct Waypoint 
        {
            public Vector3 position;
            public Vector3 rotation;
        }
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject;
        TrackGenerate = GameObject.Find("TrackGenerator");
        Player = GameObject.FindWithTag("Player");;
        new Waypoint { position = new Vector3(0, 0, -5), rotation = new Vector3(1, 0, 0) };
        
    //    Instantiate(unobject);
    //    unobject.AddComponent<makeparent>();
    //    unobject.GetComponent<makeparent>().parent = parent;
      //  unobject.transform.parent = parent.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D()
    {
       Debug.Log("Col");
       TrackCreator.instance.track = LeTrack;
       Player.GetComponent<Cinemachine.CinemachineDollyCart>().m_Path = LeTrack;
    }

    void OnTriggerExit2D()
    {
 /*       Debug.Log(RotationList[4].z);
        foreach (Quaternion rot in RotationList) 
        {
         //   Debug.Log(rot.z);
     //    Line.transform.rotation.z = rot.z;
        } */
        foreach (Transform child in gameObject.transform) 
        {
         GameObject.Destroy(child.gameObject, 0.2f);
        }
        foreach (Vector3 pos in VectorList) 
        {
        //    Debug.Log("rota.z");
         Instantiate(Line,new Vector3(pos.x, pos.y, pos.z), new Quaternion(0,0,0,0), parent.transform);
         Line.GetComponent<makeparent>().parent = parent;
         StartCoroutine(Timer(delayTime));
    //     Line.transform.parent = parent.transform;
       //    Line.transform.SetParent(parent.transform);
        }
     /*           foreach (Quaternion rot in RotationList) 
        {
            gameObject.transform.GetChild(0).GetComponent<IndexRotation>().RotationList.Add(rot);
        } */
    }

    IEnumerator Timer(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   VectorList.Clear();
   RotationList.Clear();
}
}
