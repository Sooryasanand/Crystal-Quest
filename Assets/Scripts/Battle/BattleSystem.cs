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

    GameObject playerPrefab;
    GameObject enemyPrefab;

    Unit playerUnit;
    Unit enemyUnit;
    
    Animator enemyAnimator;
    Animator playerAnimator;

    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    public GameObject AttackButton;
    public GameObject HealButton;
    GameObject Player;

    public AudioSource audioSource;
    public AudioClip HeroHit;
    public AudioClip VillanHit;
    public AudioClip Win;
    public AudioClip Lose;

    public float timer;

    public BattleState state;
    
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        enemyPrefab = GameObject.FindGameObjectWithTag("Enemy");

        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();

        enemyAnimator = enemyPrefab.GetComponent<Animator>();
        playerAnimator = playerPrefab.GetComponent<Animator>();

        enemyUnit.currentHP = enemyUnit.maxHP;
        
        if (playerUnit.currentHP <= 5)
        {
            AttackButton.SetActive(false);
            HealButton.SetActive(false);
            dialogueText.text = "Your low on heart!! Revive please";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
        }
        

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
        
        playerUnit.transform.position = new Vector3(-17.56f, 0.5f);
        playerUnit.transform.localScale = new Vector3(2, 2, 2);
 
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        dialogueText.text = "What will " + enemyUnit.unitName + " do?";
        
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        
        SaveData();
        SaveSystem.SaveToDisk();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        // Damage the Enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        
        yield return new WaitForSeconds(2f);
        
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

    IEnumerator EnemyTurn()
    {
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        yield return new WaitForSeconds(2f);
        
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
        
        audioSource.PlayOneShot(VillanHit);
        dialogueText.text = enemyUnit.unitName + " attacks!!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        
        playerHUD.SetHP(playerUnit.currentHP);
        
        yield return new WaitForSeconds(1f);

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
    
    public void SaveData()
    {
        SaveSystem.SetFloat("unitLevel", playerUnit.unitLevel);
        SaveSystem.SetFloat("unitDamage", playerUnit.damage);
        SaveSystem.SetFloat("unitMaxHP", playerUnit.maxHP);
        SaveSystem.SetFloat("unitCurrentHP", playerUnit.currentHP);
    }

    IEnumerator EndBattle()
    {
        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        if (state == BattleState.WON)
        {
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
            audioSource.PlayOneShot(Win);
            dialogueText.text = "You have won the battle!";
            yield return new WaitForSeconds(3f);
            playerUnit.LevelFinish(1, 10, 40);
            SaveData();
            SaveSystem.SaveToDisk();
            SceneManager.LoadScene(1);
            Destroy(playerPrefab);
            Destroy(enemyPrefab);
        } else if (state == BattleState.LOST)
        {
            playerAnimator.Play("Player_Dead");
            audioSource.PlayOneShot(Lose);
            dialogueText.text = "You have lost the battle!";
            yield return new WaitForSeconds(3f);
            playerUnit.LevelFinish(0, 3, 15);
            SaveData();
            SaveSystem.SaveToDisk();
            SceneManager.LoadScene(1);
            Destroy(playerPrefab);
            Destroy(enemyPrefab);
        }
    }

    void PlayerTurn()
    {
        AttackButton.SetActive(true);
        if (timer != 0)
        {
            HealButton.SetActive(true);
        }
        else
        {
            HealButton.SetActive(false);
        }
        
        
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);
        
        dialogueText.text = "You have renewed your Strength!";

        AttackButton.SetActive(false);
        HealButton.SetActive(false);
        
        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        StartCoroutine(PlayerAttack());
    }
    
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        AttackButton.SetActive(false);
        HealButton.SetActive(false);

        StartCoroutine(PlayerHeal());
    }

}
