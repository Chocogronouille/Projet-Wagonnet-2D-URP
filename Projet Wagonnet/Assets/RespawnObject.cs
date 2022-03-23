using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RespawnObject : MonoBehaviour
{
    public GameObject Line;
    public List<Vector3> VectorList = new List<Vector3>();
    public GameObject parent;
    public GameObject unobject;
    private float delayTime = 0.2f;

    private GameObject TrackGenerate;
    private GameObject Player;
    public CinemachinePath LeTrack;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject;
        TrackGenerate = GameObject.Find("TrackGenerator");
        Player = GameObject.FindWithTag("Player");;
        
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
        foreach (Transform child in gameObject.transform) 
        {
         GameObject.Destroy(child.gameObject, 0.2f);
        }
        foreach (Vector3 pos in VectorList) 
        {
         Instantiate(Line,new Vector3(pos.x, pos.y, pos.z), Quaternion.identity, parent.transform);
         Line.GetComponent<makeparent>().parent = parent;
         StartCoroutine(Timer(delayTime));
    //     Line.transform.parent = parent.transform;
       //    Line.transform.SetParent(parent.transform);
        }
    }

    IEnumerator Timer(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   VectorList.Clear();
}
}
