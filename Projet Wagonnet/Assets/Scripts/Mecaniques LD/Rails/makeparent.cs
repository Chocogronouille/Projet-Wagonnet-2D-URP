using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeparent : MonoBehaviour
{
    public GameObject parent;
    public List<Quaternion> RotList = new List<Quaternion>();
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        RotList = gameObject.transform.parent.GetComponent<RespawnObject>().RotationList;
        index = transform.GetSiblingIndex();
        gameObject.transform.rotation = RotList[index];
    }

}
