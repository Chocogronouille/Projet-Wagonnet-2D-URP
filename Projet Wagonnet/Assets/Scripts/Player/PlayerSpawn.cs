using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("Player").transform.position = transform.position;
    }
}
