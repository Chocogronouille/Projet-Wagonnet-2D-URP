using System;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroyOnLoad : MonoBehaviour
{
    public GameObject[] objects;
    public Animator fadeSystem;

    
    private void Awake()
    {
        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
            fadeSystem.SetTrigger("FadeIn");
        }
    }
}
