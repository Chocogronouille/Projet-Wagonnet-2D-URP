using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterAttraction : MonoBehaviour
{
    public int currentAttractionCount;
    public static CounterAttraction instance;
    public TMP_Text interactCountText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance d'Interact Counter dans la scène");
            return;
        }
        instance = this;
    }

    public void AddCounterAttraction(int count)
    {
        currentAttractionCount += count;
        interactCountText.text = currentAttractionCount.ToString();
    }
}
