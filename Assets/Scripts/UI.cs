using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private GameObject Player;
    private AudioSource AudioManager;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        AudioManager = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        AudioManager.volume -= Time.deltaTime*340;
        PlayerPrefs.SetInt("SceneReload", 9);
        PlayerPrefs.SetFloat("PosX", 0f);
        PlayerPrefs.SetFloat("PosY", 1.5f);
        PlayerPrefs.SetInt("CurrentHealth", 30);
        PlayerPrefs.SetInt("RegenerationLevel", 0);
        PlayerPrefs.SetInt("Mana", 2);
        Player.GetComponent<LevelManager>().StartTransition(1,1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
