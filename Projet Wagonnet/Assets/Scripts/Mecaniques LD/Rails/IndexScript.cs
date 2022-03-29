using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexScript : MonoBehaviour
{
    public int index = 0;
    public Transform parent;
    private float delayTime = 0.05f ;
    // Start is called before the first frame update
    void Start()
    {
        index = gameObject.transform.GetSiblingIndex();
        parent = transform.parent;
        parent.gameObject.AddComponent<ListOfChild>();
        parent.GetComponent<ListOfChild>().Newindex = index;
        parent.GetComponent<ListOfChild>().enabled = true;
        Destroy(gameObject,0.1f);
        StartCoroutine(LaCouroutine(delayTime));
    }

    IEnumerator LaCouroutine(float delayTime)
    {
       yield return new WaitForSeconds(delayTime);
       TrackCreator.instance.InstanciateTest();
    }
}
