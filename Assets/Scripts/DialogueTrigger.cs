using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueController>().StartDialogue(dialogue);
        transform.position = new Vector2(10f, -51.07f);
    }
}
