using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;
    
    public Slider hpSlider;

    GameObject camera;
    public int scene;
    
    public AudioSource audioSource;
    public AudioClip HealAudio;

    public Scene currentScene;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true; return false;
    }
    
    static string GetPath()
    {
        return Application.persistentDataPath + "/" + "Gamedata.bin";
    }
    
    public void LoadData()
    {
            float UnitLevel = SaveSystem.GetFloat("unitLevel");
            float UnitDamage = SaveSystem.GetFloat("unitDamage");
            float UnitMaxHP = SaveSystem.GetFloat("unitMaxHP");
            float UnitCurrentHP = SaveSystem.GetFloat("unitCurrentHP");
            float PlayerScene = SaveSystem.GetFloat("playerScene");
            string PlayerObjective = SaveSystem.GetString("playerObjective");

            unitLevel = (int)UnitLevel;
            damage = (int)UnitDamage;
            maxHP = (int)UnitMaxHP;
            currentHP = (int)UnitCurrentHP;
            scene = (int)PlayerScene;
            currentScene = SceneManager.GetActiveScene();
            GameObject.FindGameObjectWithTag("Objective").GetComponent<ObjectiveUpdate>().updateObjective(PlayerObjective);
            
            if (gameObject.tag == "Player")
            {
                if (currentScene.buildIndex == 1)
                {
                    gameObject.transform.position = new Vector2(-658.7261f, 80.7179f);
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
            }
        }
       
    }

    private void Update()
    {
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;
    }
}
