using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    [SerializeField] GameObject dialogueObject;
    [SerializeField] GameObject player;
    [SerializeField] GameObject scoreObject;

    ScoreBoard score;
    public Animator animator;

    private Queue<string> sentences;


    void Start() 
    {
        sentences = new Queue<string>();
        animator.SetBool("isOpen", true);
        score = scoreObject.GetComponent<ScoreBoard>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        player.SetActive(true);
        score.begin = true;
        dialogueObject.SetActive(false);
    }
}
