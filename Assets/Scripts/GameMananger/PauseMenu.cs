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
    public Animator ObjectiveAnimation;
    
    public float SaveTimeLeft = 1800f;

    public string previousObjective;


    // Start is called before the first frame update
    void Start()
    {
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        playerUnit = playerPrefab.GetComponent<Unit>();
        
        scene = SceneManager.GetActiveScene();
        pauseMenu.SetActive(false);
    }

    IEnumerator SaveInfo()
    {
        ObjectiveAnimation.Play("ObjectiveBoxExit");
        yield return new WaitForSeconds(2f);
        previousObjective = currentObjective.text;
        currentObjective.text = "Auto Saving... (Please do not quit the game)";
        ObjectiveAnimation.Play("ObjectiveBoxEntry");
        
        yield return new WaitForSeconds(5f);
        
        ObjectiveAnimation.Play("ObjectiveBoxExit");
        yield return new WaitForSeconds(2f);
        currentObjective.text = previousObjective;
        ObjectiveAnimation.Play("ObjectiveBoxEntry");
    }

    // Update is called once per frame
    void Update()
    {
        SaveTimeLeft -= Time.deltaTime;
        if (SaveTimeLeft <= 0f)
        {
            SaveData();
            SaveSystem.SaveToDisk();
            SaveTimeLeft = 1800f;
            StartCoroutine(SaveInfo());
        }
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
        SaveSystem.SetVector3("playerPosition", playerPrefab.transform.position);
    }

    public void QuitGame()
    {
        SaveData();
        SaveSystem.SaveToDisk();
        Application.Quit();
    }
}
