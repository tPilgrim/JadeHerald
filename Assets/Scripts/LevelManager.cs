using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator TransitionAnim;
    private GameObject Player;
    private Rigidbody2D PlayerRb;
    private int Scene;
    private int Gate;

    public GameObject Gate1;
    public GameObject Gate2;
    public GameObject Gate3;
    public GameObject Gate4;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        StartCoroutine(Teleport());
    }

    IEnumerator Teleport()
    {
        TransitionAnim.SetTrigger("End");
        PlayerRb.gravityScale = 20;
        yield return new WaitForSeconds(0.1f);
        PlayerRb.gravityScale = 1;

        if (PlayerPrefs.GetInt("Respawn") == 0)
        {
            if (PlayerPrefs.GetInt("Gate") == 1)
            {
                gameObject.transform.position = Gate1.transform.position;
            }

            if (PlayerPrefs.GetInt("Gate") == 2)
            {
                gameObject.transform.position = Gate2.transform.position;
            }

            if (PlayerPrefs.GetInt("Gate") == 3)
            {
                gameObject.transform.position = Gate3.transform.position;
            }

            if (PlayerPrefs.GetInt("Gate") == 4)
            {
                gameObject.transform.position = Gate4.transform.position;
            }
        }
        else
        {
            PlayerPrefs.SetInt("Respawn", 0);
        }
    }

    public void StartTransition(int SceneNum, int GateNum)
    {
        Scene = SceneNum;
        Gate = GateNum;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        PlayerPrefs.SetInt("Gate", Gate);
        TransitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(Scene);
    }
/*
    public void Teleport(int SceneNum, int GateNum)
    {
        Scene = SceneNum;
        Gate = GateNum;
        if(Task == 1)
        {
            if (this.TransitionAnim.GetCurrentAnimatorStateInfo(0).IsName("Transition End"))
            {
                if (PlayerPrefs.GetInt("Gate") == 1)
                {
                    gameObject.transform.position = Gate1.transform.position;
                }

                if (PlayerPrefs.GetInt("Gate") == 2)
                {
                    gameObject.transform.position = Gate2.transform.position;
                }

                if (PlayerPrefs.GetInt("Gate") == 3)
                {
                    gameObject.transform.position = Gate3.transform.position;
                }
            }
            else
            {
                TransitionAnim.SetTrigger("End");
            }
        }
        else
        {
            PlayerPrefs.SetInt("Gate", Gate);
            StartCoroutine(Transition());
        }
    }

    public void StartTransition()
    {
        if (Gate == PlayerPrefs.GetInt("Gate"))
        {
            Task++;
        }
    }

    IEnumerator Transition()
    {
        TransitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(Scene);
    }
*/
}
