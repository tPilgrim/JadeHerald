using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateManager : MonoBehaviour
{
    public int Scene;
    public int Gate;
    private GameObject Player;
    public bool FloatUp;
    private Rigidbody2D PlayerRb;
    //public GameObject EnemyComponent;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        //EnemyComponent = GameObject.FindGameObjectWithTag("Enemy");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(FloatUp == true)
            {
                PlayerRb.gravityScale = -5;
            }

            for (int i = 0; i <= 3; i++)
            {
                PlayerPrefs.SetInt("Scene" + SceneManager.GetActiveScene().buildIndex, 0);
            }
            Player.GetComponent<PlayerController>().Save();
            Player.GetComponent<LevelManager>().StartTransition(Scene, Gate);
        }
    }
}
