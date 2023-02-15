using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Mission1MettingAurorium mission1;
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    
    GameObject playerPrefab;
    public Text CurrentObjective;

    Unit playerUnit;

    public GameObject enemyToMove;
    private Message[] currentMessages;
    private Actor[] currentActors;
    private int activeMessage = 0;
    public static bool isActive = false;
    private string nextObjective;
    public Animator ObjectiveAnimation;
    public bool isEnemyTrigger;
    public string TheMission;
    GameObject Player;
    

    public void OpenDialogue(Message[] messages, Actor[] actors, string nextObject, bool isEnemy, string Mission, GameObject Enemy, Text currentObjective)
    {
        ObjectiveAnimation.Play("ObjectiveBoxExit");
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        nextObjective = nextObject;
        isEnemyTrigger = isEnemy;
        CurrentObjective = currentObjective;
        TheMission = Mission;
        enemyToMove = Enemy;
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        
        AnimateTextColor();
    }
    
    public void SaveData()
    {
        SaveSystem.SetFloat("unitLevel", playerUnit.unitLevel);
        SaveSystem.SetFloat("unitDamage", playerUnit.damage);
        SaveSystem.SetFloat("unitMaxHP", playerUnit.maxHP);
        SaveSystem.SetFloat("unitCurrentHP", playerUnit.currentHP);
        SaveSystem.SetString("playerObjective", CurrentObjective.text);
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            
            if (isEnemyTrigger)
            {
                SaveData();
                Player = GameObject.FindGameObjectWithTag("Player");
                Player.transform.parent = null;
                enemyToMove.transform.parent = null;
                DontDestroyOnLoad(enemyToMove);
                DontDestroyOnLoad(Player);
                SceneManager.LoadScene(2);
            }
            
            if (nextObjective == "")
            {
                ObjectiveAnimation.Play("ObjectiveBoxEntry");
                return;
            }

            if (TheMission == "Mission 1")
            {
                mission1.Prerequistes();
            }

            
        }
    }

    public void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        playerPrefab = GameObject.FindGameObjectWithTag("Player");

        playerUnit = playerPrefab.GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isActive)
        {
            NextMessage();
        }
    }
}
