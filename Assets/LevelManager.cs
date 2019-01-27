﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevel() {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}