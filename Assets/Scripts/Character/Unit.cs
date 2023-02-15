using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    // Reference for player units
    public string unitName;
    public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public Slider hpSlider;
    public Vector3 playerPosition;

    // Reference game object for camera
    GameObject camera;
    public int scene;
    
    // Reference for audio source and clip
    public AudioSource audioSource;
    public AudioClip HealAudio;

    // Reference for the currentscene
    public Scene currentScene;
    
    public bool TakeDamage(int dmg)
    // Purpose: Takes player damage
    {
        // Reduces the damage from the current hp depending on the value entered when calling the function
        currentHP -= dmg;
        
        if (currentHP <= 0)
            return true; return false;
    }
    
    static string GetPath()
    // Purpose: Get the path of the player data file
    {
        return Application.persistentDataPath + "/" + "Gamedata.bin";
    }
    
    public void LoadData()
    // Purpose: Load all the player data and objectives from the data file
    {
            float UnitLevel = SaveSystem.GetFloat("unitLevel");
            float UnitDamage = SaveSystem.GetFloat("unitDamage");
            float UnitMaxHP = SaveSystem.GetFloat("unitMaxHP");
            float UnitCurrentHP = SaveSystem.GetFloat("unitCurrentHP");
            float PlayerScene = SaveSystem.GetFloat("playerScene");
            string PlayerObjective = SaveSystem.GetString("playerObjective");
            Vector3 PlayerPosition = SaveSystem.GetVector3("playerPosition");

            // Sets the data from the file to the above referenced player units
            unitLevel = (int)UnitLevel;
            damage = (int)UnitDamage;
            maxHP = (int)UnitMaxHP;
            currentHP = (int)UnitCurrentHP;
            scene = (int)PlayerScene;
            playerPosition = PlayerPosition;
            Debug.Log("PlayerPosition");
            currentScene = SceneManager.GetActiveScene();
            if (PlayerObjective == "Auto Saving... (Please do not quit the game)")
            {
                GameObject.FindGameObjectWithTag("Objective").GetComponent<ObjectiveUpdate>().updateObjective("Talk to Wizard");
            }
            else
            {
                GameObject.FindGameObjectWithTag("Objective").GetComponent<ObjectiveUpdate>().updateObjective(PlayerObjective);
            }
            
            
            if (gameObject.tag == "Player")
            {
                if (currentScene.buildIndex == 1)
                {
                    gameObject.transform.position = playerPosition;
                }
            }
    }

    public void Heal(int amount)
    {
        audioSource.PlayOneShot(HealAudio);
        currentHP += amount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        SaveSystem.SetFloat("unitCurrentHP", currentHP);
    }
    
    public void LevelFinish(int Level, int Damage, int MaxHP)
    {
        if (currentHP >= 0)
        {
            currentHP = 5;
        }
        unitLevel += Level;
        damage += Damage;
        maxHP += MaxHP;
    }

    private void Start()
    {
        if (File.Exists(GetPath()))
        {
            LoadData();
        }
        else
        {
            unitLevel = 1;
            damage = 10;
            maxHP = 40;
            currentHP = 40;
            scene = 1;

            if (gameObject.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("Objective").GetComponent<ObjectiveUpdate>().updateObjective("Talk to Wizard");
                gameObject.transform.position = new Vector2(-658.7261f, 80.7179f);
            }
        }
       
    }

    private void Update()
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
        
        
    }
}
