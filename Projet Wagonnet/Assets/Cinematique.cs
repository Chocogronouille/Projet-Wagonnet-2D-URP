using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematique : MonoBehaviour
{

    private GameObject CineDebut;
    private GameObject CineMillieu;
    private GameObject CineFin;

    private Dialogue dialogue1;
    // Start is called before the first frame update
    void Start()
    {
        CineDebut = GameObject.Find("Cinématique_Début");
        CineMillieu = GameObject.Find("Cinématique_Millieu");
        CineFin = GameObject.Find("Cinématique_Fin");
        CineDebut.SetActive(true);
        CineMillieu.SetActive(false);
        CineFin.SetActive(false);
        dialogue1 = gameObject.GetComponent<DialogueTrigger>().dialogue;
        DialogueManager.instance.StartDialogue(dialogue1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
