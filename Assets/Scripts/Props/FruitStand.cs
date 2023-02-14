using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitStand : MonoBehaviour
{
    public GameObject userQuestionGameObject;
    public Text userQuestion;
    bool triggerStay;
    private Unit playerUnit;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && triggerStay)
        {
            playerUnit.Heal(20);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = true;
            playerUnit = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
            userQuestionGameObject.SetActive(true);
            userQuestion.text = "Do you want to eat an apple to revive? Press the E key";
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            triggerStay = false;
            userQuestionGameObject.SetActive(false);
        }
    }
}
