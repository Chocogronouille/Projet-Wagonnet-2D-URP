using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChargeMenu : MonoBehaviour
{
    private float MusicVolume;
    private float SoundVolume;
    private bool isDownVolume;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    //    MusicVolume = SettingsMenu.instance.musicSlider.GetComponent<Slider>().value;
        if(isDownVolume)
        {
          StartCoroutine(DownSound());
        }
    }

    public void ChargeMainMenu()
    {
      GameManage.instance.LoadMenu();
    }
        public void BaseVolume()
    {
      MusicVolume = SettingsMenu.instance.musicSlider.GetComponent<Slider>().value;
      SoundVolume = SettingsMenu.instance.soundSlider.GetComponent<Slider>().value;
      isDownVolume = true;
    }

    private IEnumerator DownSound()
    {
      isDownVolume = false;
      SettingsMenu.instance.SetVolume(MusicVolume -= 1f);
      SettingsMenu.instance.SetSoundVolume(SoundVolume -= 1f);
      yield return new WaitForSeconds(0.1f);
      isDownVolume = true;
    }
}
