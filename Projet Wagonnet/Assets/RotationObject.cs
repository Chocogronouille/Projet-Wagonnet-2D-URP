using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    public bool isFlip;
    public bool isDetect;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlip)
        {
          gameObject.transform.Rotate(0,0,-0.4f);
        }
        else
        {
          gameObject.transform.Rotate(0,0,0.4f);
        }

        if(isDetect)
        {
           Player.transform.localRotation = gameObject.transform.localRotation;
        }
  /*        else if(!isDetect)
        {
           Player.transform.localRotation = new Quaternion(0,0,0,0);
        } */
       
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.CompareTag("TheDetection"))
        {
          isDetect = true;
        }
    }

        void OnTriggerExit2D(Collider2D col)
    {
           if(col.CompareTag("TheDetection"))
        {
          isDetect = false;
        }
    }
}
