using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeparent : MonoBehaviour
{
    public GameObject parent;
    private float delayTime = 0.2f;
   public GameObject LeParent;
    // Start is called before the first frame update
    void Start()
    {
   //     parent = GameObject.Find("Track");
        StartCoroutine(Respawn(delayTime));
  //     LeParent = gameObject.transform.parent.gameObject;
 //       gameObject.transform.parent = parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     IEnumerator Respawn(float delayTime)
{
   yield return new WaitForSeconds(delayTime);
//   gameObject.transform.parent = LeParent.transform;
}
}
