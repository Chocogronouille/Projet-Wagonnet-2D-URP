using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chronometre : MonoBehaviour
{
    public Text Chrono;
 //   public Text ChronoDix;
    public float timer = 0;
    public bool isTiming;
/*    public float seconde;
    public float dix; */
    // Start is called before the first frame update

    public static Chronometre instance;

    private void Awake()
    {
            if (instance != null)
            {
              Destroy(gameObject);
              return;
            }
            instance = this;
    }

    void Start()
    {
        timer = PlayerPrefs.GetFloat("Timer");
    }

    // Update is called once per frame
    void Update()
    {
//      seconde = Mathf.Floor (timer % 60);
    //  dix = timer - seconde;
    if(isTiming)
    {
      timer += Time.deltaTime;
    }
  //    Chrono.text = "" + timer;  
      Chrono.text = string.Format ("{0:00}:{1:00}", Mathf.Floor (timer / 60), timer % 60);
   //   ChronoDix.text = "" + dix;
    }

    public void SaveTimer()
    {
        PlayerPrefs.SetFloat("Timer", timer);
    }
    public void LoadSaveTimer()
    {
        timer = PlayerPrefs.GetFloat("Timer");
    }
}
