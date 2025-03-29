using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    private Animator Anim;    

    public GameObject EnemyComponent;

    public int HP;
    private int CurrentHealth;
    private bool IsDead = false;
    private bool IsDefending;

    public SpriteRenderer Enemy;

    public Material SpriteMaterial;
    public Material LightMaterial;

    public ParticleSystem DamageParticles;

    public bool IsAnObject;

    private AudioSource AudioManager;
    public AudioClip HitSound;
    public AudioClip ShieldSound;

    void Start()
    {
        CurrentHealth = HP;
        Anim = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("Scene" + SceneManager.GetActiveScene().buildIndex) == 1)
        {
            PlayerPrefs.SetInt(gameObject.transform.name + SceneManager.GetActiveScene().buildIndex, HP);
        }

        if(GetComponent<AudioSource>()!=null)
        {
            AudioManager = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        //Debug.Log(gameObject.transform.name + " " + CurrentHealth);
        Health();

        if (Input.GetButton("Reset"))
        {
            for(int i = 0; i<=6; i++)
            {
                PlayerPrefs.SetInt("Scene" + i, 1);
            }
            //PlayerPrefs.SetInt(gameObject.transform.name + SceneManager.GetActiveScene().buildIndex, HP);
        }
    }

    void Health()
    {
        if(CurrentHealth <= 0 && IsAnObject == false)
        {
            PlayerPrefs.SetInt(gameObject.transform.name + SceneManager.GetActiveScene().buildIndex, 0);
            Anim.SetBool("IsDying", true);
            IsDead = true;
        }
    }

    IEnumerator Flash()
    {
        GetComponent<Renderer>().material = SpriteMaterial;
        yield return new WaitForSeconds(0.15f);
        GetComponent<Renderer>().material = LightMaterial;
    }

    public void TakeDamage(int Damage)
    {
        if (IsDefending == true)
        {
            if (EnemyComponent.GetComponent<Bogle>() != null)
            {
                EnemyComponent.GetComponent<Bogle>().StartParry();
            }

            if (GetComponent<AudioSource>() != null)
            {
                AudioManager.PlayOneShot(ShieldSound);
                AudioManager.volume = 0.3f;
            }
        }
        else
        {
            if (GetComponent<AudioSource>() != null)
            {
                AudioManager.PlayOneShot(HitSound);
                AudioManager.volume = 0.3f;
            }
            StartCoroutine(Flash());
            if (IsDead == false && IsDefending == false)
            {
                DamageParticles.Play();
                CurrentHealth -= Damage;
            }
        }
    }

    public void Defend(bool CanParry)
    {
        IsDefending = CanParry;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Spikes")
        {
            CurrentHealth -= 999;
        }
    }

    public void DisableEnemy(string Name)
    {
        if(Name == gameObject.transform.name && IsAnObject == false)
        {
            CurrentHealth -= 999;
        }
    }
}
