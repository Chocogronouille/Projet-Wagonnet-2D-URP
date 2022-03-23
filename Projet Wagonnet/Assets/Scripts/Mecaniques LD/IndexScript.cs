using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexScript : MonoBehaviour
{
    public int index = 0;
    public Transform parent;
    public float posZ;
    private float delayTime = 0.05f ;
    // Start is called before the first frame update
    void Start()
    {
        index = gameObject.transform.GetSiblingIndex();     
        Debug.Log(index);
        parent = transform.parent;
        parent.gameObject.AddComponent<ListOfChild>();
        parent.GetComponent<ListOfChild>().Newindex = index;
        parent.GetComponent<ListOfChild>().enabled = true;
        posZ = gameObject.transform.position.z ;
    //    Destroy(gameObject.GetComponent<IndexScript>(),1f);
    //    gameObject.transform.SetSiblingIndex(index + 1); 
        Destroy(gameObject,0.1f);
        StartCoroutine(LaCouroutine(delayTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LaCouroutine(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
 //  Instaciate.instance.InstanciateTest();
}
}
