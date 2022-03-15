using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexScript : MonoBehaviour
{
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = transform.GetSiblingIndex();     
        Debug.Log(index);
    //    gameObject.transform.SetSiblingIndex(index + 1); 
    }

    // Update is called once per frame
    void Update()
    {
    /*     if(index == 0)
        {
        //    Debug.Log("0");
        }
        else
        {
           gameObject.transform.SetSiblingIndex(index + 1);
        } */
        
    }
}
