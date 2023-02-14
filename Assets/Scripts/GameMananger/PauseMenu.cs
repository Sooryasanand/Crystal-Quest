using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    
    GameObject playerPrefab;
    Unit playerUnit;
    private Scene scene;
    
    public static bool isPaused;
    public Text currentObjective;


    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        playerUnit = playerPrefab.GetComponent<Unit>();
        
        scene = SceneManager.GetActiveScene();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void SaveData()
    {
        SaveSystem.SetFloat("playerScene", scene.buildIndex);
        SaveSystem.SetFloat("unitLevel", playerUnit.unitLevel);
        SaveSystem.SetFloat("unitDamage", playerUnit.damage);
        SaveSystem.SetFloat("unitMaxHP", playerUnit.maxHP);
        SaveSystem.SetFloat("unitCurrentHP", playerUnit.currentHP);
        SaveSystem.SetString("playerObjective", currentObjective.text);
    }

    public void QuitGame()
    {
        SaveData();
        SaveSystem.SaveToDisk();
        Application.Quit();
    }
}
