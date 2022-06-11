using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrderInHierachy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetSiblingIndex(11);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
