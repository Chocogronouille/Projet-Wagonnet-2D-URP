using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using Cinemachine;

public class TrackCreator : MonoBehaviour
{
    [SerializeField] CinemachinePath track;   
    [SerializeField] bool loopedTrack = false;

    private  CinemachinePath.Waypoint[] generatedWaypoints;
    private int waypointCount;
    int currentWaypointIndex = 0;


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
        // GenerateTrack();

    }

    public void GenerateTrack()
    {
        if(!track) Debug.Log("No track assigned.");

        currentWaypointIndex = 0;

        waypointCount = loopedTrack ? track.transform.childCount : track.transform.childCount + 1;

        generatedWaypoints = new CinemachinePath.Waypoint[waypointCount];

        for (int i = 0; i < track.transform.childCount; i++)
        {
            Transform currentChild = track.transform.GetChild(i);
     //       new Vector3(wp.position.x, 1.0f, 0.0f);

            if (i == 0 || loopedTrack)
            {
                AddWaypoint(currentChild, 0);
            }

            if (!loopedTrack)
            {
                AddWaypoint(currentChild, 1);
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
           //     new Vector3(targetWP.position.x,targetWP.position.y,targetWP.position.z);
                Vector3 objectScale = child.transform.localScale;
      //          Instantiate(EmptyObject,child.transform);
            //    EmptyObject.transform.position = new Vector3(0,0,0);
            //      EmptyObject.transform.localScale = objectScale;
             //   EmptyObject.transform.SetParent(child.transform);
           //     Debug.Log(targetWP.position.x);
            }

    

}
