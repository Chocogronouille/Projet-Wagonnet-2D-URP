using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CounterCassette : MonoBehaviour
{
    public float currentCassetteCount;
    public float CassetteLevel;
    public static CounterCassette instance;
    public TMP_Text interactCountText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        LoadSaveCassette();
        interactCountText.text = currentCassetteCount.ToString();
        CassetteLevel = PlayerPrefs.GetFloat("Cassette");
    }

    public void AddCounterCassette(int count)
    {
        currentCassetteCount += count;
        interactCountText.text = currentCassetteCount.ToString();
        SaveCassette();
    }
    public void SaveCassette()
    {
        PlayerPrefs.SetFloat("Cassette", currentCassetteCount);
    }
    public void LoadSaveCassette()
    {
        currentCassetteCount = PlayerPrefs.GetFloat("Cassette");
        Debug.Log(PlayerPrefs.GetFloat("Cassette"));
    }
}
