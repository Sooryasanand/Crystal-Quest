using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{
    // Enemy and Player prefab refrences
    GameObject playerPrefab;
    GameObject enemyPrefab;

    // Reference for player and enemy units 
    Unit playerUnit;
    Unit enemyUnit;
    
    // Animator Reference for the player and enemy
    Animator enemyAnimator;
    Animator playerAnimator;

    // Reference for dialogue text
    public Text dialogueText;
    
    // Reference for HUD which shows the player and enemy stats
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    
    // Attack and Heal buttons reference
    public GameObject AttackButton;
    public GameObject HealButton;
    public GameObject SkipYesButton;
    public GameObject SkipNoButton;
    
    // The Player Game object reference
    GameObject Player;

    // Reference for audio source of the battle system and all the sounds
    public AudioSource audioSource;
    public AudioClip HeroHit;
    public AudioClip VillanHit;
    public AudioClip Win;
    public AudioClip Lose;

    // Reference to the current state of the game
    public BattleState state;
    
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        // Start the SetupBattle coroutine
        StartCoroutine(SetupBattle());
    }
    
    //==============Procedure===============//
    // Used the IEnumerator function so we can yield for a certain time
    IEnumerator SetupBattle()
    // Purpose:  Set's up everything including units and the player prefab before the battle start
    {
        // Set appropriate prefabs
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");

        // Set appropriate character units
        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();

        // Set appropriate animators for each character
        enemyAnimator = enemyPrefab.GetComponent<Animator>();
        playerAnimator = playerPrefab.GetComponent<Animator>();

        enemyUnit.currentHP = enemyUnit.maxHP;
        
        // To check if the user has a low heart and not enough heart to play
        if (playerUnit.currentHP <= 5)
        {
            AttackButton.SetActive(false);
            HealButton.SetActive(false);
            dialogueText.text = "Your low on heart!! Revive please";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }

        // Checks if the player has double the enemy        
        if (enemyUnit.maxHP < 2 * playerUnit.currentHP)
        {
            AttackButton.SetActive(false);
            HealButton.SetActive(false);
            dialogueText.text = "Do you want to Skip?";
            SkipYesButton.SetActive(true);
            SkipNoButton.SetActive(true);
            yield return new WaitForSeconds(10f);
        }
        
        SkipYesButton.SetActive(false);
        SkipNoButton.SetActive(false);
        
        // Position to set when the enemy is spawned.
        if (enemyUnit.name == "Vulture")
        {
            enemyUnit.transform.position = new Vector3(-8.7f, 3.7f);
            enemyUnit.transform.localScale = new Vector3(2, 2, 2);
        }
        
        if (enemyUnit.name == "Deceased")
        {
            enemyUnit.transform.position = new Vector3(-8.7f, 3.7f);
            enemyUnit.transform.localScale = new Vector3(2, 2, 2);
        }
        
        // Position to set when the player is spawned
        playerUnit.transform.position = new Vector3(-17.56f, 0.5f);
        playerUnit.transform.localScale = new Vector3(2, 2, 2);
 
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        dialogueText.text = "What will " + enemyUnit.unitName + " do?";
        
        // Set the units to HUD 
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        
        // Save the current data and to file
        SaveData();
        SaveSystem.SaveToDisk();

        // Set state to player turns
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // Returns to the main menu
    public void OnSkipYes()
    {
        SceneManager.LoadScene(1);
    }
    
    // Continue the battle
    public void OnSkipNo()
    {
        return;
    }
    
    //==============Procedure===============//
    IEnumerator PlayerAttack()
    // Purpose: Function when the player attacks the enemy
    {
        // Damage the Enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        
        yield return new WaitForSeconds(2f);
        
        // Set Enemy HP and play animations when hurt depending on the enemy
        enemyHUD.SetHP(enemyUnit.currentHP);
        if (enemyUnit.name == "Deceased")
        {
            audioSource.PlayOneShot(HeroHit);
            enemyAnimator.Play("Deceased_Hurt");
            yield return new WaitForSeconds(1f);
            enemyAnimator.Play("Deceased_Idle");
        }

        if (enemyUnit.name == "Vulture")
        {
            enemyAnimator.Play("Vulture_Hurt");
            audioSource.PlayOneShot(HeroHit);
            yield return new WaitForSeconds(1f);
            enemyAnimator.Play("Vulture_idle");
        }

        dialogueText.text = "The attack is successful!";

        // if statement to check if the enemy is dead or not to continue the game
        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    //==============Procedure===============//
    IEnumerator EnemyTurn()
    // Purpose: Function to run when it's the enemy's turn and enemy's attack
    {
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        
        // play animations when attacking depending on the enemy
        if (enemyUnit.name == "Deceased")
        {
            enemyAnimator.Play("Deceased_Attack");
            yield return new WaitForSeconds(1f);
            enemyAnimator.Play("Deceased_Idle");
        }
        if (enemyUnit.name == "Vulture")
        {
            enemyAnimator.Play("Vulture_Attack");
            yield return new WaitForSeconds(1f);
            enemyAnimator.Play("Vulture_idle");
        }
        
        // Attack audio and dialogue text
        audioSource.PlayOneShot(VillanHit);
        dialogueText.text = enemyUnit.unitName + " attacks!!";

        yield return new WaitForSeconds(1f);
        
        // To check if the player is dead or not based on the health
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        
        playerHUD.SetHP(playerUnit.currentHP);
        
        yield return new WaitForSeconds(1f);

        // If statement to run based on if the player is dead or not
        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }
    
    //==============Procedure===============//
    public void SaveData()
    // Purpose: Set the player data to be saved
    {
        SaveSystem.SetFloat("unitLevel", playerUnit.unitLevel);
        SaveSystem.SetFloat("unitDamage", playerUnit.damage);
        SaveSystem.SetFloat("unitMaxHP", playerUnit.maxHP);
        SaveSystem.SetFloat("unitCurrentHP", playerUnit.currentHP);
    }

    IEnumerator EndBattle()
    // Purpose: If the battle ends, it saves all the stats and returns to the main scene
    {
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        // If statement to run based on if the player lost or won the battle
        if (state == BattleState.WON)
        {
            // Dead animation to play by the enemy
            if (enemyUnit.name == "Deceased")
            {
                enemyAnimator.Play("Deceased_Dead");
                yield return new WaitForSeconds(1f);
            }
            if (enemyUnit.name == "Vulture")
            {
                enemyAnimator.Play("Vulture_Dead");
                yield return new WaitForSeconds(1f);
            }
            // Win audio sound to play and the dialogue text
            audioSource.PlayOneShot(Win);
            dialogueText.text = "You have won the battle!";
            yield return new WaitForSeconds(3f);
            // Set the player units including the level damage and maxHP
            playerUnit.LevelFinish(1, 10, 40);
            // Save the data from the above list
            SaveData();
            SaveSystem.SaveToDisk();
            // Load the scene back and destroy the game objects
            SceneManager.LoadScene(1);
            Destroy(playerPrefab);
            Destroy(enemyPrefab);
        } else if (state == BattleState.LOST)
        {
            // Dead animation to play by the player
            playerAnimator.Play("Player_Dead");
            // Lose audio sound to play and the dialogue text
            audioSource.PlayOneShot(Lose);
            dialogueText.text = "You have lost the battle!";
            yield return new WaitForSeconds(3f);
            // Set the player units including the level damage and maxHP
            playerUnit.LevelFinish(0, 3, 15);
            // Save the data from the above list
            SaveData();
            SaveSystem.SaveToDisk();
            // Go back to the home scene back and destroy the game objects
            SceneManager.LoadScene(1);
            Destroy(playerPrefab);
            Destroy(enemyPrefab);
        }
    }
    
    //==============Procedure===============//
    void PlayerTurn()
    // Purpose: Function when it's players turn
    {
        // Show the button
        AttackButton.SetActive(true);
        HealButton.SetActive(true);
        
        // Set dialogue text
        dialogueText.text = "Choose an action:";
    }

    //==============Procedure===============//
    IEnumerator PlayerHeal()
    // Purpose: when player heal button is clicked it regains the player heal
    {
        // Increase player Heal by 5 and update that on the screen
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);
        
        // Give information back to the user
        dialogueText.text = "You have renewed your Strength!";

        // Set button in active
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        yield return new WaitForSeconds(1f);

        // Set the state to enemy's turn and run the EnemyTurn function
        state = BattleState.ENEMYTURN;
        EnemyTurn();
    }

    //==============Procedure===============//
    public void OnAttackButton()
    // Purpose: When attack button is pressed
    {
        // Guard to check if it's actually the player's turn
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        // Start the PlayerAttack function
        StartCoroutine(PlayerAttack());
    }
    
    //==============Procedure===============//
    public void OnHealButton()
    // Purpose: When heal button is pressed
    {
        // Guard to check if it's actually the player's turn
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        // Start the PlayerHeal function
        StartCoroutine(PlayerHeal());
    }

}
