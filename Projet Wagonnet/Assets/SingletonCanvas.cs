using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCanvas : MonoBehaviour
{
    public static SingletonCanvas instance;
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
