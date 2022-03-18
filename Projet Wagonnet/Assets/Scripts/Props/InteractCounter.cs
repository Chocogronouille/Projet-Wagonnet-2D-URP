using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCounter : MonoBehaviour
{
    public int interactCount;
    public static InteractCounter instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance d'Interact Counter dans la scène");
            return;
        }
        instance = this;
    }

    public void AddCounter(int count)
    {
        interactCount += count;
    }
}
