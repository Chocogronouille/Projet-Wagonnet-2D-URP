using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;
    public Vector3 posOffSet;
    private Vector3 velocity;
    
    
    /*
     private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player Test").GetComponent<GameObject>();
    }
    */
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffSet, ref velocity,
            timeOffset);
    }
}
