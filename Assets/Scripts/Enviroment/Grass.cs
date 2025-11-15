using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Grass : MonoBehaviour
{
    private Animator anim;
    private bool isSwaying = false;

    private AudioSource AudioManager;
    public AudioClip Grass1;
    public AudioClip Grass2;
    public AudioClip Grass3;
    public AudioClip Grass4;

    void Start()
    {
        anim = GetComponent<Animator>();
        AudioManager = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isSwaying && other.gameObject.tag != "Untagged")
        {
            anim.SetTrigger("Sway");
            isSwaying = true;

            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    AudioManager.PlayOneShot(Grass1);
                    break;
                case 1:
                    AudioManager.PlayOneShot(Grass2);
                    break;
                case 2:
                    AudioManager.PlayOneShot(Grass3);
                    break;
                case 3:
                    AudioManager.PlayOneShot(Grass4);
                    break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isSwaying && other.gameObject.tag == "Enemy")
        {
            anim.SetTrigger("Sway");
            isSwaying = true;
        }
    }

    public void ResetSway()
    {
        isSwaying = false;
    }
}
