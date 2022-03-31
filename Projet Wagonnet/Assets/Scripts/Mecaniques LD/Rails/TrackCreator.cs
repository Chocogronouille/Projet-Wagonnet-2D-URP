using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using Cinemachine;

public class TrackCreator : MonoBehaviour
{
    public CinemachinePath track;   
    [SerializeField] bool loopedTrack = false;

    public  CinemachinePath.Waypoint[] generatedWaypoints;
    private int waypointCount;
    int currentWaypointIndex = 0;
    private GameObject player;
    public GameObject test;

    public Transform currentChild;
    private float decalage = 5f;


 //   public GameObject EmptyObject;

     public static TrackCreator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de TrackCreator dans la scène");
            return;
        }

        instance = this;
    }


    void Start()
    {
        player = GameObject.Find("Player");
        Instantiate(test);
    }
     public void InstanciateTest()
    {
        Instantiate(test);
    }

    public void GenerateTrack()
    {
        player.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position = 0.07f;
        if(!track) Debug.Log("No track assigned.");

        currentWaypointIndex = 0;

        waypointCount = loopedTrack ? track.transform.childCount : track.transform.childCount + 1;

        generatedWaypoints = new CinemachinePath.Waypoint[waypointCount];

        for (int i = 0; i < track.transform.childCount; i++)
        {
             currentChild = track.transform.GetChild(i).transform.GetChild(1);
          //   LeCurrent = currentChild.transform.GetChild(1);

            if (i == 0 || loopedTrack)
            {
                AddWaypoint(currentChild, 0);
            }

            if (!loopedTrack)
            {
                AddWaypoint(currentChild, 1);
      //          Destroy(CinemachinePath.Waypoint[1]);
            }

        }
        track.m_Waypoints = generatedWaypoints;
        track.m_Looped = loopedTrack;
    }

    void AddWaypoint(Transform child, int idx)
            {
                if(!child.GetComponent<CinemachinePath>()) return;
                CinemachinePath childCinemachinePath = child.GetComponent<CinemachinePath>();
                CinemachinePath.Waypoint wp = childCinemachinePath.m_Waypoints[idx];
                CinemachinePath.Waypoint targetWP = new CinemachinePath.Waypoint();
                targetWP.position = child.localRotation * wp.position + child.transform.position - track.transform.position;
                targetWP.position.z = 0f;
                targetWP.tangent = child.localRotation * wp.tangent;
                targetWP.roll = wp.roll;
                generatedWaypoints[currentWaypointIndex] = targetWP;
                currentWaypointIndex ++;
                Vector3 objectScale = child.transform.localScale;
            }

    

}
