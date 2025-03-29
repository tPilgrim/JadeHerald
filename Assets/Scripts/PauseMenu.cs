using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void Checkpoint()
    {
        PlayerPrefs.SetInt("Respawn", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("SceneReload"));
    }

    public void MainMenu()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
