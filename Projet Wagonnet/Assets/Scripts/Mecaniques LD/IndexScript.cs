using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexScript : MonoBehaviour
{
    public int index = 0;
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        index = gameObject.transform.GetSiblingIndex();     
        Debug.Log(index);
        parent = transform.parent;
        parent.gameObject.AddComponent<ListOfChild>();
        parent.GetComponent<ListOfChild>().Newindex = index;
        parent.GetComponent<ListOfChild>().enabled = true;
    //    gameObject.transform.SetSiblingIndex(index + 1); 
    //   Destroy(gameObject,1);
    }

    // Update is called once per frame
    void Update()
    {
        index = gameObject.transform.GetSiblingIndex();  
   /*     if (index == 9)
        {
            gameObject.GetComponent<IndexScript>().enabled = false;
        } */
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
