using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instaciate : MonoBehaviour
{
    public GameObject test;

    public static Instaciate instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de TrackCreator dans la scène");
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InstanciateTest()
    {
        Instantiate(test);
    } 
}
