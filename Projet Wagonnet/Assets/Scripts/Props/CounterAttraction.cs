using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterAttraction : MonoBehaviour
{
    public float currentAttractionCount;
    public float AttractionLevel;
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
        LoadSaveAttraction();
        interactCountText.text = currentAttractionCount.ToString();
        AttractionLevel = PlayerPrefs.GetFloat("Attraction");
    }

    public void AddCounterAttraction(int count)
    {
        currentAttractionCount += count;
        interactCountText.text = currentAttractionCount.ToString();
        SaveAttraction();
    }
    public void SaveAttraction()
    {
        PlayerPrefs.SetFloat("Attraction", currentAttractionCount);
    }
    public void LoadSaveAttraction()
    {
        currentAttractionCount = PlayerPrefs.GetFloat("Attraction");
        Debug.Log(PlayerPrefs.GetFloat("Attraction"));
    }
}
