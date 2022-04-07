using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeparent : MonoBehaviour
{
    public GameObject parent;
    public List<Quaternion> RotList = new List<Quaternion>();
    public List<Vector3> PosList = new List<Vector3>();
    public List<Vector3> ScaList = new List<Vector3>();
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        RotList = gameObject.transform.parent.GetComponent<RespawnObject>().RotationList;
        PosList = gameObject.transform.parent.GetComponent<RespawnObject>().VectorList;
        ScaList = gameObject.transform.parent.GetComponent<RespawnObject>().ScaleList;
        index = transform.GetSiblingIndex();

        if(index >= 0)
        {
            if(index < RotList.Count)
            gameObject.transform.rotation = RotList[index];
            if(index < PosList.Count)
            gameObject.transform.position = PosList[index];
            if(index < ScaList.Count)
            gameObject.transform.localScale = ScaList[index];
        }
    }

}
