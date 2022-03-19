using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRails : MonoBehaviour
{
    public GameObject PlayerPosPos;
    public bool isDestroy;
    public int indexPlayer;

    public static DestroyRails instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DestroyRails dans la scène");
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDestroy)
        {
           indexPlayer = PlayerPosPos.transform.GetSiblingIndex(); 
        }
        if(indexPlayer == 0)
        {

        }
        else
        {
          Debug.Log("Oui");
        }
        
    }
}
