using UnityEngine;
using Cinemachine;

public class TrackCreator : MonoBehaviour
{
    [SerializeField] CinemachinePath track;   
    [SerializeField] bool loopedTrack = false;

    private  CinemachinePath.Waypoint[] generatedWaypoints;
    private int waypointCount;
    int currentWaypointIndex = 0;


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
                targetWP.position = child.localRotation * wp.position + child.localPosition;
                targetWP.tangent = child.localRotation * wp.tangent;
                targetWP.roll = wp.roll;
                generatedWaypoints[currentWaypointIndex] = targetWP;
                currentWaypointIndex++;
            }

    

}
