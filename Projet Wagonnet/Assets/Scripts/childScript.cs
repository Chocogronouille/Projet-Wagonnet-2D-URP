using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childScript : MonoBehaviour
{

    public Vector3 LocalPosition;
    public Vector3 GlobalPosition;
    // Start is called before the first frame update
    void Start()
    {
     LocalPosition = gameObject.GetComponent<Transform>().position;
     GlobalPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
