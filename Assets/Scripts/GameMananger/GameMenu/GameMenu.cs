using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public Button startButton;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Home");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
