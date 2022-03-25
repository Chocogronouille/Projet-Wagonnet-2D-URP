using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfChild : MonoBehaviour
{
 //   public GameObject TheEmpty;
    public int Theindex;
    public int Newindex;
    public List<int> goList = new List<int>();
    private float delayTime = 0.0001f;

    // List of GameObjects
    public List<Vector3> TheList = new List<Vector3>();

    void Start () {
      
         Debug.Log("Child Objects: " + CountChildren(transform));
    //     gameObject.GetComponent<ListOfChild>().enabled = false;
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
                 //        Debug.Log("Child: "+b);
             childCount ++;
     //        childCount += CountChildren(b);
             Theindex = b.transform.GetSiblingIndex();
             Debug.Log("On est là");
             goList.Add(Theindex);
             gameObject.GetComponent<RespawnObject>().VectorList.Add(b.transform.position);
             gameObject.GetComponent<RespawnObject>().RotationList.Add(b.transform.rotation);
             Debug.Log("Destruction");
             if(Theindex < Newindex)
             {
                 Destroy(b.gameObject);
             }

             }
         }
         StartCoroutine(DeleteFunction(delayTime));
         return childCount;
     }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator DeleteFunction(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
   Destroy(gameObject.GetComponent<ListOfChild>());
}
}
