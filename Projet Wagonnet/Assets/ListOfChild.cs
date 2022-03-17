using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfChild : MonoBehaviour
{
    public GameObject TheEmpty;
    public int Theindex;
    public int Newindex;
    public List<int> goList = new List<int>();
    void Start () {
      
         Debug.Log("Child Objects: " + CountChildren(transform));
     }
 
     int CountChildren(Transform a)
     {
         goList.Clear();
         int childCount = 0;
         foreach (Transform b in a)
         {
     //        Debug.Log("Child: "+b);
             childCount ++;
     //        childCount += CountChildren(b);
             Theindex = b.transform.GetSiblingIndex();
             Debug.Log(b.transform.GetSiblingIndex());
             goList.Add(Theindex);
             if(Theindex < Newindex)
             {
                 Destroy(b.gameObject);
             }
         }
         return childCount;
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
