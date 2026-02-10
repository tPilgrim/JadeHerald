using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crimson : MonoBehaviour
{
    private Animator Anim;
    public Animator AnimUI;
    private GameObject Player;
    public ParticleSystem Burst;

    private bool IsInReach;
    private AudioSource AudioManager;

    void Start()
    {
        PlayerPrefs.SetInt(gameObject.transform.name, 0);
        Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        if(PlayerPrefs.GetInt(gameObject.transform.name) == 1)
        {
            Anim.SetTrigger("LightUp");
            Burst.Play();
        }
        AudioManager = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Interact") && IsInReach == true)
        {
            if (PlayerPrefs.GetInt(gameObject.transform.name) == 0)
            {
                AudioManager.Play();
                Burst.Play();
            }
            Anim.SetTrigger("LightUp");
            Player.GetComponent<PlayerController>().CheckPoint(transform.position.x, transform.position.y);
            Player.GetComponent<PlayerController>().StartSit();
            PlayerPrefs.SetInt(gameObject.transform.name, 1);
        }
    }

    public void Button()
    {
        if (PlayerPrefs.GetInt(gameObject.transform.name) == 0)
        {
            AudioManager.Play();
            Burst.Play();
        }
        Anim.SetTrigger("LightUp");
        Player.GetComponent<PlayerController>().CheckPoint(transform.position.x, transform.position.y);
        PlayerPrefs.SetInt(gameObject.transform.name, 1);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
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
