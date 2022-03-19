using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterCassette : MonoBehaviour
{
    public int currentCassetteCount;
    public static CounterCassette instance;
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

    public void AddCounterCassette(int count)
    {
        currentCassetteCount += count;
        interactCountText.text = currentCassetteCount.ToString();
    }
}
