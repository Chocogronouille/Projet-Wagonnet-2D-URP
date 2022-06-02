using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterToken : MonoBehaviour
{
    public float currentTokenCount;
    public float TokenLevel;
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
        LoadSaveToken();
        interactCountText.text = currentTokenCount.ToString(); 
        TokenLevel = PlayerPrefs.GetFloat("Token");
    }

    public void AddCounterToken(int count)
    {
        currentTokenCount += count;
        interactCountText.text = currentTokenCount.ToString();   
        SaveToken();
    }

        public void SaveToken()
    {
        PlayerPrefs.SetFloat("Token", currentTokenCount);
    }
    public void LoadSaveToken()
    {
        currentTokenCount = PlayerPrefs.GetFloat("Token");
        Debug.Log(PlayerPrefs.GetFloat("Token"));
    }
}
