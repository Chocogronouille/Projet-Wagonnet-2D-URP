using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCam : MonoBehaviour
{
     public static SingletonCam instance;
     private void Awake()
    {
            if (instance != null)
            {
              Destroy(gameObject);
              return;
            }
            instance = this;
    }
}
