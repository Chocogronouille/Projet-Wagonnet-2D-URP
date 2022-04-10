using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonParallax : MonoBehaviour
{
    public static SingletonParallax instance;

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
