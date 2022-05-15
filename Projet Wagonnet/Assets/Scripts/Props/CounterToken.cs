using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterToken : MonoBehaviour
{
    public int currentTokenCount;
    public static CounterToken instance;
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

    public void AddCounterToken(int count)
    {
        currentTokenCount += count;
        interactCountText.text = currentTokenCount.ToString();
    }
}
