using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabineConcierge : MonoBehaviour
{

    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().SetBool("isPassed",true);
        AudioManager.instance.PlayClipAt(sound, transform.position);
        StartCoroutine(isPassedFalse());
    }

    IEnumerator isPassedFalse()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Animator>().SetBool("isPassed",false);
    }
}
