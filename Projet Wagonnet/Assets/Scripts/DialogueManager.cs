using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    private GameObject player;
    private string sentence;
    public bool isFinished;

    public static DialogueManager instance;

    private void Awake()
    {
        player = GameObject.Find("Player");
        if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DialogueManager dans la scène");
            return;
        }

        instance = this;

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        StopCoroutine(Timer());
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
        StartCoroutine(LeButton());
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    private IEnumerator LeButton()
    {
        yield return new WaitForSeconds(0.2f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameManage.instance.DialogueButton);
    }

    public void DisplayNextSentence()
    {
         if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

         sentence = sentences.Dequeue();
      //   StopAllCoroutines();
         StartCoroutine(TypeSentence(sentence));

        
    }
        public void DNSButton()
    {
        if(!isFinished)
        {
           StopAllCoroutines();
           dialogueText.text = sentence;
           isFinished = true;
        }
        else
        {
         if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

         sentence = sentences.Dequeue();
         StopAllCoroutines();
         StartCoroutine(TypeSentence(sentence));

        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            isFinished = false;
            yield return new WaitForSeconds(0.02f);
        }
        isFinished = true;
    }

    public void EndDialogue()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(Timer());
        animator.SetBool("isOpen", false);
        DialogueTrigger.instance.isOpen = false;
        Debug.Log("EndDialogue");
    }

    IEnumerator Timer()
    {
            yield return new WaitForSeconds(0.8f);
            player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }
}