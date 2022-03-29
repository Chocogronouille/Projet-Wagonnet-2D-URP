using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfChild : MonoBehaviour
{
    public int Theindex;
    public int Newindex;
    public List<int> goList = new List<int>();
    private float delayTime = 0.0001f;

    // List of GameObjects
    public List<Vector3> TheList = new List<Vector3>();

    void Start () 
      {     
          Debug.Log("Child Objects: " + CountChildren(transform));
      }
 
     int CountChildren(Transform a)
     {
         goList.Clear();
         int childCount = 0;
         foreach (Transform b in a)
         {
             if(b.gameObject.CompareTag("Empty"))
             {
                 Debug.Log("Empty");
             }
             else
             {
             childCount ++;
             Theindex = b.transform.GetSiblingIndex();
             goList.Add(Theindex);
             gameObject.GetComponent<RespawnObject>().VectorList.Add(b.transform.position);
             gameObject.GetComponent<RespawnObject>().RotationList.Add(b.transform.rotation);
             if(Theindex < Newindex)
             {
                 Destroy(b.gameObject);
             }

             }
         }
         StartCoroutine(DeleteFunction(delayTime));
         return childCount;
     }

     IEnumerator DeleteFunction(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   Destroy(gameObject.GetComponent<ListOfChild>());
}
}
