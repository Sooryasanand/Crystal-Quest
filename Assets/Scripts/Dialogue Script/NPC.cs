using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DialogueTrigger trigger;
    public GameObject triggerText;
    bool triggerStay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && triggerStay)
        {
            triggerText.SetActive(false);
            trigger.StartDialogue();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
