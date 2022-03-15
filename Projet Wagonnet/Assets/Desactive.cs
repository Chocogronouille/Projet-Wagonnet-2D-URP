using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desactive : MonoBehaviour
{
    public bool isSurfing2;
    // Start is called before the first frame update
     public static Desactive instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Desactive dans la scène");
            return;
        }

        instance = this;
    }
    void Start()
    {
        InstanciateObject.instance.isInstancie = false;
    }

    // Update is called once per frame
    void Update()
    {
 /*       if(isSurfing2)
        {
           EmptyObjectScript.instance.isSurfing = true;
        } */
        
    }
}
