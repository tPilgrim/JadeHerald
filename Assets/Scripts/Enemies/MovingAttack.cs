using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAttack : MonoBehaviour
{
    public int PhysicalDamage;
    public int MagicDamage;
    public int PoiseDamage;
    private GameObject PlayerComponent;
    private bool HitSheild;

    void Start()
    {
        PlayerComponent = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sheild")
        {
            HitSheild = true;
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, true);
            gameObject.SetActive(false);
        }
        else if ((other.gameObject.tag == "Player" || other.gameObject.tag == "PlayerCollider") && HitSheild == false)
        {
            PlayerComponent.GetComponent<PlayerController>().AccesHealth(PhysicalDamage, MagicDamage, PoiseDamage, false);
            gameObject.SetActive(false);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sheild")
        {
            HitSheild = false;
        }
    }
}
