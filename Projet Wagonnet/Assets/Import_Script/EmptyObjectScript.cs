using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.transform.localScale = new Vector3(1,1,0);
   //     GetComponent<BoxCollider2D>().size = new Vector2(5f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Oui");
        }
    }
        
    }
}
