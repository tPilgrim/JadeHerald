using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    private GameObject Player;
    public Animator AnimUI;

    private bool IsInReach;

    void Start()
    {
        //PlayerPrefs.SetInt(gameObject.transform.name, 0);
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetButton("Interact") && IsInReach == true)
        {
            SceneManager.LoadScene(0);
            for(int i = 1; i<=4; i++)
            {
                PlayerPrefs.SetInt("Crimson " + i, 0);
            }
        }
    }

    public void Button()
    {
        Player.GetComponent<LevelManager>().StartTransition(0, 1);
        for (int i = 1; i <= 4; i++)
        {
            PlayerPrefs.SetInt("Crimson " + i, 0);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AnimUI.SetBool("Active", true);
            IsInReach = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AnimUI.SetBool("Active", false);
            IsInReach = false;
        }
    }
}
