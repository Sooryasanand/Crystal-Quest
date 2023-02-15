using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public DialogueTrigger trigger;
    public GameObject triggerText;
    public Text userQuestion;
    bool triggerStay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && triggerStay)
        {
            triggerText.SetActive(false);
            trigger.StartDialogue();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            userQuestion.text = "Press 'C' to chat";
            triggerStay = true;
            triggerText.SetActive(true);
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = false;
            triggerText.SetActive(false);
        }
    }
}
